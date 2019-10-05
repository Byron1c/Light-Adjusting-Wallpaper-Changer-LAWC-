using BrightIdeasSoftware;
using LAWC.Common;
using LAWC.Objects;
using Microsoft.Win32;
using OpenWeatherAPI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using static LAWC.FrmMain;



namespace LAWC
{

    public partial class frmSettingsAdvanced : Form
    {
        internal FrmMain parentForm;

        internal Thread threadAdjustImage;

        internal Boolean testingTime;

        private Bitmap bmpPreview;

        private readonly Boolean initializing;

        internal Boolean FormChanged;

        private String foundCityName;
        private double foundCityLat;
        private double foundCityLong;

        private Boolean weatherEnabled = false;
        private readonly float TabFontSize = 8.25F;       // font drawing size

        internal Boolean redrawWallpaper = false;


        public frmSettingsAdvanced(FrmMain vParentForm)
        {
            InitializeComponent();

            initializing = true;

            parentForm = vParentForm;

            fillKeyBoxes();

            setHandlers();

            numSizeKBytesMin.Maximum = decimal.MaxValue; // nowhere else to put this
            FormChanged = false;

            foundCityName = string.Empty;
            foundCityLat = 0;
            foundCityLong = 0;

            initializing = false;
        }


        private void setHandlers()
        {
            this.FormClosing += FrmSettingsAdvanced_FormClosing;

            pnlBackgroundColourDark.MouseUp += new MouseEventHandler(PnlBackgroundColourDark_MouseUp);
            pnlBackgroundColourLight.MouseUp += new MouseEventHandler(PnlBackgroundColourLight_MouseUp);
            pnlColourTint.MouseUp += new MouseEventHandler(PnlColourTint_MouseUp);

            cbDuration.SelectedValueChanged += new EventHandler(CbDuration_SelectedValueChanged);
            cbWallpaperMode.SelectedValueChanged += new EventHandler(cbWallpaperMode_SelectedValueChanged);

            tbAjustTintStrength.MouseUp += TbTintStrength_MouseUp;
            tbAdjustContrast.MouseUp += TbAdjustBrightness_MouseUp;
            tbAdjustGamma.MouseUp += TbAdjustGamma_MouseUp;
            tbAdjustAlpha.MouseUp += TbAdjustAlpha_MouseUp;
            tbAdjustBrightness.MouseUp += TbAdjustContrast_MouseUp;

            tbScreenGamma.MouseUp += new MouseEventHandler(tbScreenGamma_MouseUp);
            tbScreenGamma.MouseLeave += new EventHandler(tbScreenGamma_MouseLeave);

            clbShowScreenImage.ItemCheck += ClbShowScreenImage_ItemCheck;
            lvScreenScales.DoubleClick += LvScreenScales_DoubleClick;

            cbShowScreenID.CheckedChanged += CbShowScreenID_CheckedChanged;
            tbBrightnessMin.Scroll += tbBrightnessMin_Scroll;
            tbBrightnessMin.MouseUp += tbBrightnessMin_MouseUp;
            tbBrightnessMax.Scroll += tbBrightnessMax_Scroll;
            tbBrightnessMax.MouseUp += tbBrightnessMax_MouseUp;
            numWidth.ValueChanged += numWidth_ValueChanged;
            numHeight.ValueChanged += numHeight_ValueChanged;

            olvEvents.MouseUp += LvEvents_MouseUp;
            olvEvents.ColumnClick += LvEvents_ColumnClick;
            olvEvents.MouseDoubleClick += LvEvents_MouseDoubleClick;
            olvEvents.MouseHover += LvEvents_MouseHover;
            olvEvents.KeyDown += LvEvents_KeyDown;
            txtHardwareSensorsFound.MouseDoubleClick += TxtHardwareSensorsFound_MouseDoubleClick;

            tabSettings.DrawItem += TabScreens_DrawItem;

            olvEvents.DrawColumnHeader += LvEvents_DrawColumnHeader;
            olvEvents.DrawSubItem += LvEvents_DrawSubItem;

        }

        private void FrmSettingsAdvanced_FormClosing(object sender, FormClosingEventArgs e)
        {
            cbTimeTest.Checked = false;
            EnableTest(false);
        }

        private void TbAdjustContrast_MouseUp(object sender, MouseEventArgs e)
        {
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);
        }

