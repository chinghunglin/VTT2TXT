
using System.Runtime.CompilerServices;

namespace VTT2TXT
{
    enum DisplayMode
    {
        Console,
        GUI
    }

    internal static class Program
    {
        private static DisplayMode mode = DisplayMode.Console;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // 呼叫轉換程式
                mode = DisplayMode.Console;
                RunScheduledTask(args[0]);
            }
            else
            {
                // GUI 模式
                mode = DisplayMode.GUI;
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        private static void RunScheduledTask(string filepath)
        {
            // 使用類型名稱來呼叫靜態方法
            Vtt2TxtConverter.ProcessSubtitleFile(filepath);
        }
    }
}