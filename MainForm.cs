using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace AutoClicker
{
    public partial class MainForm : Form
    {
        // Import user32.dll for mouse_event and RegisterHotKey functions
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Constants for mouse_event
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        // Constants for RegisterHotKey
        private const int HOTKEY_ID = 1;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_A = 0x41; // Virtual key code for 'A'

        // For the click thread
        private BackgroundWorker clickWorker;
        private bool isClicking = false;
        private int clickInterval = 100; // Default interval in milliseconds

        public MainForm()
        {
            InitializeComponent();

            // Set the form title with version info
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = "AutoClicker v" + version.Major + "." + version.Minor + "." + version.Build;

            // Explicitly set the form icon
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_icon.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    // Try to find the icon in the application directory
                    iconPath = "app_icon.ico";
                    if (File.Exists(iconPath))
                    {
                        this.Icon = new Icon(iconPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Just log or ignore the error - the default icon will be used
                Console.WriteLine("Error loading icon: " + ex.Message);
            }

            // Set up NotifyIcon
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = true;
            notifyIcon.Text = "AutoClicker";

            // Setup background worker for clicking
            clickWorker = new BackgroundWorker();
            clickWorker.DoWork += ClickWorker_DoWork;
            clickWorker.WorkerSupportsCancellation = true;

            // Set up context menu items
            if (contextMenuStrip.Items.Count == 1) // Only has Exit by default
            {
                ToolStripMenuItem showMenuItem = new ToolStripMenuItem("Show");
                showMenuItem.Click += ShowMenuItem_Click;
                
                // Insert at the beginning
                contextMenuStrip.Items.Insert(0, showMenuItem);
                
                // Add a separator between Show and Exit
                contextMenuStrip.Items.Insert(1, new ToolStripSeparator());
            }

            // Register hotkey (Ctrl+Shift+A)
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_A);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Check if we received a hotkey message
            if (m.Msg == 0x0312) // WM_HOTKEY
            {
                int id = m.WParam.ToInt32();
                
                if (id == HOTKEY_ID)
                {
                    ToggleClicking();
                }
            }
        }

        private void ToggleClicking()
        {
            if (isClicking)
            {
                // Stop clicking
                StopClicking();
            }
            else
            {
                // Start clicking
                StartClicking();
            }
        }

        private void StartClicking()
        {
            if (!isClicking)
            {
                isClicking = true;
                statusLabel.Text = "Status: Running";
                startStopButton.Text = "Stop";
                
                // Show notification
                notifyIcon.ShowBalloonTip(2000, "AutoClicker", "AutoClicker started", ToolTipIcon.Info);
                
                // Start the clicking worker
                if (!clickWorker.IsBusy)
                {
                    clickWorker.RunWorkerAsync();
                }
            }
        }

        private void StopClicking()
        {
            if (isClicking)
            {
                isClicking = false;
                statusLabel.Text = "Status: Stopped";
                startStopButton.Text = "Start";
                
                // Show notification
                notifyIcon.ShowBalloonTip(2000, "AutoClicker", "AutoClicker stopped", ToolTipIcon.Info);
                
                // Cancel the clicking worker
                if (clickWorker.IsBusy)
                {
                    clickWorker.CancelAsync();
                }
            }
        }

        private void ClickWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            while (!worker.CancellationPending)
            {
                // Simulate a left mouse click at the current cursor position
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                
                // Wait for the specified interval
                Thread.Sleep(clickInterval);
            }
            
            e.Cancel = true;
        }

        private void intervalTextBox_TextChanged(object sender, EventArgs e)
        {
            int interval;
            if (int.TryParse(intervalTextBox.Text, out interval) && interval > 0)
            {
                clickInterval = interval;
            }
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            ToggleClicking();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }
        
        private void ShowMenuItem_Click(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreFromTray();
        }

        /// <summary>
        /// Restores the application window from system tray
        /// </summary>
        public void RestoreFromTray()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.BringToFront();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Minimize to tray instead of closing
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                
                // Show notification
                notifyIcon.ShowBalloonTip(2000, "AutoClicker", "AutoClicker is still running in the system tray", ToolTipIcon.Info);
            }
            else
            {
                // Actually close the application
                CloseApplication();
            }
        }

        private void CloseApplication()
        {
            // Stop the clicker if running
            if (isClicking)
            {
                StopClicking();
            }
            
            // Unregister hotkey
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            
            // Remove tray icon
            notifyIcon.Visible = false;
            
            // Close the application
            Application.Exit();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Minimize to tray
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.ShowBalloonTip(2000, "AutoClicker", "AutoClicker is now running in the system tray", ToolTipIcon.Info);
            }
        }
    }
} 