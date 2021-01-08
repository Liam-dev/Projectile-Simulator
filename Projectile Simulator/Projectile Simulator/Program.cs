using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projectile_Simulator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);          

            string name = string.Empty;
            if (args != null && args.Length > 0)
            {
                string fileName = args[0];
                if (File.Exists(fileName))
                {
                    //Load file
                }
            }

            Application.Run(new Homepage());
        }
    }
}
