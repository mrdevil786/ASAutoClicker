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
        // Import the SendInput function from user32.dll (more modern approach than mouse_event)
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Define input structures for SendInput
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        }

        // Constants for SendInput
        private const uint INPUT_MOUSE = 0;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        // Constants for RegisterHotKey
        private const int HOTKEY_ID = 1;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_A = 0x41; // Virtual key code for 'A'

        // For the click thread
        private BackgroundWorker clickWorker;
        private bool isClicking = false;
        private int clickInterval = 100; // Default interval in milliseconds
        private Random random = new Random(); // Adding randomness to appear less bot-like

        public MainForm()
        {
            InitializeComponent();

            // Set the form title with version info
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = "AutoClicker v" + version.Major + "." + version.Minor + "." + version.Build;

            // Use the embedded icon
            try 
            {
                // Use the application icon already set in the project properties
                this.Icon = Properties.Resources.AppIcon ?? SystemIcons.Application;
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
                // Simulate a left mouse click at the current cursor position using SendInput
                PerformClick();
                
                // Wait for the specified interval with a small random variation to appear less bot-like
                int variableInterval = clickInterval + random.Next(-5, 5);
                if (variableInterval < 10) variableInterval = 10; // Ensure minimum interval
                Thread.Sleep(variableInterval);
            }
            
            e.Cancel = true;
        }

        // Perform mouse click using SendInput API (more modern and less likely to trigger AV)
        private void PerformClick()
        {
            // Create the input for mouse down
            INPUT[] inputs = new INPUT[2];
            
            // Mouse down
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dx = 0;
            inputs[0].mi.dy = 0;
            inputs[0].mi.mouseData = 0;
            inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            inputs[0].mi.time = 0;
            inputs[0].mi.dwExtraInfo = IntPtr.Zero;
            
            // Mouse up
            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dx = 0;
            inputs[1].mi.dy = 0;
            inputs[1].mi.mouseData = 0;
            inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
            inputs[1].mi.time = 0;
            inputs[1].mi.dwExtraInfo = IntPtr.Zero;
            
            // Send the input
            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
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