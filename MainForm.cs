using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics; // For Stopwatch
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Linq;

namespace AutoClicker
{
    public partial class MainForm : Form
    {
        // Import the SendInput function from user32.dll (more modern approach than mouse_event)
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
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
        private readonly BackgroundWorker _clickWorker;
        private volatile bool _isClicking = false;
        private int _clickInterval = 100; // Default interval in milliseconds
        private readonly Random _random = new Random(); // Adding randomness to appear less bot-like

        // Advanced options fields
        private enum ClickLimitType { Indefinite, Count, Duration }
        private ClickLimitType _currentClickLimitType = ClickLimitType.Indefinite;
        private decimal _clickLimitCount = 0;
        private decimal _clickLimitDuration = 0; // in seconds
        private long _currentClickCount = 0;
        private readonly Stopwatch _durationStopwatch = new Stopwatch();
        private MouseButtons _selectedMouseButton = MouseButtons.Left;
        
        // Thread synchronization
        private readonly object _lockObject = new object();
        
        // Pre-allocated input arrays for efficiency
        private readonly INPUT[] _inputs = new INPUT[2];
        
        // Flag to track if hotkey registration was successful
        private bool _hotkeyRegistered = false;

        public MainForm()
        {
            InitializeComponent();

            // Initialize advanced options UI
            InitializeMouseButtonComboBox();
            radioButtonIndefinite.Checked = true;

            // Set the form title with version info
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            // Display only major.minor.build, ignore revision to match release versioning format
            this.Text = string.Format("AutoClicker v{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            // Initialize notification icon
            InitializeNotifyIcon();

            // Setup background worker for clicking
            _clickWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            _clickWorker.DoWork += ClickWorker_DoWork;

            // Set up context menu items
            SetupContextMenu();

            // Register hotkey (Ctrl+Shift+A)
            RegisterHotkey();
            
            // Pre-initialize input structures
            InitializeInputStructures();
        }
        
        private void InitializeMouseButtonComboBox()
        {
            comboBoxMouseButton.Items.Clear();
            comboBoxMouseButton.Items.Add("Left");
            comboBoxMouseButton.Items.Add("Right");
            comboBoxMouseButton.Items.Add("Middle");
            comboBoxMouseButton.SelectedIndex = 0; // Default to Left button
            comboBoxMouseButton.SelectedIndexChanged += ComboBoxMouseButton_SelectedIndexChanged;
        }
        
        private void InitializeNotifyIcon()
        {
            try 
            {
                // Use the application icon already set in the project properties
                this.Icon = Properties.Resources.AppIcon ?? SystemIcons.Application;
                notifyIcon.Icon = this.Icon;
                notifyIcon.Visible = true;
                notifyIcon.Text = "AutoClicker";
            }
            catch (Exception ex)
            {
                // Just log the error - the default icon will be used
                Debug.WriteLine("Error loading icon: " + ex.Message);
            }
        }
        
        private void SetupContextMenu()
        {
            if (contextMenuStrip.Items.Count == 1) // Only has Exit by default
            {
                ToolStripMenuItem showMenuItem = new ToolStripMenuItem("Show");
                showMenuItem.Click += ShowMenuItem_Click;
                
                // Insert at the beginning
                contextMenuStrip.Items.Insert(0, showMenuItem);
                
                // Add a separator between Show and Exit
                contextMenuStrip.Items.Insert(1, new ToolStripSeparator());
            }
        }
        
        private void RegisterHotkey()
        {
            _hotkeyRegistered = RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_A);
            if (!_hotkeyRegistered)
            {
                int error = Marshal.GetLastWin32Error();
                MessageBox.Show(string.Format("Could not register the hotkey (Ctrl+Shift+A). It may be in use by another application. Error code: {0}",
                    error), "Hotkey Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void InitializeInputStructures()
        {
            // Initialize common properties for both input structures
            _inputs[0].type = INPUT_MOUSE;
            _inputs[0].mi.dx = 0;
            _inputs[0].mi.dy = 0;
            _inputs[0].mi.mouseData = 0;
            _inputs[0].mi.time = 0;
            _inputs[0].mi.dwExtraInfo = IntPtr.Zero;
            
            // Copy to second input
            _inputs[1] = _inputs[0];
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
            if (_isClicking)
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
                    _currentClickLimitType = ClickLimitType.Indefinite;
                    numericUpDownClicks.Enabled = false;
                    numericUpDownDuration.Enabled = false;
                }
                else if (rb == radioButtonClicks)
                {
                    _currentClickLimitType = ClickLimitType.Count;
                    numericUpDownClicks.Enabled = true;
                    numericUpDownDuration.Enabled = false;
                }
                else if (rb == radioButtonDuration)
                {
                    _currentClickLimitType = ClickLimitType.Duration;
                    numericUpDownClicks.Enabled = false;
                    numericUpDownDuration.Enabled = true;
                }
            }
        }

        // Event handler for mouse button selection
        private void ComboBoxMouseButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedButton = comboBoxMouseButton.SelectedItem != null ? comboBoxMouseButton.SelectedItem.ToString() : null;
            switch (selectedButton)
            {
                case "Left":
                    _selectedMouseButton = MouseButtons.Left;
                    break;
                case "Right":
                    _selectedMouseButton = MouseButtons.Right;
                    break;
                case "Middle":
                    _selectedMouseButton = MouseButtons.Middle;
                    break;
                default:
                    _selectedMouseButton = MouseButtons.Left;
                    break;
            }
        }

