using Codeer.Friendly;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using Codeer.Friendly.Windows.NativeStandardControls;
using HakuVoiceNarratorLibrary.Common;
using NAudio.Wave;
using NLog;
using Ong.Friendly.FormsStandardControls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary
{
    /// <summary>
    /// VOICEROID Ex互換エンジン使用ソフト用
    /// </summary>
    public class Voiceroid : IDisposable
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// リモートコントロール用オブジェクト
        /// </summary>
        private WindowsAppFriend _app;

        /// <summary>
        /// Window Z Index情報オブジェクト
        /// </summary>
        private Models.WindowZindexInfo _zindexInfo = new Models.WindowZindexInfo();

        /// <summary>
        /// VOICEROIDの値バックアップ用オブジェクト
        /// </summary>
        private Models.VoiceParameter _backupVoiceParameter = new Models.VoiceParameter();

        /// <summary>
        /// VOICEROID管理情報オブジェクト
        /// </summary>
        private Models.VoiceroidInfo _voiceroidInfo = new Models.VoiceroidInfo();

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="id">話者ID</param>
        public Voiceroid(int id)
        {
            logger.Trace("==============================  Start   ==============================");

            try
            {
                int returnVal = 0;
                List<Models.VoiceroidInfo> tmpVoiceroidInfos;
                List<Models.WindowZindexInfo> tmpZindexInfos;

                // 設定読み込み
                (returnVal, tmpVoiceroidInfos) = Controllers.VoiceroidInfo.LodAppConfig();
                if (returnVal == 0 && tmpVoiceroidInfos.Count > id)
                {
                    this._voiceroidInfo = tmpVoiceroidInfos[id];
                }
                else
                {
                    throw new Exception("VOICEROID管理情報1の読み込みに失敗しました。");
                }

                (returnVal, tmpZindexInfos) = Controllers.WindowZindexInfo.LodAppConfig();
                if (returnVal == 0 && tmpZindexInfos.Count > id)
                {
                    this._zindexInfo = tmpZindexInfos[id];
                }
                else
                {
                    throw new Exception("VOICEROID管理情報2の読み込みに失敗しました。");
                }

                Process mainWindowHandle = Process.GetProcessesByName(this._voiceroidInfo.ProcessName)[0];
                this._app = new WindowsAppFriend(mainWindowHandle);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Voiceroidの初期化でエラーが発生しました。メッセージ：{0}", ex.Message);
                throw ex;
            }

            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// VOICEROID実行メソッド(音声再生)
        /// </summary>
        /// <param name="parameter">読み上げパラメーター</param>
        /// <returns></returns>
        public async Task<int> TakeVoiceroid(Models.VoiceParameter parameter)
        {
            int returnVal = -1;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                // -------------------- VOICEROID制御ロジック --------------------
                // 設定値バックアップ
                if (this.GetVoiceParameter() == 0)
                {
                    logger.Debug("VOICEROIDのパラメーターをバックアップしました。");
                    // 音声効果反映
                    if (this.SendParameter(parameter) == 0)
                    {
                        logger.Debug("VOICEROIDへパラメーターを設定しました。");
                        // テキスト設定
                        foreach (var tmpString in parameter.Texts.Select((Value, Index) => (Value, Index)))
                        {
                            if (SendText(parameter, (uint)tmpString.Index) != 0)
                            {
                                returnVal = -1;
                                logger.Error("VOICEROIDへテキストの反映に失敗しました。行数:{0},文字列:{1}", tmpString.Index, tmpString.Value);
                                break;
                            }
                        }
                        logger.Debug("VOICEROIDへテキストを設定しました。");
                        // 音声再生
                        if (this._app is not null)
                        {
                            // 操作対象ウィンドウ
                            var targetWindows = WindowControl.FromZTop(this._app);
                            logger.Debug("操作対象Windowを取得しました。ハンドルID:{0}", targetWindows.Handle);

                            // 再生ボタン
                            FormsButton btPlay = new FormsButton(targetWindows.IdentifyFromZIndex(
                                (int)this._zindexInfo.PlayButton[0],
                                (int)this._zindexInfo.PlayButton[1],
                                (int)this._zindexInfo.PlayButton[2],
                                (int)this._zindexInfo.PlayButton[3],
                                (int)this._zindexInfo.PlayButton[4],
                                (int)this._zindexInfo.PlayButton[5],
                                (int)this._zindexInfo.PlayButton[6],
                                (int)this._zindexInfo.PlayButton[7]));
                            btPlay.EmulateClick();
                            logger.Debug("再生ボタンをクリックしました。ダイアログID:{0}", btPlay.DialogId);
                        }
                        else
                        {
                            returnVal = -1;
                            logger.Error("TakeVoiceroidでエラーが発生しました。メッセージ：VOICEROIDが起動していません。");
                        }

                        returnVal = 0;
                    }
                    else
                    {
                        returnVal = -1;
                        logger.Error("VOICEROIDへ音声効果の反映に失敗しました。");
                    }
                }
                else
                {
                    returnVal = -1;
                    logger.Error("VOICEROIDのパラメータバックアップに失敗しました。");
                }

            }
            catch (Exception ex)
            {
                returnVal = -1;
                logger.Error(ex, "TakeVoiceroidでエラーが発生しました。メッセージ：{0}", ex.Message);
            }
            finally
            {
                try
                {
                    bool isError = false;

                    logger.Debug("COICEROIDの設定を復元します。");

                    if (this.SendParameter(this._backupVoiceParameter) != 0)
                        isError = true;
                    foreach (var tmpString in this._backupVoiceParameter.Texts.Select((Value, Index) => (Value, Index)))
                    {
                        if (SendText(this._backupVoiceParameter, (uint)tmpString.Index) != 0)
                            isError = true;
                    }

                    if (isError)
                    {
                        throw new Exception("ToDo");
                    }
                    else
                    {
                        logger.Debug("COICEROIDの設定を復元しました。");
                    }

                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "VOICEROID設定の復元に失敗しました。メッセージ：{0}", ex.Message);
                }
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// VOICEROID実行メソッド(音声保存)
        /// </summary>
        /// <param name="wavePath">保存先</param>
        /// <param name="parameter">読み上げパラメーター</param>
        /// <param name="spanMS">ポーズ(ミリ秒)</param>
        /// <returns></returns>
        public async Task<(int, int, double[])> SaveVoiceroid(string wavePath, Models.VoiceParameter parameter, int spanMS)
        {
            List<double> waveArrayList;
            int sampleRate, returnVal = -1;
            double[] waveArray;

            logger.Debug("==============================  Start   ==============================");

            try
            {
                // -------------------- VOICEROID制御ロジック --------------------
                // 設定値バックアップ
                if (this.GetVoiceParameter() == 0)
                {
                    logger.Debug("VOICEROIDのパラメーターをバックアップしました。");
                    // 音声効果反映
                    if (this.SendParameter(parameter) == 0)
                    {
                        logger.Debug("VOICEROIDへパラメーターを設定しました。");
                        // テキスト設定
                        foreach (var tmpString in parameter.Texts.Select((Value, Index) => (Value, Index)))
                        {
                            if (SendText(parameter, (uint)tmpString.Index) != 0)
                            {
                                returnVal = -1;
                                sampleRate = -1;
                                waveArray = new double[0];
                                logger.Error("VOICEROIDへテキストの反映に失敗しました。行数:{0},文字列:{1}", tmpString.Index, tmpString.Value);
                                break;
                            }
                        }
                        logger.Debug("VOICEROIDへテキストを設定しました。");
                        // 音声保存
                        if (this._app is not null)
                        {
                            // 操作対象ウィンドウ
                            var targetWindows = WindowControl.FromZTop(this._app);
                            logger.Debug("操作対象Windowを取得しました。ハンドルID:{0}", targetWindows.Handle);

                            // 音声保存ボタン
                            FormsButton btSave = new FormsButton(targetWindows.IdentifyFromZIndex(
                                (int)this._zindexInfo.SaveButton[0],
                                (int)this._zindexInfo.SaveButton[1],
                                (int)this._zindexInfo.SaveButton[2],
                                (int)this._zindexInfo.SaveButton[3],
                                (int)this._zindexInfo.SaveButton[4],
                                (int)this._zindexInfo.SaveButton[5],
                                (int)this._zindexInfo.SaveButton[6],
                                (int)this._zindexInfo.SaveButton[7]));
                            btSave.EmulateClick();
                            logger.Debug("音声保存ボタンをクリックしました。ダイアログID:{0}", btSave.DialogId);

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
                                logger.Error("SaveVoiceroidでVOICEROIDにて音声生成に失敗しました。");
                            }
                        }
                        else
                        {
                            returnVal = -1;
                            sampleRate = -1;
                            waveArray = new double[0];
                            logger.Error("SaveVoiceroidでエラーが発生しました。メッセージ：VOICEROIDが起動していません。");
                        }


                        returnVal = 0;
                    }
                    else
                    {
                        returnVal = -1;
                        sampleRate = -1;
                        waveArray = new double[0];
                        logger.Error("VOICEROIDへ音声効果の反映に失敗しました。");
                    }
                }
                else
                {
                    returnVal = -1;
                    sampleRate = -1;
                    waveArray = new double[0];
                    logger.Error("VOICEROIDのパラメータバックアップに失敗しました。");
                }
            }
            catch (Exception ex)
            {
                returnVal = -1;
                sampleRate = -1;
                waveArray = new double[0];
                logger.Error(ex, "SaveVoiceroidでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return (returnVal, sampleRate, waveArray);
        }

        /// <summary>
        /// VOICEROIDへパラメーターをセットする処理
        /// </summary>
        /// <param name="parameter">ボイスパラメーター</param>
        /// <returns></returns>
        private int SendParameter(Models.VoiceParameter parameter)
        {
            int returnVal = -1;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (this._app is not null)
                {
                    // 操作対象ウィンドウ
                    var targetWindows = WindowControl.FromZTop(this._app);
                    logger.Debug("操作対象Windowを取得しました。ハンドルID:{0}", targetWindows.Handle);

                    // 音声効果タブ
                    FormsTabControl targetTab = new FormsTabControl(targetWindows.IdentifyFromZIndex(
                       (int)this._zindexInfo.VoiceParameterTab[0],
                       (int)this._zindexInfo.VoiceParameterTab[1],
                       (int)this._zindexInfo.VoiceParameterTab[2],
                       (int)this._zindexInfo.VoiceParameterTab[3],
                       (int)this._zindexInfo.VoiceParameterTab[4]));
                    targetTab.EmulateTabSelect(2);
                    logger.Debug("音声効果タブに切り替えました。ダイアログID:{0}", targetTab.DialogId);

                    // 音量
                    FormsTextBox tbVolume = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Volume[0],
                        (int)this._zindexInfo.Volume[1],
                        (int)this._zindexInfo.Volume[2],
                        (int)this._zindexInfo.Volume[3],
                        (int)this._zindexInfo.Volume[4],
                        (int)this._zindexInfo.Volume[5],
                        (int)this._zindexInfo.Volume[6],
                        (int)this._zindexInfo.Volume[7]));
                    tbVolume.EmulateChangeText(parameter.Volume.ToString("0.00"));
                    logger.Debug("音量をセットしました。ダイアログID:{0},セット値:{1}", tbVolume.DialogId, tbVolume.GetWindowText());

                    // 話速
                    FormsTextBox tbSpeed = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Speed[0],
                        (int)this._zindexInfo.Speed[1],
                        (int)this._zindexInfo.Speed[2],
                        (int)this._zindexInfo.Speed[3],
                        (int)this._zindexInfo.Speed[4],
                        (int)this._zindexInfo.Speed[5],
                        (int)this._zindexInfo.Speed[6],
                        (int)this._zindexInfo.Speed[7]));
                    tbSpeed.EmulateChangeText(parameter.Speed.ToString("0.00"));
                    logger.Debug("話速をセットしました。ダイアログID:{0},セット値:{1}", tbSpeed.DialogId, tbSpeed.GetWindowText());

                    // 高さ
                    FormsTextBox tbTone = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Tone[0],
                        (int)this._zindexInfo.Tone[1],
                        (int)this._zindexInfo.Tone[2],
                        (int)this._zindexInfo.Tone[3],
                        (int)this._zindexInfo.Tone[4],
                        (int)this._zindexInfo.Tone[5],
                        (int)this._zindexInfo.Tone[6],
                        (int)this._zindexInfo.Tone[7]));
                    tbTone.EmulateChangeText(parameter.Tone.ToString("0.00"));
                    logger.Debug("高さをセットしました。ダイアログID:{0},セット値:{1}", tbTone.DialogId, tbTone.GetWindowText());

                    // 抑揚
                    FormsTextBox tbIntonation = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Intonation[0],
                        (int)this._zindexInfo.Intonation[1],
                        (int)this._zindexInfo.Intonation[2],
                        (int)this._zindexInfo.Intonation[3],
                        (int)this._zindexInfo.Intonation[4],
                        (int)this._zindexInfo.Intonation[5],
                        (int)this._zindexInfo.Intonation[6],
                        (int)this._zindexInfo.Intonation[7]));
                    tbIntonation.EmulateChangeText(parameter.Intonation.ToString("0.00"));
                    logger.Debug("抑揚をセットしました。ダイアログID:{0},セット値:{1}", tbIntonation.DialogId, tbIntonation.GetWindowText());
                }
                else
                {
                    returnVal = -1;
                    logger.Error("SendParameterでエラーが発生しました。メッセージ：VOICEROIDが起動していません。");
                }

                returnVal = 0;
            }
            catch (Exception ex)
            {
                returnVal = -1;
                logger.Error(ex, "SendParameterでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// VOICEROIDへ読み上げる文字列をセットする処理
        /// </summary>
        /// <param name="parameter">ボイスパラメーター</param>
        /// <param name="row">読み上げ対象行番</param>
        /// <returns></returns>
        private int SendText(Models.VoiceParameter parameter, uint row)
        {
            int returnVal = -1;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (this._app is not null)
                {
                    // 操作対象ウィンドウ
                    var targetWindows = WindowControl.FromZTop(this._app);
                    logger.Debug("操作対象Windowを取得しました。ハンドルID:{0}", targetWindows.Handle);

                    // テキストボックス
                    FormsRichTextBox tbText = new FormsRichTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.TextBot[0],
                        (int)this._zindexInfo.TextBot[1],
                        (int)this._zindexInfo.TextBot[2],
                        (int)this._zindexInfo.TextBot[3],
                        (int)this._zindexInfo.TextBot[4],
                        (int)this._zindexInfo.TextBot[5],
                        (int)this._zindexInfo.TextBot[6],
                        (int)this._zindexInfo.TextBot[7]));
                    tbText.EmulateChangeText(parameter.Texts[(int)row]);
                    logger.Debug("テキストをセットしました。ダイアログID:{0},セット値:{1}", tbText.DialogId, tbText.GetWindowText());
                }
                else
                {
                    returnVal = -1;
                    logger.Error("SendTextでエラーが発生しました。メッセージ：VOICEROIDが起動していません。");
                }

                returnVal = 0;
            }
            catch (Exception ex)
            {
                returnVal = -1;
                logger.Error(ex, "SendTextでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// VOICEROIDのパラメーターを取得する処理
        /// </summary>
        /// <returns></returns>
        private int GetVoiceParameter()
        {
            int returnVal = -1;
            logger.Debug("==============================  Start   ==============================");

            try
            {
                if (this._app is not null)
                {
                    float tmpFloatVal = 0.0f;
                    string tmpString = "";
                    // 操作対象ウィンドウ
                    var targetWindows = WindowControl.FromZTop(this._app);
                    logger.Debug("操作対象Windowを取得しました。ハンドルID:{0}", targetWindows.Handle);

                    // テキストボックス
                    FormsRichTextBox tbText = new FormsRichTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.TextBot[0],
                        (int)this._zindexInfo.TextBot[1],
                        (int)this._zindexInfo.TextBot[2],
                        (int)this._zindexInfo.TextBot[3],
                        (int)this._zindexInfo.TextBot[4],
                        (int)this._zindexInfo.TextBot[5],
                        (int)this._zindexInfo.TextBot[6],
                        (int)this._zindexInfo.TextBot[7]));
                    this._backupVoiceParameter.Texts = new List<string>();
                    this._backupVoiceParameter.Texts = tbText.GetWindowText().Split(new char[] { '\n' }).ToList();
                    logger.Debug("テキストを読み取りしました。ダイアログID:{0},読み取り値:{1}", tbText.DialogId, tbText.GetWindowText());

                    // 音声効果タブ
                    FormsTabControl targetTab = new FormsTabControl(targetWindows.IdentifyFromZIndex(
                       (int)this._zindexInfo.VoiceParameterTab[0],
                       (int)this._zindexInfo.VoiceParameterTab[1],
                       (int)this._zindexInfo.VoiceParameterTab[2],
                       (int)this._zindexInfo.VoiceParameterTab[3],
                       (int)this._zindexInfo.VoiceParameterTab[4]));
                    targetTab.EmulateTabSelect(2);
                    logger.Debug("音声効果タブに切り替えました。ダイアログID:{0}", targetTab.DialogId);

                    // 音量
                    FormsTextBox tbVolume = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Volume[0],
                        (int)this._zindexInfo.Volume[1],
                        (int)this._zindexInfo.Volume[2],
                        (int)this._zindexInfo.Volume[3],
                        (int)this._zindexInfo.Volume[4],
                        (int)this._zindexInfo.Volume[5],
                        (int)this._zindexInfo.Volume[6],
                        (int)this._zindexInfo.Volume[7]));
                    if (float.TryParse(tbVolume.GetWindowText(), out tmpFloatVal))
                    {
                        this._backupVoiceParameter.Volume = tmpFloatVal;
                    }
                    else
                    {
                        this._backupVoiceParameter.Volume = 1.0f;
                        logger.Warn("音量の値が数値ではありません。初期値(1.0)を使用します。");
                    }
                    logger.Debug("音量を読み取りしました。ダイアログID:{0},読み取り値:{1}", tbVolume.DialogId, this._backupVoiceParameter.Volume);

                    // 話速
                    FormsTextBox tbSpeed = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Speed[0],
                        (int)this._zindexInfo.Speed[1],
                        (int)this._zindexInfo.Speed[2],
                        (int)this._zindexInfo.Speed[3],
                        (int)this._zindexInfo.Speed[4],
                        (int)this._zindexInfo.Speed[5],
                        (int)this._zindexInfo.Speed[6],
                        (int)this._zindexInfo.Speed[7]));
                    if (float.TryParse(tbSpeed.GetWindowText(), out tmpFloatVal))
                    {
                        this._backupVoiceParameter.Speed = tmpFloatVal;
                    }
                    else
                    {
                        this._backupVoiceParameter.Speed = 1.0f;
                        logger.Warn("話速の値が数値ではありません。初期値(1.0)を使用します。");
                    }
                    logger.Debug("話速を読み取りしました。ダイアログID:{0},読み取り値:{1}", tbSpeed.DialogId, this._backupVoiceParameter.Speed);

                    // 高さ
                    FormsTextBox tbTone = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Tone[0],
                        (int)this._zindexInfo.Tone[1],
                        (int)this._zindexInfo.Tone[2],
                        (int)this._zindexInfo.Tone[3],
                        (int)this._zindexInfo.Tone[4],
                        (int)this._zindexInfo.Tone[5],
                        (int)this._zindexInfo.Tone[6],
                        (int)this._zindexInfo.Tone[7]));
                    if (float.TryParse(tbTone.GetWindowText(), out tmpFloatVal))
                    {
                        this._backupVoiceParameter.Tone = tmpFloatVal;
                    }
                    else
                    {
                        this._backupVoiceParameter.Tone = 1.0f;
                        logger.Warn("高さの値が数値ではありません。初期値(1.0)を使用します。");
                    }
                    logger.Debug("高さを読み取りしました。ダイアログID:{0},読み取り値:{1}", tbTone.DialogId, this._backupVoiceParameter.Tone);

                    // 抑揚
                    FormsTextBox tbIntonation = new FormsTextBox(targetWindows.IdentifyFromZIndex(
                        (int)this._zindexInfo.Intonation[0],
                        (int)this._zindexInfo.Intonation[1],
                        (int)this._zindexInfo.Intonation[2],
                        (int)this._zindexInfo.Intonation[3],
                        (int)this._zindexInfo.Intonation[4],
                        (int)this._zindexInfo.Intonation[5],
                        (int)this._zindexInfo.Intonation[6],
                        (int)this._zindexInfo.Intonation[7]));
                    if (float.TryParse(tbIntonation.GetWindowText(), out tmpFloatVal))
                    {
                        this._backupVoiceParameter.Intonation = tmpFloatVal;
                    }
                    else
                    {
                        this._backupVoiceParameter.Intonation = 1.0f;
                        logger.Warn("抑揚の値が数値ではありません。初期値(1.0)を使用します。");
                    }
                    logger.Debug("抑揚を読み取りしました。ダイアログID:{0},読み取り値:{1}", tbIntonation.DialogId, this._backupVoiceParameter.Intonation);
                }
                else
                {
                    returnVal = -1;
                    logger.Error("SendParameterでエラーが発生しました。メッセージ：VOICEROIDが起動していません。");
                }

                returnVal = 0;
            }
            catch (Exception ex)
            {
                returnVal = -1;
                logger.Error(ex, "GetVoiceParameterでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
            return returnVal;
        }

        /// <summary>
        /// デスコンストラクター
        /// </summary>
        public void Dispose()
        {
            logger.Trace("==============================  Start   ==============================");

            if (this._app != null)
            {
                this._app.Dispose();
                this._app = null;
                logger.Trace("リモートコントロール用オブジェクトを解放しました。");
            }
            else
            {
                logger.Trace("リモートコントロール用オブジェクトはすでにNULLです。");
            }

            GC.Collect();
            GC.SuppressFinalize(this);

            logger.Trace("==============================   End    ==============================");
        }

    }
}
