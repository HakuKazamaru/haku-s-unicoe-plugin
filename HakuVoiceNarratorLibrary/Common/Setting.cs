using Microsoft.Extensions.Configuration;
using NLog.Config;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary.Common
{
    /// <summary>
    /// 設定管理クラス
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// アプリ名
        /// </summary>
        private static string appName = "Haku's App";

        /// <summary>
        /// アプリケーション設定ファイル管理用
        /// </summary>
        public IConfigurationRoot AppConfig { get; set; }

        /// <summary>
        /// NLog設定
        /// </summary>
        public LoggingConfiguration NLogConfiguration { get; set; }

        /// <summary>
        /// 設定ファイル保存パス
        /// 　(ファイル名付きフルパス)
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public Setting()
        {
            logger.Trace("==============================  Start   ==============================");

            try
            {
                string settingDir = Directory.GetCurrentDirectory();
                logger.Debug("起動パス：{0}", settingDir);

                // アプリケーション設定ファイル読込
                if (CheckUserSettingFileExists())
                {
                    settingDir = GetUserSettingPath();
                    logger.Debug("ユーザー設定値を読み出します。パス:{0}", settingDir);
                }
                else
                {
                    settingDir = Directory.GetCurrentDirectory();
                    logger.Debug("初期値を読み出します。パス:{0}", settingDir);
                }
                AppConfig = new ConfigurationBuilder()
                    .SetBasePath(settingDir)
                    .AddJsonFile(path: "appsettings.json")
                    .Build();
                ConfigPath = Path.Combine(settingDir, "appsettings.json");
                logger.Debug("アプリケーション設定ファイルを読み込みました。");

                // NLog設定読込
                NLogConfiguration = new NLogLoggingConfiguration(AppConfig.GetSection("NLog"));
                NLog.LogManager.Configuration = NLogConfiguration;
                logger.Debug("NLog設定を読み込みました。パス:{0}", ConfigPath);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Settingの初期化でエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// NLog設定取得用メソッド
        /// </summary>
        /// <returns></returns>
        public LoggingConfiguration GetNLogSetting()
        {
            return NLogConfiguration;
        }

        /// <summary>
        /// ユーザー設定ファイルの存在確認
        /// </summary>
        /// <returns></returns>
        public bool CheckUserSettingFileExists()
        {
            bool returnVal = false;
            logger.Trace("==============================  Start   ==============================");

            try
            {
                string filePath = GetUserSettingPath();
                logger.Info("ユーザー設定ファイル確認先：{0}", filePath);
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CheckUserSettingFileExistsでエラーが発生しました。メッセージ：{0}", ex.Message);
                returnVal = false;
            }

            logger.Trace("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// ユーザー設定ファイルのパス取得
        /// </summary>
        /// <returns></returns>
        private string GetUserSettingPath()
        {
            logger.Trace("==============================   Call    ==============================");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), appName, "appsettings.json");
        }

    }
}
