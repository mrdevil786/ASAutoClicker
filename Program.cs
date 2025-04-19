using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AutoClicker
{
    /// <summary>
    /// Main program class that handles application startup and instance management
    /// </summary>
    static class Program
    {
        // Win32 API imports for window handling
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        // Unique mutex name for this application
        private static readonly string ApplicationMutexName = "Global\\ASClickerApplication";

        /// <summary>
        /// The main entry point for the application.
        /// Uses a mutex to ensure only one instance runs at a time.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Check for existing instances using a named mutex
                bool createdNew;
                using (Mutex applicationMutex = new Mutex(true, ApplicationMutexName, out createdNew))
                {
                    if (createdNew)
                    {
                        // No existing instance found, run the application
                        RunApplication();
                    }
                    else
                    {
                        // Find existing instance and bring it to foreground
                        ActivateExistingInstance();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors during startup
                MessageBox.Show("An error occurred during startup: " + ex.Message, 
                    "AutoClicker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Runs the main application form
        /// </summary>
        private static void RunApplication()
        {
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Activates an existing instance of the application if one is found
        /// </summary>
        private static void ActivateExistingInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id)
                {
                    // Bring the existing window to the foreground
                    IntPtr mainWindowHandle = process.MainWindowHandle;
                    if (mainWindowHandle != IntPtr.Zero)
                    {
                        ShowWindow(mainWindowHandle, SW_RESTORE);
                        SetForegroundWindow(mainWindowHandle);
                    }
                    return; // Exit the new instance
                }
            }
        }
    }
} 