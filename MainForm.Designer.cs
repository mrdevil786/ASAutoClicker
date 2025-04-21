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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dividerPanel = new System.Windows.Forms.Panel();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip.SuspendLayout();
            this.groupBoxClickOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClicks)).BeginInit();
            this.mainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Click Interval (ms):";
            this.toolTip.SetToolTip(this.label1, "Set the time between clicks in milliseconds (1000ms = 1 second)");
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.intervalTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intervalTextBox.Location = new System.Drawing.Point(131, 4);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new System.Drawing.Size(140, 22);
            this.intervalTextBox.TabIndex = 1;
            this.intervalTextBox.Text = "100";
            this.toolTip.SetToolTip(this.intervalTextBox, "Enter a value in milliseconds (1000ms = 1 second)");
            this.intervalTextBox.TextChanged += new System.EventHandler(this.intervalTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.mainTableLayoutPanel.SetColumnSpan(this.label2, 2);
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hotkey: Ctrl+Shift+A (Toggle On/Off)";
            // 
            // startStopButton
            // 
            this.startStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.startStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.startStopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startStopButton.FlatAppearance.BorderSize = 0;
            this.startStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startStopButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startStopButton.ForeColor = System.Drawing.Color.White;
            this.startStopButton.Location = new System.Drawing.Point(131, 36);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(140, 28);
            this.startStopButton.TabIndex = 3;
            this.startStopButton.Text = "Start";
            this.toolTip.SetToolTip(this.startStopButton, "Start or stop the auto clicker");
            this.startStopButton.UseVisualStyleBackColor = false;
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            this.startStopButton.MouseEnter += new System.EventHandler(this.startStopButton_MouseEnter);
            this.startStopButton.MouseLeave += new System.EventHandler(this.startStopButton_MouseLeave);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.mainTableLayoutPanel.SetColumnSpan(this.statusLabel, 2);
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.statusLabel.Location = new System.Drawing.Point(13, 290);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(94, 15);
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
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Manual Control:";
            // 
            // groupBoxClickOptions
            // 
            this.mainTableLayoutPanel.SetColumnSpan(this.groupBoxClickOptions, 2);
            this.groupBoxClickOptions.Controls.Add(this.numericUpDownDuration);
            this.groupBoxClickOptions.Controls.Add(this.radioButtonDuration);
            this.groupBoxClickOptions.Controls.Add(this.numericUpDownClicks);
            this.groupBoxClickOptions.Controls.Add(this.radioButtonClicks);
            this.groupBoxClickOptions.Controls.Add(this.radioButtonIndefinite);
            this.groupBoxClickOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxClickOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBoxClickOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxClickOptions.Location = new System.Drawing.Point(13, 73);
            this.groupBoxClickOptions.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.groupBoxClickOptions.Name = "groupBoxClickOptions";
            this.groupBoxClickOptions.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxClickOptions.Size = new System.Drawing.Size(258, 134);
            this.groupBoxClickOptions.TabIndex = 6;
            this.groupBoxClickOptions.TabStop = false;
            this.groupBoxClickOptions.Text = "Click Options";
            this.toolTip.SetToolTip(this.groupBoxClickOptions, "Configure how long the auto-clicker should run");
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.BackColor = System.Drawing.Color.White;
            this.numericUpDownDuration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownDuration.Enabled = false;
            this.numericUpDownDuration.Location = new System.Drawing.Point(126, 91);
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
            this.numericUpDownDuration.Size = new System.Drawing.Size(100, 23);
            this.numericUpDownDuration.TabIndex = 4;
            this.numericUpDownDuration.Tag = "Duration in seconds for auto-clicking";
            this.numericUpDownDuration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // radioButtonDuration
            // 
            this.radioButtonDuration.AutoSize = true;
            this.radioButtonDuration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonDuration.Location = new System.Drawing.Point(13, 91);
            this.radioButtonDuration.Name = "radioButtonDuration";
            this.radioButtonDuration.Size = new System.Drawing.Size(106, 19);
            this.radioButtonDuration.TabIndex = 3;
            this.radioButtonDuration.Tag = "Click for a specified amount of time";
            this.radioButtonDuration.Text = "Duration (secs):";
            this.radioButtonDuration.UseVisualStyleBackColor = true;
            this.radioButtonDuration.CheckedChanged += new System.EventHandler(this.radioButtonClickLimit_CheckedChanged);
            // 
            // numericUpDownClicks
            // 
            this.numericUpDownClicks.BackColor = System.Drawing.Color.White;
            this.numericUpDownClicks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownClicks.Enabled = false;
            this.numericUpDownClicks.Location = new System.Drawing.Point(126, 56);
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
            this.numericUpDownClicks.Size = new System.Drawing.Size(100, 23);
            this.numericUpDownClicks.TabIndex = 2;
            this.numericUpDownClicks.Tag = "Number of clicks to perform";
            this.numericUpDownClicks.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // radioButtonClicks
            // 
            this.radioButtonClicks.AutoSize = true;
            this.radioButtonClicks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonClicks.Location = new System.Drawing.Point(13, 56);
            this.radioButtonClicks.Name = "radioButtonClicks";
            this.radioButtonClicks.Size = new System.Drawing.Size(114, 19);
            this.radioButtonClicks.TabIndex = 1;
            this.radioButtonClicks.Tag = "Click a specific number of times then stop";
            this.radioButtonClicks.Text = "Number of Clicks:";
            this.radioButtonClicks.UseVisualStyleBackColor = true;
            this.radioButtonClicks.CheckedChanged += new System.EventHandler(this.radioButtonClickLimit_CheckedChanged);
            // 
            // radioButtonIndefinite
            // 
            this.radioButtonIndefinite.AutoSize = true;
            this.radioButtonIndefinite.Checked = true;
            this.radioButtonIndefinite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonIndefinite.Location = new System.Drawing.Point(13, 24);
            this.radioButtonIndefinite.Name = "radioButtonIndefinite";
            this.radioButtonIndefinite.Size = new System.Drawing.Size(107, 19);
            this.radioButtonIndefinite.TabIndex = 0;
            this.radioButtonIndefinite.TabStop = true;
            this.radioButtonIndefinite.Tag = "Keep clicking until manually stopped";
            this.radioButtonIndefinite.Text = "Click Indefinitely";
            this.radioButtonIndefinite.UseVisualStyleBackColor = true;
            this.radioButtonIndefinite.CheckedChanged += new System.EventHandler(this.radioButtonClickLimit_CheckedChanged);
            // 
            // labelMouseButton
            // 
            this.labelMouseButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelMouseButton.AutoSize = true;
            this.labelMouseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMouseButton.Location = new System.Drawing.Point(13, 225);
            this.labelMouseButton.Name = "labelMouseButton";
            this.labelMouseButton.Size = new System.Drawing.Size(85, 15);
            this.labelMouseButton.TabIndex = 7;
            this.labelMouseButton.Text = "Mouse Button:";
            // 
            // comboBoxMouseButton
            // 
            this.comboBoxMouseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMouseButton.BackColor = System.Drawing.Color.White;
            this.comboBoxMouseButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMouseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxMouseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMouseButton.FormattingEnabled = true;
            this.comboBoxMouseButton.Location = new System.Drawing.Point(131, 221);
            this.comboBoxMouseButton.Name = "comboBoxMouseButton";
            this.comboBoxMouseButton.Size = new System.Drawing.Size(140, 23);
            this.comboBoxMouseButton.TabIndex = 8;
            this.toolTip.SetToolTip(this.comboBoxMouseButton, "Select which mouse button to click");
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 800;
            this.toolTip.IsBalloon = false;
            this.toolTip.ShowAlways = true;
            this.toolTip.UseFading = true;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(284, 24);
            this.menuStrip.TabIndex = 9;
            this.menuStrip.Text = "menuStrip";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dividerPanel
            // 
            this.dividerPanel.BackColor = System.Drawing.Color.LightGray;
            this.dividerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dividerPanel.Location = new System.Drawing.Point(0, 24);
            this.dividerPanel.Name = "dividerPanel";
            this.dividerPanel.Size = new System.Drawing.Size(284, 1);
            this.dividerPanel.TabIndex = 10;
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.intervalTextBox, 1, 0);
            this.mainTableLayoutPanel.Controls.Add(this.label3, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.startStopButton, 1, 1);
            this.mainTableLayoutPanel.Controls.Add(this.groupBoxClickOptions, 0, 2);
            this.mainTableLayoutPanel.Controls.Add(this.labelMouseButton, 0, 3);
            this.mainTableLayoutPanel.Controls.Add(this.comboBoxMouseButton, 1, 3);
            this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 4);
            this.mainTableLayoutPanel.Controls.Add(this.statusLabel, 0, 5);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 25);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.mainTableLayoutPanel.RowCount = 6;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(284, 312);
            this.mainTableLayoutPanel.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 337);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Controls.Add(this.dividerPanel);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 375);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoClicker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBoxClickOptions.ResumeLayout(false);
            this.groupBoxClickOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClicks)).EndInit();
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.mainTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel dividerPanel;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
    }
}