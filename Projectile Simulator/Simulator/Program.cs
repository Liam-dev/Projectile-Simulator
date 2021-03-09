using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simulator.UserInterface;

namespace Simulator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        ///  If arguments contain a filename, then an Editor will open the specified file.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
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
                Application.Run(new Homepage());
            }  
        }
    }
}
