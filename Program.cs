using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AutoClicker
{
    static class Program
    {
        // Win32 API imports for window handling
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        // Unique mutex name for this application
        private static string mutexName = "Global\\ASClickerApplication";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check for existing instances
            bool createdNew;
            using (Mutex mutex = new Mutex(true, mutexName, out createdNew))
            {
                if (createdNew)
                {
                    // No existing instance found, run the application
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    // Find existing instance and bring it to foreground
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            // Bring the existing window to the foreground
                            IntPtr hWnd = process.MainWindowHandle;
                            if (hWnd != IntPtr.Zero)
                            {
                                ShowWindow(hWnd, SW_RESTORE);
                                SetForegroundWindow(hWnd);
                            }
                            return; // Exit the new instance
                        }
                    }
                }
            }
        }
    }
} 