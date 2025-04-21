using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics; // For Stopwatch
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
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        // Constants for RegisterHotKey
        private const int HOTKEY_ID = 1;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_A = 0x41; // Virtual key code for 'A'

        // For the click thread
        private BackgroundWorker clickWorker;
        private volatile bool isClicking = false;
        private int clickInterval = 100; // Default interval in milliseconds
        private Random random = new Random(); // Adding randomness to appear less bot-like

        // Advanced options fields
        private enum ClickLimitType { Indefinite, Count, Duration }
        private ClickLimitType currentClickLimitType = ClickLimitType.Indefinite;
        private decimal clickLimitCount = 0;
        private decimal clickLimitDuration = 0; // in seconds
        private long currentClickCount = 0;
        private Stopwatch durationStopwatch = new Stopwatch();
        private MouseButtons selectedMouseButton = MouseButtons.Left;
        
        // Thread synchronization
        private readonly object lockObject = new object();

        public MainForm()
        {
            InitializeComponent();

            // Initialize advanced options UI
            comboBoxMouseButton.Items.AddRange(Enum.GetNames(typeof(MouseButtons)));
            comboBoxMouseButton.SelectedItem = MouseButtons.Left.ToString();
            radioButtonIndefinite.Checked = true;

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
                Debug.WriteLine("Error loading icon: " + ex.Message);
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
            if (!RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_A))
            {
                MessageBox.Show("Could not register the hotkey (Ctrl+Shift+A). It may be in use by another application.",
                    "Hotkey Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        // Event handler for radio buttons changing click limit type
        private void radioButtonClickLimit_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked)
            {
                if (rb == radioButtonIndefinite)
                {
                    currentClickLimitType = ClickLimitType.Indefinite;
                    numericUpDownClicks.Enabled = false;
                    numericUpDownDuration.Enabled = false;
                }
                else if (rb == radioButtonClicks)
                {
                    currentClickLimitType = ClickLimitType.Count;
                    numericUpDownClicks.Enabled = true;
                    numericUpDownDuration.Enabled = false;
                }
                else if (rb == radioButtonDuration)
                {
                    currentClickLimitType = ClickLimitType.Duration;
                    numericUpDownClicks.Enabled = false;
                    numericUpDownDuration.Enabled = true;
                }
            }
        }

        // Event handler for mouse button selection
        private void comboBoxMouseButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse<MouseButtons>(comboBoxMouseButton.SelectedItem.ToString(), out MouseButtons button))
            {
                selectedMouseButton = button;
            }
        }

        private void StartClicking()
        {
            if (!isClicking)
            {
                // Read click limit settings
                clickLimitCount = numericUpDownClicks.Value;
                clickLimitDuration = numericUpDownDuration.Value;
                
                // Reset counters
                currentClickCount = 0;
                durationStopwatch.Reset();
                durationStopwatch.Start();
                
                // Update UI
                isClicking = true;
                UpdateStatus("Running");
                startStopButton.Text = "Stop";

                // Start the click worker if it's not already running
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
                lock (lockObject)
                {
                    isClicking = false;
                    durationStopwatch.Stop();
                    UpdateStatus($"Stopped (Performed {currentClickCount} clicks)");
                    startStopButton.Text = "Start";

                    // Cancel the click worker
                    if (clickWorker.IsBusy && clickWorker.WorkerSupportsCancellation)
                    {
                        clickWorker.CancelAsync();
                    }
                }
            }
        }

        private void UpdateStatus(string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateStatus), status);
                return;
            }
            
            statusLabel.Text = $"Status: {status}";
        }

        private void ClickWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            while (!worker.CancellationPending && isClicking)
            {
                // Perform the click
                PerformClick();

                // Increment click count
                Interlocked.Increment(ref currentClickCount);

                // Check click limit
                if (currentClickLimitType == ClickLimitType.Count && currentClickCount >= (long)clickLimitCount)
                {
                    this.BeginInvoke(new Action(StopClicking));
                    break;
                }
                else if (currentClickLimitType == ClickLimitType.Duration && 
                         durationStopwatch.ElapsedMilliseconds >= (long)(clickLimitDuration * 1000))
                {
                    this.BeginInvoke(new Action(StopClicking));
                    break;
                }

                // Apply randomness to interval if enabled (to avoid detection as a bot)
                int currentInterval = clickInterval;
                
                // Introduce up to 10% variance in click timing
                // int intervalVariance = random.Next(-clickInterval / 10, clickInterval / 10);
                // currentInterval = Math.Max(10, clickInterval + intervalVariance);
                
                // Sleep for the interval
                Thread.Sleep(currentInterval);
            }
        }

        private void PerformClick()
        {
            try
            {
                // Create input structure
                INPUT[] inputs = new INPUT[2];
                inputs[0].type = INPUT_MOUSE;
                inputs[0].mi.dx = 0;
                inputs[0].mi.dy = 0;
                inputs[0].mi.mouseData = 0;
                inputs[0].mi.time = 0;
                inputs[0].mi.dwExtraInfo = IntPtr.Zero;
                
                inputs[1] = inputs[0]; // Copy the first input struct

                // Set appropriate mouse event flags based on the selected button
                switch (selectedMouseButton)
                {
                    case MouseButtons.Left:
                        inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                        inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
                        break;
                    case MouseButtons.Right:
                        inputs[0].mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
                        inputs[1].mi.dwFlags = MOUSEEVENTF_RIGHTUP;
                        break;
                    case MouseButtons.Middle:
                        inputs[0].mi.dwFlags = MOUSEEVENTF_MIDDLEDOWN;
                        inputs[1].mi.dwFlags = MOUSEEVENTF_MIDDLEUP;
                        break;
                    default:
                        inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                        inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
                        break;
                }

                // Send the down and up inputs
                SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error performing click: {ex.Message}");
            }
        }

        private void intervalTextBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(intervalTextBox.Text, out int interval) && interval > 0)
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Hide to tray when minimized
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
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
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up resources
                if (clickWorker != null && clickWorker.IsBusy)
                {
                    clickWorker.CancelAsync();
                    // Give a short time for worker to complete
                    Thread.Sleep(100);
                }
                
                // Properly dispose of the timer
                if (durationStopwatch != null && durationStopwatch.IsRunning)
                {
                    durationStopwatch.Stop();
                }
                
                // Unregister hotkey to be safe
                try
                {
                    UnregisterHotKey(this.Handle, HOTKEY_ID);
                }
                catch
                {
                    // Ignore errors during cleanup
                }
            }
            
            base.Dispose(disposing);
        }
    }
}