        private void TbAdjustAlpha_MouseUp(object sender, MouseEventArgs e)
        {
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);
        }

        private void TbAdjustGamma_MouseUp(object sender, MouseEventArgs e)
        {
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);
        }

        private void TbAdjustBrightness_MouseUp(object sender, MouseEventArgs e)
        {
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);
        }

        private void TbTintStrength_MouseUp(object sender, MouseEventArgs e)
        {
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);// (Bitmap)bmpPreview.Clone());
        }

        private void adjustMyObjectListViewHeader()
        {
            foreach (OLVColumn item in olvEvents.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(parentForm.colourDarker);
                headerstyle.SetForeColor(parentForm.colourLightest);
                item.HeaderFormatStyle = headerstyle;
            }

        }

        internal void DrawEventList()
        {
            adjustMyObjectListViewHeader();

            olvEvents.CheckBoxes = false;
            parentForm.settings.Events.Sort(EventInfo.CompareByOrder);

            this.olvEvents.SetObjects(parentForm.settings.Events);

            if (this.parentForm.settings.Events.Count == 0)
            {
                lblNoEntriesEvents.Visible = true;
            }
            else
            {
                lblNoEntriesEvents.Visible = false;
            }

        }

        private void LvEvents_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            parentForm.DrawSubItem(sender, e);
        }

        private void LvEvents_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            parentForm.DrawColumnHeader(sender, e);
        }

        private void TabScreens_DrawItem(object sender, DrawItemEventArgs e)
        {
            drawTabPage(sender, e);
        }

        private void drawTabPage(object sender, DrawItemEventArgs e)
        {
            // get ref to this page
            TabPage tp = ((TabControl)sender).TabPages[e.Index];

            using (Brush br = new SolidBrush(parentForm.colourDarkest))
            {
                Rectangle rect = e.Bounds;
                e.Graphics.FillRectangle(br, e.Bounds);

                using (Font f = new Font(tp.Font.FontFamily, TabFontSize))
                {
                    // shift for a gutter/padding
                    rect.Offset(1, 1);
                    TextRenderer.DrawText(e.Graphics, tp.Text,
                                  f, rect,
                                  parentForm.colourLightest);
                }


                // draw the border
                rect = e.Bounds;
                rect.Offset(0, 1);
                rect.Inflate(0, -1);

                // ControlDark looks right for the border
                using (Pen p = new Pen(SystemColors.ControlDark))
                {
                    e.Graphics.DrawRectangle(p, rect);
                }

                if (e.State == DrawItemState.Selected) e.DrawFocusRectangle();

                // Draw the stupid space to the right of the tabs, that is not set by BackColor
                SolidBrush forebrush = new SolidBrush(parentForm.colourLightest);
                SolidBrush backbrush = new SolidBrush(parentForm.colourDarkest);

                //draw rectangle behind the tabs
                Rectangle lasttabrect = this.tabSettings.GetTabRect(this.tabSettings.TabPages.Count - 1);
                Rectangle background = new Rectangle
                {
                    Location = new Point(lasttabrect.Right, 0)
                };

                //pad the rectangle to cover the 1 pixel line between the top of the tabpage and the start of the tabs
                background.Size = new Size(this.tabSettings.Right - background.Left, lasttabrect.Height + 1);
                e.Graphics.FillRectangle(backbrush, background);

            }
        }

        private void LvScreenScales_DoubleClick(object sender, EventArgs e)
        {
            if (lvScreenScales.SelectedItems.Count > 0)
            {
                String screen = lvScreenScales.SelectedItems[0].Text;
                screen = screen.Substring(screen.IndexOf("#", StringComparison.InvariantCulture) + 1, 1); // Limitation: Only up to 9 (NINE) SCREENS
                int screenNum = int.Parse(screen, CultureInfo.InvariantCulture);
                int percent = (int)(parentForm.settings.ExtraScreenInfo[screenNum].Scale * 100f);

                frmScreenInfoEdit frmEditScreenInfo = new frmScreenInfoEdit(percent);
                frmEditScreenInfo.ShowDialog();

                int newScale;// = 100;
                if (frmEditScreenInfo.OK)
                {
                    newScale = (int)frmEditScreenInfo.numScale.Value;
                    float fScale = (float)(double)(newScale / 100f);
                    parentForm.settings.ExtraScreenInfo[screenNum].Scale = (float)Math.Round(fScale, 2);
                    SetScreenImageCheckBoxes();
                }
            }
        }

        private void TxtHardwareSensorsFound_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //do nothing
        }

        private void LvEvents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Delete))
            {
                DeleteSelectedEvent();
            }

        }

        private void LvEvents_MouseHover(object sender, EventArgs e)
        {
            olvEvents.Focus();
        }

        private void EditSelectedEvent()
        {
            // if nothing is selected, return
            if (olvEvents.SelectedItem == null) return;

            if (parentForm.HWSensors.Count == 0)
            {
                MessageBox.Show("Sorry, you have not selected any sensors.\n\n"
                    + "In Advanced Settings - Locations, tick the Sensor categories you want to use, and press the Set button.",
                    "No Sensors Available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                return; // the user has to have at least one sensor to be able to add an event
            }

            String result = string.Empty;
            String categoryList = string.Empty;

            // check that the event sensor category is enabled
            if (parentForm.checkEventSensorAvailable(((EventInfo)(this.olvEvents.SelectedItem.RowObject)), true,
                ref categoryList, ref result) == false)
            {
                MessageBox.Show("The sensor for this Event is not turned on. \n\n"
                    + "Please go to Advanced Settings - Events, and Set the following sensor:\n\n"
                    + result); // categoryList);
                return;
            }

            // find the event with a natural key
            int index = parentForm.settings.GetEventIndex(
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).TypeOfEvent,
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).CheckValueDecimal,
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Message,
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).ImagePath,
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).SensorName,
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).ShowNotification,
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).CheckValueString
                );

            if (index == -1) return;

            frmEvent eventForm = new frmEvent(this, frmEvent.FormMode.Edit, index)
            {
                applyingSettings = true
            };

            eventForm.fillSensorList(parentForm.settings.Events[index].SensorName, parentForm.settings.Events[index].TypeOfEvent, parentForm.settings.Events[index].SensorName);

            eventForm.numOrderPos.Value = parentForm.settings.Events[index].OrderPos;
            eventForm.numOrderPos.Maximum = parentForm.settings.Events.Count;

            eventForm.txtMessage.Text = parentForm.settings.Events[index].Message;

            if (parentForm.settings.Events[index].CheckSeconds < eventForm.numCheckSeconds.Minimum)
            {
                parentForm.settings.Events[index].CheckSeconds = (int)eventForm.numCheckSeconds.Minimum;
            }
            eventForm.numCheckSeconds.Value = parentForm.settings.Events[index].CheckSeconds;
            eventForm.cbCheckAction.SelectedItem = parentForm.settings.Events[index].CheckAction.ToString();
            eventForm.numValue.Value = parentForm.settings.Events[index].CheckValueDecimal;
            eventForm.txtValue.Text = parentForm.settings.Events[index].CheckValueString;
            eventForm.cbOverride.Checked = parentForm.settings.Events[index].OverrideWallpaper;
            eventForm.cbEnabled.Checked = parentForm.settings.Events[index].Enabled;
            eventForm.ImagePath = parentForm.settings.Events[index].ImagePath;
            eventForm.cbShowNotification.Checked = parentForm.settings.Events[index].ShowNotification;
            eventForm.numFontSize.Value = (decimal)parentForm.settings.Events[index].FontSize;
            eventForm.pnlFontColour.BackColor = parentForm.settings.Events[index].FontColour;
            eventForm.lastRun = parentForm.settings.Events[index].LastRun;
            eventForm.numTransparent.Value = parentForm.settings.Events[index].Transparency; 

            if (parentForm.settings.Events[index].SensorName.ToUpper(CultureInfo.InvariantCulture) == "INTERNETWEBSITE")
            {
                eventForm.cbAvailable.Checked = (parentForm.settings.Events[index].CheckValueDecimal == 1);
            }            

            if (parentForm.settings.Events[index].CheckAction == EventInfo.CheckActionType.DisplayAlways) eventForm.clearImage();

            // Load/display image
            eventForm.loadEventImage(eventForm.ImagePath);
            eventForm.updateLogicText();
            eventForm.updateCurrentSensorValue(parentForm.settings.Events[index].CheckValueString);

            eventForm.applyingSettings = false;
            eventForm.ShowDialog();

            eventForm.Hide();
            eventForm.Dispose();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);

            DrawEventList();
        }

        private void LvEvents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditSelectedEvent();
        }

        private void LvEvents_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //do nothing
        }

        private void LvEvents_MouseUp(object sender, MouseEventArgs e)
        {
            // show context menu
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                setEventPopupMenu();

                this.cmEvents.Show(olvEvents, e.Location);
            }
        }

        private void setEventPopupMenu()
        {
            editEventToolStripMenuItem.Visible = false;
            addEventToolStripMenuItem.Visible = true;
            removeEventToolStripMenuItem.Visible = false;
            enableToolStripMenuItem.Visible = false;
            disableToolStripMenuItem.Visible = false;
            runEventToolStripMenuItem.Visible = false;

            if (this.olvEvents.SelectedItem != null) 
            {
                editEventToolStripMenuItem.Visible = true;
                runEventToolStripMenuItem.Visible = true;
                removeEventToolStripMenuItem.Visible = true;

                if (((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Enabled == true)
                {
                    enableToolStripMenuItem.Visible = false;
                    disableToolStripMenuItem.Visible = true;
                }
                else
                {
                    enableToolStripMenuItem.Visible = true;
                    disableToolStripMenuItem.Visible = false;
                }
            }
        }

        private void tbBrightnessMin_MouseUp(object sender, MouseEventArgs e)
        {
            FormChanged = true;
        }

        private void tbBrightnessMax_MouseUp(object sender, MouseEventArgs e)
        {
            FormChanged = true;
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            // shift the cancel button off the screen, as we only use it to make the ESC key hide the form
            btnCancel.Location = new System.Drawing.Point(btnCancel.Location.X + this.Width, btnCancel.Location.Y);
        }

        private void fillKeyBoxes()
        {
            cbKey3.Items.Clear();

            foreach (Keys key in (Keys[])Enum.GetValues(typeof(Keys)))
            {

                if (key != Keys.LShiftKey
                    && key != Keys.RShiftKey
                    && key != Keys.LControlKey
                    && key != Keys.RControlKey
                    && key != Keys.Alt
                    && key != Keys.Control
                    && key != Keys.Shift
                    && key != Keys.ShiftKey
                    && key != Keys.ControlKey
                    && key != Keys.None
                    && key != Keys.NoName
                   )
                {
                    cbKey3.Items.Add(key.ToString());
                }
            }
        }


        private void tbScreenGamma_MouseLeave(object sender, EventArgs e)
        {
            ScreenFunctions.setGamma(this.parentForm.settings.ScreenGamma);
        }

        private void tbScreenGamma_MouseUp(object sender, MouseEventArgs e)
        {
            ScreenFunctions.setGamma(this.parentForm.settings.ScreenGamma);
        }        

        private void cbWallpaperMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbWallpaperMode_SelectedValueChanged(object sender, EventArgs e)
        {
            setModeSelection();
        }

        private void setModeSelection()
        {
            if (!this.testingTime)
            {
                redrawWallpaper = true;

                // enumStringToType:
                this.parentForm.settings.WallpaperMode = (Wallpaper.WallpaperModes)Enum.Parse(typeof(Wallpaper.WallpaperModes), cbWallpaperMode.SelectedItem.ToString());
                cbMultiMonitorMode.Enabled = true;
                lblMultiMonDisplay.Enabled = (this.parentForm.settings.WallpaperMode != Wallpaper.WallpaperModes.LAWC);
                cbMultiMonitorMode.Enabled = (this.parentForm.settings.WallpaperMode != Wallpaper.WallpaperModes.LAWC);
                if (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC)
                {
                    cbMultiMonitorMode.SelectedItem = "DifferentOnAll";
                    parentForm.settings.MultiMonitorMode = MultiMonitorModes.DifferentOnAll;
                    cbMultiMonitorMode.Enabled = false;
                }

                //reset the multi mon mode if only 1 screen is present
                if (parentForm.settings.MultiMonitorMode == MultiMonitorModes.DifferentOnAll && Screen.AllScreens.Length == 1)
                {
                    parentForm.settings.MultiMonitorMode = MultiMonitorModes.SameOnAll;
                    cbMultiMonitorMode.SelectedItem = "SameOnAll";
                    cbMultiMonitorMode.Enabled = false;
                }

                numImageSizeScalePercent.Value = (decimal)this.parentForm.settings.ImageSizeScalePercentPREV; // percent;

                SetImageScalePercent(); // set the values etc

                parentForm.AdjustDesktopImages();
            }

            //hide/show LAWC settings options
            lblWideAspectThreshold.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            numWideAspectThreshold.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            lblNarrowAspectThreshold.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            numNarrowAspectThreshold.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            numMarginEnlarge.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            lblMargin.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            lblMarginPercent.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            lblWideMargin.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            lblWideMarginPercent.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
            numWideThresholdPercent.Enabled = (this.parentForm.settings.WallpaperMode == Wallpaper.WallpaperModes.LAWC);
        }

        private void CbDuration_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!this.parentForm.applyingSettings && !this.testingTime)
            {
                this.parentForm.settings.DurationMins = int.Parse(cbDuration.SelectedItem.ToString(), CultureInfo.InvariantCulture);
                this.numImageAdjustFreqSecs.Maximum = this.parentForm.settings.DurationMins * 60;
                parentForm.UpdateScreenState();
                parentForm.SetInterfaceColour();
                parentForm.AdjustDesktopImages();

            }

        }
        private void CbDuration_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PnlBackgroundColourDark_MouseUp(object sender, MouseEventArgs e)
        {
            FormChanged = true;
            PickColourDark();
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); 
        }

        private void PnlBackgroundColourLight_MouseUp(object sender, MouseEventArgs e)
        {
            FormChanged = true;
            PickColourLight();
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); 
        }

        private void PnlColourTint_MouseUp(object sender, MouseEventArgs e)
        {
            PickColourTint();
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);  
        }

        internal void SetFormValues()
        {
            parentForm.applyingSettings = true;

            CheckAutoRunState();

            this.numSizeKBytesMin.Value = (long)(this.parentForm.settings.SizeKBytesMin);
            this.tbBrightnessMax.Value = this.parentForm.settings.BrightnessMax;
            this.tbBrightnessMin.Value = this.parentForm.settings.BrightnessMin;

            this.tbAdjustGamma.Value = (int)(this.parentForm.settings.Gamma);
            this.tbAjustTintStrength.Value = this.parentForm.settings.TintStrength;
            this.tbAdjustContrast.Value = (int)(this.parentForm.settings.Contrast);
            this.tbAdjustBrightness.Value = (int)(this.parentForm.settings.Brightness);
            this.tbAdjustAlpha.Value = (int)(this.parentForm.settings.Alpha);

            this.pnlBackgroundColourDark.BackColor = this.parentForm.settings.BackgroundColourDark;
            this.pnlBackgroundColourLight.BackColor = this.parentForm.settings.BackgroundColourLight;
            this.pnlColourTint.BackColor = this.parentForm.settings.TintColour;
            this.cbDuration.SelectedItem = this.parentForm.settings.DurationMins.ToString(CultureInfo.InvariantCulture);
            this.cbImageQuality.SelectedItem = this.parentForm.settings.CompressionQuality;
            this.cbWallpaperFileType.SelectedItem = this.parentForm.settings.WallpaperFormat.ToString();
            this.numAdjustX.Value = this.parentForm.settings.AdjustX;
            this.numAdjustY.Value = this.parentForm.settings.AdjustY;
            this.cbWallpaperMode.SelectedItem = this.parentForm.settings.WallpaperMode.ToString();
            this.cbUseDarkLightTimes.Checked = this.parentForm.settings.UseDarkLightTimes;
            this.cbUseHSV.Checked = this.parentForm.settings.UseHSV;
            this.cbImageOrder.SelectedItem = this.parentForm.settings.WallpaperOrder.ToString();
            this.cbUseAdjustmentsAlways.Checked = this.parentForm.settings.AdjustmentsUseAlways;
            this.cbChangeOnStartup.Checked = this.parentForm.settings.ChangeOnStartup;
            this.numWallpaperChangeMins.Value = this.parentForm.settings.WallpaperChangeFrequencyMins;
            this.numWidth.Value = this.parentForm.settings.MinImageWidth;
            this.numHeight.Value = this.parentForm.settings.MinImageHeight;
            this.cbShowScreenID.Checked = this.parentForm.settings.ShowScreenID;
            this.cbStartMinimized.Checked = this.parentForm.settings.StartMinimized;
            this.cbShowSplash.Checked = Properties.Settings.Default.ShowSplash;
            this.cbPortable.Checked = Properties.Settings.Default.Portable;
            this.cbAdjustTaskbarColour.Checked = this.parentForm.settings.AdjustTaskbarColour;
            this.cbAdjustInterfaceColour.Checked = this.parentForm.settings.AdjustInterfaceColour;
            this.cbCheckForUpdates.Checked = this.parentForm.settings.CheckForUpdate;
            this.cbShowToolTips.Checked = this.parentForm.settings.ShowToolTips;
            this.parentForm.toolTip1.Active = this.parentForm.settings.ShowToolTips;
            this.parentForm.frmSettingsAdvanced.toolTip1.Active = this.parentForm.settings.ShowToolTips;
            this.cbWriteToErrorLog.Checked = this.parentForm.settings.WriteToLog;
            this.cbBlurImageEdges.Checked = this.parentForm.settings.BlurImageEdges;
            this.tbGlowAmount.Value = this.parentForm.settings.BlurAmount;
            this.numAspectMin.Value = (decimal)this.parentForm.settings.AspectMin;
            this.numAspectMax.Value = (decimal)this.parentForm.settings.AspectMax;
            this.numWideAspectThreshold.Value = (decimal)this.parentForm.settings.AspectThresholdWide;
            this.numNarrowAspectThreshold.Value = (decimal)this.parentForm.settings.AspectThresholdNarrow;
            this.numMarginEnlarge.Value = (int)this.parentForm.settings.MarginToEnlarge;
            this.numWideThresholdPercent.Value = parentForm.settings.WideThreshold;
            this.cbRandomFlipImage.Checked = this.parentForm.settings.RandomFlipImage;
            this.txtLatitude.Text = parentForm.settings.Latitude.ToString(CultureInfo.InvariantCulture);
            this.txtLongitude.Text = parentForm.settings.Longitude.ToString(CultureInfo.InvariantCulture);
            this.cbAutoBackup.Checked = parentForm.settings.AutoBackup;
            this.txtOffsetMins.Text = parentForm.settings.TimeOffsetMins.ToString(CultureInfo.InvariantCulture);
            this.txtSearch.Text = parentForm.settings.LocationPreset.ToString(CultureInfo.InvariantCulture);
            this.lblCityFound.Text = "Found: " + parentForm.settings.LocationPreset.ToString(CultureInfo.InvariantCulture);
            this.cbMultiMonitorMode.SelectedItem = this.parentForm.settings.MultiMonitorMode.ToString();
            this.cbCheckSensorsOnStartup.Checked = this.parentForm.settings.CheckSensorsOnStartup;

            this.cbKey1.SelectedItem = this.parentForm.settings.ShortcutKey1;
            this.cbKey2.SelectedItem = this.parentForm.settings.ShortcutKey2;
            //this.cbKey3.SelectedItem = this.parentForm.settings.ShortcutKey3;
            this.cbKey3.SelectedIndex = cbKey3.FindStringExact(this.parentForm.settings.ShortcutKey3);

            this.cbUseSunriseSunset.Checked = this.parentForm.settings.UseSunriseSunset;
            this.cbUseSunriseSunset2.Checked = this.parentForm.settings.UseSunriseSunset;

            numImageSizeScalePercent.Value = (int)(this.parentForm.settings.ImageSizeScalePercent);

            String aspect = string.Empty;
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                int x = Screen.AllScreens[i].Bounds.Width;
                int y = Screen.AllScreens[i].Bounds.Height;
                float aspectX = (x / ScreenFunctions.LowestCommonDivisor(x, y));
                float aspectY = (y / ScreenFunctions.LowestCommonDivisor(x, y));
                aspect += string.Format(CultureInfo.InvariantCulture, "Screen #{0}:  {1}:{2} = {3}\n", i, aspectX, aspectY, Math.Round((aspectX / aspectY), 2));
            }

            lblAspectCurrent.Text = aspect;

            setWinDarkModeText(ScreenFunctions.isWinDarkModeEnabled());
            setWinTransparencyText(ScreenFunctions.isWinTransparencyEnabled());

            SetScreenImageCheckBoxes();
            SetBlurControls();
            setMultiMonitorControls();
            
            setHWSensorsUsed();

            UpdateImageAdjustSecs();

            //Timer Test stuff
            SetTimerTestValues();
            SetTimerState();

            GetSunriseSunset();
            SetLightDarkTimeControls();

            UpdateFilteredFileCount();
            SetHoursImages();

            SetFiltersState();
            SetUseFilters();

            //hide/show the cb for quality when JPG selected
            setModeSelection(); // set the visibility of extra settings
            setQualityDD();

            parentForm.applyingSettings = false;
        }

        // set the controls to be what is in Settings
        internal void setHWSensorsUsed()
        {
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("FAN")) cbHWFan.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("GPU")) cbHWGpu.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("CPU")) cbHWCpu.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("HDD")) cbHWHDD.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("MAINBOARD")) cbHWMotherboard.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("RAM")) cbHWRAM.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("POWER")) cbPower.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("WEATHER")) cbWeather.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("LOCATION")) cbLocation.Checked = true;
            if (parentForm.settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("INTERNET")) cbInternet.Checked = true;
        }

        private void SetLightDarkTimeControls()
        {
            this.cbDarkHour.SelectedItem = this.parentForm.settings.DarkSunsetTime.Hour.ToString("D2", CultureInfo.InvariantCulture);
            this.cbDarkMin.SelectedItem = this.parentForm.settings.DarkSunsetTime.Minute.ToString("D2", CultureInfo.InvariantCulture);
            this.cbLightHour.SelectedItem = this.parentForm.settings.LightSunriseTime.Hour.ToString("D2", CultureInfo.InvariantCulture);
            this.cbLightMin.SelectedItem = this.parentForm.settings.LightSunriseTime.Minute.ToString("D2", CultureInfo.InvariantCulture);
        }

        internal void SetPreviewBMP(Bitmap vBitmap)
        {
            this.pbPreviewImage.BackColor = parentForm.GetCurrentColour();

            if (bmpPreview != null)
                this.bmpPreview.Dispose();
            if (this.pbPreviewImage.Image != null) this.pbPreviewImage.Image.Dispose();

            this.bmpPreview = (Bitmap)vBitmap.Clone();
            this.pbPreviewImage.Image = (Image)vBitmap.Clone();
            vBitmap.Dispose();

        }




        internal void SetHoursImages()
        {
            int mins = parentForm.TotalMinutesOfImages();

            //TimeSpan t = TimeSpan.FromMinutes(mins);
            //string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
            //                t.Hours,
            //                t.Minutes,
            //                t.Seconds,
            //                t.Milliseconds);

            // calc assumes setting "SameOnAll"
            if (parentForm.settings.MultiMonitorMode == MultiMonitorModes.DifferentOnAll)
            {
                mins /= Screen.AllScreens.Length;
            }            
            //int days = mins / 1440;
            //int hours = (mins % 1440) / 60;
            //int mins = mins % 60;            
            
            TimeSpan t = TimeSpan.FromMinutes(mins);

            string answer = string.Format(CultureInfo.InvariantCulture, "You have {0:D2}d:{1:D2}h:{2:D2}m of wallpapers",
                            t.Days,
                            t.Hours,
                            t.Minutes
                            );

            this.lblImageTime.Text = answer;
        }

        private void SetTimerState()
        {
            if (parentForm.settings.UseSunriseSunset == false)
            {
                this.lblStartTime.Enabled = cbUseDarkLightTimes.Checked;
                this.cbDarkHour.Enabled = cbUseDarkLightTimes.Checked;
                this.cbDarkMin.Enabled = cbUseDarkLightTimes.Checked;

                this.lblEndTime.Enabled = cbUseDarkLightTimes.Checked;
                this.cbLightHour.Enabled = cbUseDarkLightTimes.Checked;
                this.cbLightMin.Enabled = cbUseDarkLightTimes.Checked;

                lblIsOvernight.Enabled = cbUseDarkLightTimes.Checked;

            }
            else
            {
                // Use Auto Values from Sunrise/Sunset calculation
                this.lblStartTime.Enabled = false;
                this.cbDarkHour.Enabled = false;
                this.cbDarkMin.Enabled = false;

                this.lblEndTime.Enabled = false;
                this.cbLightHour.Enabled = false;
                this.cbLightMin.Enabled = false;

                lblIsOvernight.Enabled = false;

            }


            this.lblDuration.Enabled = cbUseDarkLightTimes.Checked;
            this.lblDurationMins.Enabled = cbUseDarkLightTimes.Checked;
            this.cbDuration.Enabled = cbUseDarkLightTimes.Checked;

        }

        private void SetTimerTestValues()
        {
            DateTime timeEndDimming = this.parentForm.settings.DarkSunsetTime.AddMinutes(this.parentForm.settings.DurationMins);
            this.lblTimerTestStart.Text = String.Format(CultureInfo.InvariantCulture, "Start {0}:{1}", this.parentForm.settings.DarkSunsetTime.Hour.ToString("D2", CultureInfo.InvariantCulture), this.parentForm.settings.DarkSunsetTime.Minute.ToString("D2", CultureInfo.InvariantCulture));
            this.lblTimerTestEnd.Text = String.Format(CultureInfo.InvariantCulture, "End {0}:{1}", timeEndDimming.Hour.ToString("D2", CultureInfo.InvariantCulture), timeEndDimming.Minute.ToString("D2", CultureInfo.InvariantCulture));
            this.tbTimerTest.Maximum = this.parentForm.settings.DurationMins;
            this.tbTimerTest.Value = this.parentForm.settings.DurationMins; // start at the END, and let the user scroll back in time
            SetCurrentTestTime();

            EnableTest(cbTimeTest.Checked);
        }

        private void EnableTest(Boolean vEnable)
        {
            
            this.tbTimerTest.Enabled = vEnable;
            
            this.tbAdjustAlpha.Enabled = !vEnable;
            this.tbAdjustContrast.Enabled = !vEnable;
            this.tbAdjustGamma.Enabled = !vEnable;
            this.tbAdjustBrightness.Enabled = !vEnable;
            this.tbAjustTintStrength.Enabled = !vEnable;

            this.cbResetImageOptions.Enabled = !vEnable;
            this.lblResetAdjustments.Enabled = !vEnable;

            // other stuff
            cbDarkHour.Enabled = !vEnable;
            cbDarkMin.Enabled = !vEnable;
            cbDuration.Enabled = !vEnable;

            cbLightHour.Enabled = !vEnable;
            cbLightMin.Enabled = !vEnable;

            cbUseDarkLightTimes.Enabled = !vEnable;

            cbPreview.Enabled = !vEnable;

            if (vEnable == true)
            {
                cbPreview.Checked = false;
            }
            else
            {
                cbPreview.Checked = true;
            }

            if (vEnable)
            {
                // if enabled, switch slider to start of time
                this.tbTimerTest.Value = this.tbTimerTest.Minimum;
                if (!parentForm.applyingSettings)
                    SetTestTimeSliderValues();
            }
            else
            {
                // switch slider back to end time so we are seeing the minimum values in the preview
                this.tbTimerTest.Value = this.tbTimerTest.Maximum;
                if (!parentForm.applyingSettings)
                    ClearTestTimeSliderValues();

            }
            SetPreview();

            if (!parentForm.applyingSettings && cbTimeTest.Checked )
            {
                if (parentForm.settings.ShowToolTips) MessageBox.Show("Test Image Adjustment settings:\n\nUse the time slider to see how the current settings will appear over the transition periods.", "Test Image Adjustment settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.testingTime = vEnable;
        }

        private void SetScreenGammaText()
        {
            this.tbScreenGamma.Value = this.parentForm.settings.ScreenGamma;
            this.lblScreenGamma.Text = (tbScreenGamma.Value).ToString(CultureInfo.InvariantCulture);

        }

        private void SetBrightnessLevelsText()
        {
            tbBrightnessMax.Value = this.parentForm.settings.BrightnessMax;
            lblBrightnessMax.Text = tbBrightnessMax.Value.ToString(CultureInfo.InvariantCulture);

            tbBrightnessMin.Value = this.parentForm.settings.BrightnessMin;
            lblBrightnessMin.Text = tbBrightnessMin.Value.ToString(CultureInfo.InvariantCulture);

        }


        private void SetTintText()
        {
            lblTintStrength.Text = ((this.tbAjustTintStrength.Value)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void SetGammaText()
        {
            lblAdjustGamma.Text = ((tbAdjustGamma.Value)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void SetAlphaText()
        {
            lblAdjustAlpha.Text = ((tbAdjustAlpha.Value)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void SetBrightnessText()
        {
            lblAdjustBrightness.Text = ((tbAdjustBrightness.Value)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void SetContrastText()
        {
            this.lblAdjustContrast.Text = ((tbAdjustContrast.Value)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void SetCurrentTestTime()
        {
            DateTime testTime = this.parentForm.settings.DarkSunsetTime.AddMinutes(tbTimerTest.Value);
            this.lblCurrentTime.Text = String.Format(CultureInfo.InvariantCulture, "{0}:{1}", testTime.Hour.ToString("D2", CultureInfo.InvariantCulture), testTime.Minute.ToString("D2", CultureInfo.InvariantCulture));
        }

        private void SetTestTimeSliderValues()
        {
            if (tbTimerTest.Value < 5)
            {
                tbTimerTest.Value = 0;
            }
            if (tbTimerTest.Value > tbTimerTest.Maximum - 5)
            {
                tbTimerTest.Value = tbTimerTest.Maximum;
            }

            float percent = tbTimerTest.Value / (float)this.parentForm.settings.DurationMins;

            percent = 1 - percent;

            // TINT
            int tintChange = (int)((this.parentForm.settings.TintStrength) * (float)Math.Round((percent), 1));
            int tintSlider = this.parentForm.settings.TintStrength - tintChange;
            tbAjustTintStrength.Value = (int)tintSlider;
            SetTintText();

            // BRIGHTNESS
            int brightnessChange = (int)((this.parentForm.settings.Brightness) * (percent));
            int brightnessSlider = (int)((this.parentForm.settings.Brightness) - (brightnessChange));
            tbAdjustContrast.Value = brightnessSlider;
            SetBrightnessText();

            // ALPHA
            int alphaChange = (int)((this.parentForm.settings.Alpha) * (percent));
            int alphaSlider = (this.parentForm.settings.Alpha) - alphaChange;
            tbAdjustAlpha.Value = (int)alphaSlider;
            SetAlphaText();


            // CONTRAST
            int contrastChange = (int)((parentForm.settings.Contrast) * percent);
            int contrastSlider = (parentForm.settings.Contrast) - contrastChange;
            tbAdjustBrightness.Value = (int)contrastSlider;
            SetContrastText();


            // GAMMA
            int gammaChange = (int)((this.parentForm.settings.Gamma - 100) * percent);
            int gammaSlider = (int)((this.parentForm.settings.Gamma) - (gammaChange) ); //100 = the Neutral / no change value for gamma
            tbAdjustGamma.Value = gammaSlider;
            SetGammaText();

            SetAdjustPreviewImage(true, !this.cbTimeTest.Checked);

        }

        private void SetAdjustPreviewImage(Boolean vDoImageAdjust, Boolean vSetSettings)//, Bitmap vPreview)
        {
            Application.DoEvents();
            DoAdjustPreviewImage(vDoImageAdjust, vSetSettings);
            Application.DoEvents();
        }

        /// <summary>
        /// Thread version not used atm, as it is unnecessary - changes happen on preview SIZED images very quickly. 
        /// The thread sometimes failed to complete and caused it to stop responding / working
        /// </summary>
        /// <param name="vDoImageAdjust"></param>
        /// <param name="vSetSettings"></param>
        private void SetAdjustPreviewImageThread(Boolean vDoImageAdjust, Boolean vSetSettings)//, Bitmap vPreview)
        {
            if (parentForm.formClosing) return;

            //if (threadAdjustImage != null && threadAdjustImage.ThreadState == System.Threading.ThreadState.Running) // should be finished quickly
            //{
            //    //threadAdjustImage.Join();
            //    //threadAdjustImage.Abort();
            //}

            // Start a new thread only if the thread is stopped 
            // or the thread has not been created yet.
            if (threadAdjustImage == null || threadAdjustImage.ThreadState == System.Threading.ThreadState.Stopped)
            {
                threadAdjustImage = new Thread(() => DoAdjustPreviewImage(vDoImageAdjust, vSetSettings))
                {
                    Name = "Adjust Image",
                    Priority = MainFunctions.threadPriority //ThreadPriority.AboveNormal
                };
                threadAdjustImage.Start();                
            } 
        }


        private void DoAdjustPreviewImage(Boolean vDoImageAdjust, Boolean vSetSettings)
        {
            if (parentForm.formClosing) return;

            try
            {
                int contrast = 0;
                int gamma = 100;
                int brightness = 0;
                int alpha = 0;
                int tint = 0;
                Boolean timerTestEnabled = false;

                float percent = 1.0f;

                if (this.tbTimerTest.InvokeRequired)
                {
                    this.tbTimerTest.Invoke(new MethodInvoker(delegate {
                        percent = this.tbTimerTest.Value / (float)this.tbTimerTest.Maximum;
                        timerTestEnabled = cbTimeTest.Checked;
                    }));
                }
                else
                {
                    percent = this.tbTimerTest.Value / (float)this.tbTimerTest.Maximum;
                    timerTestEnabled = cbTimeTest.Checked;
                }

                if (!timerTestEnabled) percent = 1.0f;

                parentForm.Invoke(new MethodInvoker(delegate () {
                    brightness = tbAdjustBrightness.Value;
                }));
                parentForm.Invoke(new MethodInvoker(delegate () {
                    SetBrightnessText();
                }));
                if (vSetSettings && !parentForm.applyingSettings && !testingTime)
                    this.parentForm.settings.Brightness = brightness;


                parentForm.Invoke(new MethodInvoker(delegate () {
                    contrast = tbAdjustContrast.Value;
                }));
                parentForm.Invoke(new MethodInvoker(delegate () {
                    SetContrastText();
                }));
                if (vSetSettings && !parentForm.applyingSettings && !testingTime) this.parentForm.settings.Contrast = contrast;


                parentForm.Invoke(new MethodInvoker(delegate () {
                    gamma = tbAdjustGamma.Value;
                }));
                parentForm.Invoke(new MethodInvoker(delegate () {
                    SetGammaText();
                }));
                if (vSetSettings && !parentForm.applyingSettings && !testingTime)
                    this.parentForm.settings.Gamma = gamma;


                parentForm.Invoke(new MethodInvoker(delegate () {
                    alpha = tbAdjustAlpha.Value;

                }));
                parentForm.Invoke(new MethodInvoker(delegate () {
                    SetAlphaText();
                }));
                if (vSetSettings && !parentForm.applyingSettings && !testingTime)
                    this.parentForm.settings.Alpha = alpha;


                parentForm.Invoke(new MethodInvoker(delegate () {
                    tint = tbAjustTintStrength.Value;
                }));
                parentForm.Invoke(new MethodInvoker(delegate () {
                    SetTintText();
                }));
                if (vSetSettings && !parentForm.applyingSettings && !testingTime) this.parentForm.settings.TintStrength = tint;

                Bitmap preview = (Bitmap)ImageFunctions.LoadImage(parentForm.settings.ImageLastSelected, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                if (preview == null)
                {
                    preview = (Bitmap)ImageFunctions.LoadImage(FrmMain.GetSampleImagePath(), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
                if (vDoImageAdjust)
                {

                    if (timerTestEnabled)
                    {
                        Color currentColour = FrmMain.CalcCurrentColour(parentForm.settings.BackgroundColourLight, parentForm.settings.BackgroundColourDark, percent);

                        float ratio = (float)pbPreviewImage.Height / (float)preview.Height;

                        preview = ImageFunctions.ResizeBitmap(preview, ratio, InterpolationMode.Low); //HighQualityBicubic

                        //ImageFunctions.AdjustImage(ref preview, -1 * (brightness / 100f), 1f - (contrast / 100f), gamma / 100f);// contrast / 100f, gamma / 100f);
                        //ImageFunctions.ImageTint(ref preview, parentForm.settings.TintColour, percent, parentForm.settings.TintStrength); //
                        //ImageFunctions.SetImageOpacity(ref preview, 1f - (alpha / 100f));
                        ImageFunctions.AdjustImage(ref preview, (1f - (brightness / 100f)), (1f - (contrast / 100f)), gamma / 100f);
                        ImageFunctions.ImageTint(ref preview, parentForm.settings.TintColour, percent, parentForm.settings.TintStrength); //
                        ImageFunctions.SetImageOpacity(ref preview, 1f - (alpha / 100f));

                        Bitmap b = new Bitmap(preview.Width, preview.Height);
                        using (Graphics g = Graphics.FromImage(b))
                        {
                            g.Clear(currentColour);
                            Rectangle r = new Rectangle(0, 0, preview.Width, preview.Height);
                            g.DrawImage(preview, r, 0, 0, preview.Width, preview.Height, GraphicsUnit.Pixel);
                        }

                        preview = b;

                        if (this.pbPreviewImage.Image != null) this.pbPreviewImage.Image.Dispose();
                        this.pbPreviewImage.Image = (Bitmap)preview.Clone();
                        this.pbPreviewImage.BackColor = currentColour;

                        preview.Dispose();
                        preview = null;
                       
                    }
                    else
                    {

                        if (preview != null)
                        {
                            Color darkTimeColour = parentForm.settings.BackgroundColourDark;
                            float ratio = (float)pbPreviewImage.Height / (float)preview.Height;
                            preview = ImageFunctions.ResizeBitmap(preview, ratio, InterpolationMode.Low); //HighQualityBicubic

                            ImageFunctions.AdjustImage(ref preview, (1f - (brightness / 100f)), (1f - (contrast / 100f)), gamma / 100f);
                            ImageFunctions.ImageTint(ref preview, parentForm.settings.TintColour, percent, parentForm.settings.TintStrength); //
                            ImageFunctions.SetImageOpacity(ref preview, 1f - (alpha / 100f));

                            Bitmap b = new Bitmap(preview.Width, preview.Height);
                            using (Graphics g = Graphics.FromImage(b))
                            {
                                g.Clear(darkTimeColour);

                                Rectangle r = new Rectangle(0, 0, preview.Width, preview.Height);
                                g.DrawImage(preview, r, 0, 0, preview.Width, preview.Height, GraphicsUnit.Pixel);
                            }

                            if (preview != null) preview.Dispose();
                            preview = b;



                            if (this.pbPreviewImage.Image != null) this.pbPreviewImage.Image.Dispose();
                            this.pbPreviewImage.BackColor = darkTimeColour;
                            this.pbPreviewImage.Image = (Bitmap)preview.Clone();
                            preview.Dispose();

                        }
                    }
                }
                else
                {
                    // dont adjust
                    if (this.pbPreviewImage.Image != null) this.pbPreviewImage.Image.Dispose();
                    this.pbPreviewImage.Image = (Image)preview.Clone();
                    this.bmpPreview = (Bitmap)preview.Clone();
                    preview.Dispose();
                }
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException)
            {
                return;
            }

        }


        /// <summary>
        /// This will APPLY THE IMAGE OPTION SETTINGS from the values in the Settings held by the parent form
        /// </summary>
        internal void ClearTestTimeSliderValues()
        {
            // ALPHA
            tbAdjustAlpha.Value = (int)(this.parentForm.settings.Alpha);
            SetAlphaText();

            // BRIGHTNESS
            tbAdjustContrast.Value = (int)(this.parentForm.settings.Brightness);
            SetBrightnessText();

            // CONTRAST
            tbAdjustBrightness.Value = (int)(this.parentForm.settings.Contrast);
            SetContrastText();


            // GAMMA
            tbAdjustGamma.Value = (int)(this.parentForm.settings.Gamma);
            SetGammaText();

            // TINT
            this.tbAjustTintStrength.Value = (int)(this.parentForm.settings.TintStrength);
            SetTintText();

            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); //showAdjustmentsPreview

        }


        private void UpdateImageAdjustSecs()
        {
            if (!parentForm.applyingSettings)
            {

                int secs = (int)(numImageAdjustFreqSecs.Value);
                parentForm.settings.ImageAdjustFrequencySecs = secs;

                parentForm.SetNextWallpaperAdjustTime();
            }
            else
            {
                numImageAdjustFreqSecs.Value = parentForm.settings.ImageAdjustFrequencySecs;
            }
        }


        internal void SetScreenImageCheckBoxes()
        {
            clbShowScreenImage.ItemCheck -= ClbShowScreenImage_ItemCheck;

            // has the screen count changed
            Boolean updateScreenSettings = parentForm.settings.ExtraScreenInfo.Count != Screen.AllScreens.Length;

            if (updateScreenSettings) parentForm.settings.ExtraScreenInfo.Clear();

            clbShowScreenImage.Items.Clear();
            lvScreenScales.Items.Clear();
            
            for (int index = 0; index < Screen.AllScreens.Length; index++)
            {
                int x = Screen.AllScreens[index].Bounds.Width;
                int y = Screen.AllScreens[index].Bounds.Height;
                string aspect = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", (x / ScreenFunctions.LowestCommonDivisor(x, y)), (y / ScreenFunctions.LowestCommonDivisor(x, y)));
                if (updateScreenSettings)
                {
                    clbShowScreenImage.Items.Add("Screen #" + index.ToString(CultureInfo.InvariantCulture) + " - " + aspect, true);
                    lvScreenScales.Items.Add("Screen #" + index.ToString(CultureInfo.InvariantCulture) + "  100%");
                }
                else
                {
                    clbShowScreenImage.Items.Add("Screen #" + index.ToString(CultureInfo.InvariantCulture) + " - " + aspect, parentForm.settings.ExtraScreenInfo[index].ShowImageOnScreen);
                    lvScreenScales.Items.Add("Screen #" + index.ToString(CultureInfo.InvariantCulture) + "  " + Math.Round((parentForm.settings.ExtraScreenInfo[index].Scale * 100), 2).ToString(CultureInfo.InvariantCulture) + "%");
                }

                //default scale of 1.0f = 100% and default to show the image on all screens
                ScreenInfoExtra info = new ScreenInfoExtra
                {
                    Scale = 1.0f,
                    ShowImageOnScreen = true
                };

                if (updateScreenSettings) parentForm.settings.ExtraScreenInfo.Add(info);
            }

            clbShowScreenImage.ItemCheck += ClbShowScreenImage_ItemCheck;

            return;
            
        }


        private void CheckAutoRunState()
        {
            RegistryKey rkApp;

            rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //this.GetType().Assembly.GetName().Name.ToString(CultureInfo.InvariantCulture) == APP NAME
            if (rkApp.GetValue(this.GetType().Assembly.GetName().Name.ToString(CultureInfo.InvariantCulture)) == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                this.cbAutoStart.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                this.cbAutoStart.Checked = true;

            }

            rkApp.Close();

        }

        private void CbPreview_CheckedChanged(object sender, EventArgs e)
        {
            SetPreview();
        }


        internal void SetPreview()
        {
            if (this.cbTimeTest.Checked == false)
            {

                if (cbPreview.Checked) 
                {
                    SetAdjustPreviewImage(true, !this.cbTimeTest.Checked);
                }
                else
                {
                    SetAdjustPreviewImage(false, !this.cbTimeTest.Checked);
                }
            }
        }



        internal void SetView()
        {
            DrawEventList();

            SetBrightnessLevelsText();
            UpdateFilteredFileCount();
            SetHoursImages();

            SetUseSunriseSunset();

            this.tbAjustTintStrength.Value = (int)(this.parentForm.settings.TintStrength);
            SetTintText();

            tbAdjustGamma.Value = (int)(this.parentForm.settings.Gamma);
            SetGammaText();

            tbAdjustContrast.Value = (int)(this.parentForm.settings.Contrast);
            SetBrightnessText();

            tbAdjustAlpha.Value = (int)(this.parentForm.settings.Alpha);
            SetAlphaText();

            tbAdjustBrightness.Value = (int)(this.parentForm.settings.Brightness);
            SetContrastText();

            SetScreenGammaText();

        }


        internal void PickColourDark()
        {
            System.Windows.Forms.ColorDialog colorPicker = new ColorDialog
            {
                // Allows the user to get help. (The default is false.)
                ShowHelp = true,
                // Sets the initial color select to the current text color.
                Color = this.parentForm.settings.BackgroundColourDark
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                pnlBackgroundColourDark.BackColor = colorPicker.Color;
                this.parentForm.settings.BackgroundColourDark = colorPicker.Color;
            }
        }

        internal void PickColourLight()
        {
            System.Windows.Forms.ColorDialog colorPicker = new ColorDialog
            {
                // Allows the user to get help. (The default is false.)
                ShowHelp = true,
                // Sets the initial color select to the current text color.
                Color = this.parentForm.settings.BackgroundColourLight
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                this.parentForm.settings.BackgroundColourLight = colorPicker.Color;
                pnlBackgroundColourLight.BackColor = colorPicker.Color;

                pbPreviewImage.BackColor = parentForm.GetCurrentColour();
            }
        }

        internal void PickColourTint()
        {
            System.Windows.Forms.ColorDialog colorPicker = new ColorDialog
            {
                // Allows the user to get help. (The default is false.)
                ShowHelp = true,
                // Sets the initial color select to the current text color.
                Color = this.parentForm.settings.TintColour
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                this.pnlColourTint.BackColor = colorPicker.Color;
                this.parentForm.settings.TintColour = colorPicker.Color;

            }
        }

        private void CbStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.StartMinimized = this.cbStartMinimized.Checked;
            this.parentForm.frmSettings.cbStartMinimized.Checked = this.cbStartMinimized.Checked;
        }

        private void CbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            parentForm.SetStartAutomatically(cbAutoStart.Checked);
            parentForm.frmSettings.cbAutoStart.Checked = cbAutoStart.Checked;
        }


        private void CbUseAdjustmentsAlways_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.AdjustmentsUseAlways = cbUseAdjustmentsAlways.Checked;
            SetUseAdjustmentAlways();
        }

        internal void SetUseAdjustmentAlways()
        {
            if (this.parentForm.settings.AdjustmentsUseAlways)
            {
                this.cbUseDarkLightTimes.Enabled = false;
                this.cbUseDarkLightTimes.Checked = false;
            }
            else
            {
                this.cbUseDarkLightTimes.Enabled = true;
            }

            parentForm.UpdateScreenState();
            SetTimerState();

            if (!parentForm.applyingSettings)
            {
                parentForm.AdjustDesktopImages(); 
            }
        }

        private void CbUseTimer_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.UseDarkLightTimes = cbUseDarkLightTimes.Checked;

            parentForm.UpdateScreenState();
            SetTimerState();

            if (!parentForm.applyingSettings)
            {
                parentForm.AdjustDesktopImages();
            }
        }

        private void CbImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.WallpaperOrder = (Wallpaper.ImageOrder)Enum.Parse(typeof(Wallpaper.ImageOrder), cbImageOrder.SelectedItem.ToString()); //cbImageOrder.SelectedItem.ToString(CultureInfo.InvariantCulture);
        }

        private void NumWallpaperChangeMins_ValueChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                UpdateWallpaperChangeMins();
                SetHoursImages();

                parentForm.applyingSettings = true;
                parentForm.frmSettings.numWallpaperChangeMins.Value = (int)numWallpaperChangeMins.Value;
                parentForm.applyingSettings = false;
            }
        }

        internal void UpdateWallpaperChangeMins()
        {

            try
            {               
               parentForm.settings.WallpaperChangeFrequencyMins = (int)numWallpaperChangeMins.Value; // mins;

                parentForm.SetNextWallpaperChangeTime();
                parentForm.SetNextWallpaperAdjustTime();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("You must enter a number only.  Error: " + ex.Message, "Numbers only", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CbChangeOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.ChangeOnStartup = cbChangeOnStartup.Checked;
        }

        private void tbBrightnessMin_Scroll(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                FormChanged = true;
                DoBrightnessMinAdjustment();

                // if we adjust the slider to make the MIN more than the MAX, then we need to increase the max
                if (tbBrightnessMin.Value >= tbBrightnessMax.Value)
                {
                    // if the MAX brightness adjustment is going to be MORE than its maximum, then put up a message
                    if (tbBrightnessMax.Value + 1 > tbBrightnessMax.Maximum)
                    {
                        MessageBox.Show("The Minimum Brightness must be LESS than the Maximum Brightness", "Min Must Be Less Than Max", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbBrightnessMax.Value = tbBrightnessMax.Maximum;

                        tbBrightnessMin.Value = tbBrightnessMax.Value - 1;
                        DoBrightnessMinAdjustment();
                    }
                    else
                    {
                        // increase the max value by one more than the min
                        tbBrightnessMax.Value = tbBrightnessMin.Value + 1;
                    }

                    DoBrightnessMinAdjustment();
                }
            }

        }

        private void DoBrightnessMinAdjustment()
        {
            lblBrightnessMin.Text = tbBrightnessMin.Value.ToString(CultureInfo.InvariantCulture);
            this.parentForm.settings.BrightnessMin = tbBrightnessMin.Value;

            SetBrightnessLevelsText(); 
        }


        private void tbBrightnessMax_Scroll(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {

                FormChanged = true;
                DoBrightnessMaxAdjustment();

                // if we adjust the slider to make the MAX LESS than the MIN, then we need to increase the max
                if (tbBrightnessMax.Value <= tbBrightnessMin.Value)
                {
                    // if the MAX brightness adjustment is going to be MORE than its maximum, then put up a message
                    if (tbBrightnessMin.Value - 1 < tbBrightnessMin.Minimum)
                    {
                        MessageBox.Show("The Maximum Brightness must be MORE than the Minimum Brightness", "Brightness: Max Must Be More Than Min", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbBrightnessMin.Value = tbBrightnessMin.Minimum;

                        tbBrightnessMax.Value = tbBrightnessMin.Value + 1;
                        DoBrightnessMaxAdjustment();
                    }
                    else
                    {
                        // increase the max value by one more than the min
                        tbBrightnessMin.Value = tbBrightnessMax.Value - 1;
                    }

                    DoBrightnessMinAdjustment();
                }
            }
        }

        private void DoBrightnessMaxAdjustment()
        {
            lblBrightnessMax.Text = tbBrightnessMax.Value.ToString(CultureInfo.InvariantCulture);
            this.parentForm.settings.BrightnessMax = tbBrightnessMax.Value;

            SetBrightnessLevelsText(); 
        }

        private void numWidth_ValueChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;
                this.parentForm.settings.MinImageWidth = (int)this.numWidth.Value;
            }
        }

        private void numHeight_ValueChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;
                this.parentForm.settings.MinImageHeight = (int)this.numHeight.Value;
            }
        }

        private void TbScreenGamma_Scroll(object sender, EventArgs e)
        {
            this.lblScreenGamma.Text = tbScreenGamma.Value.ToString(CultureInfo.InvariantCulture);
            this.parentForm.settings.ScreenGamma = tbScreenGamma.Value;

            SetScreenGammaText();
        }

        private void CbShowScreenID_CheckedChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                parentForm.settings.ShowScreenID = cbShowScreenID.Checked;
                parentForm.AdjustDesktopImages();
            }
        }

        private void CbShowToolTips_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.ShowToolTips = cbShowToolTips.Checked;
            this.parentForm.toolTip1.Active = cbShowToolTips.Checked;
            this.parentForm.frmSettingsAdvanced.toolTip1.Active = cbShowToolTips.Checked;
        }

        private void TbTintStrength_Scroll(object sender, EventArgs e)
        {
            // snap to the default values on either side of the slider
            if (tbAjustTintStrength.Value < 5)
            {
                tbAjustTintStrength.Value = 0; 
            }
            if (tbAjustTintStrength.Value > 45 && tbAjustTintStrength.Value < 55)
            {
                tbAjustTintStrength.Value = 50;
            }
            if (tbAjustTintStrength.Value > 95)
            {
                tbAjustTintStrength.Value = 100;
            }
            if (!testingTime) parentForm.settings.TintStrength = tbAjustTintStrength.Value;
            SetTintText();
            
            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);
        }

        private void TbAdjustBrightness_Scroll(object sender, EventArgs e)
        {
            // snap to the default value 1
            if (tbAdjustBrightness.Value < 5)
            {
                tbAdjustBrightness.Value = 0;
            }
            if (tbAdjustBrightness.Value > 45 && tbAdjustBrightness.Value < 55)
            {
                tbAdjustBrightness.Value = 50;
            }
            if (tbAdjustBrightness.Value > 95)
            {
                tbAdjustBrightness.Value = 100;
            }
            if (!testingTime)
                parentForm.settings.Brightness = tbAdjustBrightness.Value;
            SetBrightnessText();

            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); //showAdjustmentsPreview

        }

        private void TbAdjustGamma_Scroll(object sender, EventArgs e)
        {
            // snap to the default value
            
            if (tbAdjustGamma.Value < 5)
            {
                tbAdjustGamma.Value = 0;
            }
            if (tbAdjustGamma.Value > 95 && tbAdjustGamma.Value < 105)
            {
                tbAdjustGamma.Value = 100;
            }
            if (tbAdjustGamma.Value > 195 && tbAdjustGamma.Value < 205)
            {
                tbAdjustGamma.Value = 200;
            }
            if (tbAdjustGamma.Value > 295 && tbAdjustGamma.Value < 305)
            {
                tbAdjustGamma.Value = 300;
            }

            if (!testingTime) parentForm.settings.Gamma = tbAdjustGamma.Value;
            SetGammaText();

            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked);
        }

        private void TbAdjustAlpha_Scroll(object sender, EventArgs e)
        {
            // snap to the default value
            if (tbAdjustAlpha.Value < 5)
            {
                tbAdjustAlpha.Value = 0;
            }
            if (tbAdjustAlpha.Value > 95)
            {
                tbAdjustAlpha.Value = 100;
            }
            if (!testingTime) parentForm.settings.Alpha = (tbAdjustAlpha.Value);
            SetAlphaText();

            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); 
        }

        private void TbAdjustContrast_Scroll(object sender, EventArgs e)
        {
            // snap to the default value 1
            if (tbAdjustContrast.Value > 95)
            {
                tbAdjustContrast.Value = 100;
            }
            if (tbAdjustContrast.Value < 5)
            {
                tbAdjustContrast.Value = 0;
            }
            if (tbAdjustContrast.Value > 95)
            {
                tbAdjustContrast.Value = 100;
            }
            if (!testingTime) parentForm.settings.Contrast =  tbAdjustContrast.Value;
            SetContrastText();

            SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); 
        }

        private void CbTimeTest_CheckedChanged(object sender, EventArgs e)
        {
            SetTimerTestValues();
        }

        private void TbTimerTest_Scroll(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                SetCurrentTestTime();
                SetTestTimeSliderValues();
            }
        }

        private void CbDarkHour_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetDarkSunsetTime();

            if (!parentForm.applyingSettings && !testingTime)
            {
                parentForm.AdjustDesktopImages(); 
            }
        }

        private void CbDarkMin_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetDarkSunsetTime();

            if (!parentForm.applyingSettings && !testingTime)
            {
                parentForm.AdjustDesktopImages();
            }
        }

        private void CbLightHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetLightSunriseTime();

            if (!parentForm.applyingSettings && !testingTime)
            {
                parentForm.AdjustDesktopImages(); 
            }
        }

        private void CbLightMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetLightSunriseTime();

            if (!parentForm.applyingSettings && !testingTime)
            {
                parentForm.AdjustDesktopImages(); 
            }
        }

        internal void SetDarkSunsetTime()
        {

            if (!parentForm.applyingSettings && !testingTime)
            {
                this.parentForm.settings.DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(cbDarkHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(cbDarkMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            }
            setOvernight();
        }

        private void setOvernight()
        {
            if (parentForm.IsLightTimeOvernight())
            {
                lblIsOvernight.Visible = true;
            }
            else
            {
                lblIsOvernight.Visible = false;
            }
        }

        internal void SetLightSunriseTime()
        {
            if (!parentForm.applyingSettings && !testingTime)
            {
                this.parentForm.settings.LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(cbLightHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(cbLightMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            }
            setOvernight();
        }


        private void NumImageAdjustFreqSecs_ValueChanged(object sender, EventArgs e)
        {
            UpdateImageAdjustSecs();
        }

        private void CbAdjustTaskbarColour_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.AdjustTaskbarColour = this.cbAdjustTaskbarColour.Checked;
            this.parentForm.settings.AdjustTaskbarColour = false;
        }

        private void ClbShowScreenImage_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            if (!initializing)
            {
                if (parentForm.settings.ExtraScreenInfo.Count > 0)
                {
                    if (e.NewValue == CheckState.Checked)
                    {
                        parentForm.settings.ExtraScreenInfo[e.Index].ShowImageOnScreen = true;
                    }
                    else
                    {
                        parentForm.settings.ExtraScreenInfo[e.Index].ShowImageOnScreen = false;
                    }
                }

                if (!parentForm.applyingSettings)
                {
                    parentForm.AdjustDesktopImages(); 
                }                
            }
        }


        private void BtnOpenWallpaperFolder_Click(object sender, EventArgs e)
        {
            string argument = @"/select, " + Wallpaper.GetWallpaperPath(parentForm.wallpaperFileNum, FrmMain.getWallpaperExtension(parentForm.settings.WallpaperFormat));

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }


        private void BtnOpenSettingsFolder_Click(object sender, EventArgs e)
        {
            string argument = @"/select, " + Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable);

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        internal void SetImageScalePercent()
        {
            parentForm.settings.ImageSizeScalePercentPREV = parentForm.settings.ImageSizeScalePercent;
            parentForm.settings.ImageSizeScalePercent = (float)numImageSizeScalePercent.Value;

            parentForm.frmSettings.numImageSizeScalePercent.Value = (int)(this.parentForm.settings.ImageSizeScalePercent );

            if (parentForm.settings.ImageSizeScalePercent / 100f >= 1)
            {
                // disable blur
                cbBlurImageEdges.Checked = false;
                parentForm.settings.BlurImageEdges = false;
                SetBlurControls();
            }

            parentForm.SetPreviewImages(string.Empty);

            SetAdjustPreviewImage(cbPreview.Checked , !this.cbTimeTest.Checked);

        }

        private void CbCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {
            parentForm.settings.CheckForUpdate = cbCheckForUpdates.Checked;
            parentForm.frmSettings.cbCheckForUpdates.Checked = cbCheckForUpdates.Checked;
        }

        private void CbWriteToErrorLog_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.WriteToLog = this.cbWriteToErrorLog.Checked;
        }

        private void BtnViewErrorLog_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable))))
            {
                // Do Nothing
            }
            else
            {
                FileFunctions.CreateEmptyFile(Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable)));
            }

            Process.Start(Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable)));
        }

        private void CbResetImageOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (parentForm.applyingSettings)
            {
                return;
            }

            if (cbResetImageOptions.SelectedItem.ToString().Equals("*RESET IMAGE OPTIONS*", StringComparison.InvariantCulture))
            {
                return; // do nothing as they cancelled
            }

