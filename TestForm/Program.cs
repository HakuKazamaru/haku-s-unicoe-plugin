using HakuVoiceNarratorLibrary;
using NLog;

namespace TestFoem
{
    internal static class Program
    {
        /// <summary>
        /// ê›íËì«çû
        /// </summary>
        public static Setting config = new Setting();

        /// <summary>
        /// NLogÉçÉKÅ[
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // NLogÇÃê›íËîΩâf
            NLog.LogManager.Configuration = config.GetNLogSetting();

            logger.Info("==============================  Start   ==============================");
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            logger.Info("==============================   End    ==============================");
        }
    }
}