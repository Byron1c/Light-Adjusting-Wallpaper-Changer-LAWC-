using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MBG.SimpleWizard;
using LAWC;
using LAWC.Common;
using OpenWeatherAPI;
using LAWC.Objects;
using System.Globalization;

namespace LAWC.Wizard
{
    public partial class Page2 : UserControl, IWizardPage
    {
        private readonly FrmMain parentForm;

        private String foundCityName;
        private double foundCityLat;
        private double foundCityLong;

        public Page2(FrmMain vParentForm)
        {
            InitializeComponent();

            parentForm = vParentForm;

            this.pnlBackgroundDarkColour.BackColor = parentForm.settings.BackgroundColourDark;
            this.pnlBackgroundLightColour.BackColor = parentForm.settings.BackgroundColourLight;

            pnlBackgroundDarkColour.MouseUp += new MouseEventHandler(pnlBackgroundColour_MouseUp);
            pnlBackgroundLightColour.MouseUp += new MouseEventHandler(pnlBackgroundLightColour_MouseUp);


            // initialise
            //doBorderCheckChanged();
            doTimerCheckChanged();

            foundCityName = string.Empty;
            foundCityLat = 0;
            foundCityLong = 0;

            Boolean darkModeEnabled = ScreenFunctions.isWinDarkModeEnabled();
            setWinDarkModeText(darkModeEnabled);
        }

        

        void pnlBackgroundLightColour_MouseUp(object sender, MouseEventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.PickColourLight();
            this.pnlBackgroundLightColour.BackColor = this.parentForm.settings.BackgroundColourLight;
        }

        void pnlBackgroundColour_MouseUp(object sender, MouseEventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.PickColourDark();
            this.pnlBackgroundDarkColour.BackColor = this.parentForm.settings.BackgroundColourDark;
        }

        #region IWizardPage Members

        public UserControl Content
        {
            get { return this; }
        }

        public new void Load()
        {

        }

