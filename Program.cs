using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check for correct .NET Framework version programmatically instead of using config file
            EnsureCorrectFrameworkVersion();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Ensure only one instance is running
                bool createdNew;
                using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "AutoClickerApplicationMutex", out createdNew))
                {
                    if (createdNew)
                    {
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        MessageBox.Show("AutoClicker is already running.", "AutoClicker", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Ensures that the correct .NET Framework version is installed
        /// This replaces the functionality of the .config file specifying the framework version
        /// </summary>
        private static void EnsureCorrectFrameworkVersion()
        {
            try
            {
                // Check if .NET Framework 4.7.2 or higher is installed
                const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
                using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        int releaseKey = (int)ndpKey.GetValue("Release");
                        // .NET Framework 4.7.2 corresponds to value 461808
                        if (releaseKey < 461808)
                        {
                            MessageBox.Show(
                                "This application requires .NET Framework 4.7.2 or higher.\n" +
                                "Please install the latest .NET Framework from Microsoft's website.",
                                "Framework Version Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        // Could not detect .NET Framework version
                        MessageBox.Show(
                            "Could not detect .NET Framework version.\n" +
                            "This application requires .NET Framework 4.7.2 or higher.",
                            "Framework Detection Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions during the check
                MessageBox.Show(
                    $"Error checking .NET Framework version: {ex.Message}\n" +
                    "This application requires .NET Framework 4.7.2 or higher.",
                    "Framework Check Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
    }
} 