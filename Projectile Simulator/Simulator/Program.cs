using Simulator.UserInterface;
using System;
using System.IO;
using System.Windows.Forms;

namespace Simulator
{
      static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        ///  If arguments contain a filename, then an Editor will open the specified file.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0)
            {
                // Gets filename argument
                string filename = args[0];
                if (File.Exists(filename))
                {
                    Application.Run(new Editor(filename, false));
                }
            }
            else
            {
                // If no arguments given, then run Homepage form
                Application.Run(new Homepage());
            }
        }
    }
}