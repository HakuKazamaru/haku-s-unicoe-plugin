using HakuVoiceNarratorLibrary.Common;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary.Controllers
{
    /// <summary>
    /// Z-Index情報管理情報処理クラス
    /// </summary>
    public class WindowZindexInfo
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
        public static (int, List<Models.WindowZindexInfo>) LodAppConfig()
        {
            int returnVal1 = -1;
            List<Models.WindowZindexInfo> returnVal2 = null;

            logger.Debug("==============================  Start   ==============================");

            try
            {
                IConfigurationSection section = _setting.AppConfig.GetSection("VoiceroidInfo");
                returnVal2 = new List<Models.WindowZindexInfo>();

                foreach (var config in section.Get<List<IConfigurationSection>>().Select((Value, Index) => (Value, Index)))
                {
                    bool readError = false;
                    Models.WindowZindexInfo tmpWindowZindexInfo = new Models.WindowZindexInfo();
                    List<string> tmpStrings;


                    logger.Debug("・読み取り対象");
                    logger.Debug("TalkerId   :{0}", config.Value["TalkerId"]);
                    logger.Debug("TalkerName :{0}", config.Value["TalkerName"]);
                    logger.Debug("ProcessName:{0}", config.Value["ProcessName"]);

                    logger.Debug("Z-Index:Text:{0}", config.Value.GetSection("Z-Index")["Text"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Text"].Split(',').ToList();
                    tmpWindowZindexInfo.TextBot = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.TextBot.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Text:Count:{0}", tmpWindowZindexInfo.TextBot.Count);


                    logger.Debug("Z-Index:Play:{0}", config.Value.GetSection("Z-Index")["Play"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Play"].Split(',').ToList();
                    tmpWindowZindexInfo.PlayButton = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.PlayButton.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Play:Count:{0}", tmpWindowZindexInfo.PlayButton.Count);

                    logger.Debug("Z-Index:Save:{0}", config.Value.GetSection("Z-Index")["Save"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Save"].Split(',').ToList();
                    tmpWindowZindexInfo.SaveButton = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.SaveButton.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Save:Count:{0}", tmpWindowZindexInfo.SaveButton.Count);

                    logger.Debug("Z-Index:VoiceParameter:{0}", config.Value.GetSection("Z-Index")["VoiceParameter"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["VoiceParameter"].Split(',').ToList();
                    tmpWindowZindexInfo.VoiceParameterTab = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.VoiceParameterTab.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:VoiceParameter:Count:{0}", tmpWindowZindexInfo.VoiceParameterTab.Count);

                    logger.Debug("Z-Index:Volume:{0}", config.Value.GetSection("Z-Index")["Volume"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Volume"].Split(',').ToList();
                    tmpWindowZindexInfo.Volume = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.Volume.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Volume:Count:{0}", tmpWindowZindexInfo.Volume.Count);

                    logger.Debug("Z-Index:Speed:{0}", config.Value.GetSection("Z-Index")["Speed"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Speed"].Split(',').ToList();
                    tmpWindowZindexInfo.Speed = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.Speed.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Speed:Count:{0}", tmpWindowZindexInfo.Speed.Count);

                    logger.Debug("Z-Index:Tone:{0}", config.Value.GetSection("Z-Index")["Tone"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Tone"].Split(',').ToList();
                    tmpWindowZindexInfo.Tone = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.Tone.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Tone:Count:{0}", tmpWindowZindexInfo.Tone.Count);

                    logger.Debug("Z-Index:Intonation:{0}", config.Value.GetSection("Z-Index")["Intonation"]);
                    tmpStrings = config.Value.GetSection("Z-Index")["Intonation"].Split(',').ToList();
                    tmpWindowZindexInfo.Intonation = new List<uint>();
                    foreach (var tmpString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        uint tmpUint = 0;
                        if (uint.TryParse(tmpString.Value, out tmpUint))
                        {
                            tmpWindowZindexInfo.Intonation.Add(tmpUint);
                        }
                        else
                        {
                            readError = true;
                            logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", tmpString.Value);
                            break;
                        }
                    }
                    logger.Debug("Z-Index:Intonation:Count:{0}", tmpWindowZindexInfo.Intonation.Count);

                    if (readError)
                    {
                        logger.Warn("この設定値の読み取りをスキップします。TalkerId:{0},TalkerName:{1}",
                            config.Value["TalkerId"], config.Value["TalkerName"]);
                    }
                    else
                    {
                        returnVal2.Add(tmpWindowZindexInfo);
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
