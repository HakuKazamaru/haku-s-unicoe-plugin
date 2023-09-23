using Controllers = HakuVoiceNarratorLibrary.Controllers;
using Models = HakuVoiceNarratorLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceroidCommandRunner
{
    /// <summary>
    /// 引数処理クラス
    /// </summary>
    internal static class Command
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 引数確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static uint CheckBootParameter(string[] args)
        {
            uint returnVal = 255;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                returnVal = 0b00000000;
                if (args is not null)
                {
                    if (args.Length > 0)
                    {
                        // ヘルプコマンドの確認
                        foreach (var arg in args.Select((Value, Index) => (Value, Index)))
                        {
                            // コマンド引数のみ対応([/]または[-]から始まる場合)
                            if (arg.Value.IndexOf("/") == 0 || arg.Value.IndexOf("-") == 0)
                            {
                                string commandString = arg.Value.Replace("/", "").Replace("-", "").ToLower();

                                switch (commandString)
                                {
                                    case "o":
                                    case "out":
                                    case "t":
                                    case "text":
                                    case "v":
                                    case "volume":
                                    case "s":
                                    case "speed":
                                    case "p":
                                    case "pitch":
                                    case "i":
                                    case "intonation":
                                    case "b":
                                    case "break":
                                    case "c":
                                    case "character":
                                    case "n":
                                    case "name":
                                    case "l":
                                    case "list":
                                        {
                                            // 制御コマンドは何もしない
                                            break;
                                        }
                                    case "?":
                                    case "h":
                                    default:
                                        {
                                            returnVal = 0b10000000;
                                            PrintHelp();
                                            break;
                                        }
                                }


                            }
                        }

                        // ヘルプコマンドを使用していない場合、各パラメータの確認
                        if (returnVal != 0b10000000)
                        {
                            uint parameterOk = 0b00000000;
                            foreach (var arg in args.Select((Value, Index) => (Value, Index)))
                            {
                                // コマンド引数のみ対応([/]または[-]から始まる場合)
                                if (arg.Value.IndexOf("/") == 0 || arg.Value.IndexOf("-") == 0)
                                {
                                    string commandString = arg.Value.Replace("/", "").Replace("-", "").ToLower();

                                    switch (commandString)
                                    {
                                        case "o":
                                        case "out":
                                            {
                                                if (!CheckOutputParameter(args, arg.Index)) { returnVal += 0b00000001; }
                                                else { parameterOk += 0b00000001; }
                                                break;
                                            }
                                        case "t":
                                        case "text":
                                            {
                                                if (!CheckTextParameter(args, arg.Index)) { returnVal += 0b00000010; }
                                                else { parameterOk += 0b00000010; }
                                                break;
                                            }
                                        case "v":
                                        case "volume":
                                            {
                                                if (!CheckVolumeParameter(args, arg.Index)) { returnVal += 0b00000100; }
                                                else { parameterOk += 0b00000100; }
                                                break;
                                            }
                                        case "s":
                                        case "speed":
                                            {
                                                if (!CheckSpeedParameter(args, arg.Index)) { returnVal += 0b00001000; }
                                                else { parameterOk += 0b00001000; }
                                                break;
                                            }
                                        case "p":
                                        case "pitch":
                                            {
                                                if (!CheckPitchParameter(args, arg.Index)) { returnVal += 0b00010000; }
                                                else { parameterOk += 0b00010000; }
                                                break;
                                            }
                                        case "i":
                                        case "intonation":
                                            {
                                                if (!CheckIntonationParameter(args, arg.Index)) { returnVal += 0b00100000; }
                                                else { parameterOk += 0b00100000; }
                                                break;
                                            }
                                        case "b":
                                        case "break":
                                            {
                                                if (!CheckBreakParameter(args, arg.Index)) { returnVal += 0b01000000; }
                                                else { parameterOk += 0b01000000; }
                                                break;
                                            }
                                        case "c":
                                        case "character":
                                            {
                                                if (!CheckCharacterParameter(args, arg.Index)) { returnVal += 0b01000000; }
                                                else { parameterOk += 0b10000000; }
                                                break;
                                            }
                                        case "n":
                                        case "name":
                                            {
                                                if (!CheckNameParameter(args, arg.Index)) { returnVal += 0b01000000; }
                                                else { parameterOk += 0b10000000; }
                                                break;
                                            }
                                        case "l":
                                        case "list":
                                            {
                                                break;
                                            }
                                        case "?":
                                        case "h":
                                        default:
                                            {
                                                // 制御コマンド以外は何もしない
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        returnVal += 0b10000000;
                        PrintHelp();
                    }
                }
                else
                {
                    returnVal += 0b10000000;
                    PrintHelp();
                }
            }
            catch (Exception ex)
            {
                returnVal = 255;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 出力先確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckOutputParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    char[] invalidFileChars = Path.GetInvalidFileNameChars();
                    char[] invalidPathChars = Path.GetInvalidPathChars();
                    string fullPath = args[index + 1];
                    string fileName = Path.GetFileName(fullPath);
                    if (fileName.IndexOfAny(invalidFileChars) < 0 && fullPath.IndexOfAny(invalidPathChars) < 0)
                    {
                        string saveDirectory = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(saveDirectory))
                        {
                            returnVal = false;
                            logger.Error("音声ファイルの保存先フォルダーが存在していません。パス:{0}", saveDirectory);
                        }
                        else
                        {
                            Program.wavePath = fullPath;
                            returnVal = true;
                        }
                    }
                    else
                    {
                        returnVal = false;
                        logger.Error("音声ファイルの保存先のファイルパスに使用できない文字列が存在しています。パス:{0}", fullPath);
                    }
                }
                else
                {
                    returnVal = false;
                    logger.Error("音声ファイルの保存先が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /O 保存先\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /O C:\\Output.wav\r\n";
                    logger.Info(messageString);
                }
            }
            catch (PathTooLongException ex)
            {
                returnVal = false;
                logger.Error(ex, "音声ファイルの保存先のファイルパスが長すぎます。");
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 読み上げ文確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckTextParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    string tmpString = args[index + 1];
                    List<string> tmpStrings = tmpString.Split("\\n").ToList();

                    Program.voiceParameter.Texts = new List<string>();

                    foreach (var rowString in tmpStrings.Select((Value, Index) => (Value, Index)))
                    {
                        Program.voiceParameter.Texts.Add(rowString.Value);
                    }

                    returnVal = true;
                }
                else
                {
                    returnVal = false;
                    logger.Error("読み上げる文章が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /T 読み上げ文章\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /T こんにちは、今日はいい天気ですね。\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 音量確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckVolumeParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    string tmpString = args[index + 1];
                    float tmpFloat = 0.0f;

                    if (!float.TryParse(tmpString, out tmpFloat))
                    {
                        returnVal = false;
                        logger.Error("音量が数値で指定されていません。");
                    }
                    else
                    {
                        if (tmpFloat > 2.0f)
                        {
                            returnVal = false;
                            logger.Error("音量が最大値(2.00)を超えています。指定値:{0}", tmpFloat);
                        }
                        else if (tmpFloat < 0.0f)
                        {
                            returnVal = false;
                            logger.Error("音量が最小値(0.00)を下回っています。指定値:{0}", tmpFloat);
                        }
                        else
                        {
                            returnVal = true;
                            Program.voiceParameter.Volume = tmpFloat;
                        }
                    }
                }
                else
                {
                    returnVal = false;
                    logger.Error("音量が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /V 音量\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /V 1.00\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 話速確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckSpeedParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    string tmpString = args[index + 1];
                    float tmpFloat = 0.0f;

                    if (!float.TryParse(tmpString, out tmpFloat))
                    {
                        returnVal = false;
                        logger.Error("話速が数値で指定されていません。");
                    }
                    else
                    {
                        if (tmpFloat > 4.0f)
                        {
                            returnVal = false;
                            logger.Error("話速が最大値(4.00)を超えています。指定値:{0}", tmpFloat);
                        }
                        else if (tmpFloat < 0.5f)
                        {
                            returnVal = false;
                            logger.Error("話速が最小値(0.50)を下回っています。指定値:{0}", tmpFloat);
                        }
                        else
                        {
                            returnVal = true;
                            Program.voiceParameter.Speed = tmpFloat;
                        }
                    }
                }
                else
                {
                    returnVal = false;
                    logger.Error("話速が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /S 話速\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /S 1.00\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 高さ確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckPitchParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    string tmpString = args[index + 1];
                    float tmpFloat = 0.0f;

                    if (!float.TryParse(tmpString, out tmpFloat))
                    {
                        returnVal = false;
                        logger.Error("高さが数値で指定されていません。");
                    }
                    else
                    {
                        if (tmpFloat > 2.0f)
                        {
                            returnVal = false;
                            logger.Error("高さが最大値(2.00)を超えています。指定値:{0}", tmpFloat);
                        }
                        else if (tmpFloat < 0.50f)
                        {
                            returnVal = false;
                            logger.Error("高さが最小値(0.50)を下回っています。指定値:{0}", tmpFloat);
                        }
                        else
                        {
                            returnVal = true;
                            Program.voiceParameter.Tone = tmpFloat;
                        }
                    }
                }
                else
                {
                    returnVal = false;
                    logger.Error("高さが指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /P 高さ\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /P 1.00\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 抑揚確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckIntonationParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    string tmpString = args[index + 1];
                    float tmpFloat = 0.0f;

                    if (!float.TryParse(tmpString, out tmpFloat))
                    {
                        returnVal = false;
                        logger.Error("抑揚が数値で指定されていません。");
                    }
                    else
                    {
                        if (tmpFloat > 2.0f)
                        {
                            returnVal = false;
                            logger.Error("抑揚が最大値(2.00)を超えています。指定値:{0}", tmpFloat);
                        }
                        else if (tmpFloat < 0.0f)
                        {
                            returnVal = false;
                            logger.Error("抑揚が最小値(0.00)を下回っています。指定値:{0}", tmpFloat);
                        }
                        else
                        {
                            returnVal = true;
                            Program.voiceParameter.Intonation = tmpFloat;
                        }
                    }
                }
                else
                {
                    returnVal = false;
                    logger.Error("抑揚が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /I 抑揚\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /I 1.00\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// ポーズ確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckBreakParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    string tmpString = args[index + 1];
                    int tmpInt = 0;

                    if (!int.TryParse(tmpString, out tmpInt))
                    {
                        returnVal = false;
                        logger.Error("ポーズが数値で指定されていません。");
                    }
                    else
                    {
                        if (tmpInt > 500)
                        {
                            returnVal = false;
                            logger.Error("ポーズが最大値(500)を超えています。指定値:{0}", tmpInt);
                        }
                        else if (tmpInt < 0)
                        {
                            returnVal = false;
                            logger.Error("ポーズが最小値(0)を下回っています。指定値:{0}", tmpInt);
                        }
                        else
                        {
                            returnVal = true;
                            Program.spanMS = tmpInt;
                        }
                    }
                }
                else
                {
                    returnVal = false;
                    logger.Error("ポーズが指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /B ポーズ\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /P 50\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 話者ID確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckCharacterParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    uint tmpInt = 0;
                    string tmpString = args[index + 1];

                    if (!uint.TryParse(tmpString, out tmpInt))
                    {
                        returnVal = false;
                        logger.Error("話者が数値(整数)で指定されていません。");
                    }
                    else
                    {
                        int checkFlag = Controllers.VoiceroidInfo.SearchByTalkerId(tmpInt, Program.voiceroidInfos);
                        if (checkFlag == 1)
                        {
                            returnVal = true;
                            Program.voiceParameter.TalkerId = tmpInt;
                            Program.isTalkerOk = true;
                        }
                        else
                        {
                            returnVal = false;
                            Program.isTalkerOk = false;
                            logger.Error("指定された話者が存在していません。");
                        }
                    }
                }
                else
                {
                    returnVal = false;
                    Program.isTalkerOk = false;

                    logger.Error("話者が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /C 話者ID\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /C 0\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                Program.isTalkerOk = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// 話者名確認処理
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool CheckNameParameter(string[] args, int index)
        {
            bool returnVal = false;
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (args.Length > index + 1)
                {
                    int checkFlag = -1;
                    string tmpString = args[index + 1];
                    Models.VoiceroidInfo voiceroidInfo;

                    Program.voiceParameter.IsAiTalk = tmpString.IndexOf("|") >= 0;

                    // かんたん! AITalk話者指定あり時は「|」より前を話者名として使用
                    tmpString = Program.voiceParameter.IsAiTalk ? tmpString.Split('|')[0] : tmpString;
                    (checkFlag, voiceroidInfo) = Controllers.VoiceroidInfo.GetVoiceroidInfoByTalkerName(tmpString, Program.voiceroidInfos);

                    if (checkFlag == 1)
                    {
                        returnVal = true;
                        Program.voiceParameter.TalkerId = voiceroidInfo.TalkerId;

                        if (Program.voiceParameter.IsAiTalk)
                        {
                            tmpString = args[index + 1].Split('|')[1];
                            Program.voiceParameter.CharacterNo = Controllers.VoiceroidInfo.GetCharacterNoByCharacterName(tmpString, voiceroidInfo);
                            if (Program.voiceParameter.CharacterNo < 0)
                            {
                                logger.Error("指定されたAI Talkのキャラクターが存在していません。");
                                Program.isTalkerOk = false;
                            }
                            else
                            {
                                Program.isTalkerOk = true;
                            }
                        }
                        else
                        {
                            Program.isTalkerOk = true;
                        }
                    }
                    else
                    {
                        returnVal = false;
                        Program.isTalkerOk = false;
                        logger.Error("指定された話者が存在していません。");
                    }
                }
                else
                {
                    returnVal = false;
                    Program.isTalkerOk = false;

                    logger.Error("話者が指定されていません。");
                    messageString += "指定方法：\r\n";
                    messageString += "\r\n";
                    messageString += "VoiceroidCommandRunner /N 話者名\r\n";
                    messageString += "\r\n";
                    messageString += "指定例：\r\n";
                    messageString += "VoiceroidCommandRunner /N \"VOICEROID+ 結月ゆかり EX\"\r\n";
                    logger.Info(messageString);
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
                Program.isTalkerOk = false;
                logger.Error(ex, "予期せぬエラーが発生しました。エラーメッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// ヘルプ表示処理
        /// </summary>
        private static void PrintHelp()
        {
            string messageString = "";
            logger.Debug("==============================  Start   ==============================");

            messageString += "VOICEROID Ex系ソフトコマンド制御ツール\r\n";
            messageString += "\r\n";
            messageString += "使用方法：\r\n";
            messageString += "\r\n";
            messageString += "VoiceroidCommandRunner /O 保存先 /T 読み上げ文章 /V 音量 /S 話速 /P 高さ\r\n";
            messageString += "                       /I 抑揚 /B ポーズ [/C 話者ID /N 話者名] [/L]\r\n";
            messageString += "\r\n";
            messageString += "  /O 保存先       生成した音声ファイルの保存先を指定します。\r\n";
            messageString += "  /T 読み上げ文章 読み上げる文章をテキストで指定します。\r\n";
            messageString += "  /V 音量         読み上げ音量を指定します。\r\n";
            messageString += "                  初期値:1.00 最小値:0.00 最大値:2.00\r\n";
            messageString += "  /S 話速         読み上げる速さを指定します。\r\n";
            messageString += "                  初期値:1.00 最小値:0.50 最大値:4.00\r\n";
            messageString += "  /P 高さ         読み上げ時の声の高さを指定します。\r\n";
            messageString += "                  初期値:1.00 最小値:0.50 最大値:2.00\r\n";
            messageString += "  /I 抑揚         読み上げ時の抑揚の強さを指定します。\r\n";
            messageString += "                  初期値:1.00 最小値:0.00 最大値:2.00\r\n";
            messageString += "  /B ポーズ       読み上げる文章毎の読み上げ間隔を指定します。\r\n";
            messageString += "  /C 話者ID       読み上げるキャラクターIDを指定します。\r\n";
            messageString += "  /N 話者名       読み上げるキャラクター名を指定します。\r\n";
            messageString += "  /L              読み上げるキャラクターのIDと名前の一覧を表示します。\r\n";

            logger.Info(messageString);

            logger.Debug("==============================   End    ==============================");
        }
    }
}
