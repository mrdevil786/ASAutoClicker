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

        // Mutex name for single instance check
        private static readonly string ApplicationMutexName = "AutoClickerApplicationMutex";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check for correct .NET Framework version programmatically instead of using config file
            if (!EnsureCorrectFrameworkVersion())
            {
                return; // Exit if framework version is insufficient
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Ensure only one instance is running
                bool createdNew;
                using (Mutex mutex = new Mutex(true, ApplicationMutexName, out createdNew))
                {
                    if (createdNew)
                    {
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        // Try to activate the existing instance
                        if (!TryActivateExistingInstance())
                        {
                            MessageBox.Show("AutoClicker is already running.", "AutoClicker", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
        /// Activates an existing instance of the application if one is found
        /// </summary>
        /// <returns>True if successfully activated an existing instance, false otherwise</returns>
        private static bool TryActivateExistingInstance()
        {
            try
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
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                // If there's any error, just return false to show the message
                return false;
            }
        }

        /// <summary>
        /// Ensures that the correct .NET Framework version is installed
        /// This replaces the functionality of the .config file specifying the framework version
        /// </summary>
        /// <returns>True if the framework version is sufficient, false otherwise</returns>
        private static bool EnsureCorrectFrameworkVersion()
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
                            return false;
                        }
                        return true;
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
                        return false;
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
                return false;
            }
        }
    }
}