        private void StartClicking()
        {
            if (!_isClicking)
            {
                try
                {
                    // Read click limit settings
                    _clickLimitCount = numericUpDownClicks.Value;
                    _clickLimitDuration = numericUpDownDuration.Value;
                    
                    // Reset counters
                    _currentClickCount = 0;
                    _durationStopwatch.Reset();
                    _durationStopwatch.Start();
                    
                    // Update UI
                    _isClicking = true;
                    UpdateStatus("Running");
                    startStopButton.Text = "Stop";

                    // Start the click worker if it's not already running
                    if (!_clickWorker.IsBusy)
                    {
                        _clickWorker.RunWorkerAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Error starting clicker: {0}", ex.Message));
                    MessageBox.Show(string.Format("Error starting the auto clicker: {0}", ex.Message), 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    // Make sure we reset to stopped state
                    _isClicking = false;
                    UpdateStatus("Error");
                    startStopButton.Text = "Start";
                }
            }
        }

        private void StopClicking()
        {
            if (_isClicking)
            {
                lock (_lockObject)
                {
                    _isClicking = false;
                    _durationStopwatch.Stop();
                    UpdateStatus(string.Format("Stopped (Performed {0} clicks)", _currentClickCount));
                    startStopButton.Text = "Start";

                    // Cancel the click worker
                    if (_clickWorker.IsBusy && _clickWorker.WorkerSupportsCancellation)
                    {
                        _clickWorker.CancelAsync();
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
            
            statusLabel.Text = string.Format("Status: {0}", status);
        }

        private void ClickWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            while (!worker.CancellationPending && _isClicking)
            {
                try
                {
                    // Perform the click
                    PerformClick();

                    // Increment click count
                    Interlocked.Increment(ref _currentClickCount);

                    // Check click limit
                    if (_currentClickLimitType == ClickLimitType.Count && _currentClickCount >= (long)_clickLimitCount)
                    {
                        this.BeginInvoke(new Action(StopClicking));
                        break;
                    }
                    else if (_currentClickLimitType == ClickLimitType.Duration && 
                            _durationStopwatch.ElapsedMilliseconds >= (long)(_clickLimitDuration * 1000))
                    {
                        this.BeginInvoke(new Action(StopClicking));
                        break;
                    }

                    // Apply randomness to interval if enabled (to avoid detection as a bot)
                    int currentInterval = _clickInterval;
                    
                    // Introduce up to 5% variance in click timing
                    int intervalVariance = _random.Next(-_clickInterval / 20, _clickInterval / 20);
                    currentInterval = Math.Max(10, _clickInterval + intervalVariance);
                    
                    // Sleep for the interval
                    Thread.Sleep(currentInterval);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Error in click worker: {0}", ex.Message));
                    // Continue execution despite errors
                }
            }
        }

        private void PerformClick()
        {
            try
            {
                // Set appropriate mouse event flags based on the selected button
                switch (_selectedMouseButton)
                {
                    case MouseButtons.Left:
                        _inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                        _inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
                        break;
                    case MouseButtons.Right:
                        _inputs[0].mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
                        _inputs[1].mi.dwFlags = MOUSEEVENTF_RIGHTUP;
                        break;
                    case MouseButtons.Middle:
                        _inputs[0].mi.dwFlags = MOUSEEVENTF_MIDDLEDOWN;
                        _inputs[1].mi.dwFlags = MOUSEEVENTF_MIDDLEUP;
                        break;
                    default:
                        _inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
                        _inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
                        break;
                }

                // Send the down and up inputs
                SendInput(2, _inputs, Marshal.SizeOf(typeof(INPUT)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error performing click: {0}", ex.Message));
                throw; // Rethrow to let the worker handle it
            }
        }

        private void intervalTextBox_TextChanged(object sender, EventArgs e)
        {
            int interval;
            if (int.TryParse(intervalTextBox.Text, out interval) && interval > 0)
            {
                _clickInterval = interval;
            }
            else if (!string.IsNullOrWhiteSpace(intervalTextBox.Text))
            {
                // Invalid input - highlight field to indicate an error
                intervalTextBox.BackColor = Color.LightPink;
            }
            else
            {
                // Reset background color if it's empty
                intervalTextBox.BackColor = SystemColors.Window;
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the about form
            using (AboutForm aboutForm = new AboutForm())
            {
                // Set the form icon
                aboutForm.Icon = this.Icon;
                
                // If form has a PictureBox for the icon, set it
                PictureBox iconBox = aboutForm.Controls.Find("pictureBoxIcon", true).FirstOrDefault() as PictureBox;
                if (iconBox != null)
                {
                    iconBox.Image = this.Icon.ToBitmap();
                }
                
                // Show dialog
                aboutForm.ShowDialog(this);
            }
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
            // Stop clicking if active
            if (_isClicking)
            {
                StopClicking();
            }
            
            // Clean up background worker
            if (_clickWorker != null && _clickWorker.IsBusy)
            {
                _clickWorker.CancelAsync();
                Thread.Sleep(50); // Brief pause to allow worker to complete
            }
            
            // Unregister the hotkey if it was registered
            if (_hotkeyRegistered)
            {
                UnregisterHotKey(this.Handle, HOTKEY_ID);
                _hotkeyRegistered = false;
            }
            
            // Remove tray icon
            notifyIcon.Visible = false;
            
            // Close the application
            Application.Exit();
        }
    }
}