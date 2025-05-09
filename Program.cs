
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
                // 文字介面的排程模式
                RunScheduledTask();
            }
            else
            {
                // GUI 模式
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