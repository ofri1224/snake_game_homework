using System;
using System.Windows.Forms;

namespace SnakeGamePlatform
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Board b = new Board();
            Application.Run(b.GetBoard());
        }
    }
}
