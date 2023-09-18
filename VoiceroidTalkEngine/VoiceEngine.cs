using NAudio.Wave;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Yomiage.SDK;
using Yomiage.SDK.Config;
using Yomiage.SDK.Talk;
using Yomiage.SDK.VoiceEffects;

namespace UniCoeEnginePlugin
{
    public class VoiceEngine : VoiceEngineBase
    {
        /// <summary>
        /// waveファイルの保存先管理
        /// </summary>
        private string wavePath => Path.Combine(DllDirectory, "output.wav");


        /// <summary>
        /// 音声合成メソッド
        /// </summary>
        /// <param name="mainVoice"></param>
        /// <param name="subVoice"></param>
        /// <param name="talkScript"></param>
        /// <param name="masterEffect"></param>
        /// <param name="setSamplingRate_Hz"></param>
        /// <param name="submitWavePart"></param>
        /// <returns></returns>

        public override async Task<double[]> Play(VoiceConfig mainVoice, VoiceConfig subVoice, TalkScript talkScript, MasterEffectValue masterEffect, Action<int> setSamplingRate_Hz, Action<double[]> submitWavePart)
        {

            (int returnVal, int sampleRate, double[] wave) = await TakeVoiceroid(
                talkScript.OriginalText,
                (float)talkScript.Volume,
                (float)talkScript.Speed,
                (float)talkScript.Pitch,
                (float)1.0,
                talkScript.Sections.First().Pause.Span_ms);

            if (returnVal == 0)
            {
                setSamplingRate_Hz(sampleRate);
                return wave;
            }
            else
            {
                return new double[0];
            }

        }

        /// <summary>
        /// VOICEROID実行メソッド
        /// </summary>
        /// <param name="text"></param>
        /// <param name="volume"></param>
        /// <param name="speed"></param>
        /// <param name="tone"></param>
        /// <param name="accent"></param>
        /// <returns></returns>
        private async Task<(int, int, double[])> TakeVoiceroid(string text, float volume, float speed, float tone, float accent, int spanMS)
        {

            try
            {
                // VOICEROID制御ロジック

                // メインウィンドウのハンドル取得
                var mainWindowHandle = Process.GetProcessesByName("galacoTalke")[0].MainWindowHandle;
                // コントロール取得
                var all = Window.GetAllChildWindows(Window.GetWindow(mainWindowHandle), new List<Window>());
                var textBox = all.Where(x => x.ClassName.IndexOf("RichEdit") > -1).ToArray();
                var button = all.Where(x => x.Title == "再生" && x.ClassName.IndexOf("RichEdit") > -1).ToArray();

                if (textBox.Count() > 0 && button.Count() > 0)
                {
                    Window.SendString(textBox[0].hWnd, text);
                    Window.SendMessage(textBox[0].hWnd, Window.WM_LBUTTONDOWN, Window.MK_LBUTTON, 0x000A000A);
                    Window.SendMessage(textBox[0].hWnd, Window.WM_LBUTTONUP, 0x00000000, 0x000A000A);
                }
                else
                {
                    return (-1, -1, new double[0]);
                }

                // 出力ファイルの存在確認
                if (File.Exists(wavePath))
                {
                    List<double> waveArray;
                    int sampleRate;

                    using (var fileReader = new WaveFileReader(wavePath))
                    {
                        sampleRate = fileReader.WaveFormat.SampleRate;
                        waveArray = new List<double>(spanMS * sampleRate / 1000);
                        while (fileReader.Position < fileReader.Length)
                        {
                            var samples = fileReader.ReadNextSampleFrame();
                            waveArray.Add(samples.First());
                        }
                    }
                    return (0, sampleRate, waveArray.ToArray());
                }
                else
                {
                    return (-1, -1, new double[0]);
                }
            }
            catch (Exception ex)
            {
                return (-1, -1, new double[0]);
            }
        }

    }
}
