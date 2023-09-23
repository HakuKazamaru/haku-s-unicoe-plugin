using NAudio.Wave;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yomiage.SDK;
using Yomiage.SDK.Common;
using Yomiage.SDK.Config;
using Yomiage.SDK.Settings;
using Yomiage.SDK.Talk;
using Yomiage.SDK.VoiceEffects;

namespace VoiceroidTalkEngine
{
    public class VoiceEngine : VoiceEngineBase
    {
        /// <summary>
        /// waveファイルの保存先管理
        /// </summary>
        private string wavePath => Path.Combine(DllDirectory, "output.wav");

        /// <summary>
        /// ボイロコマンドコントロールツールパス
        /// </summary>
        private string exePath => Path.Combine(DllDirectory, "VoiceroidCommandRunner.exe");

        #region 本体連携用

        /// <summary>
        /// ステータステキスト更新用
        /// </summary>
        public override string StateText { get; protected set; } = string.Empty;
        #endregion

        /// <summary>
        /// プラグイン初期化処理
        /// </summary>
        /// <param name="configDirectory"></param>
        /// <param name="dllDirectory"></param>
        /// <param name="config"></param>
        public override void Initialize(string configDirectory, string dllDirectory, EngineConfig config)
        {
            base.Initialize(configDirectory, dllDirectory, config);
            StateText = "初期化されました。  ";
        }

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
            bool? tmpBool;
            int spanMs = 50;
            float volume = 1.0f, speed = 1.0f, pitch = 1.0f, intonation = 1.0f;
            double[] returnData;
            string talkerName = "", talkText = "", argsString = ""; ;
            StringSetting setting = null;

            try
            {
                // 音声データが生成済みの場合は削除
                if (File.Exists(wavePath)) { File.Delete(wavePath); }

                StateText = "引数情報を読み取り開始します。";

                // 話者取得
                tmpBool = mainVoice.Library.Settings.Strings?.TryGetSetting("voiceName", out setting);
                if (tmpBool != null && setting != null)
                {
                    talkerName = (bool)tmpBool ? setting.Value : "";
                }

                // 音声効果取得
                volume = (float)mainVoice.VoiceEffect.Volume;
                speed = (float)mainVoice.VoiceEffect.Speed;
                pitch = (float)mainVoice.VoiceEffect.Pitch;
                intonation = (float)mainVoice.VoiceEffect.Emphasis;

                // 読み上げテキスト取得
                talkText = talkScript.OriginalText;

                // 文末ポーズ取得
                spanMs = talkScript.Sections.First().Pause.Span_ms;

                StateText = "引数情報を読み取りました。";

                // 引数文字列生成
                argsString = string.Format(
                            "/O {0} /T \"{1}\" /V {2} /S {3} /P {4} /I {5} /B {6} /N \"{7}\"",
                            wavePath, talkText, volume, speed, pitch, intonation, spanMs, talkerName);

                // 音声ファイル生成
                if (File.Exists(exePath))
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo()
                    {
                        FileName = exePath,
                        Arguments = argsString,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = DllDirectory,
                    };

                    StateText = "VOICEROIDで読み上げを実行します。";
                    using (Process process = Process.Start(processStartInfo))
                    {
                        process.WaitForExit();
                        StateText = "VOICEROIDで読み上げが完了しました。";

                        if (process.ExitCode == 0)
                        {
                            // プロセスが正常終了した場合
                            if (File.Exists(wavePath))
                            {
                                StateText = "音声ファイルの読み込みを開始します。";
                                // ファイルが存在する場合
                                using (var waveFileReader = new WaveFileReader(wavePath))
                                {
                                    int fileSampleRate = waveFileReader.WaveFormat.SampleRate;
                                    setSamplingRate_Hz(fileSampleRate);
                                    var wave = new List<double>(spanMs * fileSampleRate / 1000);
                                    while (waveFileReader.Position < waveFileReader.Length)
                                    {
                                        var samples = waveFileReader.ReadNextSampleFrame();
                                        wave.Add(samples.First());
                                    }
                                    wave.AddRange(new double[(spanMs + (int)masterEffect.EndPause) * fileSampleRate / 1000]);
                                    returnData = wave.ToArray();
                                }
                                StateText = "音声ファイルを読み込みました。";
                            }
                            else
                            {
                                // ファイルが存在しない場合
                                StateText = "音声ファイルの生成に失敗しました。";
                                returnData = new double[0];
                            }
                        }
                        else
                        {
                            // プロセスが異常終了した場合
                            StateText = "VOICEROIDの実行に失敗しました。引数:"+ argsString;
                            returnData = new double[0];
                        }
                    }
                }
                else
                {
                    // 実行ファイルが存在しない場合
                    StateText = "プラグインに欠損があります。対象パス:" + exePath;
                    returnData = new double[0];
                }
            }
            catch (Exception ex)
            {
                StateText = "予期せぬエラーが発生しました。エラー：" + ex.Message;
                returnData = new double[0];
            }

            return returnData;
        }
    }
}
