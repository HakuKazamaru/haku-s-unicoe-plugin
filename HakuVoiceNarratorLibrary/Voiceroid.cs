using NAudio.Wave;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary
{
    /// <summary>
    /// VOICEROID Ex互換エンジン使用ソフト用
    /// </summary>
    public class Voiceroid
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// VOICEROID実行メソッド
        /// </summary>
        /// <param name="text">読み上げテキスト</param>
        /// <param name="volume">音量</param>
        /// <param name="speed">話速</param>
        /// <param name="tone">高さ</param>
        /// <param name="accent">抑揚</param>
        /// <param name="spanMS">ポーズ(ミリ秒)</param>
        /// <returns></returns>
        public static async Task<(int, int, double[])> TakeVoiceroid(string wavePath, string text, float volume, float speed, float tone, float accent, int spanMS)
        {
            logger.Debug("==============================  Start   ==============================");
            List<double> waveArrayList;
            int sampleRate, returnVal = -1;
            double[] waveArray;

            try
            {
                // -------------------- VOICEROID制御ロジック --------------------

                // メインウィンドウのハンドル取得
                var mainWindowHandle = Process.GetProcessesByName("galacoTalk")[0].MainWindowHandle;
                // コントロール取得
                var all = Window.GetAllChildWindows(Window.GetWindow(mainWindowHandle), new List<Window>());
                var textBox = all.Where(x => x.ClassName.IndexOf("RichEdit") > -1).ToArray();
                var button = all.Where(x => x.ClassName.IndexOf("BUTTON") > -1).Where(x => x.Title.IndexOf("再生") > -1).ToArray();

                if (textBox.Count() > 0 && button.Count() > 0)
                {
                    Window.SendString(textBox[0].hWnd, text);
                    Window.SendMessage(button[0].hWnd, Window.WM_LBUTTONDOWN, Window.MK_LBUTTON, 0x000A000A);
                    Window.SendMessage(button[0].hWnd, Window.WM_LBUTTONUP, 0x00000000, 0x000A000A);

                    // 出力ファイルの存在確認
                    if (File.Exists(wavePath))
                    {
                        using (var fileReader = new WaveFileReader(wavePath))
                        {
                            sampleRate = fileReader.WaveFormat.SampleRate;
                            waveArrayList = new List<double>(spanMS * sampleRate / 1000);
                            while (fileReader.Position < fileReader.Length)
                            {
                                var samples = fileReader.ReadNextSampleFrame();
                                waveArrayList.Add(samples.First());
                            }
                        }
                        waveArray = waveArrayList.ToArray();
                        returnVal = 0;
                    }
                    else
                    {
                        returnVal = -1;
                        sampleRate = -1;
                        waveArray = new double[0];
                        logger.Error("TakeVoiceroidでVOICEROIDにて音声生成に失敗しました。");
                    }
                }
                else
                {
                    returnVal = -1;
                    sampleRate = -1;
                    waveArray = new double[0];
                    logger.Error("TakeVoiceroidでVOICEROIDのリモート実行に失敗しました。");
                    if (textBox.Count() > 0)
                    {
                        logger.Error("ボタンのハンドル取得に失敗しました。");

                    }
                    else if (button.Count() > 0)
                    {
                        logger.Error("テキストボックスのハンドル取得に失敗しました。");

                    }
                    else
                    {
                        logger.Error("すべてのハンドル取得に失敗しました。");
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = -1;
                sampleRate = -1;
                waveArray = new double[0];
                logger.Error(ex, "TakeVoiceroidでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================  Start   ==============================");
            return (returnVal, sampleRate, waveArray);
        }
    }
}
