using HakuVoiceNarratorLibrary;
using HakuVoiceNarratorLibrary.Models;

namespace TestFoem
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// イベントハンドル排他制御フラグ
        /// </summary>
        private bool nowProc = false;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MainForm()
        {
            logger.Debug("==============================  Start   ==============================");

            InitializeComponent();

            this.tbSpeakerId.Text = "0";
            this.tbText.Text = "読み上げる内容を入力してください。";

            logger.Info("フォームを初期化しました。");

            logger.Debug("==============================   End    ==============================");
        }

        /// <summary>
        /// テキストボックス→スライドバー値反映用処理
        /// </summary>
        /// <param name="targetTrackBar">変換対象コントロール</param>
        /// <param name="originalValue">変換値</param>
        /// <param name="magnification">倍率</param>
        /// <param name="defaultValue">初期値</param>
        private float ValidateTextBoxValue(TrackBar targetTrackBar, ref string originalValue, float magnification, float defaultValue)
        {
            float tmpVal = 0;
            logger.Debug("==============================  Start   ==============================");

            logger.Debug("・バリデーション情報");
            logger.Debug("対象スライドバー：{0}", targetTrackBar.Name);
            logger.Debug("チェック文字列　：{0}", originalValue);
            logger.Debug("倍率設定　　　　：{0}", magnification);
            logger.Debug("初期値　　　　　：{0}", defaultValue);

            if (float.TryParse(originalValue, out tmpVal))
            {
                tmpVal *= magnification;
                if (targetTrackBar.Maximum < tmpVal)
                {
                    originalValue = (targetTrackBar.Maximum / magnification).ToString("0.00");
                    tmpVal = targetTrackBar.Maximum;
                }
                else if (targetTrackBar.Minimum > tmpVal)
                {
                    originalValue = (targetTrackBar.Minimum / magnification).ToString("0.00");
                    tmpVal = targetTrackBar.Minimum;
                }
                else
                {
                    originalValue = (tmpVal / magnification).ToString("0.00");
                }
            }
            else
            {
                originalValue = defaultValue.ToString("0.00");
                tmpVal = defaultValue * magnification;
            }

            logger.Debug("==============================   End    ==============================");
            return tmpVal;
        }

        #region ボタンイベント
        /// <summary>
        /// 音声再生ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btText2Speech_Click(object sender, EventArgs e)
        {
            logger.Debug("==============================  Start   ==============================");

            try
            {
                float tmpFloat = 0.0f;
                uint tmpUint = 0;
                VoiceParameter voiceParameter = new VoiceParameter();

                // 音量
                if (float.TryParse(this.tbVolume.Text, out tmpFloat))
                    voiceParameter.Volume = tmpFloat;
                else
                    voiceParameter.Volume = 1.0f;

                // 話速
                if (float.TryParse(this.tbSpeed.Text, out tmpFloat))
                    voiceParameter.Speed = tmpFloat;
                else
                    voiceParameter.Speed = 1.0f;

                // 高さ
                if (float.TryParse(this.tbPitch.Text, out tmpFloat))
                    voiceParameter.Tone = tmpFloat;
                else
                    voiceParameter.Tone = 1.0f;

                // 抑揚
                if (float.TryParse(this.tbIntonation.Text, out tmpFloat))
                    voiceParameter.Intonation = tmpFloat;
                else
                    voiceParameter.Intonation = 1.0f;

                // テキスト
                voiceParameter.Texts = new List<string>();
                if (string.IsNullOrEmpty(this.tbText.Text))
                    throw new Exception("読み上げるテキストを入力してください。");
                voiceParameter.Texts.Add(this.tbText.Text);

                if (!uint.TryParse(this.tbSpeakerId.Text, out tmpUint))
                    throw new Exception("SpeakerIDは正の整数で入力してください。");

                using (var obgVoiceroid = new Voiceroid((int)tmpUint))
                {
                    int returnVal = await obgVoiceroid.TakeVoiceroid(voiceParameter);
                    if (returnVal == 0)
                    {
                        logger.Info("VOICEROIDで音声を再生しました。");
                    }
                    else
                    {
                        string msgString = "VOICEROIDでの音声再生に失敗しました。";
                        MessageBox.Show(msgString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(msgString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex, "btText2Speech_Clickでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
        }

        /// <summary>
        /// 音声保存ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btText2File_Click(object sender, EventArgs e)
        {
            logger.Debug("==============================  Start   ==============================");

            try
            {
                float tmpFloat = 0.0f;
                uint tmpUint = 0;
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "output.wav");
                VoiceParameter voiceParameter = new VoiceParameter();

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    logger.Debug("旧ファイルを削除しました。ファイルパス：{0}", savePath);
                }

                // 音量
                if (float.TryParse(this.tbVolume.Text, out tmpFloat))
                    voiceParameter.Volume = tmpFloat;
                else
                    voiceParameter.Volume = 1.0f;

                // 話速
                if (float.TryParse(this.tbSpeed.Text, out tmpFloat))
                    voiceParameter.Speed = tmpFloat;
                else
                    voiceParameter.Speed = 1.0f;

                // 高さ
                if (float.TryParse(this.tbPitch.Text, out tmpFloat))
                    voiceParameter.Tone = tmpFloat;
                else
                    voiceParameter.Tone = 1.0f;

                // 抑揚
                if (float.TryParse(this.tbIntonation.Text, out tmpFloat))
                    voiceParameter.Intonation = tmpFloat;
                else
                    voiceParameter.Intonation = 1.0f;

                // テキスト
                voiceParameter.Texts = new List<string>();
                if (string.IsNullOrEmpty(this.tbText.Text))
                    throw new Exception("読み上げるテキストを入力してください。");
                voiceParameter.Texts.Add(this.tbText.Text);

                if (!uint.TryParse(this.tbSpeakerId.Text, out tmpUint))
                    throw new Exception("SpeakerIDは正の整数で入力してください。");

                using (var obgVoiceroid = new Voiceroid((int)tmpUint))
                {
                    (int returnVal, int sampleRate, double[] wave) = await obgVoiceroid.SaveVoiceroid(savePath, voiceParameter, 50);
                    if (returnVal == 0)
                    {
                        string msgString = "VOICEROIDで音声を生成しました。";
                        MessageBox.Show(msgString + "\r\n保存先：" + savePath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        logger.Info(msgString);
                    }
                    else
                    {
                        string msgString = "VOICEROIDでの音声生成に失敗しました。";
                        MessageBox.Show(msgString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error(msgString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex, "btText2Speech_Clickでエラーが発生しました。メッセージ：{0}", ex.Message);
            }

            logger.Debug("==============================   End    ==============================");
        }
        #endregion

        #region ボイスパラメーター関係

        #region 音声効果
        #region ボリューム
        /// <summary>
        /// ボリュームスライドバー値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrVolume_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbVolume.Text = (this.trcbrVolume.Value / 100.0).ToString("0.00");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// ボリューム値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbVolume_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbVolume.Text;
                this.nowProc = true;
                this.trcbrVolume.Value = (int)ValidateTextBoxValue(this.trcbrVolume, ref tmpVal, 100, 1);
                this.tbVolume.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region 話速
        /// <summary>
        /// 話速スライドバー値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrSpeed_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbSpeed.Text = (this.trcbrSpeed.Value / 100.0).ToString("0.00");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// 話速値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSpeed_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbSpeed.Text;
                this.nowProc = true;
                this.trcbrSpeed.Value = (int)ValidateTextBoxValue(this.trcbrSpeed, ref tmpVal, 100, 1);
                this.tbSpeed.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region ピッチ
        /// <summary>
        /// ピッチスライドバー値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrPitch_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbPitch.Text = (this.trcbrPitch.Value / 100.0).ToString("0.00");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// ピッチ値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPitch_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbPitch.Text;
                this.nowProc = true;
                this.trcbrPitch.Value = (int)ValidateTextBoxValue(this.trcbrPitch, ref tmpVal, 100, 0);
                this.tbPitch.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region 抑揚
        /// <summary>
        /// 抑揚スライドバー値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrIntonation_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbIntonation.Text = (this.trcbrIntonation.Value / 100.0).ToString("0.00");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// 抑揚値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIntonation_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbIntonation.Text;
                this.nowProc = true;
                this.trcbrIntonation.Value = (int)ValidateTextBoxValue(this.trcbrIntonation, ref tmpVal, 100, 1);
                this.tbIntonation.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion
        #endregion

        #region ポーズ設定
        #region 読点ポーズ
        /// <summary>
        /// 抑揚スライドバー値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrToTen_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbToTen.Text = (this.trcbrToTen.Value / 10.0).ToString("0.0");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// 抑揚値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbToTen_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbToTen.Text;
                this.nowProc = true;
                this.trcbrToTen.Value = (int)ValidateTextBoxValue(this.trcbrToTen, ref tmpVal, 10, (float)0.7);
                this.tbToTen.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region 句点ポーズ
        /// <summary>
        /// 抑揚スライドバー値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrKuTen_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbKuTen.Text = (this.trcbrKuTen.Value / 10.0).ToString("0.0");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// 抑揚値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbKuTen_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbKuTen.Text;
                this.nowProc = true;
                this.trcbrKuTen.Value = (int)ValidateTextBoxValue(this.trcbrKuTen, ref tmpVal, 10, (float)0.4);
                this.tbKuTen.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion
        #endregion

        #endregion

    }
}