namespace AutoClicker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.startStopButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxClickOptions = new System.Windows.Forms.GroupBox();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.radioButtonDuration = new System.Windows.Forms.RadioButton();
            this.numericUpDownClicks = new System.Windows.Forms.NumericUpDown();
            this.radioButtonClicks = new System.Windows.Forms.RadioButton();
            this.radioButtonIndefinite = new System.Windows.Forms.RadioButton();
            this.labelMouseButton = new System.Windows.Forms.Label();
            this.comboBoxMouseButton = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip.SuspendLayout();
            this.groupBoxClickOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClicks)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Click Interval (ms):";
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Location = new System.Drawing.Point(100, 12);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new System.Drawing.Size(100, 20);
            this.intervalTextBox.TabIndex = 1;
            this.intervalTextBox.Text = "100";
            this.intervalTextBox.TextChanged += new System.EventHandler(this.intervalTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 13);
            this.label2.Location = new System.Drawing.Point(12, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hotkey: Ctrl+Shift+A (Toggle On/Off)";
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(100, 45);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 23);
            this.startStopButton.Location = new System.Drawing.Point(100, 75);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 23);
            this.startStopButton.TabIndex = 3;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 128);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(83, 13);
            this.statusLabel.Location = new System.Drawing.Point(12, 265);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(83, 13);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Status: Stopped";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "AutoClicker";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(93, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.Location = new System.Drawing.Point(12, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Manual Control:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 291);
            this.Controls.Add(this.comboBoxMouseButton);
            this.Controls.Add(this.labelMouseButton);
            this.Controls.Add(this.groupBoxClickOptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "AutoClicker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBoxClickOptions.ResumeLayout(false);
            this.groupBoxClickOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClicks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            // 
            // groupBoxClickOptions
            // 
            this.groupBoxClickOptions.Controls.Add(this.numericUpDownDuration);
            this.groupBoxClickOptions.Controls.Add(this.radioButtonDuration);
            this.groupBoxClickOptions.Controls.Add(this.numericUpDownClicks);
            this.groupBoxClickOptions.Controls.Add(this.radioButtonClicks);
            this.groupBoxClickOptions.Controls.Add(this.radioButtonIndefinite);
            this.groupBoxClickOptions.Location = new System.Drawing.Point(15, 105);
            this.groupBoxClickOptions.Name = "groupBoxClickOptions";
            this.groupBoxClickOptions.Size = new System.Drawing.Size(257, 100);
            this.groupBoxClickOptions.TabIndex = 6;
            this.groupBoxClickOptions.TabStop = false;
            this.groupBoxClickOptions.Text = "Click Options";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Enabled = false;
            this.numericUpDownDuration.Location = new System.Drawing.Point(140, 68);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownDuration.TabIndex = 4;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // radioButtonDuration
            // 
            this.radioButtonDuration.AutoSize = true;
            this.radioButtonDuration.Location = new System.Drawing.Point(15, 68);
            this.radioButtonDuration.Name = "radioButtonDuration";
            this.radioButtonDuration.Size = new System.Drawing.Size(107, 17);
            this.radioButtonDuration.TabIndex = 3;
            this.radioButtonDuration.Text = "Duration (secs):";
            this.radioButtonDuration.UseVisualStyleBackColor = true;
            this.radioButtonDuration.CheckedChanged += new System.EventHandler(this.radioButtonClickLimit_CheckedChanged);
            // 
            // numericUpDownClicks
            // 
            this.numericUpDownClicks.Enabled = false;
            this.numericUpDownClicks.Location = new System.Drawing.Point(140, 45);
            this.numericUpDownClicks.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownClicks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownClicks.Name = "numericUpDownClicks";
            this.numericUpDownClicks.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownClicks.TabIndex = 2;
            this.numericUpDownClicks.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // radioButtonClicks
            // 
            this.radioButtonClicks.AutoSize = true;
            this.radioButtonClicks.Location = new System.Drawing.Point(15, 45);
            this.radioButtonClicks.Name = "radioButtonClicks";
            this.radioButtonClicks.Size = new System.Drawing.Size(109, 17);
            this.radioButtonClicks.TabIndex = 1;
            this.radioButtonClicks.Text = "Number of Clicks:";
            this.radioButtonClicks.UseVisualStyleBackColor = true;
            this.radioButtonClicks.CheckedChanged += new System.EventHandler(this.radioButtonClickLimit_CheckedChanged);
            // 
            // radioButtonIndefinite
            // 
            this.radioButtonIndefinite.AutoSize = true;
            this.radioButtonIndefinite.Checked = true;
            this.radioButtonIndefinite.Location = new System.Drawing.Point(15, 22);
            this.radioButtonIndefinite.Name = "radioButtonIndefinite";
            this.radioButtonIndefinite.Size = new System.Drawing.Size(98, 17);
            this.radioButtonIndefinite.TabIndex = 0;
            this.radioButtonIndefinite.TabStop = true;
            this.radioButtonIndefinite.Text = "Click Indefinitely";
            this.radioButtonIndefinite.UseVisualStyleBackColor = true;
            this.radioButtonIndefinite.CheckedChanged += new System.EventHandler(this.radioButtonClickLimit_CheckedChanged);
            // 
            // labelMouseButton
            // 
            this.labelMouseButton.AutoSize = true;
            this.labelMouseButton.Location = new System.Drawing.Point(12, 215);
            this.labelMouseButton.Name = "labelMouseButton";
            this.labelMouseButton.Size = new System.Drawing.Size(76, 13);
            this.labelMouseButton.TabIndex = 7;
            this.labelMouseButton.Text = "Mouse Button:";
            // 
            // comboBoxMouseButton
            // 
            this.comboBoxMouseButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMouseButton.FormattingEnabled = true;
            this.comboBoxMouseButton.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Middle"});
            this.comboBoxMouseButton.Location = new System.Drawing.Point(100, 212);
            this.comboBoxMouseButton.Name = "comboBoxMouseButton";
            this.comboBoxMouseButton.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMouseButton.TabIndex = 8;

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox intervalTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxClickOptions;
        private System.Windows.Forms.RadioButton radioButtonIndefinite;
        private System.Windows.Forms.RadioButton radioButtonClicks;
        private System.Windows.Forms.NumericUpDown numericUpDownClicks;
        private System.Windows.Forms.RadioButton radioButtonDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label labelMouseButton;
        private System.Windows.Forms.ComboBox comboBoxMouseButton;
    }
}