        public void Save()
        {
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public bool IsBusy
        {
            //get { throw new NotImplementedException(); }
            get { return false; }
            //
        }

        public bool PageValid
        {
            get { return true; }
        }

        public string ValidationMessage
        {
            //get { throw new NotImplementedException(); }
            get { return ""; }
            //
        }

        #endregion

        private void pnlBackgroundColour_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbUseTime_CheckedChanged(object sender, EventArgs e)
        {
            doTimerCheckChanged();
        }

        private void doTimerCheckChanged()
        {
            this.parentForm.frmSettingsAdvanced.cbUseDarkLightTimes.Checked = cbUseTime.Checked;


            this.cbDarkHour.Enabled = cbUseTime.Checked;
            this.cbDarkMin.Enabled = cbUseTime.Checked;
            this.cbLightHour.Enabled = cbUseTime.Checked;
            this.cbLightMin.Enabled = cbUseTime.Checked;

            this.cbDuration.Enabled = cbUseTime.Checked;

            cbUseSunriseSunset.Enabled = cbUseTime.Checked;

            //txtSearch.Enabled = cbUseTime.Checked;
            //btnFindLocation.Enabled = cbUseTime.Checked;
            //txtOffsetMins.Enabled = cbUseTime.Checked;
            //lblCityFound.Enabled = cbUseTime.Checked;

            txtSearch.Enabled = true;
            btnFindLocation.Enabled = true;
            txtOffsetMins.Enabled = true;
            lblCityFound.Enabled = true;

            //cbPopularLocations.Enabled = cbUseSunriseSunset.Checked;
            //txtLatitude.Enabled = cbUseSunriseSunset.Checked;
            //txtLongitude.Enabled = cbUseSunriseSunset.Checked;
            lblSunriseSunset.Enabled = cbUseSunriseSunset.Checked;
            btnGetLocation.Enabled = cbUseSunriseSunset.Checked;

            cbDarkHour.Enabled = !cbUseSunriseSunset.Checked;
            cbDarkMin.Enabled = !cbUseSunriseSunset.Checked;
            cbLightHour.Enabled = !cbUseSunriseSunset.Checked;
            cbLightMin.Enabled = !cbUseSunriseSunset.Checked;
        }



        private void cbStartHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbDarkHour.SelectedItem = this.cbDarkHour.SelectedItem;
            this.parentForm.settings.DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(this.parentForm.frmSettingsAdvanced.cbDarkHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(this.parentForm.frmSettingsAdvanced.cbDarkMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            this.parentForm.UpdateScreenState();
        }

        private void cbStartMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbDarkMin.SelectedItem = this.cbDarkMin.SelectedItem;
            this.parentForm.settings.DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(this.parentForm.frmSettingsAdvanced.cbDarkHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(this.parentForm.frmSettingsAdvanced.cbDarkMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            this.parentForm.UpdateScreenState();
        }

        private void cbEndHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbLightHour.SelectedItem = this.cbLightHour.SelectedItem;
            this.parentForm.settings.LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(this.parentForm.frmSettingsAdvanced.cbLightHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(this.parentForm.frmSettingsAdvanced.cbLightMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            this.parentForm.UpdateScreenState();
        }

        private void cbEndMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbLightMin.SelectedItem = this.cbLightMin.SelectedItem;
            this.parentForm.settings.LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(this.parentForm.frmSettingsAdvanced.cbLightHour.SelectedItem.ToString(), CultureInfo.InvariantCulture), int.Parse(this.parentForm.frmSettingsAdvanced.cbLightMin.SelectedItem.ToString(), CultureInfo.InvariantCulture), 0);
            this.parentForm.UpdateScreenState();
        }

        private void cbDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbDuration.SelectedItem = this.cbDuration.SelectedItem;
            this.parentForm.settings.DurationMins = int.Parse(this.cbDuration.SelectedItem.ToString(), CultureInfo.InvariantCulture);

            this.parentForm.frmSettingsAdvanced.numImageAdjustFreqSecs.Maximum = this.parentForm.settings.DurationMins * 60;
        }

        private void cbBorder_CheckedChanged(object sender, EventArgs e)
        {
            doBorderCheckChanged();            
        }

        private void doBorderCheckChanged()
        {
            lblBorderPercent.Enabled = cbBorder.Checked;
            numPercentPictureSize.Enabled = cbBorder.Checked;

            if (cbBorder.Checked == true)
            {
                //this.parentForm.settings.WallpaperMode = LAWC.Objects.Wallpaper.WallpaperModes.Centre; // "Keep Aspect Ratio";
                this.parentForm.settings.ImageSizeScalePercent = 95;
                this.numPercentPictureSize.Value = 95;
            }
            else
            {
                //this.parentForm.settings.WallpaperMode = LAWC.Objects.Wallpaper.WallpaperModes.Stretch; // "Stretch";
                this.numPercentPictureSize.Value = 100;
                this.parentForm.settings.ImageSizeScalePercent = 100;
            }
        }

        //private void cbPopularLocations_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (this.parentForm.applyingSettings == false)
        //    {
        //        if (cbPopularLocations.SelectedItem.ToString(CultureInfo.InvariantCulture) != "[NONE]")
        //        {
        //            /// more locations: http://www.tageo.com/index-e-as-cities-AU.htm

        //            double lat = 0;
        //            double lng = 0;

        //            FrmMain.GetLatLongByName(cbPopularLocations.SelectedItem.ToString(CultureInfo.InvariantCulture), out lat, out lng);
        //            txtLatitude.Text = lat.ToString(CultureInfo.InvariantCulture);
        //            txtLongitude.Text = lng.ToString(CultureInfo.InvariantCulture);

        //            setLocationInfo();

        //            SetUseSunriseSunset();
        //            doTimerCheckChanged();

        //            SetUseSunriseSunset();
        //            //doTimerCheckChanged();

        //        }
        //        else
        //        {
        //            txtLatitude.Text = "-0.000000";
        //            txtLongitude.Text = "0.000000";

        //        }

        //        // either way, set the values
        //        //setLocationInfo();

        //        //SetUseSunriseSunset();
        //        //doTimerCheckChanged();
        //        //if (parentForm.settings.UseSunriseSunset)
        //        //{
        //        //    GetSunriseSunset();
        //        //}


        //    }
        //}

        //private void GetSunriseSunset()
        //{
        //    int offsetMins = 0;
        //    offsetMins = int.Parse(txtOffsetMins.Text);

        //    SunTimes.LatitudeCoords.Direction latDirection = SunTimes.LatitudeCoords.Direction.South;
        //    SunTimes.LongitudeCoords.Direction longDirection = SunTimes.LongitudeCoords.Direction.East;

        //    if (double.Parse(txtLatitude.Text) > 0) latDirection = SunTimes.LatitudeCoords.Direction.North;
        //    if (double.Parse(txtLongitude.Text) <= 0) longDirection = SunTimes.LongitudeCoords.Direction.West;

        //    parentForm.GetSunriseSunset(double.Parse(txtLatitude.Text), double.Parse(txtLongitude.Text), latDirection, longDirection);

        //    DateTime sunrise = parentForm.sunrise;
        //    DateTime sunset = parentForm.sunset;

        //    sunrise = sunrise.AddMinutes(offsetMins);
        //    sunset = sunset.AddMinutes(offsetMins);

        //    // round mins to nearest 5
        //    double sunriseMins = sunrise.Minute;
        //    double sunsetMins = sunset.Minute;

        //    sunriseMins = MathExtra.RoundCustom(sunriseMins, 5);
        //    sunsetMins = MathExtra.RoundCustom(sunsetMins, 5);

        //    if (sunriseMins > 55) sunriseMins = 55;
        //    if (sunsetMins > 55) sunsetMins = 55;

        //    parentForm.sunrise = new DateTime(sunrise.Year, sunrise.Month, sunrise.Day, sunrise.Hour, (int)(sunriseMins), 0);
        //    parentForm.sunset = new DateTime(sunset.Year, sunset.Month, sunset.Day, sunset.Hour, (int)(sunsetMins), 0);

        //    lblSunriseSunset.Text = ("Sunrise @ " + sunrise.ToString("HH:mm") + "\nSunset @ " + sunset.ToString("HH:mm"));

        //    if (parentForm.settings.UseSunriseSunset)
        //    {
        //        if (cbPopularLocations.SelectedItem.ToString(CultureInfo.InvariantCulture) != "[NONE]")
        //        {
        //            parentForm.settings.LightSunriseTime = parentForm.sunrise;
        //            parentForm.settings.DarkSunsetTime = parentForm.sunset;

        //            // Set the Light and Dark Times
        //            SetLightDarkTimeControls();
        //        }
        //    }

        //}


        private void GetSunriseSunset()
        {
            int offsetMins;// = 0;
            offsetMins = int.Parse(txtOffsetMins.Text, CultureInfo.InvariantCulture);

            SunTimes.LatitudeCoords.Direction latDirection = SunTimes.LatitudeCoords.Direction.North;
            SunTimes.LongitudeCoords.Direction longDirection = SunTimes.LongitudeCoords.Direction.East;

            double lat = double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture);
            double lon = double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture);

