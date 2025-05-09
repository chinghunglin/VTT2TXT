
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
                // �I�s�ഫ�{��
                mode = DisplayMode.Console;
                RunScheduledTask(args[0]);
            }
            else
            {
                // GUI �Ҧ�
                mode = DisplayMode.GUI;
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        private static void RunScheduledTask(string filepath)
        {
            // �ϥ������W�٨өI�s�R�A��k
            Vtt2TxtConverter.ProcessSubtitleFile(filepath);
        }
    }
}