using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projectile_Simulator.UserInterface;

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

            if (args != null && args.Length > 0)
            {
                string fileName = args[0];
                if (File.Exists(fileName))
                {
                    Application.Run(new Editor(fileName));
                }
            }
            else
            {
                Application.Run(new Homepage());
            }  
        }
    }
}
