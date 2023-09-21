using HakuVoiceNarratorLibrary.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary.Controllers
{
    /// <summary>
    /// 音声合成パラメーター情報処理クラス
    /// </summary>
    public class VoiceroidInfo
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 設定ファイル
        /// </summary>
        private static Setting _setting = new Setting();

        /// <summary>
        /// アプリケーション設定読み込み
        /// </summary>
        /// <returns></returns>
        public static (int, List<Models.VoiceroidInfo>) LodAppConfig()
        {
            int returnVal1 = -1;
            List<Models.VoiceroidInfo> returnVal2 = null;

            logger.Debug("==============================  Start   ==============================");

            try
            {
                IConfigurationSection section = _setting.AppConfig.GetSection("VoiceroidInfo");
                returnVal2 = new List<Models.VoiceroidInfo>();

                foreach (var config in section.Get<List<IConfigurationSection>>().Select((Value, Index) => (Value, Index)))
                {
                    bool readError = false;
                    uint tmpUint = 0;
                    Models.VoiceroidInfo tmpVoiceroidInfo = new Models.VoiceroidInfo();

                    logger.Debug("・読み取り対象");

                    logger.Debug("TalkerId   :{0}", config.Value["TalkerId"]);
                    if (uint.TryParse(config.Value["TalkerId"], out tmpUint))
                    {
                        tmpVoiceroidInfo.TalkerId = tmpUint;
                    }
                    else
                    {
                        readError = true;
                        logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", config.Value["TalkerId"]);
                    }


                    logger.Debug("TalkerName :{0}", config.Value["TalkerName"]);
                    tmpVoiceroidInfo.TalkerName = config.Value["TalkerName"];

                    logger.Debug("ProcessName:{0}", config.Value["ProcessName"]);
                    tmpVoiceroidInfo.ProcessName = config.Value["ProcessName"];

                    if (readError)
                    {
                        logger.Warn("この設定値の読み取りをスキップします。TalkerId:{0},TalkerName:{1}",
                            config.Value["TalkerId"], config.Value["TalkerName"]);
                    }
                    else
                    {
                        returnVal2.Add(tmpVoiceroidInfo);
                    }

                }

                returnVal1 = 0;
            }
            catch (Exception ex)
            {
                returnVal1 = -1;
                returnVal2 = null;
                logger.Error(ex, "LodAppConfigでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");

            return (returnVal1, returnVal2);
        }
    }
}
