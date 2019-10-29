using System;
using System.Collections.Generic;
using System.Linq;
using LAWC.Objects;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using static LAWC.Objects.EventInfo;
using static LAWC.Objects.Wallpaper;
using static LAWC.FrmMain;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace LAWC.Common
{

    internal class ScreenInfoExtra
    {
        internal Boolean ShowImageOnScreen;
        internal float Scale;

        internal ScreenInfoExtra()
        {
            Scale = 1.0f;
            ShowImageOnScreen = true;
        }
    }


    internal class Setting
    {
        //internal static int MaxRecentImageHistoryCount = 10;
        
        internal static int DefaultChangeMinutes = 10;
        internal static int DefaultAdjustSeconds = 120;


        internal List<ImageInfo> Images;
        internal List<ImageInfo> AvailableImages;
        internal List<FolderInfo> Folders;
        //internal List<String> RecentImageHistory;
        internal List<ImageInfo> FoundImages;
        internal List<EventInfo> Events;
        internal List<String> RecentImages;
        internal List<String> ImageHistory;
        internal List<WebsiteInfo> Websites;
        //internal List<Boolean> ShowImageOnScreen;
        internal List<ScreenInfoExtra> ExtraScreenInfo;


        #region Parameters

        /// <summary>
        /// The last parent folder the user accessed when selecting a folder
        /// </summary>
        private String lastFolderPath;
        public String LastFolderPath
        {
            get { return lastFolderPath; }
            set { lastFolderPath = value; }
        }

        /// <summary>
        /// The last parent folder the user accessed when selecting a Settings file
        /// </summary>
        private String lastSettingsFolderPath;
        public String LastSettingsFolderPath
        {
            get { return lastSettingsFolderPath; }
            set { lastSettingsFolderPath = value; }
        }

        /// <summary>
        /// The number of times the app has been started up
        /// </summary>
        private int useCount;
        public int UseCount
        {
            get { return useCount; }
            set { useCount = value; }
        }
        
        /// <summary>
        /// The file format that the wallpaper is saved as
        /// </summary>
        private ImageFormat wallpaperFormat;
        public ImageFormat WallpaperFormat
        {
            get { return wallpaperFormat; }
            set { wallpaperFormat = value; }
        }

        /// <summary>
        /// How compressed should the (usually only jpg) image be
        /// </summary>
        private int compressionQuality;
        public int CompressionQuality
        {
            get { return compressionQuality; }
            set { compressionQuality = value; }
        }

        private int adjustX;
        public int AdjustX
        {
            get { return adjustX; }
            set { adjustX = value; }
        }
        private int adjustY;
        public int AdjustY
        {
            get { return adjustY; }
            set { adjustY = value; }
        }

        /// <summary>
        /// How big or small the border should be around a centered image
        /// Stored as an integer (eg 100%)
        /// Returns a float version (eg 1.0f)
        /// </summary>
        private float imageScalePercent;
        public float ImageSizeScalePercent
        {
            get
            {
                return imageScalePercent;// / 100f;
            }
            set
            {
                int val = (int)value;
                // earlier versions of LAWC used a float value - this converts them to the int version to store
                if (val <= 4)
                    val = (int)(value * 100);
                imageScalePercent = val;
            }
        }

        /// <summary>
        /// Temporary variable to store the previous size of the border
        /// </summary>
        private float imageScalePercentPREV;
        public float ImageSizeScalePercentPREV
        {
            get { return imageScalePercentPREV; }
            set { imageScalePercentPREV = value; }
        }
        
        /// <summary>
        /// The brightness filter's maximum "brightest" setting allowed for a wallpaper
        /// </summary>
        private int brightnessMax;
        public int BrightnessMax
        {
            get { return brightnessMax; }
            set
            {

                brightnessMax = value;

                if (brightnessMax <= BrightnessMin)
                {
                    BrightnessMin = brightnessMax - 1;

                    if (BrightnessMin < 0)
                    {
                        BrightnessMin = 0;
                        brightnessMax = 1;
                    }
                }

            }
        }

        /// <summary>
        /// The brightness filter's minimum "darkest" setting allowed for a wallpaper
        /// </summary>
        private int brightnessMin;
        public int BrightnessMin
        {
            get { return brightnessMin; }
            set
            {
                brightnessMin = value;

                if (brightnessMin >= BrightnessMax)
                {
                    BrightnessMax = brightnessMin + 1;

                    if (BrightnessMax > 100)
                    {
                        BrightnessMin = 99;
                        brightnessMax = 100;
                    }
                }
            }
        }

        /// <summary>
        /// Should any filters be used
        /// </summary>
        private Boolean useFilters;
        public Boolean UseFilters
        {
            get { return useFilters; }
            set { useFilters = value; }
        }

        /// <summary>
        /// Does the user want an Auto backup done periodically
        /// </summary>
        private Boolean autoBackup;
        public Boolean AutoBackup
        {
            get { return autoBackup; }
            set { autoBackup = value; }
        }

        /// <summary>
        /// Filter for smallest file size allowed for wallpapers
        /// </summary>
        private long sizeKBytesMin;
        public long SizeKBytesMin
        {
            get { return sizeKBytesMin; }
            set
            {
                sizeKBytesMin = value;
            }
        }

            /// <summary>
            /// the brightness/darkness value of adjusted wallpapers
            /// </summary>
        private int gamma;
        public int Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        /// <summary>
        /// the transparency value of adjusted wallpapers
        /// </summary>
        private int alpha;
        public int Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        /// <summary>
        /// the brightness/darkness value of adjusted wallpapers
        /// </summary>
        private int brightness;
        public int Brightness
        {
            get { return brightness; }
            set { brightness = value; }
        }

        /// <summary>
        /// the brightness/darkness value of adjusted wallpapers
        /// </summary>
        private int contrast;
        public int Contrast
        {
            get { return contrast; }
            set { contrast = value; }
        }

        /// <summary>
        /// How strong should the tint effect be
        /// </summary>
        private int tintStrength;
        public int TintStrength
        {
            get { return tintStrength; }
            set { tintStrength = value; }
        }

        /// <summary>
        /// The colour that the images is mixed / tinted with
        /// </summary>
        private Color tintColour;
        public Color TintColour
        {
            get { return tintColour; }
            set { tintColour = value; }
        }
        
        /// <summary>
        /// The border/background colour used in the Dark times
        /// </summary>
        private Color backgroundColourDark;
        public Color BackgroundColourDark
        {
            get { return backgroundColourDark; }
            set { backgroundColourDark = value; }
        }

        /// <summary>
        /// The border/background colour used in the Light times
        /// </summary>
        private Color backgroundColourLight;
        public Color BackgroundColourLight
        {
            get { return backgroundColourLight; }
            set { backgroundColourLight = value; }
        }


        
        /// <summary>
        /// The column # currently used to sort the wallpaper list
        /// </summary>
        private int imageSortColumnNum;
        public int ImageSortColumnNum
        {
            get { return imageSortColumnNum; }
            set { imageSortColumnNum = value; }
        }

        /// <summary>
        /// The sort order direction for wallpapers
        /// </summary>
        private Boolean imageSortOrderDESC;
        public Boolean ImageSortOrderDESC
        {
            get { return imageSortOrderDESC; }
            set { imageSortOrderDESC = value; }
        }

        /// <summary>
        /// The time when the "day" ends (whatever time that may be)
        /// </summary>
        private DateTime startTime;
        public DateTime DarkSunsetTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// The time when the "night" ends (whatever time that may be)
        /// </summary>
        private DateTime endTime;
        public DateTime LightSunriseTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// The last search text used for location information
        /// </summary>
        private String locationPreset;
        public String LocationPreset
        {
            get { return locationPreset; }
            set { locationPreset = value; }
        }
        
        /// <summary>
        /// The amount of time to adjust the timezone by. This allows the user to adjust for daylight savings etc
        /// </summary>
        private int timeOffsetMins;
        public int TimeOffsetMins
        {
            get { return timeOffsetMins; }
            set { timeOffsetMins = value; }
        }
        
        /// <summary>
        /// The semi colon delimited list of sensor categories chosen by the user
        /// </summary>
        private String hWSensorsUsed;
        public String HWSensorCategoriessUsed
        {
            get { return hWSensorsUsed; }
            set { hWSensorsUsed = value; }
        }

        /// <summary>
        /// The last wallpapers displayed by LAWC
        /// </summary>        
        public String ImageLastWallpaper
        {
            get { return imageLastWallpaper; }
            set { imageLastWallpaper = value; }
        }
        private String imageLastWallpaper;

        /// <summary>
        /// How long should the darkening / lightening take - in minutes
        /// </summary>
        private int duration;
        public int DurationMins
        {
            get { return duration; }
            set { duration = value; }
        }
        
        /// <summary>
        /// Allows the app to pop a help message if the user has not added images, but LAWC is still running
        /// </summary>
        private DateTime settingsCreated;
        public DateTime SettingsCreated
        {
            get { return settingsCreated; }
            set { settingsCreated = value; }
        }

        /// <summary>
        /// Allows the app to do additional checks on if the user knows about the Wallpaper Website manager
        /// </summary>
        private Boolean wallpaperManagerOpened;
        public Boolean WallpaperManagerOpened
        {
            get { return wallpaperManagerOpened; }
            set { wallpaperManagerOpened = value; }
        }
        
        /// <summary>
        /// When the next wallpaper change is due to occur
        /// </summary>
        private DateTime nextWallpaperChange;
        public DateTime NextWallpaperChange
        {
            get { return nextWallpaperChange; }
            set { nextWallpaperChange = value; }
        }

        /// <summary>
        /// The standard windows modes for displaying wallpapers
        /// </summary>
        private Wallpaper.WallpaperModes imageScaling;
        public Wallpaper.WallpaperModes WallpaperMode
        {
            get { return imageScaling; }
            set { imageScaling = value; }
        }

        private Boolean useHSV;
        public Boolean UseHSV
        {
            get { return useHSV; }
            set { useHSV = value; }
        }

        private Boolean useDarkLightTimes;
        public Boolean UseDarkLightTimes
        {
            get { return useDarkLightTimes; }
            set { useDarkLightTimes = value; }
        }

        /// <summary>
        /// Does the user want to change the interface colour?
        /// </summary>
        private Boolean adjustInterfaceColour;
        public Boolean AdjustInterfaceColour
        {
            get { return adjustInterfaceColour; }
            set { adjustInterfaceColour = value; }
        }

        /// <summary>
        /// The one the USER selected last time
        /// </summary>        
        public String ImageLastSelected
        {
            get { return imageLastSelected; }
            set { imageLastSelected = value; }
        }
        private String imageLastSelected;

        ///// <summary>
        ///// The last wallpaper displayed by LAWC
        ///// </summary>        
        //public String ImageLastWallpaper
        //{
        //    get { return imageLastWallpaper; }
        //    set { imageLastWallpaper = value; }
        //}
        //private String imageLastWallpaper;
        
        /// <summary>
        /// In what order / method should wallpapers be selected
        /// </summary>
        private ImageOrder wallpaperOrder;
        public ImageOrder WallpaperOrder
        {
            get { return wallpaperOrder; }
            set { wallpaperOrder = value; }
        }

        /// <summary>
        /// How often should a new wallpaper be chosen and displayed
        /// </summary>
        private int wallpaperChangeFrequencyMins;
        public int WallpaperChangeFrequencyMins
        {
            get { return wallpaperChangeFrequencyMins; }
            set
            {
                wallpaperChangeFrequencyMins = value;
            }
        }

        /// <summary>
        /// How often should LAWC update the adjusted wallpaper
        /// </summary>
        private int imageAdjustFrequencySecs;
        public int ImageAdjustFrequencySecs
        {
            get { return imageAdjustFrequencySecs; }
            set { imageAdjustFrequencySecs = value; }
        }

        /// <summary>
        /// Should LAWC adjust the taskbar tint to match time of day etc?
        /// </summary>
        private Boolean adjustTaskbarColour;
        public Boolean AdjustTaskbarColour
        {
            get
            {
                return adjustTaskbarColour; 
                //return false;
            }
            set { adjustTaskbarColour = value; }
        }

        /// <summary>
        /// Should the adjustments always be in effect
        /// </summary>
        private Boolean adjustmentsUseAlways;
        public Boolean AdjustmentsUseAlways
        {
            get { return adjustmentsUseAlways; }
            set { adjustmentsUseAlways = value; }
        }

        /// <summary>
        /// Should LAWC use Windows 10's Dark Mode to set the interface colour
        /// </summary>
        private Boolean darkMode;
        public Boolean DarkMode
        {
            get { return darkMode; }
            set { darkMode = value; }
        }

        /// <summary>
        /// Does the user want to change the wallpaper when LAWC first starts up
        /// </summary>
        private Boolean changeOnStartup;
        public Boolean ChangeOnStartup
        {
            get { return changeOnStartup; }
            set { changeOnStartup = value; }
        }

        //private int imageQuality;
        //public int ImageQuality
        //{
        //    get { return imageQuality; }
        //    set { imageQuality = value; }
        //}

        /// <summary>
        /// A filter for the smallest wallpaper width to be displayed.
        /// </summary>
        private int minImageWidth;
        public int MinImageWidth
        {
            get { return minImageWidth; }
            set { minImageWidth = value; }
        }

        /// <summary>
        /// A filter for the smallest wallpaper height to be displayed.
        /// </summary>
        private int minImageHeight;
        public int MinImageHeight
        {
            get { return minImageHeight; }
            set { minImageHeight = value; }
        }

        /// <summary>
        /// Display the ID of each monitor
        /// </summary>
        private Boolean showScreenID;
        public Boolean ShowScreenID
        {
            get { return showScreenID; }
            set { showScreenID = value; }
        }
        
        /// <summary>
        /// vid card specific gamma setting for nvidia. Not implemented - didnt like it.
        /// </summary>
        private int screenGamma;
        public int ScreenGamma
        {
            get { return screenGamma; }
            set { screenGamma = value; }
        }

        /// <summary>
        /// Has the user finished the first startup of LAWC
        /// </summary>
        private Boolean firstRunDone;
        public Boolean FirstRunDone
        {
            get { return firstRunDone; }
            set { firstRunDone = value; }
        }
        
        /// <summary>
        /// Show a help tip on controls when the user holds the cursor over them
        /// </summary>
        private Boolean showToolTips;
        public Boolean ShowToolTips
        {
            get { return showToolTips; }
            set { showToolTips = value; }
        }

        //private String imageLastChosen;
        //public String ImageLastChosen
        //{
        //    get { return imageLastChosen; }
        //    set { imageLastChosen = value; }
        //}

        /// <summary>
        /// keep track of what wallpapers have been displayed
        /// </summary>
        private int maxRecentImageHistoryCount;
        public int MaxRecentImageHistoryCount
        {
            get { return maxRecentImageHistoryCount; }
            set { maxRecentImageHistoryCount = value; }
        }

        /// <summary>
        /// Should LAWC be minimized when started automatically, and manually
        /// </summary>
        private Boolean startMinimized;
        public Boolean StartMinimized
        {
            get { return startMinimized; }
            set { startMinimized = value; }
        }

        /// <summary>
        /// Should we check online to see if there is a new version of LAWC available
        /// </summary>
        private Boolean checkForUpdate;
        public Boolean CheckForUpdate
        {
            get { return checkForUpdate; }
            set { checkForUpdate = value; }
        }

        /// <summary>
        /// Use the sunrise/sunset times to set the dark and light times
        /// </summary>
        private Boolean useSunriseSunset;
        public Boolean UseSunriseSunset
        {
            get { return useSunriseSunset; }
            set { useSunriseSunset = value; }
        }

        /// <summary>
        /// Does the user want to record any problems with LAWC
        /// </summary>
        private Boolean writeToLog;
        public Boolean WriteToLog
        {
            get { return writeToLog; }
            set { writeToLog = value; }
        }

        /// <summary>
        /// Key1 in the change wallpaper key combo
        /// </summary>
        private String shortcutKey1;
        public String ShortcutKey1
        {
            get { return shortcutKey1; }
            set { shortcutKey1 = value; }
        }

        /// <summary>
        /// Key2 in the change wallpaper key combo
        /// </summary>
        private String shortcutKey2;
        public String ShortcutKey2
        {
            get { return shortcutKey2; }
            set { shortcutKey2 = value; }
        }

        /// <summary>
        /// Key3 in the change wallpaper key combo
        /// </summary>
        private String shortcutKey3;
        public String ShortcutKey3
        {
            get { return shortcutKey3; }
            set { shortcutKey3 = value; }
        }
        
        /// <summary>
        /// Should LAWC notify the user when the internect connection is unavailable (for Events)
        /// </summary>
        private Boolean showInternetError;
        public Boolean ShowInternetError
        {
            get { return showInternetError; }
            set { showInternetError = value; }
        }

        /// <summary>
        /// Does the user want to blur the edges?... dont like the way it works / looks
        /// </summary>
        private Boolean blurImageEdges;
        public Boolean BlurImageEdges
        {
            get { return blurImageEdges; }
            set { blurImageEdges = value; }
        }

        /// <summary>
        /// the blurred edges amount. Not used atm... dont like the way it works / looks
        /// </summary>
        private int blurAmount;
        public int BlurAmount
        {
            get { return blurAmount; }
            set { blurAmount = value; }
        }

        /// <summary>
        /// When using LAWC mode, this is used to determine at which threshold value should wallpapers start spanning displays
        /// </summary>
        private float aspectThresholdWide;
        public float AspectThresholdWide
        {
            get { return aspectThresholdWide; }
            set { aspectThresholdWide = value; }
        }
        private float aspectThresholdNarrow;
        public float AspectThresholdNarrow
        {
            get { return aspectThresholdNarrow; }
            set { aspectThresholdNarrow = value; }
        }

        private int marginToStretch;
        public int MarginToEnlarge
        {
            get { return marginToStretch; }
            set { marginToStretch = value; }
        }

        private int wideThreshold;
        public int WideThreshold
        {
            get { return wideThreshold; }
            set { wideThreshold = value; }
        }
        
        /// <summary>
        /// The lowest AspectRatio of the wallpapers that the user wants to be displayed
        /// </summary>
        private float aspectMin;
        public float AspectMin
        {
            get { return aspectMin; }
            set { aspectMin = value; }
        }

        /// <summary>
        /// The highest AspectRatio of the wallpapers that the user wants to be displayed
        /// </summary>
        private float aspectMax;
        public float AspectMax
        {
            get { return aspectMax; }
            set { aspectMax = value; }
        }

        /// <summary>
        /// Should LAWC randomly flip images horizontally? This will give some illusion of more wallpapers
        /// </summary>
        private Boolean randomFlipImage;
        public Boolean RandomFlipImage
        {
            get { return randomFlipImage; }
            set { randomFlipImage = value; }
        }

        /// <summary>
        /// The lat of the location the user has set
        /// </summary>
        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        /// The long of the location the user has set
        /// </summary>
        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        /// The amount to adjust the images by, by the description in the reset dropdown - advanced settings
        /// </summary>
        private String imageAdjustmentName;
        public String ImageAdjustmentName
        {
            get { return imageAdjustmentName; }
            set { imageAdjustmentName = value; }
        }

        /// <summary>
        /// Not used yet. How should wallpapers be displayed on multiple screens
        /// </summary>
        private MultiMonitorModes multiMonitorMode;
        public MultiMonitorModes MultiMonitorMode
        {
            get { return multiMonitorMode; }
            set { multiMonitorMode = value; }
        }

        /// <summary>
        /// Does the user want to check sensors and get a message if there is a problem with any of them
        /// </summary>
        private Boolean checkSensorsOnStartup;
        public Boolean CheckSensorsOnStartup
        {
            get { return checkSensorsOnStartup; }
            set { checkSensorsOnStartup = value; }
        }


        private String openWeatherAPIKey;
        public String OpenWeatherAPIKey
        {
            get { return openWeatherAPIKey; }
            set { openWeatherAPIKey = value; }
        }


        /// <summary>
        /// How many seconds is the lowest that users can set Weather sensors. 
        /// Set to 30 mins so that we dont overload the free OpenWeather service as much
        /// </summary>
        internal const int MinimumWeatherUpdateSeconds = 900; //15 mins
        internal const int MinimumWeatherUpdateSecondsHasKey = 60;

        private FrmMain parentForm;
#endregion


        public Setting(FrmMain vParentForm)
        {
            parentForm = vParentForm;
            Initialise();

        }

        internal void Initialise()
        {
            // DEFAULTS

            Images = new List<ImageInfo>();
            AvailableImages = new List<ImageInfo>();
            Folders = new List<FolderInfo>();
            ImageHistory = new List<String>();
            RecentImages = new List<String>();
            ExtraScreenInfo = new List<ScreenInfoExtra>();
            Websites = new List<WebsiteInfo>();
            FoundImages = new List<ImageInfo>();
            Events = new List<EventInfo>();
            
            LastFolderPath = "";
            LastSettingsFolderPath = "";
            BrightnessMax = 100;
            BrightnessMin = 0;
            TintColour = Color.FromArgb(255, 192, 64, 0);  //Color.DarkOrange; //Color.FromArgb(255, 192, 64, 0);
            TintStrength = 0;
            Gamma = 100;
            Brightness = 0;
            Alpha = 0;
            Contrast = 0;
            BackgroundColourDark = Color.FromArgb(255, 12, 5, 0); //(255, 0, 0, 64);
            BackgroundColourLight = Color.FromArgb(255, 135, 206, 250);
            DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
            LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
            DurationMins = 60;
            WallpaperMode = Wallpaper.WallpaperModes.LAWC; // "FIT";//"KEEP ASPECT RATIO";            
            //ResizeImages = true;
            UseDarkLightTimes = true;
            UseHSV = true;
            ImageLastSelected = "";
            ImageLastWallpaper = "";
            WallpaperOrder = ImageOrder.LowestViewCountRandom; // "Ordered";
            //TimerUpdateFrequencyMS = 60000;
            WallpaperChangeFrequencyMins = DefaultChangeMinutes;
            NextWallpaperChange = DateTime.Now.AddMinutes(WallpaperChangeFrequencyMins);
            ImageAdjustFrequencySecs = DefaultAdjustSeconds;
            AdjustmentsUseAlways = false;
            ChangeOnStartup = true;
            MinImageWidth = 200;
            MinImageHeight = 200;
            ShowScreenID = false;
            ScreenGamma = 12;
            FirstRunDone = false;
            ShowToolTips = true;
            //ImageLastChosen = "";
            MaxRecentImageHistoryCount = 50;            
            StartMinimized = false;
            ImageSortColumnNum = 0;
            ImageSortOrderDESC = false;
            AdjustTaskbarColour = false;
            ImageSizeScalePercent = 100.0f;
            ImageSizeScalePercentPREV = 100.0f;
            AdjustInterfaceColour = true;
            CheckForUpdate = true;
            WriteToLog = true;
            ShowInternetError = false;
            //OnlyShowFilteredImages = false;
            BlurImageEdges = false;
            BlurAmount = 5;
            aspectThresholdNarrow = 1.0f;
            AspectThresholdWide = 2.0f;
            MarginToEnlarge = 10;
            WideThreshold = 30;
            //DoubleClickIconAction = FrmMain.IconDoubleClickActions.ChangeFormState;
            AspectMin = 0;
            AspectMax = 99;
            RandomFlipImage = true;
            DarkMode = false;
            UseSunriseSunset = false;
            UseFilters = true;
            AutoBackup = true;
            LocationPreset = "[NONE]";
            ImageAdjustmentName = "Darker";
            SizeKBytesMin = 30;
            SettingsCreated = DateTime.Now;
            WallpaperManagerOpened = false;
            MultiMonitorMode = MultiMonitorModes.SameOnAll;
            CheckSensorsOnStartup = false;

            HWSensorCategoriessUsed = "";

            Latitude = 0;
            Longitude = 0;
            TimeOffsetMins = 0;

            CompressionQuality = 90;
            WallpaperFormat = ImageFormat.Jpeg; // Png; //ImageFormat.Jpeg; //ImageFormat.Bmp;

            AdjustX = 0;
            AdjustY = 0;

            ShortcutKey1 = "Control";
            ShortcutKey2 = "Alt";
            ShortcutKey3 = "C";

            OpenWeatherAPIKey = string.Empty;
        }


        internal void Reset()
        {
            Dispose();
            Initialise();
        }


        internal void Dispose()
        {
            Images.Clear();
            AvailableImages.Clear();
            Folders.Clear();
            RecentImages.Clear();
            ExtraScreenInfo.Clear();
            FoundImages.Clear();
            ImageHistory.Clear();
        }

        /// <summary>
        /// Get the settings full path WITHOUT the filename
        /// </summary>
        /// <param name="vIsPortable"></param>
        /// <returns></returns>
        public static string getSettingsFullPath(Boolean vIsPortable)
        {
            string settingsFullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                + "\\" + MainFunctions.AppPathName + "\\";

            if (vIsPortable)
            {
                settingsFullPath = Application.ExecutablePath.ToString(CultureInfo.InvariantCulture).Replace("LAWC.exe", ""); // System.IO.Directory.GetCurrentDirectory() + "\\";
            }

            return settingsFullPath;
        }


        public static string getCurrentSettingsFullPathWithFilename(Boolean vIsPortable)
        {
            String settingsFullPath;// = string.Empty;
            //String settingsFullPath = Application.StartupPath + "\\" + MainFunctions.SettingsFilenameOnly; // read file from app path
            //string settingsFullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            //    + "\\" + AppPathName + "\\" + MainFunctions.SettingsFilenameOnly;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.SettingsFilePath))
            {
                settingsFullPath = Properties.Settings.Default.SettingsFilePath;
            }
            else
            {
                settingsFullPath = getSettingsFullPath(vIsPortable) + MainFunctions.SettingsFilenameOnly;
            }
            

            return settingsFullPath;
        }

        public static string getSettingsFullPathFixed(Boolean vIsPortable)
        {
            string settingsFullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                + "\\" + MainFunctions.AppPathName + "\\";

            if (vIsPortable)
            {
                settingsFullPath = Application.ExecutablePath.ToString(CultureInfo.InvariantCulture).Replace("LAWC.exe", ""); // System.IO.Directory.GetCurrentDirectory() + "\\";
            }

            return settingsFullPath;
        }


        public static string getSettingsFullPathWithFilenameFixed(Boolean vIsPortable)
        {
            String settingsFullPath;// = string.Empty;
            //String settingsFullPath = Application.StartupPath + "\\" + MainFunctions.SettingsFilenameOnly; // read file from app path
            //string settingsFullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            //    + "\\" + AppPathName + "\\" + MainFunctions.SettingsFilenameOnly;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.SettingsFilePath))
            {
                settingsFullPath = Properties.Settings.Default.SettingsFilePath;
            }
            else
            {
                settingsFullPath = getSettingsFullPathFixed(vIsPortable) + MainFunctions.SettingsFilenameOnly;
            }


            return settingsFullPath;
        }


        public static string GetErrorLogFullPath(Boolean vIsPortable, String vSettingsFullPath)
        {
            //String settingsFullPath = Application.StartupPath + "\\" + MainFunctions.SettingsFilenameOnly; // read file from app path
            //string settingsFullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            //    + "\\" + AppPathName + "\\" + MainFunctions.ErrorLogFilenameOnly;

            //if (!vSettingsFullPath.Substring(vSettingsFullPath.Length - 1, 1).Equals(@"\", StringComparison.InvariantCulture))
            //{
            //    if (vIsPortable == false) { }
            //    // ERROR - the passed in path doesnt have a backslash
            //    int testg = 1;
            //}

            //String settingsFullPath = getSettingsFullPath(vIsPortable) + MainFunctions.ErrorLogFilenameOnly;
            String settingsFullPath = vSettingsFullPath.Replace(MainFunctions.SettingsFilenameOnly, "") + MainFunctions.ErrorLogFilenameOnly;

            if (vIsPortable)
            {
                settingsFullPath = Application.ExecutablePath.ToString(CultureInfo.InvariantCulture).Replace("LAWC.exe", "") + MainFunctions.ErrorLogFilenameOnly; // System.IO.Directory.GetCurrentDirectory() + "\\";
            }

            return settingsFullPath;
        }


        internal void ResetViewCount(String vFullPath)
        {
            int indexT = Images.FindIndex(r => r.FullPath == vFullPath);
            Images[indexT].ViewCount = 0;

        }
        
        internal void ResetAllViewCount()
        {
            for (int i = 0; i < Images.Count; i++)
            {
                Images[i].ViewCount = 0;
            }

        }

        internal void ResetFolderViewCount(int vFolderID)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (Images[i].FolderID == vFolderID)
                {
                    Images[i].ViewCount = 0;
                }
                
            }

        }

        internal int GetImageCount(int vFolderID)
        {
            int output = 0;

            for (int i = 0; i < Images.Count; i++)
            {
                if (Images[i].FolderID == vFolderID)
                {
                    output++;
                }
            }

            return output;
        }
        internal int GetImageCount(String vFolder)
        {
            int folderID = GetFolderID(vFolder);

            return GetImageCount(folderID);
        }

        internal void setFoldersImageCounts()
        {
            for (int i = 0; i < Folders.Count; i++)
            {
                Folders[i].setImageCount(this);
            }
        }

        internal String GetFolderName(int vFolderID)
        {
            int indexT = Folders.FindIndex(r => r.ID.Equals(vFolderID));

            return Path.GetFileName(Folders[indexT].Path);

        }



            internal void UpdateImageInfo(ImageInfo vNewInfo)
        {
            String newFilenameOnly = System.IO.Path.GetFileName(vNewInfo.FullPath);

            int indexT = Images.FindIndex(r => r.FullPath.Contains(newFilenameOnly));

            if (indexT < 0)
            {
                // not found
                // ADD NEW ENTRY
                Images.Add(vNewInfo);
            }
            else
            {
                // found
                // UPDATE Existing
                Images[indexT].Aspect = vNewInfo.Aspect;
                Images[indexT].AverageBrightness = vNewInfo.AverageBrightness;
                Images[indexT].AverageColour = vNewInfo.AverageColour;
                Images[indexT].FolderID = vNewInfo.FolderID;
                Images[indexT].FullPath = vNewInfo.FullPath;
                Images[indexT].Height = vNewInfo.Height;
                Images[indexT].SizeBytes = vNewInfo.SizeBytes;
                //Images[indexT].ViewCount = vNewInfo.ViewCount;
                Images[indexT].Width = vNewInfo.Width;
            }

        }


        internal void UpdateImageInfoOLD(ImageInfo vNewInfo)
        {
            String newFilenameOnly = System.IO.Path.GetFileName(vNewInfo.FullPath);

            for (int i = 0; i < Images.Count; i++)
            {
                // if we have found the file by filename only
                if (Images[i].FullPath.Contains(newFilenameOnly)) // && Images[i].FolderID == vNewInfo.FolderID)
                {
                    // UPDATE Existing
                    Images[i].Aspect = vNewInfo.Aspect;
                    Images[i].AverageBrightness = vNewInfo.AverageBrightness;
                    Images[i].AverageColour = vNewInfo.AverageColour;
                    Images[i].FolderID = vNewInfo.FolderID;
                    Images[i].FullPath = vNewInfo.FullPath;
                    Images[i].Height = vNewInfo.Height;
                    Images[i].SizeBytes = vNewInfo.SizeBytes;
                    //Images[i].ViewCount = vNewInfo.ViewCount;
                    Images[i].Width = vNewInfo.Width;
                    break;
                }
                else
                {
                    // if the image isnt already in the list, add it
                    if (FindImageByPath(vNewInfo.FullPath) == false)
                    {
                        // ADD NEW ENTRY
                        Images.Add(vNewInfo);
                        break;
                    }

                }
            }
        }


        internal Boolean ViewCountAdd(String vFullPath, int vAmount)
        {
            int indexT = Images.FindIndex(r => r.FullPath == vFullPath);
            if (indexT < 0)
            {
                return false;
            }
            else
            {
                Images[indexT].ViewCount += vAmount;
                return true;
            }

        }


        internal int GetImageIndex(String vFullPath)
        {
            int indexT = Images.FindIndex(r => r.FullPath == vFullPath);

            return indexT;

            //int output = 0;

            //for (int i = Images.Count - 1; i > 0; i--)
            //{
            //    if (Images[i].FullPath == vFullPath)
            //    {
            //        output = i;
            //        break;
            //    }

            //}
            //return output;
        }


        internal int GetFolderIDBByImageIndex(String vFullPath)
        {
            //int output = 0;

            int indexT = Images.FindIndex(r => r.FullPath == vFullPath);
            if (indexT < 0)
            {
                // not found
                return -1;
            }
            else
            {
                return Images[indexT].FolderID;
            }
            
            //for (int i = Images.Count - 1; i > 0; i--)
            //{
            //    if (Images[i].FullPath == vFullPath)
            //    {
            //        output = Images[i].FolderID;
            //        break;
            //    }

            //}
            //return output;
        }


        internal Boolean FolderExists(String vPath)
        {
            Boolean output = false;

            for (int i = Folders.Count - 1; i >= 0; i--)
            {
                if (Folders[i].Path.Equals(vPath, StringComparison.InvariantCulture))
                {
                    output = true;
                    break;
                }
            }

            return output;
        }


        internal Boolean FindImageByPath(String vFullPath)
        {
            //Boolean output = false;

            //int indexT = listEmployee.FindIndex(r >= r.Name == findName);
            int indexT = Images.FindIndex(r => r.FullPath == vFullPath);

            return (indexT > 0);

        }


        internal void RemoveFolder(int vID)
        {

            for (int i = Folders.Count - 1; i >= 0; i--)
            {
                if (Folders[i].ID.Equals(vID))
                {
                    Folders.RemoveAt(i);
                    break;
                }
            }

            // clear the history if all the folders have been cleared
            //if (Folders.Count == 0)
            //{
            //    this.RecentImageHistory.Clear();
            //}


        }


        internal void RemoveImagesByPath(String vFolder)
        {

            for (int i = Images.Count - 1; i >= 0; i--)
            {
                if (Images[i].FullPath.Contains(vFolder))
                {
                    Images.RemoveAt(i);
                }
            }
        }


        internal void RemoveImagesByFilename(String vFilename)
        {

            for (int i = Images.Count - 1; i >= 0; i--)
            {
                if (Images[i].FullPath.Contains(vFilename))
                {
                    Images.RemoveAt(i);
                    break;
                }
            }



        }

        internal Boolean HasOwnWeatherKey()//String vKey)
        {
            if (OpenWeatherAPIKey == Constants.OpenWeatherAPIKey)
            {
                return false;
            }
            else if (String.IsNullOrEmpty(OpenWeatherAPIKey))
            {
                return false;
            }
            else if (OpenWeatherAPIKey.Length != 32)
            {
                return false;
            }
            return true; 
        }

        internal void RemoveEvent(String vFile, CheckActionType vAction, String vSensorName, String vMessage)
        {

            for (int i = Events.Count - 1; i >= 0; i--)
            {
                if (Events[i].ImagePath.Contains(vFile)
                    && Events[i].CheckAction == vAction
                    && Events[i].SensorName == vSensorName
                    && Events[i].Message == vMessage
                    )
                {
                    Events.RemoveAt(i);
                    break;
                }
            }

        }


        internal int GetEventIndex(String vEventType, Decimal vValue, String vMessage, String vImagePath, String vEventName, Boolean vShowNotification, String vCheckValueString)
        {

            for (int i = Events.Count - 1; i >= 0; i--)
            {
                if (Events[i].TypeOfEvent == vEventType
                    && Events[i].SensorName == vEventName
                    && Events[i].CheckValueDecimal == vValue
                    && Events[i].Message == vMessage
                    && Events[i].ImagePath == vImagePath
                    && Events[i].ShowNotification == vShowNotification
                    && vCheckValueString.Contains(Events[i].CheckValueString)
                    )
                {
                    return i;
                }
            }

            return -1;
        }
        

        internal int GetNextID()
        {
            int output = -1;

            for (int i = 0; i < Folders.Count; i++)
            {
                if (Folders[i].ID > output)
                {
                    // get the highest value
                    output = Folders[i].ID;
                }
            }

            output++; // the next ID is clear

            return output;
        }


        internal ImageInfo GetRandomImage()
        {

            //int r = vRand.Next(this.AvailableImages.Count); //vListView.Items.Count);
            int r = (int)Randomizer.Next(0, this.Images.Count);

            ImageInfo output;

            if (r < Images.Count())
            {
                output = Images[r];
            }
            else
            {
                output = new ImageInfo();
            }


            return output;

        }


        internal ImageInfo GetRandomImage(Random vRand)
        {

            //int r = vRand.Next(Images.Count);

            //return Images[r];

            int r = vRand.Next(this.Images.Count); //vListView.Items.Count);

            ImageInfo output;

            if (r < Images.Count())
            {
                output = Images[r];
            }
            else
            {
                output = new ImageInfo();
            }


            return output;

        }
        
               

        internal Boolean IsFolderEnabled(String vPath)
        {

            for (int i = 0; i < this.Folders.Count(); i++)
            {
                if (Folders[i].Path.Equals(vPath, StringComparison.InvariantCulture))
                {
                    if (Folders[i].Enabled == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        internal int GetFolderID(String vFolderPath)
        {

            for (int i = 0; i < this.Folders.Count(); i++)
            {
                if (Folders[i].Path.Equals(vFolderPath, StringComparison.InvariantCulture))
                {
                    return i;
                }

            }

            return -1;
        }

        internal FolderInfo GetFolderByID(int vFolderID)
        {

            for (int i = 0; i < this.Folders.Count(); i++)
            {
                if (Folders[i].ID.Equals(vFolderID))
                {
                    return Folders[i];
                }

            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vWriteToLog"></param>
        /// <param name="vFirstRunDone">This is the first time the app runs</param>
        /// <returns></returns>
        public static DataSet ReadSettings(Boolean vFirstRunDone, String vSettingsFullPath = "")
        {
            String settingsFullPath = vSettingsFullPath;

            if (string.IsNullOrEmpty(settingsFullPath) && !string.IsNullOrEmpty(vSettingsFullPath))
            {
                settingsFullPath = vSettingsFullPath;// getSettingsFullPathWithFilename(Properties.Settings.Default.Portable); // Application.StartupPath + "\\" + vSettingsFilename; // read file from app path
            }
            else
            {
                settingsFullPath = getSettingsFullPathWithFilenameFixed(Properties.Settings.Default.Portable);
            }

            DataSet output = new DataSet();

            if (System.IO.File.Exists(settingsFullPath))
            {
                TextReader tr;
                XmlTextReader reader;

                tr = new StreamReader(settingsFullPath);
                reader = new XmlTextReader(tr) { DtdProcessing = DtdProcessing.Prohibit };

                try
                {
                    //XmlReadMode mode = output.ReadXml(settingsFullPath, XmlReadMode.ReadSchema);
                    
                    XmlReadMode mode = output.ReadXml(reader, XmlReadMode.ReadSchema);
                    if (mode != XmlReadMode.ReadSchema)
                    {
                        ErrorHandling.ProcessError(null, ErrorHandling.ErrorMessageType.XMLFileRead, true, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                    }

                }
                catch (FileLoadException ex)
                {
                    //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Error 008: Settings file is corrupt, a new one will be created.", vWriteToLog);
                    //MessageBox.Show("Error reading settings file (" + settingsFullPath +").\n\n" + ex.Message.ToString(CultureInfo.InvariantCulture));
                    //MessageBox.Show("Your Settings file (" + settingsFullPath + ") is corrupt.\n\n  A new one will be created.\n\nError: " + ex.Message, "Settings File Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.ReadingSettingsXML, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                    //throw;
                }
                catch (XmlException ex)
                {
                    ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.ReadingSettingsXML, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                    output = null; // no xml file loaded!
                }
                finally
                {
                    reader.Close();
                    //if (tr != null) tr.Close();
                }
            }
            else
            {
                // dont log an error if this is the first time the app has run, and needs to create the settings file
                if (vFirstRunDone) ErrorHandling.ProcessError(null, ErrorHandling.ErrorMessageType.SettingsNotFound, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Warning 001: Settings file not found, a new one will be created.", vWriteToLog);
                //MessageBox.Show("This is your first time running LAWC.\n\n  A new settings file will be created (" + settingsFullPath + ").", "Settings File Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return output;

        }


        public static void LoadDSToSettings(DataSet dsMain, ref Setting vSetting, out Boolean vNewInstall, String vSettingsFullPath)
        {
            //DataRow dr = default(DataRow);

            string currentSetting = string.Empty;
            string currentValue = string.Empty;

            vNewInstall = true;

            try
            {               

                if (dsMain.Tables["Config"] == null)
                {
                    String settingsFullPath = Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable); // Application.StartupPath + "\\" + vSettingsFilename; // read file from app path

                    if (System.IO.File.Exists(settingsFullPath))
                    {
                        vNewInstall = false;
                    } else
                    {
                        vNewInstall = true;
                    }
                        // table is null = probably a fresh startup
                        //ErrorHandling.ProcessError(null, ErrorHandling.ErrorMessageType.LoadingDSToSettings, true, true, String.Format(""));
                }
                else
                { 
                    // NOT NULL ... READ CONTENTS
                    vNewInstall = false;

                    foreach (DataRow dr in dsMain.Tables["Config"].Rows)
                    {
                        currentSetting = dr["Setting"].ToString();
                        currentValue = dr["Value"].ToString();

                        if (dr["Setting"].ToString() == "LastFolderPath")
                        {
                            vSetting.LastFolderPath = dr["Value"].ToString();
                        }
                        
                        if (dr["Setting"].ToString() == "LastSettingsFolderPath")
                        {
                            vSetting.LastSettingsFolderPath = dr["Value"].ToString();
                        }

                        if (dr["Setting"].ToString() == "UseCount")
                        {
                            vSetting.UseCount = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "SizeKBytesMin")
                        {
                            vSetting.SizeKBytesMin = uint.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "BrightnessMax")
                        {
                            vSetting.BrightnessMax = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "BrightnessMin")
                        {
                            vSetting.BrightnessMin = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        //YourEnum foo = (YourEnum) Enum.Parse(typeof(YourEnum), yourString); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! GOOD
                        //if (dr["Setting"].ToString() == "IconDoubleClickActions")
                        //{
                        //    vSetting.DoubleClickIconAction = (FrmMain.IconDoubleClickActions)Enum.Parse(typeof(FrmMain.IconDoubleClickActions), dr["Value"].ToString());
                        //}

                        //if (dr["Setting"].ToString() == "ShowFilteredImages")
                        //{
                        //    vSetting.OnlyShowFilteredImages = Boolean.Parse(dr["Value"].ToString());
                        //}
                        //if (dr["Setting"].ToString() == "DarkMode")
                        //{
                        //    vSetting.DarkMode = Boolean.Parse(dr["Value"].ToString());
                        //}                        

                        if (dr["Setting"].ToString() == "Gamma")
                        {
                            Boolean result = float.TryParse(dr["Value"].ToString(), out float val);
                            // if its a float - changed to int / percent for ver 0.9.8.0
                            if (result == true)
                            {
                                 float checkMod = val - ((int)val);
                                if (checkMod > 0 && checkMod < 1)
                                {
                                    val *= 100;
                                }
                            }
                            vSetting.Gamma = (int)val;
                        }

                        if (dr["Setting"].ToString() == "Alpha")
                        {
                            Boolean result = float.TryParse(dr["Value"].ToString(), out float val);
                            // if its a float - changed to int / percent for ver 0.9.8.0
                            if (result == true)
                            {
                                if (val > 0 && val < 1)
                                {
                                    val *= 100;
                                }                                
                            }
                            vSetting.Alpha = (int)val;
                            //vSetting.Alpha = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "Brightness")
                        {
                            Boolean result = float.TryParse(dr["Value"].ToString(), out float val);
                            // if its a float - changed to int / percent for ver 0.9.8.0
                            if (result == true)
                            {
                                if (val > 0 && val < 1)
                                {
                                    val *= 100;
                                }
                            }
                            vSetting.Brightness = (int)val;
                            //vSetting.Brightness = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "Contrast")
                        {
                            Boolean result = float.TryParse(dr["Value"].ToString(), out float val);
                            // if its a float - changed to int / percent for ver 0.9.8.0
                            if (result == true)
                            {
                                if (val > 0 && val < 1)
                                {
                                    val *= 100;
                                }
                            }
                            vSetting.Contrast = (int)val;
                            //vSetting.Contrast = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "ImageSizeScalePercent")
                        {
                            vSetting.ImageSizeScalePercent = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "DarkTime")
                        {
                            //vSetting.DarkSunsetTime = DateTime.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                            //account for AU, local, and ISO date formats
                            Boolean success = DateTime.TryParse(dr["Value"].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt);
                            if (success == false)
                            {
                                dt = DateTime.Parse(dr["Value"].ToString(), CultureInfo.GetCultureInfo("en-AU"));
                            }
                            vSetting.DarkSunsetTime = dt;

                            // Make sure the start date is always THE CURRENT DATE (TODAY)
                            if (vSetting.DarkSunsetTime.Date < DateTime.Now.Date)
                            {
                                vSetting.DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                    vSetting.DarkSunsetTime.Hour, vSetting.DarkSunsetTime.Minute, vSetting.DarkSunsetTime.Second);
                            }
                        }

                        if (dr["Setting"].ToString() == "LightTime")
                        {
                            //vSetting.LightSunriseTime = DateTime.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                            //account for AU, local, and ISO date formats
                            Boolean success = DateTime.TryParse(dr["Value"].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt);
                            if (success == false)
                            {
                                dt = DateTime.Parse(dr["Value"].ToString(), CultureInfo.GetCultureInfo("en-AU"));
                            }
                            vSetting.LightSunriseTime = dt;


                            // Make sure the start date is always THE CURRENT DATE (TODAY)
                            if (vSetting.LightSunriseTime.Date < DateTime.Now.Date)
                            {
                                vSetting.LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                    vSetting.LightSunriseTime.Hour, vSetting.LightSunriseTime.Minute, vSetting.LightSunriseTime.Second);
                            }
                        }

                        if (dr["Setting"].ToString() == "NextWallpaperChange")
                        {
                            //vSetting.NextWallpaperChange = DateTime.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                            //account for AU, local, and ISO date formats
                            Boolean success = DateTime.TryParse(dr["Value"].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt);
                            if (success == false)
                            {
                                dt = DateTime.Parse(dr["Value"].ToString(), CultureInfo.GetCultureInfo("en-AU"));
                            }
                            vSetting.NextWallpaperChange = dt;

                        }

                        if (dr["Setting"].ToString() == "SettingsCreated")
                        {
                            //vSetting.SettingsCreated = DateTime.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                            //account for AU, local, and ISO date formats
                            Boolean success = DateTime.TryParse(dr["Value"].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt);
                            if (success == false)
                            {
                                dt = DateTime.Parse(dr["Value"].ToString(), CultureInfo.GetCultureInfo("en-AU"));
                            }
                            vSetting.SettingsCreated = dt;

                        }

                        if (dr["Setting"].ToString() == "WallpaperManagerOpened")
                        {
                            vSetting.WallpaperManagerOpened = Boolean.Parse(dr["Value"].ToString());
                        }


                        if (dr["Setting"].ToString() == "Duration")
                        {
                            vSetting.DurationMins = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "BackgroundColourDark")
                        {
                            vSetting.BackgroundColourDark = Color.FromArgb(int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture));
                        }

                        if (dr["Setting"].ToString() == "BackgroundColourLight")
                        {
                            vSetting.BackgroundColourLight = Color.FromArgb(int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture));
                        }

                        if (dr["Setting"].ToString() == "TintColour")
                        {
                            vSetting.TintColour = Color.FromArgb(int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture));
                        }

                        if (dr["Setting"].ToString() == "WallpaperMode")
                        {
                            // enumStringToType:
                            vSetting.WallpaperMode = (Wallpaper.WallpaperModes)Enum.Parse(typeof(Wallpaper.WallpaperModes), dr["Value"].ToString());
                        }
                        //if (dr["Setting"].ToString() == "ResizeImages")
                        //{
                        //    vSetting.ResizeImages = Boolean.Parse(dr["Value"].ToString());
                        //}

                        if (dr["Setting"].ToString() == "UseTimer")
                        {
                            vSetting.UseDarkLightTimes = Boolean.Parse(dr["Value"].ToString());
                        }
                        if (dr["Setting"].ToString() == "UseHSV")
                        {
                            vSetting.UseHSV = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "ImageLastSelected")
                        {
                            vSetting.ImageLastSelected = dr["Value"].ToString();
                        }

                        if (dr["Setting"].ToString() == "ImageLastWallpapers" || dr["Setting"].ToString() == "ImageLastWallpaper") // old name
                        {
                            vSetting.ImageLastWallpaper = dr["Value"].ToString();
                        }

                        if (dr["Setting"].ToString() == "WallpaperOrder")
                        {
                            vSetting.WallpaperOrder = (Wallpaper.ImageOrder)Enum.Parse(typeof(Wallpaper.ImageOrder), dr["Value"].ToString()); //dr["Value"].ToString();
                        }

                        //if (dr["Setting"].ToString() == "TimerUpdateFrequencyMS")
                        //{
                        //    vSetting.TimerUpdateFrequencyMS = int.Parse(dr["Value"].ToString());
                        //}

                        if (dr["Setting"].ToString() == "WallpaperChangeFrequencyMins")
                        {
                            vSetting.WallpaperChangeFrequencyMins = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "ImageAdjustFrequencySecs")
                        {
                            vSetting.ImageAdjustFrequencySecs = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "AdjustmentsUseAlways")
                        {
                            vSetting.AdjustmentsUseAlways = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "DarkMode")
                        {
                            vSetting.DarkMode = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "UseFilters")
                        {
                            vSetting.UseFilters = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "AutoBackup")
                        {
                            vSetting.AutoBackup = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "LocationPreset")
                        {
                            vSetting.LocationPreset = dr["Value"].ToString();
                        }

                        if (dr["Setting"].ToString() == "TimeOffsetMins")
                        {
                            vSetting.TimeOffsetMins = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "HWSensorsUsed")
                        {
                            vSetting.HWSensorCategoriessUsed = dr["Value"].ToString();
                        }

                        if (dr["Setting"].ToString() == "ChangeOnStartup")
                        {
                            vSetting.ChangeOnStartup = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "MinImageWidth")
                        {
                            vSetting.MinImageWidth = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "ImageSortColumnNum")
                        {
                            vSetting.ImageSortColumnNum = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "ImageSortOrderDESC")
                        {
                            vSetting.ImageSortOrderDESC = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "MinImageHeight")
                        {
                            vSetting.MinImageHeight = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "ShowScreenID")
                        {
                            vSetting.ShowScreenID = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "ScreenGammaPercent")
                        {
                            vSetting.ScreenGamma = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "FirstRunDone")
                        {
                            vSetting.FirstRunDone = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "ShowToolTips")
                        {
                            vSetting.ShowToolTips = Boolean.Parse(dr["Value"].ToString());
                        }

                        //if (dr["Setting"].ToString() == "ImageLastChosen")
                        //{
                        //    vSetting.ImageLastChosen = dr["Value"].ToString();
                        //}

                        if (dr["Setting"].ToString() == "MaxRecentImageHistoryCount")
                        {
                            vSetting.MaxRecentImageHistoryCount = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "TintStrength")
                        {
                            vSetting.TintStrength = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "StartMinimized")
                        {
                            vSetting.StartMinimized = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "AdjustTaskbarColour")
                        {
                            vSetting.AdjustTaskbarColour = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "DarkMode")
                        {
                            vSetting.DarkMode = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "AdjustInterfaceColour")
                        {
                            vSetting.AdjustInterfaceColour = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "UseSunriseSunset")
                        {
                            vSetting.UseSunriseSunset = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "CheckForUpdate")
                        {
                            vSetting.CheckForUpdate = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "WriteToLog")
                        {
                            vSetting.WriteToLog = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "ShowInternetError")
                        {
                            vSetting.ShowInternetError = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "BlurImageEdges")
                        {
                            vSetting.BlurImageEdges = Boolean.Parse(dr["Value"].ToString());
                        }

                        if (dr["Setting"].ToString() == "BlurAmount")
                        {
                            vSetting.BlurAmount = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "ShortcutKey1")
                        {
                            vSetting.ShortcutKey1 = dr["Value"].ToString();
                        }
                        if (dr["Setting"].ToString() == "ShortcutKey2")
                        {
                            vSetting.ShortcutKey2 = dr["Value"].ToString();
                        }
                        if (dr["Setting"].ToString() == "ShortcutKey3")
                        {
                            vSetting.ShortcutKey3 = dr["Value"].ToString();
                        }
                        if (dr["Setting"].ToString() == "OpenWeatherAPIKey")
                        {
                            vSetting.OpenWeatherAPIKey = dr["Value"].ToString();
                        }
                        
                        //if (dr["Setting"].ToString() == "LatDirection")
                        //{
                        //    vSetting.LatDirection = dr["Value"].ToString();
                        //}
                        //if (dr["Setting"].ToString() == "LongDirection")
                        //{
                        //    vSetting.LongDirection = dr["Value"].ToString();
                        //}
                        if (dr["Setting"].ToString() == "Latitude")
                        {
                            vSetting.Latitude = double.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "Longitude")
                        {
                            vSetting.Longitude = double.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "AspectThresholdWide")
                        {
                            vSetting.AspectThresholdWide = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "AspectThresholdNarrow")
                        {
                            vSetting.AspectThresholdNarrow = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "MarginToStretch")
                        {
                            vSetting.MarginToEnlarge = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "WideThreshold")
                        {
                            vSetting.WideThreshold = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }                        
                        if (dr["Setting"].ToString() == "AspectMin")
                        {
                            vSetting.AspectMin = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "AspectMax")
                        {
                            vSetting.AspectMax = float.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }

                        if (dr["Setting"].ToString() == "RandomFlipImage")
                        {
                            vSetting.RandomFlipImage = Boolean.Parse(dr["Value"].ToString());
                        }
                        if (dr["Setting"].ToString() == "ImageAdjustmentName")
                        {
                            vSetting.ImageAdjustmentName = dr["Value"].ToString();
                        }
                        if (dr["Setting"].ToString() == "CompressionQuality")
                        {
                            vSetting.CompressionQuality = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "AdjustX")
                        {
                            vSetting.AdjustX = int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "AdjustY")
                        {
                            vSetting.AdjustY= int.Parse(dr["Value"].ToString(), CultureInfo.InvariantCulture);
                        }
                        if (dr["Setting"].ToString() == "WallpaperFormat")
                        {
                            vSetting.WallpaperFormat = StringToImageFormat(dr["Value"].ToString());
                        }
                        if (dr["Setting"].ToString() == "MultiMonitorMode")
                        {
                            vSetting.MultiMonitorMode = (FrmMain.MultiMonitorModes)Enum.Parse(typeof(FrmMain.MultiMonitorModes), dr["Value"].ToString());
                        }
                        if (dr["Setting"].ToString() == "CheckSensorsOnStartup")
                        {
                            vSetting.CheckSensorsOnStartup = Boolean.Parse(dr["Value"].ToString());
                        }



                    }  // end for
                } // end if

            }
            catch (FormatException ex)
            {
                ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.LoadingSettings, true, false, String.Format(CultureInfo.InvariantCulture, "**** Setting:{0}  Value:{1} ****", currentSetting, currentValue), vSettingsFullPath);
                //MessageBox.Show("Error Loading settings (Setting: " + currentSetting + " Val: " + currentValue + ")\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Error Loading settings (Setting: " + currentSetting + " Val: " + currentValue + ")\n\n" + ex.Message, vSetting.WriteToLog);
            }

        }


        public static ImageFormat StringToImageFormat(String vInput)
        {
            //ImageFormat output;// = ImageFormat.Png; //default

            switch (vInput)
            {
                case "Png": return ImageFormat.Png;
                case "Bmp": return ImageFormat.Bmp;
                case "Emf": return ImageFormat.Emf;
                case "Exif": return ImageFormat.Exif;
                case "Gif": return ImageFormat.Gif;
                case "Icon": return ImageFormat.Icon;
                case "Jpeg": return ImageFormat.Jpeg;
                case "MemoryBmp": return ImageFormat.MemoryBmp;
                case "Tiff": return ImageFormat.Tiff;
                case "Wmf": return ImageFormat.Wmf;
                default:
                    return ImageFormat.Png;
            }

        }
        

        public static Boolean SaveSettings(String vSettingsFullPath, Setting vSetting)
        {
            
            if (vSetting == null) return false;

            Boolean output = false;
            String settingsFullPath = Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable);
            //Properties.Settings.Default.SettingsFilePath = string.Empty;
            if (!string.IsNullOrEmpty(vSettingsFullPath))
            {
                //settingsFullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                //+ "\\" + MainFunctions.AppPathName + "\\" + vFileName;
                settingsFullPath = vSettingsFullPath;
            }

            DataSet dsMain = new DataSet();
            DataTable dtConfig = new DataTable();
            DataTable dtItems = new DataTable();
            DataTable dtFolders = new DataTable();
            DataTable dtImageHistory = new DataTable();
            DataTable dtExtraScreenInfo = new DataTable();
            DataTable dtWebsites = new DataTable();
            DataTable dtEvents = new DataTable();
            //DataTable dtRecentlyAdded = new DataTable();
            DataRow dr;


            //if (vSettingsFilename != string.Empty)
            //{
            //    settingsFullPath = System.IO.Path.GetDirectoryName(settingsFullPath) + vSettingsFilename;
            //}

            // CONFIG TABLE ///////////

            dtConfig.TableName = "Config";
            dtConfig.Columns.Add("Setting", System.Type.GetType("System.String"));
            dtConfig.Columns.Add("Value", System.Type.GetType("System.String"));

            // Text / Number based Settings 

            dtConfig.Rows.Add("UseCount", vSetting.UseCount);

            dtConfig.Rows.Add("LastFolderPath", vSetting.LastFolderPath); 
            dtConfig.Rows.Add("LastSettingsFolderPath", vSetting.LastSettingsFolderPath); 
            dtConfig.Rows.Add("SizeKBytesMin", vSetting.SizeKBytesMin);
            dtConfig.Rows.Add("BrightnessMax", vSetting.BrightnessMax);
            dtConfig.Rows.Add("BrightnessMin", vSetting.BrightnessMin);
            //dtConfig.Rows.Add("ShowFilteredImages", vSetting.OnlyShowFilteredImages);
            //dtConfig.Rows.Add("DarkMode", vSetting.DarkMode);
            dtConfig.Rows.Add("TintStrength", vSetting.TintStrength);
            dtConfig.Rows.Add("Gamma", vSetting.Gamma);
            dtConfig.Rows.Add("Alpha", vSetting.Alpha);
            dtConfig.Rows.Add("Contrast", vSetting.Contrast);
            dtConfig.Rows.Add("Brightness", vSetting.Brightness);
            dtConfig.Rows.Add("BackgroundColourDark", vSetting.BackgroundColourDark.ToArgb().ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("BackgroundColourLight", vSetting.BackgroundColourLight.ToArgb().ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("TintColour", vSetting.TintColour.ToArgb().ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("DarkTime", vSetting.DarkSunsetTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("UseHSV", vSetting.UseHSV);
            dtConfig.Rows.Add("LightTime", vSetting.LightSunriseTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("Duration", vSetting.DurationMins);
            dtConfig.Rows.Add("WallpaperMode", vSetting.WallpaperMode);
            //dtConfig.Rows.Add("ResizeImages", vSetting.ResizeImages);
            dtConfig.Rows.Add("UseTimer", vSetting.UseDarkLightTimes);
            dtConfig.Rows.Add("ImageLastSelected", vSetting.ImageLastSelected);
            dtConfig.Rows.Add("ImageLastWallpapers", vSetting.ImageLastWallpaper);
            dtConfig.Rows.Add("WallpaperOrder", vSetting.WallpaperOrder);
            dtConfig.Rows.Add("AdjustX", vSetting.AdjustX);
            dtConfig.Rows.Add("AdjustY", vSetting.AdjustY);
            //dtConfig.Rows.Add("TimerUpdateFrequencyMS", vSetting.TimerUpdateFrequencyMS);
            dtConfig.Rows.Add("WallpaperChangeFrequencyMins", vSetting.WallpaperChangeFrequencyMins);
            dtConfig.Rows.Add("NextWallpaperChange", vSetting.NextWallpaperChange.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("SettingsCreated", vSetting.SettingsCreated.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("WallpaperManagerOpened", vSetting.WallpaperManagerOpened);
            dtConfig.Rows.Add("ImageAdjustFrequencySecs", vSetting.ImageAdjustFrequencySecs);
            dtConfig.Rows.Add("AdjustmentsUseAlways", vSetting.AdjustmentsUseAlways);
            dtConfig.Rows.Add("ChangeOnStartup", vSetting.ChangeOnStartup);
            dtConfig.Rows.Add("CompressionQuality", vSetting.CompressionQuality);
            dtConfig.Rows.Add("WallpaperFormat", vSetting.WallpaperFormat);
            dtConfig.Rows.Add("MinImageWidth", vSetting.MinImageWidth);
            dtConfig.Rows.Add("MinImageHeight", vSetting.MinImageHeight);
            dtConfig.Rows.Add("ShowScreenID", vSetting.ShowScreenID);
            dtConfig.Rows.Add("ScreenGammaPercent", vSetting.ScreenGamma);
            dtConfig.Rows.Add("FirstRunDone", vSetting.FirstRunDone);
            dtConfig.Rows.Add("ShowToolTips", vSetting.ShowToolTips);
            //dtConfig.Rows.Add("ImageLastChosen", vSetting.ImageLastChosen);
            dtConfig.Rows.Add("MaxRecentImageHistoryCount", vSetting.MaxRecentImageHistoryCount);
            dtConfig.Rows.Add("StartMinimized", vSetting.StartMinimized);
            dtConfig.Rows.Add("ImageSortColumnNum", vSetting.ImageSortColumnNum);
            dtConfig.Rows.Add("ImageSortOrderDESC", vSetting.ImageSortOrderDESC);
            dtConfig.Rows.Add("AdjustTaskbarColour", vSetting.AdjustTaskbarColour);
            dtConfig.Rows.Add("ImageSizeScalePercent", vSetting.ImageSizeScalePercent);
            dtConfig.Rows.Add("AdjustInterfaceColour", vSetting.AdjustInterfaceColour);
            dtConfig.Rows.Add("CheckForUpdate", vSetting.CheckForUpdate);
            dtConfig.Rows.Add("WriteToLog", vSetting.WriteToLog);
            dtConfig.Rows.Add("ShowInternetError", vSetting.ShowInternetError);
            dtConfig.Rows.Add("BlurImageEdges", vSetting.BlurImageEdges);
            dtConfig.Rows.Add("BlurAmount", vSetting.BlurAmount);
            //dtConfig.Rows.Add("IconDoubleClickActions", vSetting.DoubleClickIconAction.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("RandomFlipImage", vSetting.RandomFlipImage.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("DarkMode", vSetting.DarkMode.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("UseSunriseSunset", vSetting.UseSunriseSunset.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("LocationPreset", vSetting.LocationPreset.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("Latitude", vSetting.Latitude.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("Longitude", vSetting.Longitude.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("HWSensorsUsed", vSetting.HWSensorCategoriessUsed.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("ImageAdjustmentName", vSetting.ImageAdjustmentName.ToString(CultureInfo.InvariantCulture));
            //dtConfig.Rows.Add("CompressionQuality", vSetting.CompressionQuality.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("WallpaperFormat", vSetting.WallpaperFormat.ToString());
            dtConfig.Rows.Add("CheckSensorsOnStartup", vSetting.CheckSensorsOnStartup.ToString(CultureInfo.InvariantCulture));

            dtConfig.Rows.Add("UseFilters", vSetting.UseFilters.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("AutoBackup", vSetting.AutoBackup.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("MultiMonitorMode", vSetting.MultiMonitorMode.ToString());
            dtConfig.Rows.Add("AspectThresholdWide", vSetting.AspectThresholdWide.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("AspectThresholdNarrow", vSetting.AspectThresholdNarrow.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("MarginToStretch", vSetting.MarginToEnlarge.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("WideThreshold", vSetting.WideThreshold.ToString(CultureInfo.InvariantCulture));

            //dtConfig.Rows.Add("LatDirection", vSetting.LatDirection.ToString(CultureInfo.InvariantCulture));
            //dtConfig.Rows.Add("LongDirection", vSetting.LongDirection.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("TimeOffsetMins", vSetting.TimeOffsetMins.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("AspectMin", vSetting.AspectMin.ToString(CultureInfo.InvariantCulture));
            dtConfig.Rows.Add("AspectMax", vSetting.AspectMax.ToString(CultureInfo.InvariantCulture));

            dtConfig.Rows.Add("ShortcutKey1", vSetting.ShortcutKey1);
            dtConfig.Rows.Add("ShortcutKey2", vSetting.ShortcutKey2);
            dtConfig.Rows.Add("ShortcutKey3", vSetting.ShortcutKey3);

            dtConfig.Rows.Add("OpenWeatherAPIKey", vSetting.OpenWeatherAPIKey);
            

            // Add the table
            dsMain.Tables.Add(dtConfig);


            ///// END CONFIG ////////////////////////////




            //// SHOW IMAGE ON SCREEN //////////////////////////////////

            dtExtraScreenInfo.TableName = "ExtraScreenInfo";
            dtExtraScreenInfo.Columns.Add("ShowImage", System.Type.GetType("System.Boolean"));
            dtExtraScreenInfo.Columns.Add("Scale", System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtExtraScreenInfo);



            for (int i = 0; i < vSetting.ExtraScreenInfo.Count; i++)
            {

                dr = dsMain.Tables["ExtraScreenInfo"].NewRow();

                dr["ShowImage"] = vSetting.ExtraScreenInfo[i].ShowImageOnScreen;
                dr["Scale"] = vSetting.ExtraScreenInfo[i].Scale;

                dtExtraScreenInfo.Rows.Add(dr);

            }



            //// Website URLs //////////////////////////////////

            dtWebsites.TableName = "Websites";
            dtWebsites.Columns.Add("URL", System.Type.GetType("System.String"));
            dtWebsites.Columns.Add("LastVisited", System.Type.GetType("System.String"));
            dtWebsites.Columns.Add("Done", System.Type.GetType("System.Boolean"));
            dtWebsites.Columns.Add("Name", System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtWebsites);


            for (int i = 0; i < vSetting.Websites.Count; i++)
            {
                dr = dsMain.Tables["Websites"].NewRow();

                dr["URL"] = vSetting.Websites[i].URL;
                dr["LastVisited"] = vSetting.Websites[i].LastVisited.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                dr["Done"] = vSetting.Websites[i].Done;
                dr["Name"] = vSetting.Websites[i].Name;

                dtWebsites.Rows.Add(dr);

            }
            //dr = dsMain.Tables["Websites"].NewRow();

            //dr["URL"] = "http://www.google.com.au";
            //dr["LastVisited"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            //dr["Done"] = false.ToString(CultureInfo.InvariantCulture);
            //dr["Name"] = "Google AU";

            //dtWebsites.Rows.Add(dr);



            //// RECENT HISTORY //////////////////////////////////

            dtImageHistory.TableName = "ImageHistory";
            dtImageHistory.Columns.Add("FullPath", System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtImageHistory);

            for (int i = 0; i < vSetting.ImageHistory.Count; i++)
            {
                dr = dsMain.Tables["ImageHistory"].NewRow();

                dr["FullPath"] = vSetting.ImageHistory[i];

                dtImageHistory.Rows.Add(dr);
            }

            //// FOLDERS //////////////////////////////////

            dtFolders.TableName = "Folders";
            dtFolders.Columns.Add("Path", System.Type.GetType("System.String"));
            dtFolders.Columns.Add("ID", System.Type.GetType("System.String"));
            dtFolders.Columns.Add("Enabled", System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtFolders);

            for (int i = 0; i < vSetting.Folders.Count; i++)
            {

                dr = dsMain.Tables["Folders"].NewRow();

                dr["Path"] = vSetting.Folders[i].Path.ToString(CultureInfo.InvariantCulture);
                dr["ID"] = vSetting.Folders[i].ID.ToString(CultureInfo.InvariantCulture);
                dr["Enabled"] = vSetting.Folders[i].Enabled.ToString(CultureInfo.InvariantCulture);


                dtFolders.Rows.Add(dr);
            }


            //// ITEMS / IMAGES //////////////////////////////////
            dtItems.TableName = "Items";
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FullPath], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.AverageColour], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.AverageBrightness], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FolderID], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.ViewCount], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.SizeBytes], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Aspect], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Width], System.Type.GetType("System.String"));
            dtItems.Columns.Add(StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Height], System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtItems);

            for (int i = 0; i < vSetting.Images.Count; i++)
            {
                dr = dsMain.Tables["Items"].NewRow();

                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FullPath]] = vSetting.Images[i].FullPath;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.AverageColour]] = vSetting.Images[i].AverageColour.ToArgb().ToString(CultureInfo.InvariantCulture);
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.AverageBrightness]] = vSetting.Images[i].AverageBrightness;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FolderID]] = vSetting.Images[i].FolderID;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.ViewCount]] = vSetting.Images[i].ViewCount;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.SizeBytes]] = vSetting.Images[i].SizeBytes;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Aspect]] = vSetting.Images[i].Aspect;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Width]] = vSetting.Images[i].Width;
                dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Height]] = vSetting.Images[i].Height;
                //if (vSetting.Images[i].Aspect != 0)
                //{
                //    int ix = 1;
                //}

                dtItems.Rows.Add(dr);
            }


            //// EVENTS //////////////////////////////////

            dtEvents.TableName = "Events";
            dtEvents.Columns.Add("ImagePath", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("Message", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("CheckSeconds", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("TypeOfEvent", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("CheckAction", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("CheckValueDecimal", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("CheckValueString", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("OverrideWallpaper", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("Enabled", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("NameOfEvent", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("ShowNotification", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("FontSize", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("FontColour", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("LastRun", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("OrderPos", System.Type.GetType("System.String"));
            dtEvents.Columns.Add("Transparency", System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtEvents);

            for (int i = 0; i < vSetting.Events.Count; i++)
            {

                dr = dsMain.Tables["Events"].NewRow();

                dr["ImagePath"] = vSetting.Events[i].ImagePath.ToString(CultureInfo.InvariantCulture);
                dr["Message"] = vSetting.Events[i].Message.ToString(CultureInfo.InvariantCulture);
                dr["CheckSeconds"] = vSetting.Events[i].CheckSeconds.ToString(CultureInfo.InvariantCulture);
                dr["TypeOfEvent"] = vSetting.Events[i].TypeOfEvent.ToString(CultureInfo.InvariantCulture);
                dr["CheckAction"] = vSetting.Events[i].CheckAction.ToString();
                dr["CheckValueDecimal"] = vSetting.Events[i].CheckValueDecimal.ToString(CultureInfo.InvariantCulture);
                dr["CheckValueString"] = vSetting.Events[i].CheckValueString.ToString(CultureInfo.InvariantCulture);
                dr["OverrideWallpaper"] = vSetting.Events[i].OverrideWallpaper.ToString(CultureInfo.InvariantCulture);
                dr["Enabled"] = vSetting.Events[i].Enabled.ToString(CultureInfo.InvariantCulture);
                dr["NameOfEvent"] = vSetting.Events[i].SensorName.ToString(CultureInfo.InvariantCulture);
                dr["ShowNotification"] = vSetting.Events[i].ShowNotification.ToString(CultureInfo.InvariantCulture);
                dr["FontSize"] = vSetting.Events[i].FontSize.ToString(CultureInfo.InvariantCulture);
                dr["FontColour"] = vSetting.Events[i].FontColour.ToArgb().ToString(CultureInfo.InvariantCulture);
                dr["LastRun"] = vSetting.Events[i].LastRun.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                dr["OrderPos"] = vSetting.Events[i].OrderPos.ToString(CultureInfo.InvariantCulture);
                dr["Transparency"] = vSetting.Events[i].Transparency.ToString(CultureInfo.InvariantCulture);

                dtEvents.Rows.Add(dr);
            }


            //////  WRITE THE OUTPUT //////////////////

            try
            {
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(settingsFullPath)))
                {
                    string path = System.IO.Path.GetDirectoryName(settingsFullPath);
                    System.IO.Directory.CreateDirectory(path);
                }
                dsMain.WriteXml(settingsFullPath, XmlWriteMode.WriteSchema);
                output = true;

            }
            catch (IOException ex)
            {
                ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.SavingSettings, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                //MessageBox.Show("Error saving settings: \n\n" + ex.Message);
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Error saving settings: \n\n" + ex.Message, vSetting.WriteToLog);
            }



            dtConfig.Clear();
            dtItems.Clear();
            dtFolders.Clear();
            dtImageHistory.Clear();
            dtExtraScreenInfo.Clear();
            dtWebsites.Clear();
            dsMain.Tables.Clear();
            dsMain.Clear();

            dtConfig.Dispose();
            dtItems.Dispose();
            dtFolders.Dispose();
            dtImageHistory.Dispose();
            dtExtraScreenInfo.Dispose();
            dtWebsites.Dispose();
            dsMain.Dispose();

            return output;

        }
        

        public static Boolean LoadDSTablesToArray(DataSet vDSMain, Setting vSetting)
        {
            DataRow dr;

            try
            {


                vSetting.Images.Clear();


                if (vDSMain.Tables["Items"] != null)
                {
                    //StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FullPath]

                    for (int i = 0; i < vDSMain.Tables["Items"].Rows.Count; i++)
                    {
                        ImageInfo item = new ImageInfo();

                        dr = vDSMain.Tables["Items"].Rows[i];

                        //System.Drawing.Color colour = ImageFunctions.GetColorFromString(dr["AverageColour"].ToString());
                        // choose between old tags and new tags - to adjust for shortened tags since 0.9.7.4
                        /*Object oldTagVal = dr[(int)StringIDs.ImageInfoFields.FullPath]; *///"FullPath"
                        //int key = (int)StringIDs.ImageInfoFields.FullPath;
                        //String tag = StringIDs.ImageInfoKeys[key];
                        //String newVal = dr[tag].ToString();

                        //this allows for old tags and new tags
                        Object oldTagVal = dr[(int)StringIDs.ImageInfoFields.FullPath];
                        item.FullPath = (oldTagVal != null) ? dr[(int)StringIDs.ImageInfoFields.FullPath].ToString() : dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FullPath]].ToString();
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.AverageColour];
                        item.AverageColour = (oldTagVal != null) ? Color.FromArgb(int.Parse(dr[(int)StringIDs.ImageInfoFields.AverageColour].ToString(), CultureInfo.InvariantCulture)) : Color.FromArgb(int.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.AverageColour]].ToString(), CultureInfo.InvariantCulture));
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.AverageBrightness];
                        item.AverageBrightness = (oldTagVal != null) ? float.Parse(dr[(int)StringIDs.ImageInfoFields.AverageBrightness].ToString(), CultureInfo.InvariantCulture) : float.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.AverageBrightness]].ToString(), CultureInfo.InvariantCulture);
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.ViewCount];
                        item.ViewCount = (oldTagVal != null) ? int.Parse(dr[(int)StringIDs.ImageInfoFields.ViewCount].ToString(), CultureInfo.InvariantCulture) : int.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.ViewCount]].ToString(), CultureInfo.InvariantCulture);
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.SizeBytes];
                        item.SizeBytes = (oldTagVal != null) ? long.Parse(dr[(int)StringIDs.ImageInfoFields.SizeBytes].ToString(), CultureInfo.InvariantCulture) : long.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.SizeBytes]].ToString(), CultureInfo.InvariantCulture);
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.Aspect];
                        item.Aspect = (oldTagVal != null) ? float.Parse(dr[(int)StringIDs.ImageInfoFields.Aspect].ToString(), CultureInfo.InvariantCulture) : float.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Aspect]].ToString(), CultureInfo.InvariantCulture);
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.FolderID];
                        item.FolderID = (oldTagVal != null) ? int.Parse(dr[(int)StringIDs.ImageInfoFields.FolderID].ToString(), CultureInfo.InvariantCulture) : int.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.FolderID]].ToString(), CultureInfo.InvariantCulture);
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.Width];
                        item.Width = (oldTagVal != null) ? int.Parse(dr[(int)StringIDs.ImageInfoFields.Width].ToString(), CultureInfo.InvariantCulture) : int.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Width]].ToString(), CultureInfo.InvariantCulture);
                        oldTagVal = dr[(int)StringIDs.ImageInfoFields.Height];
                        item.Height = (oldTagVal != null) ? int.Parse(dr[(int)StringIDs.ImageInfoFields.Height].ToString(), CultureInfo.InvariantCulture) : int.Parse(dr[StringIDs.ImageInfoKeys[(int)StringIDs.ImageInfoFields.Height]].ToString(), CultureInfo.InvariantCulture);

                        //item.FullPath = dr["FullPath"].ToString(CultureInfo.InvariantCulture);
                        //item.AverageColour = Color.FromArgb(int.Parse(dr["AverageColour"].ToString(CultureInfo.InvariantCulture))); // System.Drawing.Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
                        //item.AverageBrightness = float.Parse(dr["AverageBrightness"].ToString(CultureInfo.InvariantCulture));
                        //item.FolderID = int.Parse(dr["FolderID"].ToString(CultureInfo.InvariantCulture));
                        //item.ViewCount = Int32.Parse(dr["ViewCount"].ToString(CultureInfo.InvariantCulture));
                        //item.SizeBytes = long.Parse(dr["SizeBytes"].ToString(CultureInfo.InvariantCulture));
                        //item.Aspect = float.Parse(dr["Aspect"].ToString(CultureInfo.InvariantCulture));
                        //if (dr.Table.Columns.Contains("Width")) item.Width = int.Parse(dr["Width"].ToString(CultureInfo.InvariantCulture));
                        //if (dr.Table.Columns.Contains("Height")) item.Height = int.Parse(dr["Height"].ToString(CultureInfo.InvariantCulture));

                        //if (dr["CopyOnly"].ToString(CultureInfo.InvariantCulture).ToUpper() == "TRUE")
                        //{
                        //    item.CopyOnly = true;
                        //}
                        //else
                        //{
                        //    item.CopyOnly = false;
                        //}

                        // if the image isnt already in the list, add it
                        if (vSetting.FindImageByPath(item.FullPath) == false)
                        {
                            vSetting.Images.Add(item);
                        }


                    }

                } // end if


                vSetting.Folders.Clear();

                if (vDSMain.Tables["Folders"] != null)
                {

                    for (int i = 0; i < vDSMain.Tables["Folders"].Rows.Count; i++)
                    {
                        dr = vDSMain.Tables["Folders"].Rows[i];

                        FolderInfo item = new FolderInfo
                        {
                            ID = int.Parse(dr["ID"].ToString(), CultureInfo.InvariantCulture),
                            Path = dr["Path"].ToString(),
                            Enabled = Boolean.Parse(dr["Enabled"].ToString())
                        };

                        // check the paths 
                        if (System.IO.Directory.Exists(item.Path) == false)
                        {
                            string msg = string.Empty;
                            if (vDSMain.Tables["Folders"].Rows.Count > 1)
                            {
                                msg = "";
                            }
                            else
                            {
                                // only 1 row
                                msg = "Remove this folder, and add another folder (as this was the only one).";
                            }

                            MessageBox.Show("The folder " + item.Path + " no longer exists.  Maybe the folder has been moved? \n\n" + msg, "Folder Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        vSetting.Folders.Add(item);

                    }

                } // end if

                if (vDSMain.Tables["ImageHistory"] != null)
                {

                    for (int i = 0; i < vDSMain.Tables["ImageHistory"].Rows.Count; i++)
                    {
                        dr = vDSMain.Tables["ImageHistory"].Rows[i];

                        String item = dr["FullPath"].ToString();

                        vSetting.ImageHistory.Add(item);

                    }

                } // end if


                if (vDSMain.Tables["Websites"] != null)
                {

                    for (int i = 0; i < vDSMain.Tables["Websites"].Rows.Count; i++)
                    {
                        dr = vDSMain.Tables["Websites"].Rows[i];

                        String itemURL = dr["URL"].ToString();
                        //DateTime itemDT = DateTime.Parse(dr["LastVisited"].ToString(), CultureInfo.InvariantCulture);
                        //account for AU, local, and ISO date formats
                        Boolean success = DateTime.TryParse(dr["LastVisited"].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime itemDT);
                        if (success == false)
                        {
                            itemDT = DateTime.Parse(dr["LastVisited"].ToString(), CultureInfo.GetCultureInfo("en-AU"));
                        }


                        Boolean itemDone = Boolean.Parse(dr["Done"].ToString());
                        String itemName = dr["Name"].ToString();

                        WebsiteInfo item = new WebsiteInfo
                        {
                            URL = itemURL,
                            LastVisited = itemDT,
                            Done = itemDone,
                            Name = itemName
                        };

                        vSetting.Websites.Add(item);

                    }

                } // end if


                if (vDSMain.Tables["Events"] != null)
                {

                    for (int i = 0; i < vDSMain.Tables["Events"].Rows.Count; i++)
                    {
                        dr = vDSMain.Tables["Events"].Rows[i];

                        String imagePath = dr["ImagePath"].ToString();
                        String message = dr["Message"].ToString();
                        String checkSeconds = dr["CheckSeconds"].ToString();
                        String typeOfEvent = dr["TypeOfEvent"].ToString();
                        String checkAction = dr["CheckAction"].ToString();
                        String checkValueDecimal = dr["CheckValueDecimal"].ToString();
                        String checkValueString = dr["CheckValueString"].ToString();
                        String overrideWallpaper = dr["OverrideWallpaper"].ToString();
                        String enabled = dr["Enabled"].ToString();
                        String showNotification = dr["ShowNotification"].ToString();
                        String nameOfEvent = dr["NameOfEvent"].ToString();
                        String fontSize = dr["FontSize"].ToString();
                        Color fontColour = Color.FromArgb(int.Parse(dr["FontColour"].ToString(), CultureInfo.InvariantCulture)); //  dr["FontColour"].ToString();
                        //DateTime lastRun = DateTime.Parse(dr["LastRun"].ToString(), CultureInfo.InvariantCulture);

                        //account for AU, local, and ISO date formats
                        Boolean success = DateTime.TryParse(dr["LastRun"].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime lastRun);
                        if (success == false)
                        {
                            lastRun = DateTime.Parse(dr["LastRun"].ToString(), CultureInfo.GetCultureInfo("en-AU"));
                        }                        

                        //String lastRunStr = lastRun.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        int orderPos = int.Parse(dr["OrderPos"].ToString(), CultureInfo.InvariantCulture);
                        int transparency = int.Parse(dr["Transparency"].ToString(), CultureInfo.InvariantCulture);

                        EventInfo item = new EventInfo
                        {
                            ImagePath = imagePath,
                            Message = message,
                            CheckSeconds = int.Parse(checkSeconds, CultureInfo.InvariantCulture),
                            TypeOfEvent = typeOfEvent, //EventInfo.StringToCheckEventType(typeOfEvent),
                            SensorName = nameOfEvent,
                            CheckAction = EventInfo.StringToCheckActionType(checkAction),
                            CheckValueDecimal = Decimal.Parse(checkValueDecimal, CultureInfo.InvariantCulture),
                            CheckValueString = checkValueString,
                            OverrideWallpaper = Boolean.Parse(overrideWallpaper),
                            Enabled = Boolean.Parse(enabled),
                            Displayed = false,
                            ShowNotification = Boolean.Parse(showNotification),
                            FontSize = float.Parse(fontSize, CultureInfo.InvariantCulture),
                            FontColour = fontColour, //System.Drawing.ColorTranslator.FromHtml(fontColour),
                            LastRun = lastRun,
                            OrderPos = orderPos,
                            Transparency = transparency,
                        };

                        Sensor.SensorSource source = Sensor.getSensorSource(item.SensorName);

                        int updateSeconds = MinimumWeatherUpdateSeconds;
                        //if user has a key, then let them update more often
                        if (!String.IsNullOrEmpty(vSetting.OpenWeatherAPIKey))
                        {
                            updateSeconds = MinimumWeatherUpdateSecondsHasKey;
                        }

                        // stop weather from updateing too much, even if manually set from the settings file
                        if (item.CheckSeconds < updateSeconds && source == Sensor.SensorSource.Weather) //item.SensorName.ToUpperInvariant().Contains("WEATHER"))
                        {
                            item.CheckSeconds = updateSeconds;
                        }
                        else if (item.CheckSeconds < MinimumSensorUpdateSeconds)
                        {
                            item.CheckSeconds = MinimumSensorUpdateSeconds;
                        }

                        vSetting.Events.Add(item);

                    }

                } // end if



                if (vDSMain.Tables["ExtraScreenInfo"] != null)
                {

                    for (int i = 0; i < vDSMain.Tables["ExtraScreenInfo"].Rows.Count; i++)
                    {
                        if (i < Screen.AllScreens.Length)
                        {
                            dr = vDSMain.Tables["ExtraScreenInfo"].Rows[i];

                            Boolean _ShowImage = Boolean.Parse(dr["ShowImage"].ToString());
                            float _scale = float.Parse(dr["Scale"].ToString(), CultureInfo.InvariantCulture);

                            vSetting.ExtraScreenInfo.Add(new ScreenInfoExtra()
                            {
                                Scale = _scale,
                                ShowImageOnScreen = _ShowImage

                            });
                        }
                    }

                } // end if

            }
            catch (FormatException ex)
            {
                ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.LoadingDSTablesToArray, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                //MessageBox.Show("Unknown Setting Found: " + ex.Message);
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Unknown Setting Found: " + ex.Message, vSetting.WriteToLog);
                return false;
            }



            return true;

        }


        internal int GetLowestViewCountSettings()
        {
            int output = 999999999;

            Boolean found = false;

            for (int i = 0; i < Images.Count(); i++)
            {

                if (Images[i].ViewCount < output)
                {
                    output = Images[i].ViewCount;
                    found = true;

                    // if it finds Zero we need to exit - cant get lower!
                    if (output == 0) break;
                }
            }

            if (found == false)
            {
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "getLowestViewCount could not find a lowest value?!", WriteToLog);
                ErrorHandling.ProcessError(null, ErrorHandling.ErrorMessageType.LowestViewCountNotFound, true, false, String.Format(CultureInfo.InvariantCulture, ""), getSettingsFullPath(Properties.Settings.Default.Portable));
                output = 0;
            }


            return output;
        }
        

    }
}
