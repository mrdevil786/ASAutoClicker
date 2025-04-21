using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Principal;

namespace AutoClicker
{
    /// <summary>
    /// Main program class that handles application startup and instance management
    /// </summary>
    static class Program
    {
        // Win32 API imports for window handling
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;

        // Mutex name for single instance check - include user ID to allow multiple users to run the app
        private static readonly string ApplicationMutexName = $"AutoClickerApplicationMutex-{GetCurrentUserSid()}";

        // Timer to retry finding and activating existing instance 
        private static System.Threading.Timer retryTimer;
        private static int retryCount = 0;
        private static readonly int MaxRetries = 3;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable better-looking, properly scaled controls and visual styles
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Check for correct .NET Framework version programmatically
            if (!EnsureCorrectFrameworkVersion())
            {
                return; // Exit if framework version is insufficient
            }

            // Set up global exception handler
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            try
            {
                bool createdNew;
                Mutex mutex = null;
                
                try
                {
                    // Try to create or open the mutex
                    mutex = new Mutex(true, ApplicationMutexName, out createdNew);
                }
                catch (UnauthorizedAccessException)
                {
                    // If access denied (can happen in some security configurations), use a simplified name
                    ApplicationMutexName = "AutoClickerApplicationMutex-Simplified";
                    mutex = new Mutex(true, ApplicationMutexName, out createdNew);
                }
                
                if (createdNew)
                {
                    // First instance - run normally
                    using (mutex)
                    {
                        Application.Run(new MainForm());
                    }
                }
                else
                {
                    // Release mutex immediately if we're not the first instance
                    if (mutex != null)
                    {
                        mutex.Close();
                        mutex = null;
                    }
                    
                    // Try to activate the existing instance with retry mechanism
                    retryCount = 0;
                    if (!TryActivateExistingInstance())
                    {
                        // If immediate activation fails, try a few more times on a timer
                        // This helps when the other instance is starting up or minimized to tray
                        retryTimer = new System.Threading.Timer(
                            RetryActivation, 
                            null, 
                            500, // Start after 500ms 
                            500  // Try every 500ms
                        );
                        
                        // Wait for retry attempts to complete
                        Thread.Sleep(MaxRetries * 500 + 100);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors during startup
                MessageBox.Show($"An error occurred during startup: {ex.Message}\n\n{ex.StackTrace}", 
                    "AutoClicker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Retry activation of existing instance on a timer
        /// </summary>
        private static void RetryActivation(object state)
        {
            retryCount++;
            if (TryActivateExistingInstance() || retryCount >= MaxRetries)
            {
                // Success or max retries reached - clean up timer
                retryTimer.Dispose();
                retryTimer = null;
            }
        }

        /// <summary>
        /// Gets the current user's SID as a string to make the mutex name unique per user
        /// </summary>
        private static string GetCurrentUserSid()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                return identity.User.Value;
            }
            catch
            {
                // If we can't get the SID for some reason, use a fallback
                return "DefaultUser";
            }
        }

        /// <summary>
        /// Handles unhandled exceptions in the main UI thread
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }

        /// <summary>
        /// Handles unhandled exceptions in non-UI threads
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// Common handler for all unhandled exceptions
        /// </summary>
        private static void HandleUnhandledException(Exception ex)
        {
            try
            {
                string errorMsg = $"An unexpected error occurred:\n\n{ex.Message}";
                
                // Log to debug output
                Debug.WriteLine($"Unhandled exception: {ex}");
                
                // Show error message to user
                MessageBox.Show(errorMsg, "AutoClicker Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                // If showing the error fails, there's not much we can do
                try
                {
                    MessageBox.Show("A critical error occurred.", 
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    // Last resort - if even a simple message box fails, just give up
                }
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
                            // If the window is minimized, restore it
                            if (IsIconic(mainWindowHandle))
                            {
                                ShowWindow(mainWindowHandle, SW_RESTORE);
                            }
                            
                            // Activate the window
                            SetForegroundWindow(mainWindowHandle);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log failure but don't crash
                Debug.WriteLine($"Error activating existing instance: {ex.Message}");
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
                            DialogResult result = MessageBox.Show(
                                "This application requires .NET Framework 4.7.2 or higher.\n\n" +
                                "Would you like to open the download page?",
                                "Framework Version Error",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error);
                                
                            if (result == DialogResult.Yes)
                            {
                                // Open Microsoft's .NET download page
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "https://dotnet.microsoft.com/download/dotnet-framework",
                                    UseShellExecute = true
                                });
                            }
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        // Could not detect .NET Framework version
                        DialogResult result = MessageBox.Show(
                            "Could not detect .NET Framework version.\n" +
                            "This application requires .NET Framework 4.7.2 or higher.\n\n" +
                            "Would you like to open the download page?",
                            "Framework Detection Error",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error);
                            
                        if (result == DialogResult.Yes)
                        {
                            // Open Microsoft's .NET download page
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = "https://dotnet.microsoft.com/download/dotnet-framework",
                                UseShellExecute = true
                            });
                        }
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