//            DialogResult result = MessageBox.Show("Are you sure you want to reset the Image Options (to the right)?\n\nNote: You can still adjust the settings once they have been reset.", "Reset Image Options", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

//            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (cbResetImageOptions.SelectedItem.ToString().Equals("None", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 0;
                    parentForm.settings.Brightness = 0;
                    parentForm.settings.Gamma = 100; 
                    parentForm.settings.Alpha = 0;
                    parentForm.settings.Contrast = 0;
                    parentForm.settings.ImageAdjustmentName = "None";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("A Little Darker", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 2;
                    parentForm.settings.Brightness = 5;
                    parentForm.settings.Contrast = 5;
                    parentForm.settings.Alpha = 9;
                    parentForm.settings.Gamma = 125;
                    parentForm.settings.ImageAdjustmentName = "A Little Darker";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("Darker", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 5;
                    parentForm.settings.Brightness = 10;
                    parentForm.settings.Contrast = 10;
                    parentForm.settings.Alpha = 16;
                    parentForm.settings.Gamma = 150;                    
                    parentForm.settings.ImageAdjustmentName = "Darker";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("Much Darker", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 8;
                    parentForm.settings.Brightness = 15;
                    parentForm.settings.Contrast = 15;
                    parentForm.settings.Gamma = 200;
                    parentForm.settings.Alpha = 23;                    
                    parentForm.settings.ImageAdjustmentName = "Much Darker";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("Very Dark", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 10;
                    parentForm.settings.Brightness = 20;
                    parentForm.settings.Contrast = 20;
                    parentForm.settings.Gamma = 250;
                    parentForm.settings.Alpha = 30;
                    parentForm.settings.ImageAdjustmentName = "Very Dark";
                }

                // this will set the values from Settings to the form
                ClearTestTimeSliderValues();

            }

            //reset it to the first item, no matter what was selected
            cbResetImageOptions.SelectedItem = "*RESET IMAGE OPTIONS*";

        }

        private void CbKey1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!parentForm.applyingSettings)
            {
                if (CheckShortcutOK())
                {
                    parentForm.settings.ShortcutKey1 = cbKey1.SelectedItem.ToString();
                    parentForm.SetKeyPress();
                }
                else
                {
                    ShowShortcutError();
                    cbKey1.SelectedItem = parentForm.settings.ShortcutKey1;
                }
            }
        }

        private void CbKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                if (CheckShortcutOK())
                {
                    parentForm.settings.ShortcutKey2 = cbKey2.SelectedItem.ToString();
                    parentForm.SetKeyPress();
                }
                else
                {
                    ShowShortcutError();
                    cbKey2.SelectedItem = parentForm.settings.ShortcutKey2;
                }
            }
        }

        private void CbKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                if (CheckShortcutOK())
                {
                    parentForm.settings.ShortcutKey3 = cbKey3.SelectedItem.ToString();
                    parentForm.SetKeyPress();
                }
                else
                {
                    ShowShortcutError();
                    cbKey3.SelectedItem = parentForm.settings.ShortcutKey3;
                }
            }
        }

        private void ShowShortcutError()
        {
            MessageBox.Show("You cannot create a shortcut without Key 1 or Key 2 being set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Boolean CheckShortcutOK()
        {
            if (cbKey1.SelectedItem.ToString().Equals("[NONE]", StringComparison.InvariantCulture) || cbKey2.SelectedItem.ToString().Equals("[NONE]", StringComparison.InvariantCulture))
            {
                return false;
            }

            return true;

        }
        
        private void CbShowFilteredImages_CheckedChanged(object sender, EventArgs e)
        {
            FormChanged = true;
            //UpdateShowFilteredImageList(); // sets the setting here
            SetFiltersState();
        }

        private void SetUseFilters()
        {
            cbUseFilters.Checked = parentForm.settings.UseFilters;
        }

        private void SetFiltersState()
        {
            label7.Enabled = parentForm.settings.UseFilters;
            lblBrightnessMax.Enabled = parentForm.settings.UseFilters;
            tbBrightnessMax.Enabled = parentForm.settings.UseFilters;
            label13.Enabled = parentForm.settings.UseFilters;
            label21.Enabled = parentForm.settings.UseFilters;
            label23.Enabled = parentForm.settings.UseFilters;
            numWidth.Enabled = parentForm.settings.UseFilters;
            numHeight.Enabled = parentForm.settings.UseFilters;
            label35.Enabled = parentForm.settings.UseFilters;
            numSizeKBytesMin.Enabled = parentForm.settings.UseFilters;
            label36.Enabled = parentForm.settings.UseFilters;
            label39.Enabled = parentForm.settings.UseFilters;
            numAspectMin.Enabled = parentForm.settings.UseFilters;
            label40.Enabled = parentForm.settings.UseFilters;
            numAspectMax.Enabled = parentForm.settings.UseFilters;
        }


        internal void UpdateFilteredFileCount()
        {
            lblNumImages.Text = parentForm.tslblImageCount.Text;
            return;
        }


        private void CbBlurImageEdges_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                parentForm.settings.BlurImageEdges = cbBlurImageEdges.Checked;
                SetBlurControls();
            }
        }

        private void TbGlowAmount_Scroll(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                parentForm.settings.BlurAmount = tbGlowAmount.Value;
                parentForm.SetPreviewImages(string.Empty);
                SetAdjustPreviewImage(cbPreview.Checked, !this.cbTimeTest.Checked); 
            }
        }

        private void SetBlurControls()
        {
            tbGlowAmount.Enabled = cbBlurImageEdges.Checked;
            lblGlowAmount.Enabled = cbBlurImageEdges.Checked;
        }

        private void PnlColourTint_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NumSizeKBytesMin_ValueChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                FormChanged = true;
                this.parentForm.settings.SizeKBytesMin = (int)this.numSizeKBytesMin.Value;
            }
        }
        
        private void NumAspectMax_ValueChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                FormChanged = true;
                parentForm.settings.AspectMax = (float)numAspectMax.Value;
            }
        }

        private void NumAspectMin_ValueChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                FormChanged = true;
                parentForm.settings.AspectMin = (float)numAspectMin.Value;
            }
        }

        private void CbRandomFlipImage_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.RandomFlipImage = this.cbRandomFlipImage.Checked;
        }

        private void BtnGetLocation_Click(object sender, EventArgs e)
        {
            GetSunriseSunset();
        }

        private void GetSunriseSunset()
        {
            int offsetMins;// = 0;
            offsetMins = int.Parse(txtOffsetMins.Text, CultureInfo.InvariantCulture);

            SunTimes.LatitudeCoords.Direction latDirection = SunTimes.LatitudeCoords.Direction.South;
            SunTimes.LongitudeCoords.Direction longDirection = SunTimes.LongitudeCoords.Direction.East;

            if (cbLatDirection.SelectedItem.ToString() == "North") latDirection = SunTimes.LatitudeCoords.Direction.North;
            if (cbLongDirection.SelectedItem.ToString() == "West") longDirection = SunTimes.LongitudeCoords.Direction.West;

            parentForm.GetSunriseSunset(double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture), double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture), latDirection, longDirection);

            DateTime sunrise = parentForm.sunrise;
            DateTime sunset = parentForm.sunset;

            sunrise = sunrise.AddMinutes(offsetMins);
            sunset = sunset.AddMinutes(offsetMins);

            // round mins to nearest 5
            double sunriseMins = sunrise.Minute;
            double sunsetMins = sunset.Minute;

            sunriseMins = MathExtra.RoundCustom(sunriseMins, 5);
            sunsetMins = MathExtra.RoundCustom(sunsetMins, 5);

            if (sunriseMins > 55) sunriseMins = 55;
            if (sunsetMins > 55) sunsetMins = 55;

            parentForm.sunrise = new DateTime(sunrise.Year, sunrise.Month, sunrise.Day, sunrise.Hour, (int)(sunriseMins), 0);
            parentForm.sunset = new DateTime(sunset.Year, sunset.Month, sunset.Day, sunset.Hour, (int)(sunsetMins), 0);

            lblSunriseSunset.Text = ("Sunrise @ " + sunrise.ToString("HH:mm", CultureInfo.InvariantCulture) + "\nSunset @ " + sunset.ToString("HH:mm", CultureInfo.InvariantCulture));

            if (parentForm.settings.UseSunriseSunset)
            {
                parentForm.settings.LightSunriseTime = parentForm.sunrise;
                parentForm.settings.DarkSunsetTime = parentForm.sunset;

                // Set the Light and Dark Times
                SetLightDarkTimeControls();

                if (!parentForm.applyingSettings && !testingTime)
                {
                    // if not on NULL ISLAND (0,0)
                    //https://en.wikipedia.org/wiki/Null_Island
                    if ((double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture) == 0 && double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture) == 0) == false)
                    {
                        parentForm.AdjustDesktopImages(); //(false);
                    }
                }
            }
        }

        private void CbUseSunriseSunset_CheckedChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;
                cbUseSunriseSunset2.Checked = cbUseSunriseSunset.Checked;
                parentForm.frmSettings.cbUseSunriseSunset.Checked = cbUseSunriseSunset.Checked;
                SetUseSunriseSunset();
                SetTimerState();
            }

        }

        private void SetUseSunriseSunset()
        {
            setLocationInfo();

            //re-calculate
            GetSunriseSunset();
        }

        private void TbLatitude_TextChanged(object sender, EventArgs e)
        {
            parentForm.settings.Latitude = double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture);
        }

        private void TbLongitude_TextChanged(object sender, EventArgs e)
        {
            parentForm.settings.Longitude = double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture);
        }
        
        private void CbUseFilters_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                FormChanged = true;
                UpdateUseFilters();
                SetFiltersState();
            }
        }

        internal void UpdateUseFilters()
        {
            if (this.parentForm.applyingSettings == false)
            {
                parentForm.settings.UseFilters = cbUseFilters.Checked;

            }
        }

        internal void ShowEventList()
        {
            System.Drawing.Color backColourDisabled = parentForm.colourDarkest; 
            System.Drawing.Color backColour1 = parentForm.colourDarker; 
            System.Drawing.Color backColour2 = parentForm.colourDark; 
            System.Drawing.Color currentBackColour = backColour1;
            System.Drawing.Color currentForeColourEnabled = Color.GhostWhite; 
            System.Drawing.Color currentForeColourDisabled = parentForm.colourDark;
            System.Drawing.Color currentForeColour = parentForm.colourLightest; 

            if (this.parentForm.settings.AdjustInterfaceColour == false)
            {
                backColour1 = Color.LightGray;
                backColour2 = Color.WhiteSmoke;
                currentBackColour = backColour1;

                currentForeColourEnabled = Color.Black;
                currentForeColourDisabled = Color.Gray;
                currentForeColour = currentForeColourEnabled;
            }

            olvEvents.BeginUpdate();

            olvEvents.Items.Clear();
            olvEvents.Columns.Clear();
            olvEvents.Columns.Add("Message", "Message", 150);
            olvEvents.Columns.Add("Sensor", "Sensor", 100);
            olvEvents.Columns.Add("Action", "Action", 100);
            olvEvents.Columns.Add("LastRun", "Last Run", 100);
            olvEvents.Columns.Add("Image", "Image", 300);
            
            string[] fullItemText = new string[5]; // 4 = number of columns

            /* sorting */
            parentForm.settings.Events.Sort(EventInfo.CompareByOrder);

            for (int i = 0; i < parentForm.settings.Events.Count; i++)
            {

                fullItemText[0] = parentForm.settings.Events[i].Message.ToString(CultureInfo.InvariantCulture);
                fullItemText[1] = this.parentForm.settings.Events[i].SensorName.ToString(CultureInfo.InvariantCulture); 
                fullItemText[2] = this.parentForm.settings.Events[i].CheckAction.ToString();
                fullItemText[3] = this.parentForm.settings.Events[i].LastRun.ToShortTimeString();
                fullItemText[4] = this.parentForm.settings.Events[i].ImagePath.ToString(CultureInfo.InvariantCulture);

                ListViewItem item = new ListViewItem(fullItemText)
                {
                    ToolTipText = parentForm.settings.Events[i].ImagePath.ToString(CultureInfo.InvariantCulture),
                    Tag = (EventInfo)this.parentForm.settings.Events[i],

                    BackColor = currentBackColour
                };
                if (currentBackColour == backColour2)
                {
                    currentBackColour = backColour1;
                }
                else
                {
                    currentBackColour = backColour2;
                }

                // adjust display here:
                if (parentForm.settings.Events[i].Enabled == false)
                {
                    item.ForeColor = currentForeColourDisabled;
                    item.BackColor = backColourDisabled;
                }
                else
                {
                    item.ForeColor = currentForeColourEnabled;
                }

                olvEvents.Items.Add(item);
            }

            olvEvents.EndUpdate();

            if (parentForm.settings.Events.Count == 0)
            {
                lblNoEntriesEvents.Visible = true;
            }
            else
            {
                lblNoEntriesEvents.Visible = false;
            }

            olvEvents.Refresh();

        }


        private void cbAutoBackup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                parentForm.settings.AutoBackup = cbAutoBackup.Checked;

            }
        }


        internal void setLocationInfo()
        {
            parentForm.settings.UseSunriseSunset = cbUseSunriseSunset.Checked;
            parentForm.settings.Latitude = double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture);
            parentForm.settings.Longitude = double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture);
            parentForm.settings.LocationPreset = txtSearch.Text;
            parentForm.settings.TimeOffsetMins = int.Parse(txtOffsetMins.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);


            if (parentForm.settings.Latitude != 0 && parentForm.settings.Longitude != 0)
            {
                btnWeatherReport.Enabled = true;
            }
            else
            {
                btnWeatherReport.Enabled = false;
            }
        }


        private void addEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (parentForm.HWSensors.Count == 0)
            {
                MessageBox.Show("Sorry, you have not selected any sensors.\n\n"
                    + "Below, in the 'Sensor Categories to Use' section, tick the Sensor categories you want to use, and press the Set button.",
                    "No Sensors Available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );

                return;
            }

            frmEvent eventForm = new frmEvent(this, frmEvent.FormMode.Add);

            eventForm.updateLogicText();
            eventForm.fillSensorList(string.Empty, string.Empty, string.Empty);

            eventForm.numOrderPos.Value = parentForm.settings.Events.Count; 
            eventForm.numOrderPos.Maximum = parentForm.settings.Events.Count;

            eventForm.ShowDialog();

            eventForm.Hide();
            eventForm.Dispose();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);

            DrawEventList();

        }

        private void removeEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedEvent();
        }


        private void DeleteSelectedEvent()
        {
            if (olvEvents.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this Event?" + System.Environment.NewLine + System.Environment.NewLine
                    + "Message: " + ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Message + System.Environment.NewLine
                    + "Sensor:  " + ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).SensorName + System.Environment.NewLine
                    + "Action:  " + ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).CheckAction.ToString() + System.Environment.NewLine
                    //+ "Image:   " + ((EventInfo)lvEvents.SelectedItems[0].Tag).ImagePath + System.Environment.NewLine
                    , "Delete Event", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    //select the next image on the list
                    for (int i = 0; i < parentForm.settings.Events.Count; i++) 
                    {
                        if (((EventInfo)(this.olvEvents.SelectedItem.RowObject)).ImagePath == parentForm.settings.Events[i].ImagePath)
                        {
                            // remove from list
                            this.parentForm.settings.RemoveEvent(((EventInfo)(this.olvEvents.SelectedItem.RowObject)).ImagePath,
                                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).CheckAction,
                                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).SensorName,
                                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Message
                                );

                            DrawEventList();
                            
                            int indexToKeep = i - 1;
                            if (indexToKeep < 0) indexToKeep = 0; 

                            reSortOrderValues(indexToKeep);

                            if (this.parentForm.settings.Events.Count == 0)
                            {
                                lblNoEntriesEvents.Visible = true;
                            }
                            else
                            {
                                lblNoEntriesEvents.Visible = false;
                            }

                            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
                            parentForm.AdjustDesktopImages(); // re-draws the Event text messages

                            break;
                        }
                    }
                }
            }
        }


        internal void reSortOrderValues(int vIndexToKeepPos)
        {
            if (parentForm.settings.Events.Count == 0) return;

            int valToKeep = parentForm.settings.Events[vIndexToKeepPos].OrderPos;

            //sort by the new order
            parentForm.settings.Events.Sort(EventInfo.CompareByOrder);
            
            for (int y = 0; y < parentForm.settings.Events.Count; y++)
            {
                if (y + 1 < parentForm.settings.Events.Count)
                {
                    if (parentForm.settings.Events[y].OrderPos == parentForm.settings.Events[y + 1].OrderPos)
                    {
                        // the current and last entries have same position. one needs to change
                        if (y != vIndexToKeepPos)
                        {
                            // x - 1 is the value to change
                            parentForm.settings.Events[y].OrderPos += 1;
                        }
                        else
                        {

                        }
                    }
                }
            }

            //sort by the new order
            parentForm.settings.Events.Sort(EventInfo.CompareByOrder);
        }

        private void lvEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (olvEvents.SelectedItem == null) return;

            setEventPopupMenu();
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (olvEvents.SelectedItem == null) return;

            ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Enabled = false;

            setEventPopupMenu();
            DrawEventList();

            parentForm.CreateEventTimers();
            parentForm.ProcessAllEvents();

            parentForm.ChangeWallpaperNow(true, false);

            //clean messages etc if any are displayed
            parentForm.AdjustDesktopImages();
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (olvEvents.SelectedItem == null) return;

            String result = string.Empty;
            String categoryList = string.Empty;

            // check that the event sensor category is enabled
            if (parentForm.checkEventSensorAvailable(((EventInfo)(this.olvEvents.SelectedItem.RowObject)), true,
                ref categoryList, ref result) == false)
            {
                MessageBox.Show("The sensor for this Event is not turned on. \n\n"
                    + "Please go to Advanced Settings - Events, and Set the following sensor:\n\n"
                    + result); 
                return;
            }

            // Only enable the Event if it is Valid
            if (((EventInfo)(this.olvEvents.SelectedItem.RowObject)).isValid(ref result))                
            {
                ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Enabled = true;
            }
            else
            {
                MessageBox.Show("Sorry, you cannot Enable this Event as there are problems with it. Please edit it, and try again.");
            }

            DrawEventList(); 

            parentForm.CreateEventTimers();
            parentForm.ProcessAllEvents();

            //clean messages etc if any are displayed
            parentForm.AdjustDesktopImages();
        }

        private void btnRefreshHWSensors_Click(object sender, EventArgs e)
        {
            txtHardwareSensorsFound.Enabled = false;
            btnRefreshHWSensors.Enabled = false;
            parentForm.initHardwareSensors(false, false, false);
            txtHardwareSensorsFound.Enabled = true;
            btnRefreshHWSensors.Enabled = true;
        }

        private void btnWeatherReport_Click(object sender, EventArgs e)
        {
            parentForm.getWeather(false);
        }


        private void cbHWCpu_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true;
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void cbHWGpu_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true;
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void cbHWFan_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true; 
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void cbHWHDD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true;
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void cbHWRAM_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true;
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void cbHWMotherboard_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true; 
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void btnSetSensors_Click(object sender, EventArgs e)
        {
            String hwSensors = string.Empty;

            txtHardwareSensorsFound.Enabled = false;
            btnRefreshHWSensors.Enabled = false;

            if (cbHWCpu.Checked) hwSensors += "CPU;";
            if (cbHWGpu.Checked) hwSensors += "GPU;";
            if (cbHWFan.Checked) hwSensors += "FAN;";
            if (cbHWHDD.Checked) hwSensors += "HDD;";
            if (cbHWRAM.Checked) hwSensors += "RAM;";
            if (cbHWMotherboard.Checked) hwSensors += "MAINBOARD;";
            if (cbPower.Checked) hwSensors += "POWER;";
            if (cbWeather.Checked) hwSensors += "WEATHER;";
            if (cbLocation.Checked) hwSensors += "LOCATION;";
            if (cbInternet.Checked) hwSensors += "INTERNET;";

            // set setting
            parentForm.settings.HWSensorCategoriessUsed = hwSensors;
            
            // re-set the sensors

            parentForm.ShowChangeWallpaperWorking(true); 
            parentForm.initHardwareSensors(true, false, false);
            parentForm.HideChangeWallpaperWorking(); 

            txtHardwareSensorsFound.Enabled = true;
            btnRefreshHWSensors.Enabled = true;

            btnSetSensors.BackColor = parentForm.colourDarker; 

            // Save Settings
            parentForm.SaveSettings(string.Empty, this.parentForm.settings);

            // if the weather sensor has been turned on but a location is not set, then alert the user
            if (weatherEnabled && parentForm.settings.Latitude == 0 && parentForm.settings.Longitude == 0)
            {
                MessageBox.Show("You need to set a Location to use the Weather sensors. \n\nGo to the Location tab and set one.");
            }

            //redraw the event list incase any were disabled by the check process above
            DrawEventList(); 
        }

        private void editEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedEvent();
        }

        private void cbShowSplash_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSplash = cbShowSplash.Checked;
            Properties.Settings.Default.Save();
            parentForm.frmSettings.cbShowSplash.Checked = cbShowSplash.Checked;
        }

        private void txtOffsetMins_TextChanged(object sender, EventArgs e)
        {
            if (txtOffsetMins.Text.Trim().Length > 0)
            {
                if (txtOffsetMins.Text.Trim().Equals("-", StringComparison.InvariantCulture)) return; // ignore a negative symbol
                parentForm.settings.TimeOffsetMins = int.Parse(txtOffsetMins.Text, CultureInfo.InvariantCulture);
                btnSetLocation.BackColor = parentForm.colourAlert;
                GetSunriseSunset();
            }
        }

        private void cbImageQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            int quality = int.Parse(cbImageQuality.SelectedItem.ToString().Substring(0, 2), CultureInfo.InvariantCulture);

            parentForm.settings.CompressionQuality = quality;// * 10;

            FrmMain.SetImageCompressionRegistry(parentForm.settings.CompressionQuality);

            redrawWallpaper = true;
        }

        private void setQualityDD()
        {
            if (this.parentForm.settings.CompressionQuality == 10)
            {
                this.cbImageQuality.SelectedItem = "10  - Low (Fast)";
            }
            else if (this.parentForm.settings.CompressionQuality == 50)
            {
                this.cbImageQuality.SelectedItem = "50 - Normal";
            }
            else if (this.parentForm.settings.CompressionQuality == 100)
            {                
                this.cbImageQuality.SelectedItem = "100 - High (Slower)";
            }
            else
            {
                this.cbImageQuality.SelectedItem = this.parentForm.settings.CompressionQuality;
            }

            string format = this.parentForm.settings.WallpaperFormat.ToString().ToUpper(CultureInfo.InvariantCulture);
            this.cbWallpaperFileType.SelectedItem = format; 
            if (format.Equals("JPEG", StringComparison.InvariantCulture))
            {
                this.cbWallpaperFileType.SelectedItem = "JPG";
            } 
            
            lblQuality.Enabled = parentForm.settings.WallpaperFormat == ImageFormat.Jpeg;
            cbImageQuality.Enabled = parentForm.settings.WallpaperFormat == ImageFormat.Jpeg;
        }

        private void cbWallpaperFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.DeleteOldWallpaperFiles(); // delete any images with current format

            if (cbWallpaperFileType.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("BMP"))
            {
                parentForm.settings.WallpaperFormat = ImageFormat.Bmp;
            }
            if (cbWallpaperFileType.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("JPG")
                || cbWallpaperFileType.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("JPEG"))
            {
                parentForm.settings.WallpaperFormat = ImageFormat.Jpeg;
            }
            if (cbWallpaperFileType.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("PNG"))
            {
                parentForm.settings.WallpaperFormat = ImageFormat.Png;
            }
            parentForm.DeleteOldWallpaperFiles(); // delete any images with new format

            //hide/show the cb for quality when JPG selected
            setQualityDD();
            redrawWallpaper = true;
        }

        private void cbWeather_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                weatherEnabled = cbWeather.Checked;
                btnSetSensors.Enabled = true; 
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void cbPower_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true; 
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void txtHardwareSensorsFound_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true;
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void lvScreenScales_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnFindLocation_Click(object sender, EventArgs e)
        {
            parentForm.checkInternetConnection(false);

            if (parentForm.internetAvailable == false)
            {
                parentForm.showNoInternetMessage();
                return;
            }

            // Alternative Weather calls HERE:  https://www.obioberoi.com/2018/07/14/how-to-pull-weather-info-into-a-console-app/
            var client = new OpenWeatherAPI.OpenWeatherAPI(Constants.OpenWeatherAPIKey); 

            Query results;
            try
            {
                results = client.Query(txtSearch.Text);
            }
            catch (ApplicationException ex)
            {
                results = null;
                ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.FindLocation, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            
            // -34.206841, 138.599503);
            //var results = client.Query("Adelaide");

            if (results == null)
            {
                btnSetLocation.Enabled = false;
                lblCityFound.Text = "City Not Found. "; 
                foundCityLat = 0;
                foundCityLong = 0;
                foundCityName = string.Empty;
                MessageBox.Show("There was a problem getting the weather. \nThe internet connection may be unavailable \nOr the city name is not known.");
                return;
            }

            if (results.ValidRequest == false)
            {
                btnSetLocation.Enabled = false;
                lblCityFound.Text = "City Not Found. ";
                foundCityLat = 0;
                foundCityLong = 0;
                foundCityName = string.Empty;
                MessageBox.Show("There was a problem getting the weather. \nThe internet connection may be unavailable \nOr the city name is not known.");
                return;
            }

            // if we get to here then its a valid result
            btnSetLocation.Enabled = true;
            foundCityLat = results.Coord.Latitude;
            foundCityLong = results.Coord.Longitude;
            foundCityName = results.Name;
            txtSearch.Text = results.Name;
            lblCityFound.Text = "Found: " + results.Name;

            btnSetLocation.BackColor = parentForm.colourAlert;
            lblWorkingImages.BackColor = parentForm.colourAlert;

        }




        private void btnSetLocation_Click(object sender, EventArgs e)
        {
            txtLatitude.Text = this.foundCityLat.ToString(CultureInfo.InvariantCulture); 
            txtLongitude.Text = this.foundCityLong.ToString(CultureInfo.InvariantCulture);

            parentForm.settings.LocationPreset = this.foundCityName;
            parentForm.settings.Latitude = this.foundCityLat;
            parentForm.settings.Longitude = this.foundCityLong;

            txtSearch.Text = foundCityName;

            btnSetLocation.BackColor = parentForm.colourDarker; 

            setLocationInfo();
            GetSunriseSunset();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);

        }

        private void lblNoEntriesFolders_Click(object sender, EventArgs e)
        {

        }

        private void cbPortable_CheckedChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                parentForm.SetPortable(cbPortable.Checked);
            }
        }

        

        private void lblWallpaperModesExplained_Click(object sender, EventArgs e)
        {
            FrmMain.OpenModesURL();
        }

        private void cbAdjustInterfaceColour_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;

                parentForm.settings.AdjustInterfaceColour = cbAdjustInterfaceColour.Checked;

                parentForm.SetInterfaceColour();
            }
        }

        
        private void btnCancel_Click_2(object sender, EventArgs e)
        {
            this.Hide();
        }


        private void cbCheckSensorsOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.settings.AdjustmentsUseAlways)
            {
                parentForm.settings.CheckSensorsOnStartup = cbCheckSensorsOnStartup.Checked;
            }
        }

        private void btnCheckSensors_Click(object sender, EventArgs e)
        {
            if (parentForm.checkEventSensors(true, false) == true)
            {
                MessageBox.Show("The Event Sensors are okay.", "Event Sensors Check", MessageBoxButtons.OK);
            }
        }

        private void lblDisplatSettings_Click(object sender, EventArgs e)
        {
            Process.Start("ms-settings:display");
        }

        private void lblWinDarkMode_Click(object sender, EventArgs e)
        {
            toggleWinDarkMode();
        }

        private void toggleWinDarkMode()
        {
            Boolean darkModeEnabled = ScreenFunctions.isWinDarkModeEnabled();
            darkModeEnabled = !darkModeEnabled; //toggle
            ScreenFunctions.SetWindowsDarkMode(darkModeEnabled);
            setWinDarkModeText(darkModeEnabled);
            parentForm.SetInterfaceColour();
        }

        private void setWinDarkModeText(Boolean vDarkModeEnabled)
        {
            if (vDarkModeEnabled)
            {
                lblWinDarkMode.Text = "Disable Windows Dark Mode";
            }
            else
            {
                lblWinDarkMode.Text = "Enable Windows Dark Mode";
            }
        }

        private void lblWinTransparency_Click(object sender, EventArgs e)
        {
            toggleWinTransparency(); // 
        }

        private void toggleWinTransparency()
        {
            Boolean transparencyEnabled = ScreenFunctions.isWinTransparencyEnabled();
            transparencyEnabled = !transparencyEnabled; //toggle
            ScreenFunctions.SetWindowsTransparency(transparencyEnabled);
            setWinTransparencyText(transparencyEnabled);
        }

        private void setWinTransparencyText(Boolean vTransparencyEnabled)
        {
            if (vTransparencyEnabled)
            {
                lblWinTransparency.Text = "Disable Windows Transparency";
            }
            else
            {
                lblWinTransparency.Text = "Enable Windows Transparency";
            }
        }

        private void numWideThreshold_ValueChanged(object sender, EventArgs e)
        {
            parentForm.settings.AspectThresholdWide = (float)numWideAspectThreshold.Value;
            redrawWallpaper = true;
        }

        private void numNarrowAspectThreshold_ValueChanged(object sender, EventArgs e)
        {
            parentForm.settings.AspectThresholdNarrow = (float)numNarrowAspectThreshold.Value;
            redrawWallpaper = true;
        }

        private void numMargin_ValueChanged(object sender, EventArgs e)
        {
            parentForm.settings.MarginToEnlarge = (int)numMarginEnlarge.Value;
            redrawWallpaper = true;
        }

        private void numWideMargin_ValueChanged(object sender, EventArgs e)
        {
            parentForm.settings.WideThreshold = (int)numWideThresholdPercent.Value;
            redrawWallpaper = true;
        }

        private void cbMultiMonitorMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                parentForm.settings.MultiMonitorMode = (FrmMain.MultiMonitorModes)Enum.Parse(typeof(FrmMain.MultiMonitorModes), cbMultiMonitorMode.SelectedItem.ToString());
                SetHoursImages();
                parentForm.AdjustDesktopImages(); 
            }
        }

        internal void setMultiMonitorControls()
        {
            if (Screen.AllScreens.Length > 1)
            {
                // set if Multiple Monitors Found
                lblMultiMonDisplay.Enabled = (Screen.AllScreens.Length > 1);
                cbMultiMonitorMode.Enabled = (Screen.AllScreens.Length > 1);
            }
            else
            {
                // only 1
                lblMultiMonDisplay.Enabled = false;
                cbMultiMonitorMode.Enabled = false;
            }
        }

        private void lblImageScale_Click(object sender, EventArgs e)
        {

        }

        private void numAdjustX_ValueChanged(object sender, EventArgs e)
        {
            parentForm.settings.AdjustX = (int)numAdjustX.Value;
            redrawWallpaper = true;
        }

        private void numAdjustY_ValueChanged(object sender, EventArgs e)
        {
            parentForm.settings.AdjustY = (int)numAdjustY.Value;
            redrawWallpaper = true;
        }

        private void cbInternet_CheckedChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                btnSetSensors.Enabled = true;
                btnSetSensors.BackColor = parentForm.colourAlert;
            }
        }

        private void numImageSizeScalePercent_ValueChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                SetImageScalePercent();
                parentForm.frmSettings.SetImageScalePercent();
                parentForm.AdjustDesktopImages();
            }
        }

        private void CbUseHSV_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.UseHSV = cbUseHSV.Checked;

            this.parentForm.frmSettings.cbUseHSV.Checked = cbUseHSV.Checked;

            if (!parentForm.applyingSettings)
            {
                parentForm.AdjustDesktopImages();
            }
        }

        private void RunEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.olvEvents.SelectedItem.RowObject == null) return;
            foreach (System.Windows.Forms.Timer tmr in parentForm.EventTimers)
            {
                // if the timer has to be run, or if it has previously run and is displayed. 
                // In that case the call to the _tick will clean up and hide the event
                if (tmr.Enabled && ((EventInfo)tmr.Tag).Message == ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).Message)
                {
                    parentForm.DoEventTimerTick(tmr, new EventArgs());
                    ((EventInfo)(this.olvEvents.SelectedItem.RowObject)).LastRun = DateTime.Now; // set last run value here
                    DrawEventList(); // to update the Last Run column
                    break;
                }
            }
        }

        private void cbUseSunriseSunset2_CheckedChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;
                cbUseSunriseSunset.Checked = cbUseSunriseSunset2.Checked;
                parentForm.frmSettings.cbUseSunriseSunset.Checked = cbUseSunriseSunset2.Checked;
                parentForm.UpdateScreenState();
                SetUseSunriseSunset();
                SetTimerState();
            }
        }
    }
}
