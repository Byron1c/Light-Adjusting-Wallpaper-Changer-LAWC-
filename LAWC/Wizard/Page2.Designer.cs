namespace LAWC.Wizard
{
    partial class Page2
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblEndTime = new System.Windows.Forms.Label();
            this.cbLightMin = new System.Windows.Forms.ComboBox();
            this.cbLightHour = new System.Windows.Forms.ComboBox();
            this.cbDarkMin = new System.Windows.Forms.ComboBox();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.cbDarkHour = new System.Windows.Forms.ComboBox();
            this.pnlBackgroundDarkColour = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDurationMins = new System.Windows.Forms.Label();
            this.cbDuration = new System.Windows.Forms.ComboBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUseTime = new System.Windows.Forms.CheckBox();
            this.pnlBackgroundLightColour = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBorder = new System.Windows.Forms.CheckBox();
            this.numPercentPictureSize = new System.Windows.Forms.NumericUpDown();
            this.lblBorderPercent = new System.Windows.Forms.Label();
            this.cbUseSunriseSunset = new System.Windows.Forms.CheckBox();
            this.label43 = new System.Windows.Forms.Label();
            this.lblSunriseSunset = new System.Windows.Forms.Label();
            this.btnGetLocation = new System.Windows.Forms.Button();
            this.txtLongitude = new System.Windows.Forms.TextBox();
            this.txtLatitude = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label50 = new System.Windows.Forms.Label();
            this.btnSetLocation = new System.Windows.Forms.Button();
            this.lblCityFound = new System.Windows.Forms.Label();
            this.btnFindLocation = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtOffsetMins = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.lblWallpaperModesExplained = new System.Windows.Forms.Label();
            this.lblImageScaling = new System.Windows.Forms.Label();
            this.cbWallpaperMode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbImageOrder = new System.Windows.Forms.ComboBox();
            this.lblWinDarkMode = new System.Windows.Forms.Label();
            this.cbMultiMonitorMode = new System.Windows.Forms.ComboBox();
            this.lblMultiMonDisplay = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPercentPictureSize)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(12, 164);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(308, 13);
            this.lblEndTime.TabIndex = 168;
            this.lblEndTime.Text = "What time do you want the wallpaper to start getting LIGHTER?";
            // 
            // cbLightMin
            // 
            this.cbLightMin.Enabled = false;
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
            this.cbLightMin.Location = new System.Drawing.Point(366, 160);
            this.cbLightMin.Name = "cbLightMin";
            this.cbLightMin.Size = new System.Drawing.Size(36, 21);
            this.cbLightMin.TabIndex = 167;
            this.cbLightMin.Text = "00";
            this.cbLightMin.SelectedIndexChanged += new System.EventHandler(this.cbEndMin_SelectedIndexChanged);
            // 
            // cbLightHour
            // 
            this.cbLightHour.Enabled = false;
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
            this.cbLightHour.Location = new System.Drawing.Point(326, 160);
            this.cbLightHour.Name = "cbLightHour";
            this.cbLightHour.Size = new System.Drawing.Size(36, 21);
            this.cbLightHour.TabIndex = 166;
            this.cbLightHour.Text = "06";
            this.cbLightHour.SelectedIndexChanged += new System.EventHandler(this.cbEndHour_SelectedIndexChanged);
            // 
            // cbDarkMin
            // 
            this.cbDarkMin.Enabled = false;
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
            this.cbDarkMin.Location = new System.Drawing.Point(366, 187);
            this.cbDarkMin.Name = "cbDarkMin";
            this.cbDarkMin.Size = new System.Drawing.Size(36, 21);
            this.cbDarkMin.TabIndex = 165;
            this.cbDarkMin.Text = "00";
            this.cbDarkMin.SelectedIndexChanged += new System.EventHandler(this.cbStartMin_SelectedIndexChanged);
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(13, 190);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(306, 13);
            this.lblStartTime.TabIndex = 164;
            this.lblStartTime.Text = "What time do you want the wallpaper to start getting DARKER?";
            // 
            // cbDarkHour
            // 
            this.cbDarkHour.Enabled = false;
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
            this.cbDarkHour.Location = new System.Drawing.Point(326, 187);
            this.cbDarkHour.Name = "cbDarkHour";
            this.cbDarkHour.Size = new System.Drawing.Size(36, 21);
            this.cbDarkHour.TabIndex = 163;
            this.cbDarkHour.Text = "18";
            this.cbDarkHour.SelectedIndexChanged += new System.EventHandler(this.cbStartHour_SelectedIndexChanged);
            // 
            // pnlBackgroundDarkColour
            // 
            this.pnlBackgroundDarkColour.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(45)))), ((int)(((byte)(0)))));
            this.pnlBackgroundDarkColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackgroundDarkColour.Location = new System.Drawing.Point(231, 289);
            this.pnlBackgroundDarkColour.Name = "pnlBackgroundDarkColour";
            this.pnlBackgroundDarkColour.Size = new System.Drawing.Size(46, 27);
            this.pnlBackgroundDarkColour.TabIndex = 173;
            this.pnlBackgroundDarkColour.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBackgroundColour_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 298);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(198, 13);
            this.label9.TabIndex = 172;
            this.label9.Text = "Choose a Dark Time Background Colour";
            // 
            // lblDurationMins
            // 
            this.lblDurationMins.AutoSize = true;
            this.lblDurationMins.Location = new System.Drawing.Point(349, 222);
            this.lblDurationMins.Name = "lblDurationMins";
            this.lblDurationMins.Size = new System.Drawing.Size(44, 13);
            this.lblDurationMins.TabIndex = 176;
            this.lblDurationMins.Text = "Minutes";
            // 
            // cbDuration
            // 
            this.cbDuration.Enabled = false;
            this.cbDuration.FormattingEnabled = true;
            this.cbDuration.Items.AddRange(new object[] {
            "1",
            "2",
            "5",
            "10",
            "15",
            "30",
            "45",
            "60",
            "90",
            "120",
            "180",
            "240",
            "360"});
            this.cbDuration.Location = new System.Drawing.Point(298, 219);
            this.cbDuration.Name = "cbDuration";
            this.cbDuration.Size = new System.Drawing.Size(45, 21);
            this.cbDuration.TabIndex = 175;
            this.cbDuration.Text = "60";
            this.cbDuration.SelectedIndexChanged += new System.EventHandler(this.cbDuration_SelectedIndexChanged);
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(13, 222);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(279, 13);
            this.lblDuration.TabIndex = 174;
            this.lblDuration.Text = "How long do you want the darkening / lightening to take?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 13);
            this.label1.TabIndex = 177;
            this.label1.Text = "(The higer the value, the more gradual the adjustments will be)";
            // 
            // cbUseTime
            // 
            this.cbUseTime.AutoSize = true;
            this.cbUseTime.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbUseTime.Location = new System.Drawing.Point(12, 4);
            this.cbUseTime.Name = "cbUseTime";
            this.cbUseTime.Size = new System.Drawing.Size(447, 17);
            this.cbUseTime.TabIndex = 178;
            this.cbUseTime.Text = "Do you want to use the time of day to adjust when to darken and lighten your wall" +
    "papers?";
            this.cbUseTime.UseVisualStyleBackColor = true;
            this.cbUseTime.CheckedChanged += new System.EventHandler(this.cbUseTime_CheckedChanged);
            // 
            // pnlBackgroundLightColour
            // 
            this.pnlBackgroundLightColour.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pnlBackgroundLightColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackgroundLightColour.Location = new System.Drawing.Point(231, 322);
            this.pnlBackgroundLightColour.Name = "pnlBackgroundLightColour";
            this.pnlBackgroundLightColour.Size = new System.Drawing.Size(46, 27);
            this.pnlBackgroundLightColour.TabIndex = 180;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 326);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 13);
            this.label2.TabIndex = 179;
            this.label2.Text = "Choose a Light Time Background Colour";
            // 
            // cbBorder
            // 
            this.cbBorder.AutoSize = true;
            this.cbBorder.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbBorder.Location = new System.Drawing.Point(283, 289);
            this.cbBorder.Name = "cbBorder";
            this.cbBorder.Size = new System.Drawing.Size(223, 17);
            this.cbBorder.TabIndex = 181;
            this.cbBorder.Text = "Do you want a border around the picture?";
            this.cbBorder.UseVisualStyleBackColor = true;
            this.cbBorder.Visible = false;
            this.cbBorder.CheckedChanged += new System.EventHandler(this.cbBorder_CheckedChanged);
            // 
            // numPercentPictureSize
            // 
            this.numPercentPictureSize.Enabled = false;
            this.numPercentPictureSize.Location = new System.Drawing.Point(522, 288);
            this.numPercentPictureSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPercentPictureSize.Name = "numPercentPictureSize";
            this.numPercentPictureSize.Size = new System.Drawing.Size(52, 20);
            this.numPercentPictureSize.TabIndex = 182;
            this.numPercentPictureSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numPercentPictureSize.Visible = false;
            this.numPercentPictureSize.ValueChanged += new System.EventHandler(this.numPercentPictureSize_ValueChanged);
            // 
            // lblBorderPercent
            // 
            this.lblBorderPercent.AutoSize = true;
            this.lblBorderPercent.Enabled = false;
            this.lblBorderPercent.Location = new System.Drawing.Point(284, 313);
            this.lblBorderPercent.Name = "lblBorderPercent";
            this.lblBorderPercent.Size = new System.Drawing.Size(290, 13);
            this.lblBorderPercent.TabIndex = 183;
            this.lblBorderPercent.Text = "Picture Size Percent (Bigger Picture means a smaller Border)";
            this.lblBorderPercent.Visible = false;
            // 
            // cbUseSunriseSunset
            // 
            this.cbUseSunriseSunset.AutoSize = true;
            this.cbUseSunriseSunset.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbUseSunriseSunset.Location = new System.Drawing.Point(16, 27);
            this.cbUseSunriseSunset.Name = "cbUseSunriseSunset";
            this.cbUseSunriseSunset.Size = new System.Drawing.Size(359, 17);
            this.cbUseSunriseSunset.TabIndex = 184;
            this.cbUseSunriseSunset.Text = "Use Location Information for calculating the Sunrise and Sunset times?";
            this.cbUseSunriseSunset.UseVisualStyleBackColor = true;
            this.cbUseSunriseSunset.CheckedChanged += new System.EventHandler(this.cbUseSunriseSunset_CheckedChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(297, 108);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(91, 13);
            this.label43.TabIndex = 191;
            this.label43.Text = "Calculated Times:";
            // 
            // lblSunriseSunset
            // 
            this.lblSunriseSunset.AutoSize = true;
            this.lblSunriseSunset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSunriseSunset.Enabled = false;
            this.lblSunriseSunset.Location = new System.Drawing.Point(390, 104);
            this.lblSunriseSunset.Name = "lblSunriseSunset";
            this.lblSunriseSunset.Padding = new System.Windows.Forms.Padding(5);
            this.lblSunriseSunset.Size = new System.Drawing.Size(57, 38);
            this.lblSunriseSunset.TabIndex = 190;
            this.lblSunriseSunset.Text = "Sunrise:\r\nSunset:";
            // 
            // btnGetLocation
            // 
            this.btnGetLocation.Location = new System.Drawing.Point(510, 393);
            this.btnGetLocation.Name = "btnGetLocation";
            this.btnGetLocation.Size = new System.Drawing.Size(87, 23);
            this.btnGetLocation.TabIndex = 189;
            this.btnGetLocation.Text = "Calculate";
            this.btnGetLocation.UseVisualStyleBackColor = true;
            this.btnGetLocation.Visible = false;
            this.btnGetLocation.Click += new System.EventHandler(this.btnGetLocation_Click);
            // 
            // txtLongitude
            // 
            this.txtLongitude.Location = new System.Drawing.Point(73, 127);
            this.txtLongitude.Name = "txtLongitude";
            this.txtLongitude.Size = new System.Drawing.Size(71, 20);
            this.txtLongitude.TabIndex = 188;
            this.txtLongitude.Text = "0.000000";
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point(73, 101);
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size(71, 20);
            this.txtLatitude.TabIndex = 187;
            this.txtLatitude.Text = "-0.000000";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(13, 130);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(54, 13);
            this.label42.TabIndex = 186;
            this.label42.Text = "Longitude";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(22, 104);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(45, 13);
            this.label41.TabIndex = 185;
            this.label41.Text = "Latitude";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbUseSunriseSunset);
            this.panel1.Controls.Add(this.label50);
            this.panel1.Controls.Add(this.btnSetLocation);
            this.panel1.Controls.Add(this.lblCityFound);
            this.panel1.Controls.Add(this.btnFindLocation);
            this.panel1.Controls.Add(this.cbUseTime);
            this.panel1.Controls.Add(this.label47);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.txtOffsetMins);
            this.panel1.Controls.Add(this.label46);
            this.panel1.Controls.Add(this.txtLongitude);
            this.panel1.Controls.Add(this.lblEndTime);
            this.panel1.Controls.Add(this.txtLatitude);
            this.panel1.Controls.Add(this.cbLightHour);
            this.panel1.Controls.Add(this.label42);
            this.panel1.Controls.Add(this.label43);
            this.panel1.Controls.Add(this.label41);
            this.panel1.Controls.Add(this.lblSunriseSunset);
            this.panel1.Controls.Add(this.cbLightMin);
            this.panel1.Controls.Add(this.lblStartTime);
            this.panel1.Controls.Add(this.cbDarkHour);
            this.panel1.Controls.Add(this.cbDarkMin);
            this.panel1.Controls.Add(this.lblDuration);
            this.panel1.Controls.Add(this.cbDuration);
            this.panel1.Controls.Add(this.lblDurationMins);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 269);
            this.panel1.TabIndex = 194;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(12, 57);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(154, 13);
            this.label50.TabIndex = 198;
            this.label50.Text = "Find GPS Coords By City Name";
            // 
            // btnSetLocation
            // 
            this.btnSetLocation.Location = new System.Drawing.Point(253, 71);
            this.btnSetLocation.Name = "btnSetLocation";
            this.btnSetLocation.Size = new System.Drawing.Size(44, 23);
            this.btnSetLocation.TabIndex = 197;
            this.btnSetLocation.Text = "Set";
            this.btnSetLocation.UseVisualStyleBackColor = true;
            this.btnSetLocation.Click += new System.EventHandler(this.btnSetLocation_Click);
            // 
            // lblCityFound
            // 
            this.lblCityFound.AutoSize = true;
            this.lblCityFound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCityFound.Enabled = false;
            this.lblCityFound.Location = new System.Drawing.Point(390, 71);
            this.lblCityFound.Name = "lblCityFound";
            this.lblCityFound.Padding = new System.Windows.Forms.Padding(5);
            this.lblCityFound.Size = new System.Drawing.Size(79, 25);
            this.lblCityFound.TabIndex = 199;
            this.lblCityFound.Text = "Found: none";
            // 
            // btnFindLocation
            // 
            this.btnFindLocation.Location = new System.Drawing.Point(172, 71);
            this.btnFindLocation.Name = "btnFindLocation";
            this.btnFindLocation.Size = new System.Drawing.Size(75, 23);
            this.btnFindLocation.TabIndex = 196;
            this.btnFindLocation.Text = "Find";
            this.btnFindLocation.UseVisualStyleBackColor = true;
            this.btnFindLocation.Click += new System.EventHandler(this.btnFindLocation_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(313, 130);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(29, 13);
            this.label47.TabIndex = 192;
            this.label47.Text = "Mins";
            // 
            // txtSearch
            // 
            this.txtSearch.Enabled = false;
            this.txtSearch.Location = new System.Drawing.Point(15, 73);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(151, 20);
            this.txtSearch.TabIndex = 195;
            // 
            // txtOffsetMins
            // 
            this.txtOffsetMins.Enabled = false;
            this.txtOffsetMins.Location = new System.Drawing.Point(270, 127);
            this.txtOffsetMins.Name = "txtOffsetMins";
            this.txtOffsetMins.Size = new System.Drawing.Size(37, 20);
            this.txtOffsetMins.TabIndex = 191;
            this.txtOffsetMins.Text = "0";
            this.txtOffsetMins.TextChanged += new System.EventHandler(this.txtOffsetMins_TextChanged);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(150, 130);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(117, 13);
            this.label46.TabIndex = 190;
            this.label46.Text = "Daylight Savings Offset";
            // 
            // lblWallpaperModesExplained
            // 
            this.lblWallpaperModesExplained.AutoSize = true;
            this.lblWallpaperModesExplained.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblWallpaperModesExplained.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWallpaperModesExplained.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblWallpaperModesExplained.Location = new System.Drawing.Point(192, 354);
            this.lblWallpaperModesExplained.Name = "lblWallpaperModesExplained";
            this.lblWallpaperModesExplained.Size = new System.Drawing.Size(19, 20);
            this.lblWallpaperModesExplained.TabIndex = 197;
            this.lblWallpaperModesExplained.Text = "?";
            this.lblWallpaperModesExplained.Click += new System.EventHandler(this.lblWallpaperModesExplained_Click);
            // 
            // lblImageScaling
            // 
            this.lblImageScaling.AutoSize = true;
            this.lblImageScaling.Location = new System.Drawing.Point(27, 357);
            this.lblImageScaling.Name = "lblImageScaling";
            this.lblImageScaling.Size = new System.Drawing.Size(85, 13);
            this.lblImageScaling.TabIndex = 196;
            this.lblImageScaling.Text = "Wallpaper Mode";
            // 
            // cbWallpaperMode
            // 
            this.cbWallpaperMode.FormattingEnabled = true;
            this.cbWallpaperMode.Items.AddRange(new object[] {
            "Centre",
            "FillWidth",
            "FillHeight",
            "Stretch",
            "Tile",
            "Span",
            "LAWC"});
            this.cbWallpaperMode.Location = new System.Drawing.Point(118, 354);
            this.cbWallpaperMode.Name = "cbWallpaperMode";
            this.cbWallpaperMode.Size = new System.Drawing.Size(68, 21);
            this.cbWallpaperMode.TabIndex = 195;
            this.cbWallpaperMode.Text = "LAWC";
            this.cbWallpaperMode.SelectedIndexChanged += new System.EventHandler(this.cbWallpaperMode_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(27, 385);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 199;
            this.label10.Text = "Wallpaper Order";
            // 
            // cbImageOrder
            // 
            this.cbImageOrder.FormattingEnabled = true;
            this.cbImageOrder.Items.AddRange(new object[] {
            "Random",
            "Ordered",
            "LowestViewCountOrdered",
            "LowestViewCountRandom"});
            this.cbImageOrder.Location = new System.Drawing.Point(118, 382);
            this.cbImageOrder.Name = "cbImageOrder";
            this.cbImageOrder.Size = new System.Drawing.Size(159, 21);
            this.cbImageOrder.TabIndex = 198;
            this.cbImageOrder.Text = "LowestViewCountRandom";
            this.cbImageOrder.SelectedIndexChanged += new System.EventHandler(this.cbImageOrder_SelectedIndexChanged);
            // 
            // lblWinDarkMode
            // 
            this.lblWinDarkMode.AutoSize = true;
            this.lblWinDarkMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblWinDarkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWinDarkMode.Location = new System.Drawing.Point(295, 383);
            this.lblWinDarkMode.Name = "lblWinDarkMode";
            this.lblWinDarkMode.Size = new System.Drawing.Size(187, 15);
            this.lblWinDarkMode.TabIndex = 200;
            this.lblWinDarkMode.Text = "Enable Windows Dark Mode";
            this.lblWinDarkMode.Click += new System.EventHandler(this.lblWinDarkMode_Click);
            // 
            // cbMultiMonitorMode
            // 
            this.cbMultiMonitorMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMultiMonitorMode.FormattingEnabled = true;
            this.cbMultiMonitorMode.Items.AddRange(new object[] {
            "SameOnAll",
            "DifferentOnAll"});
            this.cbMultiMonitorMode.Location = new System.Drawing.Point(443, 354);
            this.cbMultiMonitorMode.Name = "cbMultiMonitorMode";
            this.cbMultiMonitorMode.Size = new System.Drawing.Size(93, 21);
            this.cbMultiMonitorMode.TabIndex = 229;
            this.cbMultiMonitorMode.Text = "DifferentOnAll";
            this.cbMultiMonitorMode.SelectedIndexChanged += new System.EventHandler(this.cbMultiMonitorMode_SelectedIndexChanged);
            // 
            // lblMultiMonDisplay
            // 
            this.lblMultiMonDisplay.AutoSize = true;
            this.lblMultiMonDisplay.Location = new System.Drawing.Point(284, 357);
            this.lblMultiMonDisplay.Name = "lblMultiMonDisplay";
            this.lblMultiMonDisplay.Size = new System.Drawing.Size(155, 13);
            this.lblMultiMonDisplay.TabIndex = 228;
            this.lblMultiMonDisplay.Text = "Multi Monitor Wallpaper Display";
            // 
            // Page2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbMultiMonitorMode);
            this.Controls.Add(this.lblMultiMonDisplay);
            this.Controls.Add(this.lblWinDarkMode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbImageOrder);
            this.Controls.Add(this.lblWallpaperModesExplained);
            this.Controls.Add(this.lblImageScaling);
            this.Controls.Add(this.cbWallpaperMode);
            this.Controls.Add(this.lblBorderPercent);
            this.Controls.Add(this.numPercentPictureSize);
            this.Controls.Add(this.cbBorder);
            this.Controls.Add(this.pnlBackgroundLightColour);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlBackgroundDarkColour);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnGetLocation);
            this.Name = "Page2";
            this.Size = new System.Drawing.Size(600, 419);
            ((System.ComponentModel.ISupportInitialize)(this.numPercentPictureSize)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.ComboBox cbLightMin;
        private System.Windows.Forms.ComboBox cbLightHour;
        private System.Windows.Forms.ComboBox cbDarkMin;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.ComboBox cbDarkHour;
        private System.Windows.Forms.Panel pnlBackgroundDarkColour;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDurationMins;
        private System.Windows.Forms.ComboBox cbDuration;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbUseTime;
        private System.Windows.Forms.Panel pnlBackgroundLightColour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbBorder;
        private System.Windows.Forms.NumericUpDown numPercentPictureSize;
        private System.Windows.Forms.Label lblBorderPercent;
        private System.Windows.Forms.CheckBox cbUseSunriseSunset;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label lblSunriseSunset;
        private System.Windows.Forms.Button btnGetLocation;
        private System.Windows.Forms.TextBox txtLongitude;
        private System.Windows.Forms.TextBox txtLatitude;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtOffsetMins;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button btnSetLocation;
        private System.Windows.Forms.Label lblCityFound;
        private System.Windows.Forms.Button btnFindLocation;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblWallpaperModesExplained;
        private System.Windows.Forms.Label lblImageScaling;
        internal System.Windows.Forms.ComboBox cbWallpaperMode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbImageOrder;
        private System.Windows.Forms.Label lblWinDarkMode;
        private System.Windows.Forms.ComboBox cbMultiMonitorMode;
        private System.Windows.Forms.Label lblMultiMonDisplay;
    }
}
