using HakuVoiceNarratorLibrary.Common;
using NLog;

namespace TestFoem
{
    /// <summary>
    /// ���C���N���X
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// �ݒ�Ǎ�
        /// </summary>
        public static Setting config = new Setting();

        /// <summary>
        /// NLog���K�[
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // NLog�̐ݒ蔽�f
            LogManager.Configuration = config.GetNLogSetting();

            logger.Info("==============================  Start   ==============================");
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            logger.Info("==============================   End    ==============================");
        }
    }
}