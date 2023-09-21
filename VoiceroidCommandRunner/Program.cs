using HakuVoiceNarratorLibrary.Common;
using NAudio.Wave;
using NLog;
using System;

namespace VoiceroidCommandRunner
{
    /// <summary>
    /// メインクラス
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 設定読込
        /// </summary>
        public static Setting config = new Setting();

        /// <summary>
        /// NLogロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// メインメソッド
        /// </summary>
        /// <param name="args">引数文字列(配列)</param>
        /// <returns></returns>
        public static uint Main(string[] args)
        {
            uint returnVal = 255;

            // NLogの設定反映
            LogManager.Configuration = config.GetNLogSetting();

            logger.Info("==============================  Start   ==============================");
            try
            {

            }
            catch (Exception ex)
            {
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }
            logger.Info("==============================   End    ==============================");

            return returnVal;
        }

    }
}
