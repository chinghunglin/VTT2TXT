
namespace VTT2TXT
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // ��r�������Ƶ{�Ҧ�
                RunScheduledTask();
            }
            else
            {
                // GUI �Ҧ�
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        private static void RunScheduledTask()
        {
            Console.WriteLine("Hello, world!");
        }
    }
}