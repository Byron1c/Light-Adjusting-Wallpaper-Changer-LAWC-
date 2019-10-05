using LAWC.Common;
using LAWC.Objects;
using OpenWeatherAPI;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using static LAWC.FrmMain;


namespace LAWC
{
    public partial class frmSettings : Form
    {

        internal FrmMain parentForm;
        internal Boolean FormChanged;

        private String foundCityName;
        private double foundCityLat;
        private double foundCityLong;
        

        public frmSettings(FrmMain vParentForm)
        {
            InitializeComponent();

            parentForm = vParentForm;

            setHandlers();

            FormChanged = false;
            foundCityName = string.Empty;
            foundCityLat = 0;
            foundCityLong = 0;
        }

        private void setHandlers()
        {
            pnlBackgroundColourDark.MouseUp += new MouseEventHandler(PnlBackgroundColourDark_MouseUp);
            pnlBackgroundColourLight.MouseUp += new MouseEventHandler(PnlBackgroundColourLight_MouseUp);

            cbWallpaperMode.SelectedValueChanged += CbWallpaperMode_SelectedValueChanged;
        }


        private void CbWallpaperMode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!this.parentForm.applyingSettings)
            {
                // enumStringToType:
                this.parentForm.settings.WallpaperMode = (Wallpaper.WallpaperModes)Enum.Parse(typeof(Wallpaper.WallpaperModes), cbWallpaperMode.SelectedItem.ToString());
                parentForm.AdjustDesktopImages(); 
            }
        }
        
        internal void SetFormValues()
        {
            parentForm.applyingSettings = true;

            cbAutoStart.Checked = parentForm.CheckAutoRunState();
            this.cbStartMinimized.Checked = this.parentForm.settings.StartMinimized;
            cbCheckForUpdates.Checked = this.parentForm.settings.CheckForUpdate;    
            cbUseHSV.Checked = this.parentForm.settings.UseHSV;
            numWallpaperChangeMins.Value =  this.parentForm.settings.WallpaperChangeFrequencyMins;
            this.txtSearch.Text = parentForm.settings.LocationPreset.ToString(CultureInfo.InvariantCulture);
            this.lblCityFound.Text = "Found: " + parentForm.settings.LocationPreset.ToString(CultureInfo.InvariantCulture);
            txtLatitude.Text = this.parentForm.settings.Latitude.ToString(CultureInfo.InvariantCulture);
            txtLongitude.Text = this.parentForm.settings.Longitude.ToString(CultureInfo.InvariantCulture);
            cbUseSunriseSunset.Checked = this.parentForm.settings.UseSunriseSunset;
            this.pnlBackgroundColourDark.BackColor = this.parentForm.settings.BackgroundColourDark;
            this.pnlBackgroundColourLight.BackColor = this.parentForm.settings.BackgroundColourLight;
            cbShowToolTips.Checked = parentForm.settings.ShowToolTips;
            //cbResetImageOptions.SelectedItem = parentForm.settings.ImageAdjustmentName;
            txtOffsetMins.Text = parentForm.settings.TimeOffsetMins.ToString(CultureInfo.InvariantCulture);
            this.cbShowSplash.Checked = Properties.Settings.Default.ShowSplash;
            //this.cbPortable.Checked = Properties.Settings.Default.Portable;
            cbImageOrder.SelectedItem = this.parentForm.settings.WallpaperOrder.ToString();
            cbWallpaperMode.SelectedItem = parentForm.settings.WallpaperMode.ToString();
            cbMultiMonitorMode.SelectedItem = parentForm.settings.MultiMonitorMode.ToString();

            numImageSizeScalePercent.Value = (int)(this.parentForm.settings.ImageSizeScalePercent);

            SetLightDarkTimeControls();
            SetHoursImages();
            GetSunriseSunset();
            SetUseSunriseSunset();
            setMultiMonitorControls();
            SetImageScalePercent();

            parentForm.applyingSettings = false;
        }

        private void PnlBackgroundColourDark_MouseUp(object sender, MouseEventArgs e)
        {
            FormChanged = true;
            PickColourDark();
        }

        private void PnlBackgroundColourLight_MouseUp(object sender, MouseEventArgs e)
        {
            FormChanged = true;
            PickColourLight();
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
            }
        }

        private void btnAdvancedSettings_Click(object sender, EventArgs e)
        {
            parentForm.ShowAdvancedSettings();

        }

        private void cbCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {
            parentForm.settings.CheckForUpdate = cbCheckForUpdates.Checked;
            parentForm.frmSettings.cbCheckForUpdates.Checked = cbCheckForUpdates.Checked;
        }

        private void cbImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.WallpaperOrder = (Wallpaper.ImageOrder)Enum.Parse(typeof(Wallpaper.ImageOrder), cbImageOrder.SelectedItem.ToString());
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


        private void numWallpaperChangeMins_ValueChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                UpdateWallpaperChangeMins();
                SetHoursImages();

                parentForm.applyingSettings = true;
                parentForm.frmSettingsAdvanced.numWallpaperChangeMins.Value = (int)numWallpaperChangeMins.Value;
                parentForm.applyingSettings = false;
            }
        }


        internal void UpdateWallpaperChangeMins()
        {

            try
            {
                int mins = (int)numWallpaperChangeMins.Value;
                if (mins < 1) mins = 1;

                parentForm.settings.WallpaperChangeFrequencyMins = mins;

                parentForm.SetNextWallpaperChangeTime();
                parentForm.SetNextWallpaperAdjustTime();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("You must enter a number only.  Error: " + ex.Message, "Numbers only", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cbShowToolTips_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.ShowToolTips = cbShowToolTips.Checked;
            this.parentForm.toolTip1.Active = cbShowToolTips.Checked;
            this.parentForm.frmSettings.toolTip1.Active = cbShowToolTips.Checked;
        }


        private void SetLightDarkTimeControls()
        {
            this.cbDarkHour.SelectedItem = this.parentForm.settings.DarkSunsetTime.Hour.ToString("D2", CultureInfo.InvariantCulture);
            this.cbDarkMin.SelectedItem = this.parentForm.settings.DarkSunsetTime.Minute.ToString("D2", CultureInfo.InvariantCulture);
            this.cbLightHour.SelectedItem = this.parentForm.settings.LightSunriseTime.Hour.ToString("D2", CultureInfo.InvariantCulture);
            this.cbLightMin.SelectedItem = this.parentForm.settings.LightSunriseTime.Minute.ToString("D2", CultureInfo.InvariantCulture);
        }


        private void GetSunriseSunset()
        {
            if (double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture) == 0 && double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture) == 0)
            {
                lblSunriseSunset.Text = "No City/Location set.";
                return;
            }

            int offsetMins;// = 0;
            offsetMins = int.Parse(txtOffsetMins.Text, CultureInfo.InvariantCulture);

            parentForm.GetSunriseSunset(double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture), double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture));

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

                if (!parentForm.applyingSettings)
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

        private void cbStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            parentForm.settings.StartMinimized = cbStartMinimized.Checked;
            this.parentForm.frmSettingsAdvanced.cbStartMinimized.Checked = this.cbStartMinimized.Checked;
        }

        private void txtLatitude_TextChanged(object sender, EventArgs e)
        {
            parentForm.settings.Latitude = double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture);
            GetSunriseSunset();
        }

        private void txtLongitude_TextChanged(object sender, EventArgs e)
        {
            parentForm.settings.Longitude = double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture);
            GetSunriseSunset();
        }

        private void cbUseSunriseSunset_CheckedChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;
                parentForm.frmSettingsAdvanced.cbUseSunriseSunset2.Checked = cbUseSunriseSunset.Checked;
                parentForm.frmSettingsAdvanced.cbUseSunriseSunset.Checked = cbUseSunriseSunset.Checked;
                SetUseSunriseSunset();
            }
        }


        private void SetUseSunriseSunset()
        {
            setLocationInfo();
            //re-calculate
            GetSunriseSunset();

            // if we are using the times, then set the drop downs
            cbDarkHour.Enabled = !cbUseSunriseSunset.Checked;
            cbDarkMin.Enabled = !cbUseSunriseSunset.Checked;
            cbLightHour.Enabled = !cbUseSunriseSunset.Checked;
            cbLightMin.Enabled = !cbUseSunriseSunset.Checked;            
        }

        private void btnWeatherReport_Click(object sender, EventArgs e)
        {
            parentForm.getWeather(false);
        }

        private void cbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            parentForm.SetStartAutomatically(cbAutoStart.Checked);
            parentForm.frmSettings.cbAutoStart.Checked = cbAutoStart.Checked;
        }

        private void CbResetImageOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbResetImageOptions.SelectedItem == null) return;
            if (cbResetImageOptions.SelectedItem.ToString().Equals("*RESET IMAGE OPTIONS*", StringComparison.InvariantCulture))
            {
                return; // do nothing as they cancelled
            }
            parentForm.settings.ImageAdjustmentName = cbResetImageOptions.SelectedItem.ToString();
            //DialogResult result = MessageBox.Show("Are you SURE you want to reset the Image Options (to the right)?\n\nNote: You can still adjust the settings once they have been reset.", "Reset Image Options", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

            //if (result == System.Windows.Forms.DialogResult.OK)
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
                parentForm.frmSettingsAdvanced.ClearTestTimeSliderValues();

            }

            //reset it to the first item, no matter what was selected
            cbResetImageOptions.SelectedItem = "*RESET IMAGE OPTIONS*";

            FormChanged = true;
        }

        private void cbShowSplash_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSplash = cbShowSplash.Checked;
            Properties.Settings.Default.Save();
            parentForm.frmSettingsAdvanced.cbShowSplash.Checked = cbShowSplash.Checked;
        }

        private void txtOffsetMins_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtOffsetMins.Text, out _))
            {
                parentForm.settings.TimeOffsetMins = int.Parse(txtOffsetMins.Text, CultureInfo.InvariantCulture);
                btnSetLocation.BackColor = parentForm.colourAlert;
                btnSetLocation.Enabled = true;
                GetSunriseSunset();
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            // shift the cancel button off the screen, as we only use it to make the ESC key hide the form
            btnCancel.Location = new System.Drawing.Point(btnCancel.Location.X + this.Width, btnCancel.Location.Y);
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
            var client = new OpenWeatherAPI.OpenWeatherAPI(Constants.OpenWeatherAPIKey); //YOUR-API-KEY

            Query results;// = null;
            try
            {
                results = client.Query(txtSearch.Text);
            }
            catch (ArithmeticException)
            {
                results = null;
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

            btnSetLocation.BackColor =  Color.PeachPuff;

            FrmMain.showWeatherReport(results, true);
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

        private void btnViewErrorLog_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable))))
            {
                // Do Nothing
            }
            else
            {
                FileFunctions.CreateEmptyFile(Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable)));
            }

            System.Diagnostics.Process.Start(Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable)));
        }

        private void btnOpenWallpaperFolder_Click(object sender, EventArgs e)
        {
            string argument = @"/select, " + Wallpaper.GetWallpaperPath(parentForm.wallpaperFileNum, FrmMain.getWallpaperExtension(parentForm.settings.WallpaperFormat));

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void btnOpenSettingsFolder_Click(object sender, EventArgs e)
        {
            string argument = @"/select, " + Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable);

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void lblWallpaperModesExplained_Click(object sender, EventArgs e)
        {
            FrmMain.OpenModesURL();
        }

        private void cbWallpaperMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbMultiMonitorMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.parentForm.applyingSettings == false)
            {
                parentForm.settings.MultiMonitorMode = (FrmMain.MultiMonitorModes)Enum.Parse(typeof(FrmMain.MultiMonitorModes), cbMultiMonitorMode.SelectedItem.ToString());
                SetHoursImages();
                parentForm.AdjustDesktopImages(); // (false);
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

        private void cbLightHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetLightSunriseTime();

            if (!parentForm.applyingSettings)
            {
                //FormChanged = true;
                parentForm.AdjustDesktopImages(); 
            }
        }

        private void cbLightMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetLightSunriseTime();

            if (!parentForm.applyingSettings)
            {
                //FormChanged = true;
                parentForm.AdjustDesktopImages();
            }

        }

        private void cbDarkHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetDarkSunsetTime();

            if (!parentForm.applyingSettings)
            {
                //FormChanged = true;
                parentForm.AdjustDesktopImages(); 

            }
        }

        private void cbDarkMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.UpdateScreenState();
            SetDarkSunsetTime();

            if (!parentForm.applyingSettings)
            {
                //FormChanged = true;
                parentForm.AdjustDesktopImages(); 
            }
        }

        internal void SetDarkSunsetTime()
        {

            if (!parentForm.applyingSettings)
            {
                this.parentForm.settings.DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(cbDarkHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(cbDarkMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            }
        }
        
        internal void SetLightSunriseTime()
        {
            if (!parentForm.applyingSettings)
            {
                this.parentForm.settings.LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(cbLightHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(cbLightMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            }
        }

        private void tbImageSizeScalePercent_Scroll(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                FormChanged = true;

                SetImageScalePercent();
                parentForm.frmSettingsAdvanced.SetImageScalePercent();

                parentForm.AdjustDesktopImages();
            }
        }


        internal void SetImageScalePercent()
        {
            parentForm.settings.ImageSizeScalePercentPREV = parentForm.settings.ImageSizeScalePercent;
            parentForm.settings.ImageSizeScalePercent = (float)numImageSizeScalePercent.Value;

            parentForm.frmSettingsAdvanced.numImageSizeScalePercent.Value = (int)(this.parentForm.settings.ImageSizeScalePercent);// * frmSettingsAdvanced.SliderScaleFactor);

            if (parentForm.settings.ImageSizeScalePercent >= 1)
            {
                // disable blur
                parentForm.settings.BlurImageEdges = false;
            }

            parentForm.SetPreviewImages(string.Empty);
            
        }

        private void numImageSizeScalePercent_ValueChanged(object sender, EventArgs e)
        {
            if (!parentForm.applyingSettings)
            {
                //FormChanged = true;

                SetImageScalePercent();
                parentForm.frmSettingsAdvanced.SetImageScalePercent();
                parentForm.AdjustDesktopImages();
            }               

            
        }

        private void CbUseHSV_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.UseHSV = cbUseHSV.Checked;            

            this.parentForm.frmSettingsAdvanced.cbUseHSV.Checked = cbUseHSV.Checked;

            if (!parentForm.applyingSettings)
            {
                //FormChanged = true;
                parentForm.AdjustDesktopImages();
            }
        }

        private void cbUseDarkLightTimes_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.UseDarkLightTimes = cbUseDarkLightTimes.Checked;
            FormChanged = true;
        }
    }
}
