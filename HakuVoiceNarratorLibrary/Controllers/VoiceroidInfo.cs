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
                    bool readError = false, tmpBool = false;
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

                    logger.Debug("AITalk:{0}", config.Value["AITalk"]);
                    if (bool.TryParse(config.Value["AITalk"], out tmpBool))
                    {
                        tmpVoiceroidInfo.IsAiTalk = tmpBool;
                    }
                    else
                    {
                        readError = true;
                        logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", config.Value["AITalk"]);
                    }

                    if (tmpVoiceroidInfo.IsAiTalk)
                    {
                        IConfigurationSection subSection = config.Value.GetSection("CharacterList");
                        tmpVoiceroidInfo.CharacterList = new List<Models.VoiceroidInfo.AiTalkCharacterInfo>();
                        foreach (var subConfig in subSection.Get<List<IConfigurationSection>>().Select((Value, Index) => (Value, Index)))
                        {
                            int tmpInt = 0;
                            Models.VoiceroidInfo.AiTalkCharacterInfo tmpCharInfo = new Models.VoiceroidInfo.AiTalkCharacterInfo();

                            logger.Debug("No  :{0}", subConfig.Value["No"]);
                            if (int.TryParse(subConfig.Value["No"], out tmpInt))
                            {
                                tmpCharInfo.No = tmpInt;
                            }
                            else
                            {
                                readError = true;
                                logger.Warn("コンフィグの読み込みに失敗しました。読取値:{0}", subConfig.Value["No"]);
                            }

                            logger.Debug("Name:{0}", subConfig.Value["Name"]);
                            tmpCharInfo.Name = subConfig.Value["Name"];

                            tmpVoiceroidInfo.CharacterList.Add(tmpCharInfo);
                        }
                    }

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

        /// <summary>
        /// 話者IDでボイロ情報リストを取得
        /// </summary>
        /// <param name="id">話者ID</param>
        /// <param name="voiceroidInfo">VOICEROID情報リスト</param>
        /// <returns></returns>
        public static (int, Models.VoiceroidInfo) GetVoiceroidInfoByTalkerId(uint id, List<Models.VoiceroidInfo> voiceroidInfos)
        {
            int returnVal1 = -1;
            Models.VoiceroidInfo returnVal2 = null;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                returnVal1 = 0;

                foreach (var voiceroidInfo in voiceroidInfos.Select((Value, Index) => (Value, Index)))
                {
                    if (voiceroidInfo.Value.TalkerId == id)
                    {
                        returnVal1 = 1;
                        logger.Debug("検索対象が見つかりました。話者ID:{0}", id);
                        logger.Debug("・ボイロ管理情報");
                        logger.Debug("話者名　　   :{0}", voiceroidInfo.Value.TalkerName);
                        logger.Debug("プロセス名   :{0}", voiceroidInfo.Value.ProcessName);
                        logger.Debug("かんたん!Talk:{0}", voiceroidInfo.Value.IsAiTalk);
                        if (voiceroidInfo.Value.IsAiTalk && voiceroidInfo.Value.CharacterList is not null)
                        {
                            logger.Debug("　・かんたん!Talk キャラクター一覧");
                            foreach (var charaInfo in voiceroidInfo.Value.CharacterList.Select((Value, Index) => (Value, Index)))
                            {
                                logger.Debug("　[{0}]キャラクターNo:{1}", charaInfo.Index, charaInfo.Value.No);
                                logger.Debug("　[{0}]キャラクター名:{2}", charaInfo.Index, charaInfo.Value.Name);
                            }
                        }
                        break;
                    }
                }

                if (returnVal1 == 0)
                {
                    logger.Debug("検索対象が見つかりませんでした。話者ID:{0}", id);
                }
            }
            catch (Exception ex)
            {
                returnVal1 = -1;
                returnVal2 = null;
                logger.Error(ex, "GetVoiceroidInfoByTalkerIdでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return (returnVal1, returnVal2);
        }

        /// <summary>
        /// 話者IDでボイロ情報リストを検索
        /// </summary>
        /// <param name="id"></param>
        /// <param name="voiceroidInfos">VOICEROID情報リスト</param>
        /// <returns></returns>
        public static int SearchByTalkerId(uint id, List<Models.VoiceroidInfo> voiceroidInfos)
        {
            int returnVal = -1;
            logger.Debug("==============================  Start   ==============================");

            (returnVal, var tmpObj) = GetVoiceroidInfoByTalkerId(id, voiceroidInfos);

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 話者名でボイス情報を取得
        /// </summary>
        /// <param name="name">話者名</param>
        /// <param name="voiceroidInfos">VOICEROID情報リスト</param>
        /// <returns></returns>
        public static (int, Models.VoiceroidInfo) GetVoiceroidInfoByTalkerName(string name, List<Models.VoiceroidInfo> voiceroidInfos)
        {
            int returnVal1 = -1;
            Models.VoiceroidInfo returnVal2 = null;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                returnVal1 = 0;

                foreach (var voiceroidInfo in voiceroidInfos.Select((Value, Index) => (Value, Index)))
                {
                    if (voiceroidInfo.Value.TalkerName == name)
                    {
                        returnVal1 = 1;
                        logger.Debug("検索対象が見つかりました。話者名:{0}", name);
                        logger.Debug("・ボイロ管理情報");
                        logger.Debug("話者ID　　   :{0}", voiceroidInfo.Value.TalkerId);
                        logger.Debug("プロセス名   :{0}", voiceroidInfo.Value.ProcessName);
                        logger.Debug("かんたん!Talk:{0}", voiceroidInfo.Value.IsAiTalk);
                        if (voiceroidInfo.Value.IsAiTalk && voiceroidInfo.Value.CharacterList is not null)
                        {
                            logger.Debug("　・かんたん!Talk キャラクター一覧");
                            foreach (var charaInfo in voiceroidInfo.Value.CharacterList.Select((Value, Index) => (Value, Index)))
                            {
                                logger.Debug("　[{0}]キャラクターNo:{1}", charaInfo.Index, charaInfo.Value.No);
                                logger.Debug("　[{0}]キャラクター名:{2}", charaInfo.Index, charaInfo.Value.Name);
                            }
                        }
                        returnVal2 = voiceroidInfo.Value;
                        break;
                    }
                }

                if (returnVal1 == 0)
                {
                    logger.Debug("検索対象が見つかりませんでした。話者名:{0}", name);
                }
            }
            catch (Exception ex)
            {
                returnVal1 = -1;
                returnVal2 = null;
                logger.Error(ex, "GetVoiceroidInfoByTalkerNameでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return (returnVal1, returnVal2);
        }

        /// <summary>
        /// キャラ名でAI Talkキャラ情報のキャラ番号を取得
        /// </summary>
        /// <param name="name">キャラクター名</param>
        /// <param name="voiceroidInfo">VOICEROID情報</param>
        /// <returns></returns>
        public static int GetCharacterNoByCharacterName(string name, Models.VoiceroidInfo voiceroidInfo)
        {
            int returnVal = -2;
            logger.Debug("==============================   End    ==============================");

            try
            {
                returnVal = 0;

                foreach (var tmpCharacterInfo in voiceroidInfo.CharacterList.Select((Value, Index) => (Value, Index)))
                {
                    if (tmpCharacterInfo.Value.Name == name)
                    {
                        returnVal = tmpCharacterInfo.Value.No;
                        logger.Debug("検索対象が見つかりました。キャラクターNo:{0},キャラクター名:{1}", tmpCharacterInfo.Value.No, tmpCharacterInfo.Value.Name);
                    }
                }

                if (returnVal == 0)
                {
                    logger.Debug("検索対象が見つかりませんでした。キャラクター名:{0}", name);
                }

            }
            catch (Exception ex)
            {
                returnVal = -2;
                logger.Error(ex, "GetVoiceroidInfoByTalkerNameでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

    }
}