            if (lat < 0) latDirection = SunTimes.LatitudeCoords.Direction.South;
            if (lon < 0) longDirection = SunTimes.LongitudeCoords.Direction.West;

            parentForm.GetSunriseSunset(lat, lon, latDirection, longDirection);

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

        private void SetLightDarkTimeControls()
        {

            this.cbDarkHour.SelectedItem = this.parentForm.settings.DarkSunsetTime.Hour.ToString("D2", CultureInfo.InvariantCulture);
            this.cbDarkMin.SelectedItem = this.parentForm.settings.DarkSunsetTime.Minute.ToString("D2", CultureInfo.InvariantCulture);
            this.cbLightHour.SelectedItem = this.parentForm.settings.LightSunriseTime.Hour.ToString("D2", CultureInfo.InvariantCulture);
            this.cbLightMin.SelectedItem = this.parentForm.settings.LightSunriseTime.Minute.ToString("D2", CultureInfo.InvariantCulture);
        }


        private void setLocationInfo()
        {
            parentForm.settings.UseSunriseSunset = cbUseSunriseSunset.Checked;
            parentForm.settings.Latitude = double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture);
            parentForm.settings.Longitude = double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture);
            parentForm.settings.LocationPreset = txtSearch.Text;

            if (parentForm.settings.Latitude != 0 && parentForm.settings.Longitude != 0)//cbPopularLocations.SelectedItem.ToString(CultureInfo.InvariantCulture) != "[NONE]")
            {
                //btnWeatherReport.Enabled = true;
                //cbUseSunriseSunset.Checked = true;
            }
            else
            {
                //btnWeatherReport.Enabled = false;
                //cbUseSunriseSunset.Checked = false;
            }
        }

        private void cbUseSunriseSunset_CheckedChanged(object sender, EventArgs e)
        {
            SetUseSunriseSunset();
            doTimerCheckChanged();
            
        }

        /// <summary>
        /// setLocationInformation and GetSunriseSunset
        /// </summary>
        private void SetUseSunriseSunset()
        {
            setLocationInfo();
            //re-calculate
            GetSunriseSunset();

            //if (parentForm.settings.UseSunriseSunset == true)
            {
                //re-calculate
                
            }

        }


        private void btnGetLocation_Click(object sender, EventArgs e)
        {
            //SetUseSunriseSunset();
            //doTimerCheckChanged();
        }

        private void txtOffsetMins_TextChanged(object sender, EventArgs e)
        {
            if (txtOffsetMins.Text.Trim().Length > 0)
            {
                if (txtOffsetMins.Text.Trim().Equals("-", StringComparison.InvariantCulture)) return;
                parentForm.settings.TimeOffsetMins = int.Parse(txtOffsetMins.Text, CultureInfo.InvariantCulture);
                btnSetLocation.BackColor = parentForm.colourAlert;
                GetSunriseSunset();
            }
        }

        private void btnFindLocation_Click(object sender, EventArgs e)
        {
            int errorNum = 0;
            //parentForm.internetAvailable = MainFunctions.CheckForInternetConnection();
            parentForm.checkInternetConnection(false);

            if (parentForm.internetAvailable == false)
            {
                parentForm.showNoInternetMessage();
                return;
            }

            // Alternative Weather calls HERE:  https://www.obioberoi.com/2018/07/14/how-to-pull-weather-info-into-a-console-app/
            var client = new OpenWeatherAPI.OpenWeatherAPI(Constants.OpenWeatherAPIKey); 

            Query results;// = null;
            try
            {
                results = client.Query(txtSearch.Text);
            }
            catch (ArithmeticException)
            {
                results = null;
            }
            catch (System.Net.WebException ex)
            {
                results = null;
                if (ex.Message.Contains("(429)")) errorNum = 429;
            }

            // -34.206841, 138.599503);
            //var results = client.Query("Adelaide");

            if (results == null)
            {
                //btnSetLocation.Enabled = false;
                lblCityFound.Text = "City Not Found. "; //string.Empty;
                foundCityLat = 0;
                foundCityLong = 0;
                foundCityName = string.Empty;

                if (errorNum == 429)
                {
                    MessageBox.Show("There was a problem getting the weather. \nThere have been too many calls to the Weather service. \n\nPlease try again later, or manually enter the GPS coordinates");
                }
                else
                {
                    MessageBox.Show("There was a problem getting the weather. \nThe internet connection may be unavailable \nOr the city name is not known.");
                }
                return;
            }

            if (results.ValidRequest == false)
            {
                //btnSetLocation.Enabled = false;
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

            btnSetLocation.BackColor = Color.PeachPuff;

            FrmMain.showWeatherReport(results, true);

            //String output = string.Empty;

            //if (results.Name != string.Empty)
            //{
            //    output += "City: " + results.Name + "\n";
            //}
            //output += "--------------------\n";
            //output += "Temp (CelsiusCurrent): " + results.Main.Temperature.CelsiusCurrent + "°C\n";
            ////output += "Temp (CelsiusMinimum): " + results.Main.Temperature.CelsiusMinimum + "\n";
            ////output += "Temp (CelsiusMaximum): " + results.Main.Temperature.CelsiusMaximum + "\n";
            //output += "Humidity: " + results.Main.Humdity + "\n";
            //output += "Pressure: " + results.Main.Pressure + "\n";
            //output += "Clouds: " + results.Clouds.All.ToString(CultureInfo.InvariantCulture) + "\n";
            //if (results.Rain != null) output += "Rain: " + results.Rain.H3 + "\n"; else output += "Rain: 0" + "\n";
            //if (results.Snow != null) output += "Snow: " + results.Snow.H3 + "\n"; else output += "Snow: 0" + "\n";
            //output += "Sunrise: " + results.Sys.Sunrise + "\n";
            //output += "Sunset: " + results.Sys.Sunset + "\n";
            //output += "Visibility: " + results.Visibility + "\n";
            //output += "Wind (Degree): " + results.Wind.Degree + "°\n";
            //output += "Wind (Direction): " + results.Wind.Direction + "\n";
            //output += "Wind (Gust): " + results.Wind.Gust + "\n";
            //output += "Speed (MetersPerSecond): " + results.Wind.SpeedMetersPerSecond
            //    + " (" + Math.Round((results.Wind.SpeedMetersPerSecond * 60 * 60) / 1000f, 2) + " Kph)"
            //    + "\n";
            ////output += "Weathers: " + results.Weathers.Count() + "\n";

            //foreach (Weather w in results.Weathers)
            //{
            //    output += "Main: " + w.Main + "\n";
            //    output += "Description: " + w.Description + "\n";
            //    //output += "  Weathers Icon: " + w.Icon.ToString(CultureInfo.InvariantCulture) + "\n";
            //}
            //output += "Latitude: " + results.Coord.Latitude.ToString(CultureInfo.InvariantCulture) + "\n";
            //output += "Longitude: " + results.Coord.Longitude.ToString(CultureInfo.InvariantCulture) + "\n";

            //output += "\n\n";
            //output += "INSTRUCTIONS: If this is the correct city, press the Set button. \nOr, search for another city.";

            //MessageBox.Show(output);
        }

        private void btnSetLocation_Click(object sender, EventArgs e)
        {
            if (foundCityLat != 0) txtLatitude.Text = this.foundCityLat.ToString(CultureInfo.InvariantCulture);  //results.Coord.Latitude.ToString(CultureInfo.InvariantCulture);
            if (foundCityLong != 0) txtLongitude.Text = this.foundCityLong.ToString(CultureInfo.InvariantCulture); // results.Coord.Longitude.ToString(CultureInfo.InvariantCulture);

            if (!String.IsNullOrEmpty(foundCityName.ToString(CultureInfo.InvariantCulture) )) parentForm.settings.LocationPreset = this.foundCityName;
            if (foundCityLat != 0) parentForm.settings.Latitude = this.foundCityLat;
            if (foundCityLong != 0) parentForm.settings.Longitude = this.foundCityLong;

            if (!String.IsNullOrEmpty(foundCityName.ToString(CultureInfo.InvariantCulture))) txtSearch.Text = foundCityName;

            btnSetLocation.BackColor = parentForm.colourDark; // Control.DefaultBackColor; // Color.PeachPuff;
                                                                 //parentForm.frmSettings.cbPopularLocations.Text = this.foundCityName;

            setLocationInfo();
            GetSunriseSunset();

            SetUseSunriseSunset();
            doTimerCheckChanged();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
        }

        private void cbImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.parentForm.settings.WallpaperOrder = (Wallpaper.ImageOrder)Enum.Parse(typeof(Wallpaper.ImageOrder), cbImageOrder.SelectedItem.ToString()); //cbImageOrder.SelectedItem.ToString();
        }

        private void cbWallpaperMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // enumStringToType:
            this.parentForm.settings.WallpaperMode = (Wallpaper.WallpaperModes)Enum.Parse(typeof(Wallpaper.WallpaperModes), cbWallpaperMode.SelectedItem.ToString());

            switch (this.parentForm.settings.WallpaperMode)
            {
                case Wallpaper.WallpaperModes.None:
                    break;
                case Wallpaper.WallpaperModes.Centre:
                    //int percent = (int)(this.parentForm.settings.ImageSizeScalePercentPREV * 100);
                    //if (percent == 100)
                    //{
                    //    percent = 95;
                    //}
                    //tbImageSizeScalePercent.Value = percent;
                    break;
                case Wallpaper.WallpaperModes.FillWidth:
                    break;
                case Wallpaper.WallpaperModes.Stretch:
                    break;
                case Wallpaper.WallpaperModes.Tile:
                    break;
                case Wallpaper.WallpaperModes.Span:
                    break;
                case Wallpaper.WallpaperModes.LAWC:
                    break;
                default:

                    break;
            }

        }

        private void lblWallpaperModesExplained_Click(object sender, EventArgs e)
        {
            FrmMain.OpenModesURL();
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

        private void cbMultiMonitorMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.settings.MultiMonitorMode = (FrmMain.MultiMonitorModes)Enum.Parse(typeof(FrmMain.MultiMonitorModes), cbMultiMonitorMode.SelectedItem.ToString());
           
        }

        private void numPercentPictureSize_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
