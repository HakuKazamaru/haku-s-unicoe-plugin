using HakuVoiceNarratorLibrary;
using HakuVoiceNarratorLibrary.Models;

namespace TestFoem
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// NLog���K�[
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// �C�x���g�n���h���r������t���O
        /// </summary>
        private bool nowProc = false;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.tbSpeakerId.Text = "0";
            this.tbText.Text = "�ǂݏグ����e����͂��Ă��������B";
        }

        /// <summary>
        /// �e�L�X�g�{�b�N�X���X���C�h�o�[�l���f�p����
        /// </summary>
        /// <param name="targetTrackBar">�ϊ��ΏۃR���g���[��</param>
        /// <param name="originalValue">�ϊ��l</param>
        /// <param name="magnification">�{��</param>
        /// <param name="defaultValue">�����l</param>
        private float ValidateTextBoxValue(TrackBar targetTrackBar, ref string originalValue, float magnification, float defaultValue)
        {
            float tmpVal = 0;

            if (float.TryParse(originalValue, out tmpVal))
            {
                tmpVal *= magnification;
                if (targetTrackBar.Maximum < tmpVal)
                {
                    originalValue = (targetTrackBar.Maximum / magnification).ToString("0.0");
                    tmpVal = targetTrackBar.Maximum;
                }
                else if (targetTrackBar.Minimum > tmpVal)
                {
                    originalValue = (targetTrackBar.Minimum / magnification).ToString("0.0");
                    tmpVal = targetTrackBar.Minimum;
                }
                else
                {
                    originalValue = (tmpVal / magnification).ToString("0.0");
                }
            }
            else
            {
                originalValue = defaultValue.ToString("0.0");
                tmpVal = defaultValue * magnification;
            }

            return tmpVal;
        }

        #region �{�^���C�x���g
        /// <summary>
        /// �����Đ��{�^���N���b�N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btText2Speech_Click(object sender, EventArgs e)
        {
            try
            {
                float tmpFloat = 0.0f;
                uint tmpUint = 0;
                VoiceParameter voiceParameter = new VoiceParameter();

                // ����
                if (float.TryParse(this.tbVolume.Text, out tmpFloat))
                    voiceParameter.Volume = tmpFloat;
                else
                    voiceParameter.Volume = 1.0f;

                // �b��
                if (float.TryParse(this.tbSpeed.Text, out tmpFloat))
                    voiceParameter.Speed = tmpFloat;
                else
                    voiceParameter.Speed = 1.0f;

                // ����
                if (float.TryParse(this.tbPitch.Text, out tmpFloat))
                    voiceParameter.Tone = tmpFloat;
                else
                    voiceParameter.Tone = 1.0f;

                // �}�g
                if (float.TryParse(this.tbIntonation.Text, out tmpFloat))
                    voiceParameter.Intonation = tmpFloat;
                else
                    voiceParameter.Intonation = 1.0f;

                // �e�L�X�g
                voiceParameter.Texts = new List<string>();
                if (string.IsNullOrEmpty(this.tbText.Text))
                    throw new Exception("�ǂݏグ��e�L�X�g����͂��Ă��������B");
                voiceParameter.Texts.Add(this.tbText.Text);

                if (!uint.TryParse(this.tbSpeakerId.Text, out tmpUint))
                    throw new Exception("SpeakerID�͐��̐����œ��͂��Ă��������B");

                using (var obgVoiceroid = new Voiceroid((int)tmpUint))
                {
                    await obgVoiceroid.TakeVoiceroid(voiceParameter);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region �{�C�X�p�����[�^�[�֌W

        #region ��������
        #region �{�����[��
        /// <summary>
        /// �{�����[���X���C�h�o�[�l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrVolume_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbVolume.Text = (this.trcbrVolume.Value / 10.0).ToString("0.0");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// �{�����[���l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbVolume_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbVolume.Text;
                this.nowProc = true;
                this.trcbrVolume.Value = (int)ValidateTextBoxValue(this.trcbrVolume, ref tmpVal, 10, 1);
                this.tbVolume.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region �b��
        /// <summary>
        /// �b���X���C�h�o�[�l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrSpeed_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbSpeed.Text = (this.trcbrSpeed.Value / 10.0).ToString("0.0");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// �b���l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSpeed_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbSpeed.Text;
                this.nowProc = true;
                this.trcbrSpeed.Value = (int)ValidateTextBoxValue(this.trcbrSpeed, ref tmpVal, 10, 1);
                this.tbSpeed.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region �s�b�`
        /// <summary>
        /// �s�b�`�X���C�h�o�[�l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrPitch_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbPitch.Text = (this.trcbrPitch.Value / 1000.0).ToString("0.0");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// �s�b�`�l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPitch_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbPitch.Text;
                this.nowProc = true;
                this.trcbrPitch.Value = (int)ValidateTextBoxValue(this.trcbrPitch, ref tmpVal, 1000, 0);
                this.tbPitch.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion

        #region �}�g
        /// <summary>
        /// �}�g�X���C�h�o�[�l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trcbrIntonation_Scroll(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                this.nowProc = true;
                this.tbIntonation.Text = (this.trcbrIntonation.Value / 10.0).ToString("0.0");
                this.nowProc = false;
            }
        }

        /// <summary>
        /// �}�g�l�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIntonation_Validated(object sender, EventArgs e)
        {
            if (!this.nowProc)
            {
                string tmpVal = this.tbIntonation.Text;
                this.nowProc = true;
                this.trcbrIntonation.Value = (int)ValidateTextBoxValue(this.trcbrIntonation, ref tmpVal, 10, 1);
                this.tbIntonation.Text = tmpVal;
                this.nowProc = false;
            }
        }
        #endregion
        #endregion

        #region �|�[�Y�ݒ�
        #region �Ǔ_�|�[�Y
        /// <summary>
        /// �}�g�X���C�h�o�[�l�ύX
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
        /// �}�g�l�ύX
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

        #region ��_�|�[�Y
        /// <summary>
        /// �}�g�X���C�h�o�[�l�ύX
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
        /// �}�g�l�ύX
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