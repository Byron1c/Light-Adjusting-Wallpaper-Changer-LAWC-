namespace LAWC
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.lblImageTime = new System.Windows.Forms.Label();
            this.cbCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.cbStartMinimized = new System.Windows.Forms.CheckBox();
            this.cbShowToolTips = new System.Windows.Forms.CheckBox();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.numWallpaperChangeMins = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbImageOrder = new System.Windows.Forms.ComboBox();
            this.pnlBackgroundColourDark = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.cbDarkMin = new System.Windows.Forms.ComboBox();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.cbDarkHour = new System.Windows.Forms.ComboBox();
            this.pnlBackgroundColourLight = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.cbLightMin = new System.Windows.Forms.ComboBox();
            this.cbLightHour = new System.Windows.Forms.ComboBox();
            this.cbResetImageOptions = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.cbUseSunriseSunset = new System.Windows.Forms.CheckBox();
            this.btnWeatherReport = new System.Windows.Forms.Button();
            this.btnAdvancedSettings = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtLongitude = new System.Windows.Forms.TextBox();
            this.txtLatitude = new System.Windows.Forms.TextBox();
            this.lblSunriseSunset = new System.Windows.Forms.Label();
            this.cbShowSplash = new System.Windows.Forms.CheckBox();
            this.txtOffsetMins = new System.Windows.Forms.TextBox();
            this.lblCityFound = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.btnSetLocation = new System.Windows.Forms.Button();
            this.btnFindLocation = new System.Windows.Forms.Button();
            this.btnViewErrorLog = new System.Windows.Forms.Button();
            this.btnOpenWallpaperFolder = new System.Windows.Forms.Button();
            this.btnOpenSettingsFolder = new System.Windows.Forms.Button();
            this.lblWallpaperModesExplained = new System.Windows.Forms.Label();
            this.cbWallpaperMode = new System.Windows.Forms.ComboBox();
            this.label44 = new System.Windows.Forms.Label();
            this.numImageSizeScalePercent = new System.Windows.Forms.NumericUpDown();
            this.cbUseHSV = new System.Windows.Forms.CheckBox();
            this.cbUseDarkLightTimes = new System.Windows.Forms.CheckBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblImageScaling = new System.Windows.Forms.Label();
            this.cbMultiMonitorMode = new System.Windows.Forms.ComboBox();
            this.lblMultiMonDisplay = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numWallpaperChangeMins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numImageSizeScalePercent)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblImageTime
            // 
            this.lblImageTime.AutoSize = true;
            this.lblImageTime.Location = new System.Drawing.Point(35, 99);
            this.lblImageTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageTime.Name = "lblImageTime";
            this.lblImageTime.Size = new System.Drawing.Size(198, 16);
            this.lblImageTime.TabIndex = 177;
            this.lblImageTime.Text = "You have xx hours of wallpapers";
            // 
            // cbCheckForUpdates
            // 
            this.cbCheckForUpdates.AutoSize = true;
            this.cbCheckForUpdates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbCheckForUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCheckForUpdates.Location = new System.Drawing.Point(291, 24);
            this.cbCheckForUpdates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCheckForUpdates.Name = "cbCheckForUpdates";
            this.cbCheckForUpdates.Size = new System.Drawing.Size(199, 20);
            this.cbCheckForUpdates.TabIndex = 176;
            this.cbCheckForUpdates.Text = "Check for Updates on Startup";
            this.toolTip1.SetToolTip(this.cbCheckForUpdates, "Check for a new version of LAWC when it starts");
            this.cbCheckForUpdates.UseVisualStyleBackColor = true;
            this.cbCheckForUpdates.CheckedChanged += new System.EventHandler(this.cbCheckForUpdates_CheckedChanged);
            // 
            // cbStartMinimized
            // 
            this.cbStartMinimized.AutoSize = true;
            this.cbStartMinimized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbStartMinimized.Location = new System.Drawing.Point(129, 24);
            this.cbStartMinimized.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbStartMinimized.Name = "cbStartMinimized";
            this.cbStartMinimized.Size = new System.Drawing.Size(119, 20);
            this.cbStartMinimized.TabIndex = 175;
            this.cbStartMinimized.Text = "Start Minimized";
            this.toolTip1.SetToolTip(this.cbStartMinimized, "Minimise LAWC when it starts up");
            this.cbStartMinimized.UseVisualStyleBackColor = true;
            this.cbStartMinimized.CheckedChanged += new System.EventHandler(this.cbStartMinimized_CheckedChanged);
            // 
            // cbShowToolTips
            // 
            this.cbShowToolTips.AutoSize = true;
            this.cbShowToolTips.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbShowToolTips.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbShowToolTips.Location = new System.Drawing.Point(724, 24);
            this.cbShowToolTips.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowToolTips.Name = "cbShowToolTips";
            this.cbShowToolTips.Size = new System.Drawing.Size(210, 20);
            this.cbShowToolTips.TabIndex = 174;
            this.cbShowToolTips.Text = "Show Help Balloons in Settings";
            this.toolTip1.SetToolTip(this.cbShowToolTips, "Show these tips when you put the cursor over each of the settings above");
            this.cbShowToolTips.UseVisualStyleBackColor = true;
            this.cbShowToolTips.CheckedChanged += new System.EventHandler(this.cbShowToolTips_CheckedChanged);
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAutoStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAutoStart.Location = new System.Drawing.Point(8, 24);
            this.cbAutoStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(82, 20);
            this.cbAutoStart.TabIndex = 173;
            this.cbAutoStart.Text = "Auto Start";
            this.toolTip1.SetToolTip(this.cbAutoStart, "Start LAWC when Windows starts up");
            this.cbAutoStart.UseVisualStyleBackColor = true;
            this.cbAutoStart.CheckedChanged += new System.EventHandler(this.cbAutoStart_CheckedChanged);
            // 
            // numWallpaperChangeMins
            // 
            this.numWallpaperChangeMins.Location = new System.Drawing.Point(200, 70);
            this.numWallpaperChangeMins.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numWallpaperChangeMins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWallpaperChangeMins.Name = "numWallpaperChangeMins";
            this.numWallpaperChangeMins.Size = new System.Drawing.Size(65, 22);
            this.numWallpaperChangeMins.TabIndex = 172;
            this.toolTip1.SetToolTip(this.numWallpaperChangeMins, "How often should LAWC change the wallpaper");
            this.numWallpaperChangeMins.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numWallpaperChangeMins.ValueChanged += new System.EventHandler(this.numWallpaperChangeMins_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(270, 72);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 16);
            this.label11.TabIndex = 171;
            this.label11.Text = "Mins";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 72);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(187, 16);
            this.label12.TabIndex = 170;
            this.label12.Text = "Wallpaper Change Frequency";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 34);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 16);
            this.label10.TabIndex = 169;
            this.label10.Text = "Wallpaper Order";
            // 
            // cbImageOrder
            // 
            this.cbImageOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbImageOrder.FormattingEnabled = true;
            this.cbImageOrder.Items.AddRange(new object[] {
            "Random",
            "Ordered",
            "LowestViewCountOrdered",
            "LowestViewCountRandom"});
            this.cbImageOrder.Location = new System.Drawing.Point(120, 30);
            this.cbImageOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbImageOrder.Name = "cbImageOrder";
            this.cbImageOrder.Size = new System.Drawing.Size(202, 24);
            this.cbImageOrder.TabIndex = 168;
            this.cbImageOrder.Text = "LowestViewCountRandom";
            this.toolTip1.SetToolTip(this.cbImageOrder, "Do you want wallpapers to be chosen one after the other, or randomly?");
            this.cbImageOrder.SelectedIndexChanged += new System.EventHandler(this.cbImageOrder_SelectedIndexChanged);
            // 
            // pnlBackgroundColourDark
            // 
            this.pnlBackgroundColourDark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(11)))), ((int)(((byte)(0)))));
            this.pnlBackgroundColourDark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackgroundColourDark.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlBackgroundColourDark.Location = new System.Drawing.Point(212, 125);
            this.pnlBackgroundColourDark.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBackgroundColourDark.Name = "pnlBackgroundColourDark";
            this.pnlBackgroundColourDark.Size = new System.Drawing.Size(166, 42);
            this.pnlBackgroundColourDark.TabIndex = 197;
            this.toolTip1.SetToolTip(this.pnlBackgroundColourDark, "Choose the background colour for the Dark time");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(212, 105);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 16);
            this.label9.TabIndex = 196;
            this.label9.Text = "Back Colour Dark";
            // 
            // cbDarkMin
            // 
            this.cbDarkMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDarkMin.FormattingEnabled = true;
            this.cbDarkMin.Items.AddRange(new object[] {
            "00",
            "05",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55"});
            this.cbDarkMin.Location = new System.Drawing.Point(324, 65);
            this.cbDarkMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDarkMin.Name = "cbDarkMin";
            this.cbDarkMin.Size = new System.Drawing.Size(44, 24);
            this.cbDarkMin.TabIndex = 195;
            this.cbDarkMin.Text = "00";
            this.toolTip1.SetToolTip(this.cbDarkMin, "The time to start darkening the wallpapers");
            this.cbDarkMin.SelectedIndexChanged += new System.EventHandler(this.cbDarkMin_SelectedIndexChanged);
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(194, 69);
            this.lblStartTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(70, 16);
            this.lblStartTime.TabIndex = 194;
            this.lblStartTime.Text = "Dark Time";
            // 
            // cbDarkHour
            // 
            this.cbDarkHour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDarkHour.FormattingEnabled = true;
            this.cbDarkHour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cbDarkHour.Location = new System.Drawing.Point(271, 65);
            this.cbDarkHour.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDarkHour.Name = "cbDarkHour";
            this.cbDarkHour.Size = new System.Drawing.Size(44, 24);
            this.cbDarkHour.TabIndex = 193;
            this.cbDarkHour.Text = "19";
            this.toolTip1.SetToolTip(this.cbDarkHour, "The time to start darkening the wallpapers");
            this.cbDarkHour.SelectedIndexChanged += new System.EventHandler(this.cbDarkHour_SelectedIndexChanged);
            // 
            // pnlBackgroundColourLight
            // 
            this.pnlBackgroundColourLight.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pnlBackgroundColourLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackgroundColourLight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlBackgroundColourLight.Location = new System.Drawing.Point(11, 125);
            this.pnlBackgroundColourLight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBackgroundColourLight.Name = "pnlBackgroundColourLight";
            this.pnlBackgroundColourLight.Size = new System.Drawing.Size(168, 42);
            this.pnlBackgroundColourLight.TabIndex = 192;
            this.toolTip1.SetToolTip(this.pnlBackgroundColourLight, "Choose the background colour for the Light time");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 105);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 16);
            this.label4.TabIndex = 191;
            this.label4.Text = "Back Colour Light";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(8, 69);
            this.lblEndTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(69, 16);
            this.lblEndTime.TabIndex = 190;
            this.lblEndTime.Text = "Light Time";
            // 
            // cbLightMin
            // 
            this.cbLightMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLightMin.FormattingEnabled = true;
            this.cbLightMin.Items.AddRange(new object[] {
            "00",
            "05",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55"});
            this.cbLightMin.Location = new System.Drawing.Point(132, 65);
            this.cbLightMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLightMin.Name = "cbLightMin";
            this.cbLightMin.Size = new System.Drawing.Size(44, 24);
            this.cbLightMin.TabIndex = 189;
            this.cbLightMin.Text = "00";
            this.toolTip1.SetToolTip(this.cbLightMin, "The time to start lightening the wallpapers");
            this.cbLightMin.SelectedIndexChanged += new System.EventHandler(this.cbLightMin_SelectedIndexChanged);
            // 
            // cbLightHour
            // 
            this.cbLightHour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLightHour.FormattingEnabled = true;
            this.cbLightHour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cbLightHour.Location = new System.Drawing.Point(80, 65);
            this.cbLightHour.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLightHour.Name = "cbLightHour";
            this.cbLightHour.Size = new System.Drawing.Size(44, 24);
            this.cbLightHour.TabIndex = 188;
            this.cbLightHour.Text = "8";
            this.toolTip1.SetToolTip(this.cbLightHour, "The time to start lightening the wallpapers");
            this.cbLightHour.SelectedIndexChanged += new System.EventHandler(this.cbLightHour_SelectedIndexChanged);
            // 
            // cbResetImageOptions
            // 
            this.cbResetImageOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbResetImageOptions.FormattingEnabled = true;
            this.cbResetImageOptions.Items.AddRange(new object[] {
            "A Little Darker",
            "Darker",
            "Much Darker",
            "Very Dark",
            "None"});
            this.cbResetImageOptions.Location = new System.Drawing.Point(216, 181);
            this.cbResetImageOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbResetImageOptions.Name = "cbResetImageOptions";
            this.cbResetImageOptions.Size = new System.Drawing.Size(156, 24);
            this.cbResetImageOptions.TabIndex = 199;
            this.cbResetImageOptions.Text = "A Little Darker";
            this.toolTip1.SetToolTip(this.cbResetImageOptions, "Adjust the wallpaper images during the Dark time");
            this.cbResetImageOptions.SelectedIndexChanged += new System.EventHandler(this.CbResetImageOptions_SelectedIndexChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(8, 185);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(205, 16);
            this.label32.TabIndex = 198;
            this.label32.Text = "Dark Time Wallpaper Adjustment";
            // 
            // cbUseSunriseSunset
            // 
            this.cbUseSunriseSunset.AutoSize = true;
            this.cbUseSunriseSunset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUseSunriseSunset.Location = new System.Drawing.Point(8, 171);
            this.cbUseSunriseSunset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUseSunriseSunset.Name = "cbUseSunriseSunset";
            this.cbUseSunriseSunset.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbUseSunriseSunset.Size = new System.Drawing.Size(313, 20);
            this.cbUseSunriseSunset.TabIndex = 200;
            this.cbUseSunriseSunset.Text = "Use your Location to set the Dark and Light times";
            this.toolTip1.SetToolTip(this.cbUseSunriseSunset, "Use the calculated Dark and Light times to determine when to darken and lighten y" +
        "our wallpapers");
            this.cbUseSunriseSunset.UseVisualStyleBackColor = true;
            this.cbUseSunriseSunset.CheckedChanged += new System.EventHandler(this.cbUseSunriseSunset_CheckedChanged);
            // 
            // btnWeatherReport
            // 
            this.btnWeatherReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWeatherReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWeatherReport.Location = new System.Drawing.Point(366, 168);
            this.btnWeatherReport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnWeatherReport.Name = "btnWeatherReport";
            this.btnWeatherReport.Size = new System.Drawing.Size(190, 29);
            this.btnWeatherReport.TabIndex = 203;
            this.btnWeatherReport.Text = "Get Current Weather Report";
            this.btnWeatherReport.UseVisualStyleBackColor = true;
            this.btnWeatherReport.Click += new System.EventHandler(this.btnWeatherReport_Click);
            // 
            // btnAdvancedSettings
            // 
            this.btnAdvancedSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvancedSettings.Location = new System.Drawing.Point(805, 460);
            this.btnAdvancedSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdvancedSettings.Name = "btnAdvancedSettings";
            this.btnAdvancedSettings.Size = new System.Drawing.Size(172, 29);
            this.btnAdvancedSettings.TabIndex = 204;
            this.btnAdvancedSettings.Text = "Advanced Settings";
            this.btnAdvancedSettings.UseVisualStyleBackColor = true;
            this.btnAdvancedSettings.Click += new System.EventHandler(this.btnAdvancedSettings_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // txtLongitude
            // 
            this.txtLongitude.Location = new System.Drawing.Point(252, 100);
            this.txtLongitude.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLongitude.MaxLength = 100;
            this.txtLongitude.Name = "txtLongitude";
            this.txtLongitude.Size = new System.Drawing.Size(84, 22);
            this.txtLongitude.TabIndex = 208;
            this.txtLongitude.Text = "0.000000";
            this.toolTip1.SetToolTip(this.txtLongitude, "Enter the Longitude of your location");
            this.txtLongitude.TextChanged += new System.EventHandler(this.txtLongitude_TextChanged);
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point(85, 100);
            this.txtLatitude.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLatitude.MaxLength = 100;
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size(84, 22);
            this.txtLatitude.TabIndex = 207;
            this.txtLatitude.Text = "-0.000000";
            this.toolTip1.SetToolTip(this.txtLatitude, "Enter the Latitude of your location");
            this.txtLatitude.TextChanged += new System.EventHandler(this.txtLatitude_TextChanged);
            // 
            // lblSunriseSunset
            // 
            this.lblSunriseSunset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSunriseSunset.Location = new System.Drawing.Point(345, 100);
            this.lblSunriseSunset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSunriseSunset.Name = "lblSunriseSunset";
            this.lblSunriseSunset.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lblSunriseSunset.Size = new System.Drawing.Size(190, 47);
            this.lblSunriseSunset.TabIndex = 209;
            this.lblSunriseSunset.Text = "Sunrise: (not found)\r\nSunset: (not found)";
            this.toolTip1.SetToolTip(this.lblSunriseSunset, "The calculated sunrise and sunset times for your location.  \r\nNOTE: This is not \"" +
        "Daylight Savings Aware\" and times may vary from the true times.");
            // 
            // cbShowSplash
            // 
            this.cbShowSplash.AutoSize = true;
            this.cbShowSplash.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbShowSplash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbShowSplash.Location = new System.Drawing.Point(530, 24);
            this.cbShowSplash.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowSplash.Name = "cbShowSplash";
            this.cbShowSplash.Size = new System.Drawing.Size(127, 20);
            this.cbShowSplash.TabIndex = 210;
            this.cbShowSplash.Text = "Show Startup Info";
            this.toolTip1.SetToolTip(this.cbShowSplash, "Show the Splash Screen which displays what \r\nLAWC is doing while starting up.");
            this.cbShowSplash.UseVisualStyleBackColor = true;
            this.cbShowSplash.CheckedChanged += new System.EventHandler(this.cbShowSplash_CheckedChanged);
            // 
            // txtOffsetMins
            // 
            this.txtOffsetMins.Location = new System.Drawing.Point(86, 134);
            this.txtOffsetMins.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOffsetMins.MaxLength = 50;
            this.txtOffsetMins.Name = "txtOffsetMins";
            this.txtOffsetMins.Size = new System.Drawing.Size(45, 22);
            this.txtOffsetMins.TabIndex = 212;
            this.txtOffsetMins.Text = "0";
            this.toolTip1.SetToolTip(this.txtOffsetMins, "Enter the daylight savings offset (or other) to get they times set correctly");
            this.txtOffsetMins.TextChanged += new System.EventHandler(this.txtOffsetMins_TextChanged);
            // 
            // lblCityFound
            // 
            this.lblCityFound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCityFound.Location = new System.Drawing.Point(371, 54);
            this.lblCityFound.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCityFound.Name = "lblCityFound";
            this.lblCityFound.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lblCityFound.Size = new System.Drawing.Size(163, 31);
            this.lblCityFound.TabIndex = 219;
            this.lblCityFound.Text = "Found: none";
            this.toolTip1.SetToolTip(this.lblCityFound, "The name of the city found when you enter a city name, and press Find");
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(8, 36);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(171, 16);
            this.label50.TabIndex = 218;
            this.label50.Text = "Find Location By City Name";
            this.toolTip1.SetToolTip(this.label50, "Use this tool to find the GPS coordinates for your city.\r\nEnter the name and pres" +
        "s Find.\r\nIf the correct city is found, press the Set button to save them to LAWC" +
        ".");
            // 
            // btnSetLocation
            // 
            this.btnSetLocation.Enabled = false;
            this.btnSetLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetLocation.Location = new System.Drawing.Point(309, 54);
            this.btnSetLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSetLocation.Name = "btnSetLocation";
            this.btnSetLocation.Size = new System.Drawing.Size(55, 29);
            this.btnSetLocation.TabIndex = 217;
            this.btnSetLocation.Text = "Set";
            this.toolTip1.SetToolTip(this.btnSetLocation, "Use this tool to find the GPS coordinates for your city.\r\nEnter the name and pres" +
        "s Find.\r\nIf the correct city is found, press the Set button to save them to LAWC" +
        ".");
            this.btnSetLocation.UseVisualStyleBackColor = true;
            this.btnSetLocation.Click += new System.EventHandler(this.btnSetLocation_Click);
            // 
            // btnFindLocation
            // 
            this.btnFindLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindLocation.Location = new System.Drawing.Point(208, 54);
            this.btnFindLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFindLocation.Name = "btnFindLocation";
            this.btnFindLocation.Size = new System.Drawing.Size(94, 29);
            this.btnFindLocation.TabIndex = 216;
            this.btnFindLocation.Text = "Find";
            this.toolTip1.SetToolTip(this.btnFindLocation, "Use this tool to find the GPS coordinates for your city.\r\nEnter the name and pres" +
        "s Find.\r\nIf the correct city is found, press the Set button to save them to LAWC" +
        ".");
            this.btnFindLocation.UseVisualStyleBackColor = true;
            this.btnFindLocation.Click += new System.EventHandler(this.btnFindLocation_Click);
            // 
            // btnViewErrorLog
            // 
            this.btnViewErrorLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewErrorLog.Location = new System.Drawing.Point(15, 460);
            this.btnViewErrorLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnViewErrorLog.Name = "btnViewErrorLog";
            this.btnViewErrorLog.Size = new System.Drawing.Size(172, 29);
            this.btnViewErrorLog.TabIndex = 220;
            this.btnViewErrorLog.Text = "View Error Log";
            this.toolTip1.SetToolTip(this.btnViewErrorLog, "Open the error log file.");
            this.btnViewErrorLog.UseVisualStyleBackColor = true;
            this.btnViewErrorLog.Click += new System.EventHandler(this.btnViewErrorLog_Click);
            // 
            // btnOpenWallpaperFolder
            // 
            this.btnOpenWallpaperFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenWallpaperFolder.Location = new System.Drawing.Point(195, 460);
            this.btnOpenWallpaperFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenWallpaperFolder.Name = "btnOpenWallpaperFolder";
            this.btnOpenWallpaperFolder.Size = new System.Drawing.Size(172, 29);
            this.btnOpenWallpaperFolder.TabIndex = 221;
            this.btnOpenWallpaperFolder.Text = "Open Wallpaper Folder";
            this.toolTip1.SetToolTip(this.btnOpenWallpaperFolder, "Open the folder where the wallpaper is stored.");
            this.btnOpenWallpaperFolder.UseVisualStyleBackColor = true;
            this.btnOpenWallpaperFolder.Click += new System.EventHandler(this.btnOpenWallpaperFolder_Click);
            // 
            // btnOpenSettingsFolder
            // 
            this.btnOpenSettingsFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSettingsFolder.Location = new System.Drawing.Point(375, 460);
            this.btnOpenSettingsFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenSettingsFolder.Name = "btnOpenSettingsFolder";
            this.btnOpenSettingsFolder.Size = new System.Drawing.Size(172, 29);
            this.btnOpenSettingsFolder.TabIndex = 222;
            this.btnOpenSettingsFolder.Text = "Open Settings Folder";
            this.toolTip1.SetToolTip(this.btnOpenSettingsFolder, "Open the folder where the settings XML file is stored.");
            this.btnOpenSettingsFolder.UseVisualStyleBackColor = true;
            this.btnOpenSettingsFolder.Click += new System.EventHandler(this.btnOpenSettingsFolder_Click);
            // 
            // lblWallpaperModesExplained
            // 
            this.lblWallpaperModesExplained.AutoSize = true;
            this.lblWallpaperModesExplained.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblWallpaperModesExplained.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWallpaperModesExplained.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblWallpaperModesExplained.Location = new System.Drawing.Point(524, 30);
            this.lblWallpaperModesExplained.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWallpaperModesExplained.Name = "lblWallpaperModesExplained";
            this.lblWallpaperModesExplained.Size = new System.Drawing.Size(24, 25);
            this.lblWallpaperModesExplained.TabIndex = 225;
            this.lblWallpaperModesExplained.Text = "?";
            this.toolTip1.SetToolTip(this.lblWallpaperModesExplained, "Open a link to an article, explaining the differences between them.");
            this.lblWallpaperModesExplained.Click += new System.EventHandler(this.lblWallpaperModesExplained_Click);
            // 
            // cbWallpaperMode
            // 
            this.cbWallpaperMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbWallpaperMode.FormattingEnabled = true;
            this.cbWallpaperMode.Items.AddRange(new object[] {
            "Centre",
            "FillWidth",
            "FillHeight",
            "Stretch",
            "Tile",
            "Span",
            "LAWC"});
            this.cbWallpaperMode.Location = new System.Drawing.Point(431, 30);
            this.cbWallpaperMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbWallpaperMode.Name = "cbWallpaperMode";
            this.cbWallpaperMode.Size = new System.Drawing.Size(84, 24);
            this.cbWallpaperMode.TabIndex = 223;
            this.cbWallpaperMode.Text = "LAWC";
            this.toolTip1.SetToolTip(this.cbWallpaperMode, "How do you want the image displayed as the wallpaper");
            this.cbWallpaperMode.SelectedIndexChanged += new System.EventHandler(this.cbWallpaperMode_SelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(842, 72);
            this.label44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(19, 16);
            this.label44.TabIndex = 233;
            this.label44.Text = "%";
            this.toolTip1.SetToolTip(this.label44, "The percentage of how much the image can be enlarged, \r\nbefore changing modes\r\n");
            // 
            // numImageSizeScalePercent
            // 
            this.numImageSizeScalePercent.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numImageSizeScalePercent.Location = new System.Drawing.Point(764, 70);
            this.numImageSizeScalePercent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numImageSizeScalePercent.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numImageSizeScalePercent.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numImageSizeScalePercent.Name = "numImageSizeScalePercent";
            this.numImageSizeScalePercent.Size = new System.Drawing.Size(70, 22);
            this.numImageSizeScalePercent.TabIndex = 232;
            this.toolTip1.SetToolTip(this.numImageSizeScalePercent, "Sets the size of the image as a percent value.  \r\nRange from 5% to 500%");
            this.numImageSizeScalePercent.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numImageSizeScalePercent.ValueChanged += new System.EventHandler(this.numImageSizeScalePercent_ValueChanged);
            // 
            // cbUseHSV
            // 
            this.cbUseHSV.AutoSize = true;
            this.cbUseHSV.Location = new System.Drawing.Point(719, 32);
            this.cbUseHSV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUseHSV.Name = "cbUseHSV";
            this.cbUseHSV.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbUseHSV.Size = new System.Drawing.Size(197, 20);
            this.cbUseHSV.TabIndex = 234;
            this.cbUseHSV.Text = "Enhance Wallpaper Colours";
            this.toolTip1.SetToolTip(this.cbUseHSV, resources.GetString("cbUseHSV.ToolTip"));
            this.cbUseHSV.UseVisualStyleBackColor = true;
            this.cbUseHSV.CheckedChanged += new System.EventHandler(this.CbUseHSV_CheckedChanged);
            // 
            // cbUseDarkLightTimes
            // 
            this.cbUseDarkLightTimes.AutoSize = true;
            this.cbUseDarkLightTimes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbUseDarkLightTimes.Checked = true;
            this.cbUseDarkLightTimes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseDarkLightTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUseDarkLightTimes.Location = new System.Drawing.Point(8, 35);
            this.cbUseDarkLightTimes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUseDarkLightTimes.Name = "cbUseDarkLightTimes";
            this.cbUseDarkLightTimes.Size = new System.Drawing.Size(155, 20);
            this.cbUseDarkLightTimes.TabIndex = 235;
            this.cbUseDarkLightTimes.Text = "Use Dark/Light Times";
            this.toolTip1.SetToolTip(this.cbUseDarkLightTimes, "Use the Dark and Light settings to adjust the wallpapers");
            this.cbUseDarkLightTimes.UseVisualStyleBackColor = true;
            this.cbUseDarkLightTimes.CheckedChanged += new System.EventHandler(this.cbUseDarkLightTimes_CheckedChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(178, 104);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(66, 16);
            this.label42.TabIndex = 206;
            this.label42.Text = "Longitude";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(8, 104);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(54, 16);
            this.label41.TabIndex = 205;
            this.label41.Text = "Latitude";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(140, 138);
            this.label47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(53, 16);
            this.label47.TabIndex = 213;
            this.label47.Text = "Minutes";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(8, 138);
            this.label46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(75, 16);
            this.label46.TabIndex = 211;
            this.label46.Text = "Time Offset";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(-6, 478);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 29);
            this.btnCancel.TabIndex = 214;
            this.btnCancel.Text = "Hidden - Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(11, 56);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearch.MaxLength = 100;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(188, 22);
            this.txtSearch.TabIndex = 215;
            // 
            // lblImageScaling
            // 
            this.lblImageScaling.AutoSize = true;
            this.lblImageScaling.Location = new System.Drawing.Point(338, 34);
            this.lblImageScaling.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageScaling.Name = "lblImageScaling";
            this.lblImageScaling.Size = new System.Drawing.Size(87, 16);
            this.lblImageScaling.TabIndex = 224;
            this.lblImageScaling.Text = "Wallpaper Fit";
            // 
            // cbMultiMonitorMode
            // 
            this.cbMultiMonitorMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMultiMonitorMode.FormattingEnabled = true;
            this.cbMultiMonitorMode.Items.AddRange(new object[] {
            "SameOnAll",
            "DifferentOnAll"});
            this.cbMultiMonitorMode.Location = new System.Drawing.Point(512, 69);
            this.cbMultiMonitorMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMultiMonitorMode.Name = "cbMultiMonitorMode";
            this.cbMultiMonitorMode.Size = new System.Drawing.Size(115, 24);
            this.cbMultiMonitorMode.TabIndex = 227;
            this.cbMultiMonitorMode.Text = "DifferentOnAll";
            this.cbMultiMonitorMode.SelectedIndexChanged += new System.EventHandler(this.cbMultiMonitorMode_SelectedIndexChanged);
            // 
            // lblMultiMonDisplay
            // 
            this.lblMultiMonDisplay.AutoSize = true;
            this.lblMultiMonDisplay.Location = new System.Drawing.Point(314, 72);
            this.lblMultiMonDisplay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMultiMonDisplay.Name = "lblMultiMonDisplay";
            this.lblMultiMonDisplay.Size = new System.Drawing.Size(196, 16);
            this.lblMultiMonDisplay.TabIndex = 226;
            this.lblMultiMonDisplay.Text = "Multi Monitor Wallpaper Display";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(659, 72);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 231;
            this.label1.Text = "Border / Scale";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbShowSplash);
            this.groupBox1.Controls.Add(this.cbCheckForUpdates);
            this.groupBox1.Controls.Add(this.cbStartMinimized);
            this.groupBox1.Controls.Add(this.cbAutoStart);
            this.groupBox1.Controls.Add(this.cbShowToolTips);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(962, 65);
            this.groupBox1.TabIndex = 236;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cbImageOrder);
            this.groupBox2.Controls.Add(this.cbWallpaperMode);
            this.groupBox2.Controls.Add(this.cbUseHSV);
            this.groupBox2.Controls.Add(this.lblImageScaling);
            this.groupBox2.Controls.Add(this.label44);
            this.groupBox2.Controls.Add(this.lblWallpaperModesExplained);
            this.groupBox2.Controls.Add(this.numImageSizeScalePercent);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cbMultiMonitorMode);
            this.groupBox2.Controls.Add(this.numWallpaperChangeMins);
            this.groupBox2.Controls.Add(this.lblMultiMonDisplay);
            this.groupBox2.Controls.Add(this.lblImageTime);
            this.groupBox2.Location = new System.Drawing.Point(15, 88);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(962, 135);
            this.groupBox2.TabIndex = 237;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wallpaper";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label50);
            this.groupBox3.Controls.Add(this.cbUseSunriseSunset);
            this.groupBox3.Controls.Add(this.label41);
            this.groupBox3.Controls.Add(this.label42);
            this.groupBox3.Controls.Add(this.txtLatitude);
            this.groupBox3.Controls.Add(this.txtLongitude);
            this.groupBox3.Controls.Add(this.lblSunriseSunset);
            this.groupBox3.Controls.Add(this.lblCityFound);
            this.groupBox3.Controls.Add(this.label46);
            this.groupBox3.Controls.Add(this.btnWeatherReport);
            this.groupBox3.Controls.Add(this.txtOffsetMins);
            this.groupBox3.Controls.Add(this.btnSetLocation);
            this.groupBox3.Controls.Add(this.label47);
            this.groupBox3.Controls.Add(this.btnFindLocation);
            this.groupBox3.Controls.Add(this.txtSearch);
            this.groupBox3.Location = new System.Drawing.Point(414, 230);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(563, 222);
            this.groupBox3.TabIndex = 238;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Location";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbUseDarkLightTimes);
            this.groupBox4.Controls.Add(this.cbLightHour);
            this.groupBox4.Controls.Add(this.cbLightMin);
            this.groupBox4.Controls.Add(this.lblEndTime);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.pnlBackgroundColourLight);
            this.groupBox4.Controls.Add(this.cbDarkHour);
            this.groupBox4.Controls.Add(this.lblStartTime);
            this.groupBox4.Controls.Add(this.cbDarkMin);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.pnlBackgroundColourDark);
            this.groupBox4.Controls.Add(this.cbResetImageOptions);
            this.groupBox4.Controls.Add(this.label32);
            this.groupBox4.Location = new System.Drawing.Point(15, 230);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(391, 222);
            this.groupBox4.TabIndex = 239;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Wallpaper Dark and Light Time";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(990, 502);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOpenSettingsFolder);
            this.Controls.Add(this.btnOpenWallpaperFolder);
            this.Controls.Add(this.btnViewErrorLog);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdvancedSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LAWC Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numWallpaperChangeMins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numImageSizeScalePercent)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblImageTime;
        internal System.Windows.Forms.CheckBox cbCheckForUpdates;
        internal System.Windows.Forms.CheckBox cbShowToolTips;
        internal System.Windows.Forms.CheckBox cbAutoStart;
        internal System.Windows.Forms.NumericUpDown numWallpaperChangeMins;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbImageOrder;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.ComboBox cbDarkMin;
        private System.Windows.Forms.Label lblStartTime;
        internal System.Windows.Forms.ComboBox cbDarkHour;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblEndTime;
        internal System.Windows.Forms.ComboBox cbLightMin;
        internal System.Windows.Forms.ComboBox cbLightHour;
        internal System.Windows.Forms.ComboBox cbResetImageOptions;
        private System.Windows.Forms.Label label32;
        internal System.Windows.Forms.CheckBox cbUseSunriseSunset;
        private System.Windows.Forms.Button btnWeatherReport;
        private System.Windows.Forms.Button btnAdvancedSettings;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtLongitude;
        private System.Windows.Forms.TextBox txtLatitude;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label lblSunriseSunset;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtOffsetMins;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCityFound;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button btnSetLocation;
        private System.Windows.Forms.Button btnFindLocation;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnViewErrorLog;
        private System.Windows.Forms.Button btnOpenWallpaperFolder;
        private System.Windows.Forms.Button btnOpenSettingsFolder;
        private System.Windows.Forms.Label lblWallpaperModesExplained;
        private System.Windows.Forms.Label lblImageScaling;
        internal System.Windows.Forms.ComboBox cbWallpaperMode;
        private System.Windows.Forms.ComboBox cbMultiMonitorMode;
        private System.Windows.Forms.Label lblMultiMonDisplay;
        internal System.Windows.Forms.Panel pnlBackgroundColourDark;
        internal System.Windows.Forms.Panel pnlBackgroundColourLight;
        private System.Windows.Forms.Label label44;
        internal System.Windows.Forms.NumericUpDown numImageSizeScalePercent;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.CheckBox cbUseDarkLightTimes;
        internal System.Windows.Forms.CheckBox cbUseHSV;
        internal System.Windows.Forms.CheckBox cbStartMinimized;
        internal System.Windows.Forms.CheckBox cbShowSplash;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}