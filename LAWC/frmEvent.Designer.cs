namespace LAWC
{
    partial class frmEvent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEvent));
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numCheckSeconds = new System.Windows.Forms.NumericUpDown();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSensor = new System.Windows.Forms.ComboBox();
            this.cbCheckAction = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.cbOverride = new System.Windows.Forms.CheckBox();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogicText = new System.Windows.Forms.Label();
            this.lblCurrentValue = new System.Windows.Forms.Label();
            this.lblClickImage = new System.Windows.Forms.Label();
            this.lblClearImage = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbShowNotification = new System.Windows.Forms.CheckBox();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.pnlFontColour = new System.Windows.Forms.Panel();
            this.lblCheckSecondsWarning = new System.Windows.Forms.Label();
            this.cmCheckSeconds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblDisplayAlways = new System.Windows.Forms.Label();
            this.numOrderPos = new System.Windows.Forms.NumericUpDown();
            this.numTransparent = new System.Windows.Forms.NumericUpDown();
            this.btnAddSpecialText = new System.Windows.Forms.Button();
            this.cmSpecialCharacters = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showTheValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.degreesSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentDateTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wallpaperFilenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wallpaperCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metaDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.titleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRandomColour = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtValue = new System.Windows.Forms.DateTimePicker();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbAvailable = new System.Windows.Forms.CheckBox();
            this.lblValue2 = new System.Windows.Forms.Label();
            this.cmSelectSensor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.category2LevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.cmCheckSeconds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrderPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransparent)).BeginInit();
            this.cmSpecialCharacters.SuspendLayout();
            this.cmSelectSensor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImage.BackColor = System.Drawing.Color.White;
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbImage.Location = new System.Drawing.Point(400, 46);
            this.pbImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(329, 183);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            this.toolTip1.SetToolTip(this.pbImage, "Add an image here which will be displayed when your Event is triggered");
            this.pbImage.Click += new System.EventHandler(this.pbImage_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(113, 15);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.MaxLength = 1000;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(615, 22);
            this.txtMessage.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtMessage, "Enter the message to be displayed when this Event occurs.\r\nYou can use <<Value>> " +
        "to display the current value.\r\n");
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Message";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 181);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Check every";
            // 
            // numCheckSeconds
            // 
            this.numCheckSeconds.Location = new System.Drawing.Point(117, 178);
            this.numCheckSeconds.Margin = new System.Windows.Forms.Padding(4);
            this.numCheckSeconds.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numCheckSeconds.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numCheckSeconds.Name = "numCheckSeconds";
            this.numCheckSeconds.Size = new System.Drawing.Size(73, 22);
            this.numCheckSeconds.TabIndex = 4;
            this.toolTip1.SetToolTip(this.numCheckSeconds, resources.GetString("numCheckSeconds.ToolTip"));
            this.numCheckSeconds.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numCheckSeconds.ValueChanged += new System.EventHandler(this.numCheckSeconds_ValueChanged);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Location = new System.Drawing.Point(629, 414);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(4);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(100, 28);
            this.btnOkay.TabIndex = 5;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(16, 414);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sensor";
            // 
            // cbSensor
            // 
            this.cbSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSensor.FormattingEnabled = true;
            this.cbSensor.Items.AddRange(new object[] {
            "CPUTemp",
            "CPULoad",
            "CPUClockSpeed",
            "MemoryLoad",
            "MemoryUsed",
            "MemoryAvailable",
            "BusSpeed",
            "GPUCoreTemp",
            "GPUCoreClock",
            "GPUMemoryClock",
            "GPUShaderClock",
            "GPUCoreLoad",
            "GPUVideoEngine",
            "GPUMemoryTotal",
            "GPUMemoryUsed",
            "GPUMemoryFree",
            "HDDFreeSpace",
            "HDDUsedSpace",
            "WeatherTemp"});
            this.cbSensor.Location = new System.Drawing.Point(117, 47);
            this.cbSensor.Margin = new System.Windows.Forms.Padding(4);
            this.cbSensor.Name = "cbSensor";
            this.cbSensor.Size = new System.Drawing.Size(259, 24);
            this.cbSensor.TabIndex = 8;
            this.cbSensor.Text = "Please Select One";
            this.toolTip1.SetToolTip(this.cbSensor, "The Sensor that will be checked by this Event");
            this.cbSensor.SelectedIndexChanged += new System.EventHandler(this.cbEventType_SelectedIndexChanged);
            // 
            // cbCheckAction
            // 
            this.cbCheckAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCheckAction.FormattingEnabled = true;
            this.cbCheckAction.Items.AddRange(new object[] {
            "GreaterThan",
            "LessThan",
            "EqualTo",
            "NotEqualTo",
            "DisplayAlways"});
            this.cbCheckAction.Location = new System.Drawing.Point(117, 79);
            this.cbCheckAction.Margin = new System.Windows.Forms.Padding(4);
            this.cbCheckAction.Name = "cbCheckAction";
            this.cbCheckAction.Size = new System.Drawing.Size(259, 24);
            this.cbCheckAction.TabIndex = 9;
            this.cbCheckAction.Text = "Please Select One";
            this.toolTip1.SetToolTip(this.cbCheckAction, "This is the rule used to determine if the Sensor\'s value causes this Event to sho" +
        "w a message / image.");
            this.cbCheckAction.SelectedIndexChanged += new System.EventHandler(this.cbCheckAction_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 82);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Action";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(199, 181);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Seconds";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(16, 213);
            this.lblValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(42, 16);
            this.lblValue.TabIndex = 12;
            this.lblValue.Text = "Value";
            // 
            // numValue
            // 
            this.numValue.DecimalPlaces = 2;
            this.numValue.Location = new System.Drawing.Point(117, 208);
            this.numValue.Margin = new System.Windows.Forms.Padding(4);
            this.numValue.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numValue.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(73, 22);
            this.numValue.TabIndex = 13;
            this.toolTip1.SetToolTip(this.numValue, "The value for the chosen Sensor which will cause the Event to be triggered.");
            this.numValue.ValueChanged += new System.EventHandler(this.numValue_ValueChanged);
            // 
            // cbOverride
            // 
            this.cbOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOverride.AutoSize = true;
            this.cbOverride.Enabled = false;
            this.cbOverride.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbOverride.Location = new System.Drawing.Point(16, 374);
            this.cbOverride.Margin = new System.Windows.Forms.Padding(4);
            this.cbOverride.Name = "cbOverride";
            this.cbOverride.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbOverride.Size = new System.Drawing.Size(262, 20);
            this.cbOverride.TabIndex = 14;
            this.cbOverride.Text = "Override Automatic Wallpaper Changes";
            this.toolTip1.SetToolTip(this.cbOverride, "Do you want this Event to show the image instead of switching wallpapers.");
            this.cbOverride.UseVisualStyleBackColor = true;
            this.cbOverride.CheckedChanged += new System.EventHandler(this.cbOverride_CheckedChanged);
            // 
            // cbEnabled
            // 
            this.cbEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbEnabled.Location = new System.Drawing.Point(654, 374);
            this.cbEnabled.Margin = new System.Windows.Forms.Padding(4);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbEnabled.Size = new System.Drawing.Size(76, 20);
            this.cbEnabled.TabIndex = 15;
            this.cbEnabled.Text = "Enabled";
            this.toolTip1.SetToolTip(this.cbEnabled, "Turn the Event off or on.");
            this.cbEnabled.UseVisualStyleBackColor = true;
            this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
            // 
            // lblLogicText
            // 
            this.lblLogicText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogicText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogicText.Location = new System.Drawing.Point(20, 309);
            this.lblLogicText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogicText.Name = "lblLogicText";
            this.lblLogicText.Size = new System.Drawing.Size(711, 46);
            this.lblLogicText.TabIndex = 16;
            this.lblLogicText.Text = "Checking every 10 seconds:\r\nIf xxx is yyy then zzz";
            this.lblLogicText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblLogicText, "This explains how you have set this Event");
            // 
            // lblCurrentValue
            // 
            this.lblCurrentValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentValue.AutoSize = true;
            this.lblCurrentValue.Location = new System.Drawing.Point(13, 287);
            this.lblCurrentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentValue.Name = "lblCurrentValue";
            this.lblCurrentValue.Size = new System.Drawing.Size(97, 16);
            this.lblCurrentValue.TabIndex = 17;
            this.lblCurrentValue.Text = "Current Value: -";
            // 
            // lblClickImage
            // 
            this.lblClickImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClickImage.AutoSize = true;
            this.lblClickImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClickImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClickImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClickImage.Location = new System.Drawing.Point(460, 84);
            this.lblClickImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClickImage.Name = "lblClickImage";
            this.lblClickImage.Padding = new System.Windows.Forms.Padding(4);
            this.lblClickImage.Size = new System.Drawing.Size(204, 27);
            this.lblClickImage.TabIndex = 18;
            this.lblClickImage.Text = "Click here to add a Wallpaper";
            this.toolTip1.SetToolTip(this.lblClickImage, "Add an image here which will be displayed when your Event is triggered");
            this.lblClickImage.Click += new System.EventHandler(this.lblClickImage_Click);
            // 
            // lblClearImage
            // 
            this.lblClearImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClearImage.AutoSize = true;
            this.lblClearImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblClearImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClearImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClearImage.ForeColor = System.Drawing.Color.White;
            this.lblClearImage.Location = new System.Drawing.Point(699, 46);
            this.lblClearImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClearImage.Name = "lblClearImage";
            this.lblClearImage.Padding = new System.Windows.Forms.Padding(4);
            this.lblClearImage.Size = new System.Drawing.Size(28, 28);
            this.lblClearImage.TabIndex = 19;
            this.lblClearImage.Tag = "Clear the current image.";
            this.lblClearImage.Text = "X";
            this.lblClearImage.Click += new System.EventHandler(this.lblClearImage_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // cbShowNotification
            // 
            this.cbShowNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowNotification.AutoSize = true;
            this.cbShowNotification.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbShowNotification.Location = new System.Drawing.Point(433, 374);
            this.cbShowNotification.Margin = new System.Windows.Forms.Padding(4);
            this.cbShowNotification.Name = "cbShowNotification";
            this.cbShowNotification.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbShowNotification.Size = new System.Drawing.Size(127, 20);
            this.cbShowNotification.TabIndex = 20;
            this.cbShowNotification.Text = "Show Notification";
            this.toolTip1.SetToolTip(this.cbShowNotification, "Show a notification when the Event ocurs.");
            this.cbShowNotification.UseVisualStyleBackColor = true;
            this.cbShowNotification.CheckedChanged += new System.EventHandler(this.cbShowNotification_CheckedChanged);
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(117, 146);
            this.numFontSize.Margin = new System.Windows.Forms.Padding(4);
            this.numFontSize.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numFontSize.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(73, 22);
            this.numFontSize.TabIndex = 22;
            this.toolTip1.SetToolTip(this.numFontSize, "What font size should the text be for this Event");
            this.numFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numFontSize.ValueChanged += new System.EventHandler(this.numFontSize_ValueChanged);
            // 
            // pnlFontColour
            // 
            this.pnlFontColour.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlFontColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFontColour.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlFontColour.Location = new System.Drawing.Point(323, 116);
            this.pnlFontColour.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFontColour.Name = "pnlFontColour";
            this.pnlFontColour.Size = new System.Drawing.Size(54, 24);
            this.pnlFontColour.TabIndex = 188;
            this.toolTip1.SetToolTip(this.pnlFontColour, "The background colour that is used Behind the wallpaper images.\r\n\r\nIt is suggeste" +
        "d that you use a dark blue or green (or other dark toned) image as this will ass" +
        "ist in dimming the image.");
            // 
            // lblCheckSecondsWarning
            // 
            this.lblCheckSecondsWarning.AutoSize = true;
            this.lblCheckSecondsWarning.ContextMenuStrip = this.cmCheckSeconds;
            this.lblCheckSecondsWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckSecondsWarning.ForeColor = System.Drawing.Color.Red;
            this.lblCheckSecondsWarning.Location = new System.Drawing.Point(264, 177);
            this.lblCheckSecondsWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCheckSecondsWarning.Name = "lblCheckSecondsWarning";
            this.lblCheckSecondsWarning.Size = new System.Drawing.Size(19, 25);
            this.lblCheckSecondsWarning.TabIndex = 189;
            this.lblCheckSecondsWarning.Text = "!";
            this.toolTip1.SetToolTip(this.lblCheckSecondsWarning, "Be careful when telling the Event to check too often, as it can slow down older m" +
        "achines");
            this.lblCheckSecondsWarning.Visible = false;
            this.lblCheckSecondsWarning.Click += new System.EventHandler(this.lblCheckSecondsWarning_Click);
            // 
            // cmCheckSeconds
            // 
            this.cmCheckSeconds.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmCheckSeconds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem});
            this.cmCheckSeconds.Name = "cmCheckSeconds";
            this.cmCheckSeconds.Size = new System.Drawing.Size(601, 28);
            // 
            // beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem
            // 
            this.beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem.Name = "beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuIte" +
    "m";
            this.beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem.Size = new System.Drawing.Size(600, 24);
            this.beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem.Text = "Be aware that setting this too quickly can cause slower performance on your pc";
            // 
            // lblDisplayAlways
            // 
            this.lblDisplayAlways.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisplayAlways.AutoSize = true;
            this.lblDisplayAlways.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDisplayAlways.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDisplayAlways.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayAlways.Location = new System.Drawing.Point(452, 107);
            this.lblDisplayAlways.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDisplayAlways.Name = "lblDisplayAlways";
            this.lblDisplayAlways.Padding = new System.Windows.Forms.Padding(4);
            this.lblDisplayAlways.Size = new System.Drawing.Size(220, 44);
            this.lblDisplayAlways.TabIndex = 193;
            this.lblDisplayAlways.Text = "Event Images are not displayed \r\nwhen Display Always is selected";
            this.lblDisplayAlways.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblDisplayAlways, "Add an image here which will be displayed when your Event is triggered");
            this.lblDisplayAlways.Visible = false;
            // 
            // numOrderPos
            // 
            this.numOrderPos.Location = new System.Drawing.Point(117, 114);
            this.numOrderPos.Margin = new System.Windows.Forms.Padding(4);
            this.numOrderPos.Name = "numOrderPos";
            this.numOrderPos.Size = new System.Drawing.Size(73, 22);
            this.numOrderPos.TabIndex = 195;
            this.toolTip1.SetToolTip(this.numOrderPos, "What position should this Event be in the list");
            // 
            // numTransparent
            // 
            this.numTransparent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTransparent.Location = new System.Drawing.Point(323, 146);
            this.numTransparent.Margin = new System.Windows.Forms.Padding(4);
            this.numTransparent.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.numTransparent.Name = "numTransparent";
            this.numTransparent.Size = new System.Drawing.Size(55, 22);
            this.numTransparent.TabIndex = 196;
            this.toolTip1.SetToolTip(this.numTransparent, "What position should this Event be in the list");
            this.numTransparent.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numTransparent.ValueChanged += new System.EventHandler(this.numTransparent_ValueChanged);
            // 
            // btnAddSpecialText
            // 
            this.btnAddSpecialText.ContextMenuStrip = this.cmSpecialCharacters;
            this.btnAddSpecialText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSpecialText.Location = new System.Drawing.Point(81, 12);
            this.btnAddSpecialText.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddSpecialText.Name = "btnAddSpecialText";
            this.btnAddSpecialText.Size = new System.Drawing.Size(27, 30);
            this.btnAddSpecialText.TabIndex = 190;
            this.btnAddSpecialText.Text = "+";
            this.toolTip1.SetToolTip(this.btnAddSpecialText, "Add special characters, and values");
            this.btnAddSpecialText.UseVisualStyleBackColor = true;
            this.btnAddSpecialText.Click += new System.EventHandler(this.btnAddSpecialText_Click);
            // 
            // cmSpecialCharacters
            // 
            this.cmSpecialCharacters.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmSpecialCharacters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTheValueToolStripMenuItem,
            this.degreesSymbolToolStripMenuItem,
            this.currentDateToolStripMenuItem,
            this.currentTimeToolStripMenuItem,
            this.currentDateTimeToolStripMenuItem,
            this.wallpaperFilenameToolStripMenuItem,
            this.wallpaperCategoryToolStripMenuItem,
            this.metaDataToolStripMenuItem});
            this.cmSpecialCharacters.Name = "cmSpecialCharacters";
            this.cmSpecialCharacters.Size = new System.Drawing.Size(216, 224);
            // 
            // showTheValueToolStripMenuItem
            // 
            this.showTheValueToolStripMenuItem.Enabled = false;
            this.showTheValueToolStripMenuItem.Name = "showTheValueToolStripMenuItem";
            this.showTheValueToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.showTheValueToolStripMenuItem.Text = "Show the Value";
            this.showTheValueToolStripMenuItem.Click += new System.EventHandler(this.showTheValueToolStripMenuItem_Click);
            // 
            // degreesSymbolToolStripMenuItem
            // 
            this.degreesSymbolToolStripMenuItem.Name = "degreesSymbolToolStripMenuItem";
            this.degreesSymbolToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.degreesSymbolToolStripMenuItem.Text = "Degrees Symbol";
            this.degreesSymbolToolStripMenuItem.Click += new System.EventHandler(this.degreesSymbolToolStripMenuItem_Click);
            // 
            // currentDateToolStripMenuItem
            // 
            this.currentDateToolStripMenuItem.Name = "currentDateToolStripMenuItem";
            this.currentDateToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.currentDateToolStripMenuItem.Text = "Current Date";
            this.currentDateToolStripMenuItem.Click += new System.EventHandler(this.currentDateToolStripMenuItem_Click);
            // 
            // currentTimeToolStripMenuItem
            // 
            this.currentTimeToolStripMenuItem.Name = "currentTimeToolStripMenuItem";
            this.currentTimeToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.currentTimeToolStripMenuItem.Text = "Current Time";
            this.currentTimeToolStripMenuItem.Click += new System.EventHandler(this.currentTimeToolStripMenuItem_Click);
            // 
            // currentDateTimeToolStripMenuItem
            // 
            this.currentDateTimeToolStripMenuItem.Name = "currentDateTimeToolStripMenuItem";
            this.currentDateTimeToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.currentDateTimeToolStripMenuItem.Text = "Current Date && Time";
            this.currentDateTimeToolStripMenuItem.Click += new System.EventHandler(this.currentDateTimeToolStripMenuItem_Click);
            // 
            // wallpaperFilenameToolStripMenuItem
            // 
            this.wallpaperFilenameToolStripMenuItem.Name = "wallpaperFilenameToolStripMenuItem";
            this.wallpaperFilenameToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.wallpaperFilenameToolStripMenuItem.Text = "Wallpaper Filename";
            this.wallpaperFilenameToolStripMenuItem.Click += new System.EventHandler(this.wallpaperFilenameToolStripMenuItem_Click);
            // 
            // wallpaperCategoryToolStripMenuItem
            // 
            this.wallpaperCategoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.category2LevelsToolStripMenuItem});
            this.wallpaperCategoryToolStripMenuItem.Name = "wallpaperCategoryToolStripMenuItem";
            this.wallpaperCategoryToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.wallpaperCategoryToolStripMenuItem.Text = "Wallpaper Category";
            this.wallpaperCategoryToolStripMenuItem.Click += new System.EventHandler(this.wallpaperCategoryToolStripMenuItem_Click);
            // 
            // metaDataToolStripMenuItem
            // 
            this.metaDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.titleToolStripMenuItem,
            this.subjectToolStripMenuItem,
            this.commentsToolStripMenuItem});
            this.metaDataToolStripMenuItem.Name = "metaDataToolStripMenuItem";
            this.metaDataToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.metaDataToolStripMenuItem.Text = "Meta Data";
            // 
            // titleToolStripMenuItem
            // 
            this.titleToolStripMenuItem.Name = "titleToolStripMenuItem";
            this.titleToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.titleToolStripMenuItem.Text = "Title";
            this.titleToolStripMenuItem.Click += new System.EventHandler(this.titleToolStripMenuItem_Click);
            // 
            // subjectToolStripMenuItem
            // 
            this.subjectToolStripMenuItem.Name = "subjectToolStripMenuItem";
            this.subjectToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.subjectToolStripMenuItem.Text = "Subject";
            this.subjectToolStripMenuItem.Click += new System.EventHandler(this.subjectToolStripMenuItem_Click);
            // 
            // commentsToolStripMenuItem
            // 
            this.commentsToolStripMenuItem.Name = "commentsToolStripMenuItem";
            this.commentsToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.commentsToolStripMenuItem.Text = "Comment";
            this.commentsToolStripMenuItem.Click += new System.EventHandler(this.commentsToolStripMenuItem_Click);
            // 
            // btnRandomColour
            // 
            this.btnRandomColour.ContextMenuStrip = this.cmSpecialCharacters;
            this.btnRandomColour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRandomColour.Location = new System.Drawing.Point(296, 112);
            this.btnRandomColour.Margin = new System.Windows.Forms.Padding(4);
            this.btnRandomColour.Name = "btnRandomColour";
            this.btnRandomColour.Size = new System.Drawing.Size(23, 31);
            this.btnRandomColour.TabIndex = 201;
            this.btnRandomColour.Text = ">";
            this.toolTip1.SetToolTip(this.btnRandomColour, "Add special characters, and values");
            this.btnRandomColour.UseVisualStyleBackColor = true;
            this.btnRandomColour.Click += new System.EventHandler(this.BtnRandomColour_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 149);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 16);
            this.label7.TabIndex = 21;
            this.label7.Text = "Font Size";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(212, 117);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "Text Colour";
            // 
            // dtValue
            // 
            this.dtValue.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtValue.Location = new System.Drawing.Point(556, 244);
            this.dtValue.Margin = new System.Windows.Forms.Padding(4);
            this.dtValue.Name = "dtValue";
            this.dtValue.ShowUpDown = true;
            this.dtValue.Size = new System.Drawing.Size(125, 22);
            this.dtValue.TabIndex = 191;
            this.dtValue.Value = new System.DateTime(1980, 1, 1, 12, 0, 0, 0);
            this.dtValue.Visible = false;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(556, 276);
            this.txtValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtValue.MaxLength = 1000;
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(125, 22);
            this.txtValue.TabIndex = 192;
            this.txtValue.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 117);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 16);
            this.label9.TabIndex = 194;
            this.label9.Text = "Order Pos";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(204, 151);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 16);
            this.label10.TabIndex = 197;
            this.label10.Text = "Transparency %";
            // 
            // cbAvailable
            // 
            this.cbAvailable.AutoSize = true;
            this.cbAvailable.Location = new System.Drawing.Point(117, 243);
            this.cbAvailable.Margin = new System.Windows.Forms.Padding(4);
            this.cbAvailable.Name = "cbAvailable";
            this.cbAvailable.Size = new System.Drawing.Size(86, 20);
            this.cbAvailable.TabIndex = 199;
            this.cbAvailable.Text = "Available";
            this.cbAvailable.UseVisualStyleBackColor = true;
            this.cbAvailable.Visible = false;
            this.cbAvailable.CheckedChanged += new System.EventHandler(this.cbAvailable_CheckedChanged);
            // 
            // lblValue2
            // 
            this.lblValue2.AutoSize = true;
            this.lblValue2.Location = new System.Drawing.Point(16, 244);
            this.lblValue2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValue2.Name = "lblValue2";
            this.lblValue2.Size = new System.Drawing.Size(34, 16);
            this.lblValue2.TabIndex = 200;
            this.lblValue2.Text = "Ping";
            this.lblValue2.Visible = false;
            // 
            // cmSelectSensor
            // 
            this.cmSelectSensor.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmSelectSensor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cmSelectSensor.Name = "cmCheckSeconds";
            this.cmSelectSensor.Size = new System.Drawing.Size(482, 28);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(481, 24);
            this.toolStripMenuItem1.Text = "Please select a Sensor before trying to add special characters";
            // 
            // category2LevelsToolStripMenuItem
            // 
            this.category2LevelsToolStripMenuItem.Name = "category2LevelsToolStripMenuItem";
            this.category2LevelsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.category2LevelsToolStripMenuItem.Text = "Category 2 Levels";
            this.category2LevelsToolStripMenuItem.Click += new System.EventHandler(this.category2LevelsToolStripMenuItem_Click);
            // 
            // frmEvent
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(745, 457);
            this.Controls.Add(this.btnRandomColour);
            this.Controls.Add(this.lblValue2);
            this.Controls.Add(this.cbAvailable);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numTransparent);
            this.Controls.Add(this.numOrderPos);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblDisplayAlways);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.dtValue);
            this.Controls.Add(this.btnAddSpecialText);
            this.Controls.Add(this.lblCheckSecondsWarning);
            this.Controls.Add(this.pnlFontColour);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numFontSize);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbShowNotification);
            this.Controls.Add(this.lblClearImage);
            this.Controls.Add(this.lblClickImage);
            this.Controls.Add(this.lblCurrentValue);
            this.Controls.Add(this.lblLogicText);
            this.Controls.Add(this.cbEnabled);
            this.Controls.Add(this.cbOverride);
            this.Controls.Add(this.numValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbCheckAction);
            this.Controls.Add(this.cbSensor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.numCheckSeconds);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmEvent";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LAWC Event";
            this.Load += new System.EventHandler(this.frmEvent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.cmCheckSeconds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOrderPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransparent)).EndInit();
            this.cmSpecialCharacters.ResumeLayout(false);
            this.cmSelectSensor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblLogicText;
        internal System.Windows.Forms.PictureBox pbImage;
        internal System.Windows.Forms.TextBox txtMessage;
        internal System.Windows.Forms.NumericUpDown numCheckSeconds;
        internal System.Windows.Forms.ComboBox cbSensor;
        internal System.Windows.Forms.ComboBox cbCheckAction;
        internal System.Windows.Forms.NumericUpDown numValue;
        internal System.Windows.Forms.CheckBox cbOverride;
        internal System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Label lblCurrentValue;
        private System.Windows.Forms.Label lblClickImage;
        private System.Windows.Forms.Label lblClearImage;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.CheckBox cbShowNotification;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Panel pnlFontColour;
        private System.Windows.Forms.Label lblCheckSecondsWarning;
        private System.Windows.Forms.ContextMenuStrip cmCheckSeconds;
        private System.Windows.Forms.ToolStripMenuItem beAwareThatSettingThisTooQuicklyCanCauseSlowerPerformanceOnYourPcToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmSpecialCharacters;
        private System.Windows.Forms.ToolStripMenuItem showTheValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem degreesSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentDateTimeToolStripMenuItem;
        private System.Windows.Forms.DateTimePicker dtValue;
        private System.Windows.Forms.ToolStripMenuItem currentDateToolStripMenuItem;
        internal System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblDisplayAlways;
        private System.Windows.Forms.ToolStripMenuItem wallpaperFilenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metaDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem titleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commentsToolStripMenuItem;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.NumericUpDown numOrderPos;
        internal System.Windows.Forms.NumericUpDown numTransparent;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAddSpecialText;
        internal System.Windows.Forms.CheckBox cbAvailable;
        private System.Windows.Forms.Label lblValue2;
        private System.Windows.Forms.Button btnRandomColour;
        private System.Windows.Forms.ContextMenuStrip cmSelectSensor;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem wallpaperCategoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem category2LevelsToolStripMenuItem;
    }
}