using HakuVoiceNarratorLibrary;
using HakuVoiceNarratorLibrary.Common;
using Controllers = HakuVoiceNarratorLibrary.Controllers;
using Models = HakuVoiceNarratorLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Windows.Forms;

namespace VoiceroidCommandRunner
{
    /// <summary>
    /// メインクラス
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 設定管理オブジェクト
        /// </summary>
        public static Setting config = new Setting();

        /// <summary>
        /// NLogロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ボイスパラメーター管理object
        /// </summary>
        internal static Models.VoiceParameter voiceParameter = new Models.VoiceParameter();

        /// <summary>
        /// VOICEROID管理情報オブジェクト
        /// </summary>
        internal static List<Models.VoiceroidInfo> voiceroidInfos = new List<Models.VoiceroidInfo>();

        /// <summary>
        /// 音声ファイル保存先
        /// </summary>
        internal static string wavePath = "";

        /// <summary>
        /// 文末ポーズ
        /// </summary>
        internal static int spanMS = 50;

        /// <summary>
        /// メインメソッド
        /// </summary>
        /// <returns></returns>
        static int Main(string[] args)
        {
            int returnVal = 255;

            // NLogの設定反映
            LogManager.Configuration = config.GetNLogSetting();

            logger.Info("==============================  Start   ==============================");

            try
            {
                Task<uint> task = Task.Run(() => VoiceroidMain(args));
                returnVal = (int)task.Result;
            }
            catch (Exception ex)
            {
                returnVal = 255;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Info("==============================   End    ==============================");

            return returnVal;
        }

        /// <summary>
        /// メインメソッド(async)
        /// </summary>
        /// <param name="args">引数文字列(配列)</param>
        /// <returns></returns>
        internal static async Task<uint> VoiceroidMain(string[] args)
        {
            uint returnVal = 255;

            logger.Info("==============================  Start   ==============================");
            try
            {
                int tmpInt = 0;
                // ボイロ管理情報の読み込み
                (tmpInt, voiceroidInfos) = Controllers.VoiceroidInfo.LodAppConfig();
                if (tmpInt == 0)
                {
                    // 引数チェック
                    returnVal = Command.CheckBootParameter(args);
                    if (returnVal == 0)
                    {
                        string tmpString = "";
                        
                        tmpString = voiceroidInfos[(int)voiceParameter.TalkerId].TalkerName;
                        logger.Info("話者　　　　：{0}", tmpString);
                        tmpString = "";
                        foreach (var text in voiceParameter.Texts.Select((Value, Index) => (Value, Index)))
                        {
                            tmpString += text.Value;
                        }
                        logger.Info("読み上げ文章：{0}", tmpString);
                        logger.Info("音量　　　　：{0}", voiceParameter.Volume);
                        logger.Info("話速　　　　：{0}", voiceParameter.Speed);
                        logger.Info("高さ　　　　：{0}", voiceParameter.Tone);
                        logger.Info("抑揚　　　　：{0}", voiceParameter.Intonation);
                        logger.Info("文末ポーズ　：{0}", spanMS);

                        if (File.Exists(wavePath))
                        {
                            File.Delete(wavePath);
                            logger.Debug("旧ファイルを削除しました。ファイルパス：{0}", wavePath);
                        }

                        using (var obgVoiceroid = new Voiceroid((int)voiceParameter.TalkerId))
                        {
                            (tmpInt, int sampleRate, double[] wave) = await obgVoiceroid.SaveVoiceroid(wavePath, voiceParameter, spanMS);
                            if (tmpInt == 0)
                            {
                                logger.Info("VOICEROIDで音声を生成しました。ファイルパス:{0}", wavePath);
                            }
                            else
                            {
                                logger.Error("VOICEROIDでの音声生成に失敗しました。");
                            }
                        }
                        returnVal = 0;
                    }
                }
                else
                {
                    returnVal = 1;
                    logger.Error("VOICEROID管理情報の読み込みに失敗しました。");
                }
            }
            catch (Exception ex)
            {
                returnVal = 255;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }
            logger.Info("==============================   End    ==============================");

            return returnVal;
        }

    }
}
