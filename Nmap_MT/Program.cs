using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace Nmap_MT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "INSTALLER") {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                FileSystemAccessRule fsar = new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow);
                DirectorySecurity ds = null;
                
                ds = di.GetAccessControl();
                ds.AddAccessRule(fsar);
                di.SetAccessControl(ds);                               
                
                Process.Start(Application.ExecutablePath); 
                return; 
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NmapMT());
        }
    }
}
