using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Net.NetworkInformation;
using System.Globalization;
using LAWC.Common;
using LAWC.Objects;
using MBG.SimpleWizard;
using LAWC.Wizard;
using OpenHardwareMonitor.Hardware;
using BrightIdeasSoftware;
using static LAWC.Common.ScreenExtensions;
using static LAWC.Common.ErrorHandling;
using static LAWC.Objects.Wallpaper;
using static LAWC.Objects.Sensor;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using OpenWeatherAPI;
using System.Data;
using static OpenWeatherAPI.OpenWeatherAPI;
using System.Net.Mail;



namespace LAWC
{

    public partial class FrmMain : Form
    {

        #region Enums

        internal enum HDDValueCategory
        {
            UsedSpaceMB,
            FreeSpaceMB,
            UsedSpacePercent,
            FreeSpacePercent
        }

        internal enum ScreenState
        {
            GettingDarker,
            GettingLighter,
            Dark,
            Light
        }

        //internal enum IconDoubleClickActions
        //{
        //    ChangeFormState,
        //    ChangeWallpaper
        //}

        internal enum InterfaceColours
        {
            darkest,
            darker,
            dark,
            medium,
            lighter,
            lightest,
            none
        }

        internal enum BatteryInfoCategory
        {
            BatteryChargeStatus,
            BatteryFullLifetime,
            BatteryLifePercent,
            BatteryLifeRemaining,
            PowerLineStatus,
        }

        internal enum MultiMonitorModes
        {
            SameOnAll,
            DifferentOnAll,
            Span,
        }



        #endregion


        #region Constants

        /// <summary>
        /// the output wallpaper file number to increment to, before going back to zero. Used to stop some "file in use" problems
        /// </summary>
        private const int wallpaperFileCount = 4;

        /// <summary>
        /// How many days before performing a backup
        /// </summary>
        private const int AutobackupDays = 14;

        /// <summary>
        /// How many backup files to keep before deleting older ones
        /// </summary>
        private const int BackupsToKeep = 5;

        /// <summary>
        /// On a DisplaySettingsChanged event, this is how many seconds will elapse before refreshing the monitor info etc.
        /// This is done because the event can fire multiple times
        /// </summary>
        private const int refreshDelaySeconds = 2; // minimum how long before it will change again

        /// <summary>
        /// Duh, its the time in ms before a ping timeouts :P
        /// </summary>
        internal const int pingTimeout = 500;



        /// <summary>
        /// How many seconds is the lowest users can set for NON WEATHER sensors
        /// </summary>
        internal const int MinimumSensorUpdateSeconds = 10;

        #endregion


        #region Variables

        internal int OSVersion = -1;

        /// <summary>
        /// used to time how long the rescan takes to process the found image list
        /// </summary>
        internal DateTime rescanStarted;

        // subordinate forms
        internal frmSettings frmSettings;   // Simple / Core settings
        internal frmSettingsAdvanced frmSettingsAdvanced; // All / Advanced settings
        internal frmImageMetadata frmMetaData; // show the info about the selected image
        internal frmThanks frmThanks; // show what tools, features, etc in LAWC are sourced from other places
        private frmWebsites frmWebsites; // list of wallpaper websites
        internal frmAbout frmAbout; // information about LAWC
        private readonly frmSplash splashScreen; // Startup information - to show progress while LAWC loads

        // the application's settings object
        internal Setting settings;

        internal Thread threadScanFolder;
        internal delegate void threadScanFolderChangeDelegate(String vFolder, int vCount);
        internal delegate void threadScanFolderCompleteDelegate();
        internal event threadScanFolderChangeDelegate OnWorkerThreadChange;
        internal event threadScanFolderCompleteDelegate OnWorkerThreadComplete;

        internal Thread threadChangeWallpaperFolder;
        internal delegate void threadChangeWallpaperCompleteDelegate();
        internal event threadChangeWallpaperCompleteDelegate OnWallpaperChangeThreadComplete;

        /// <summary>
        /// if set to true, the user has opted to cancel the current scan/rescan
        /// </summary>
        internal Boolean cancelProcess;

        internal Thread threadScanAllFolders;
        internal delegate void threadScanAllFoldersChangeDelegate(int vCount);
        internal delegate void threadScanAllFoldersCompleteDelegate();

        /// <summary>
        /// Is the app starting up? Used to stop events firing when form fields are populated, etc
        /// </summary>
        internal Boolean applyingSettings;

        // the settings wizard
        private WizardHost wizardHost;

        /// <summary>
        /// Records what the current state the application is in. Is the screen getting lighter, or darker, or daylight, or night time
        /// </summary>
        internal ScreenState screenStateCurrent;

        /// <summary>
        /// Should the current image be flipped horizontally. Randomly set on each wallpaper change
        /// </summary>
        internal Boolean FlipX = false;

        /// <summary>
        /// The list of the current wallpaper(s) filenames
        /// </summary>
        private List<string> wallpaperFilenames;

        /// <summary>
        /// The wallpaper folder path chosen during the startup wizard, which will be scanned when the wizard is finished
        /// </summary>
        internal string wizardInitialAddFolder;

        /// <summary>
        /// The current file number to append to the generated wallpaper file. 
        /// Used to stop the "file in use" error when changing wallpapers quickly
        /// </summary>
        internal int wallpaperFileNum;

        /// <summary>
        /// the class used to check for suitable key presses to act on (ie. Changing the current wallpaper)
        /// </summary>
        internal readonly KeyboardHook keyboardHook = new KeyboardHook();

        /// <summary>
        /// The list of folder(s) that will be scanned for wallpaper images
        /// </summary>
        internal readonly List<FolderInfo> foldersToScan = new List<FolderInfo>();

        /// <summary>
        /// The list of timers for each event
        /// </summary>
        internal List<System.Windows.Forms.Timer> EventTimers = new List<System.Windows.Forms.Timer>();

        /// <summary>
        /// The string representing all of the messages generated by the Events
        /// </summary>
        internal string EventMessages = string.Empty;

        /// <summary>
        /// Tells the app that the form is closing and not to save empty settings
        /// </summary>
        internal Boolean formClosing = false; // 

        /// <summary>
        /// The Computer object used for getting information on the Hardware sensors
        /// </summary>
        internal Computer thisComputer;

        /// <summary>
        /// Used on the Notify Icon menu as the base text for each screens menu links for opening and deleting images. Eg. "Open Wallpaper on Screen 0" would 
        /// </summary>
        private readonly String cWallpaperOnScreen = "Wallpaper on Screen";

        /// <summary>
        /// The smallest value in the image list for the ViewCount
        /// </summary>
        private int lowestViewCount = 0;
        /// <summary>
        /// The index of the smallest ViewCount in the image list
        /// </summary>
        private int lowestViewCountIndex = 0;

        // Sunrise / Sunset Stuff 
        internal DateTime date = DateTime.Today;
        internal bool isSunrise = false;
        internal bool isSunset = false;
        internal DateTime sunrise = DateTime.Now;
        internal DateTime sunset = DateTime.Now;

        /// <summary>
        /// A list of all of the sensors found
        /// </summary>
        internal List<SensorSummary> HWSensors = new List<SensorSummary>();

        //====== Interface colour values
        internal Color colourDarkest;
        internal Color colourDarker;
        internal Color colourDark;
        internal Color colourMedium;
        internal Color colourLighter;
        internal Color colourLightest;
        internal Color colourAlert;

        // ====== DEBUG:
        internal List<string> debugText;
        internal Boolean debugEnabled = false;
        internal Boolean debugWallpaperEnabled = false;

        /// <summary>
        /// Is there an internet connection available to the application?
        /// </summary>
        internal Boolean internetAvailable = false;
        /// <summary>
        /// count of how often the No Internet message is shown - stops it popping more than once
        /// </summary>
        internal int internetAvailableCount = 0;

        // click / double click stuff for the NotifyIcon
        //https://docs.microsoft.com/en-us/dotnet/framework/winforms/how-to-distinguish-between-clicks-and-double-clicks
        private Rectangle hitTestRectangle = new Rectangle();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private readonly System.Windows.Forms.Timer doubleClickTimer = new System.Windows.Forms.Timer();
        private Rectangle doubleClickRectangle = new Rectangle();

        internal Boolean isScanningFolder = false;

        DateTime RenameFirstClickStart = MainFunctions.DateNull;
        readonly int renameDelayMS = 1000;
        int renameRowNum = -1;

        //int adjustcount = 0;
        DateTime lastAdjusted = new DateTime(1980, 1, 1);

        #endregion


        #region Initialisation and Startup

        public FrmMain(string[] args)
        {
            InitializeComponent();

            // keep settings when reinstalled / updated / uninstall
            //https://stackoverflow.com/questions/3779307/how-to-keep-user-settings-on-uninstall
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            try
            {

                //**** Command Line Argument handling ***************
                if (args.Length > 0)
                {
                    //string[] allArgs = args.ToString(CultureInfo.InvariantCulture).ToLower().Split('-');
                    foreach (String arg in args)
                    {
                        if (arg.ToUpperInvariant().Contains("DEBUGON"))
                        {
                            // defaults to off, so dont need to check for "debug off"
                            this.debugEnabled = true;
                        }
                        else
                        {
                            this.debugEnabled = false;
                        }

                        if (arg.ToUpperInvariant().Contains("PORTABLEON"))
                        {
                            Properties.Settings.Default.Portable = true;
                            Properties.Settings.Default.Save();
                        }
                        if (arg.ToUpperInvariant().Contains("PORTABLEOFF"))
                        {
                            Properties.Settings.Default.Portable = false;
                            Properties.Settings.Default.Save();
                        }

                    }
                }
                //***************************************************

                debugText = new List<string>();

                if (debugEnabled) WriteText("DEBUG: LAWC Start", string.Empty);// settings.WriteToLog);                


                if (debugEnabled) WriteText("DEBUG: Creating Splash Screen started", string.Empty);
                splashScreen = new frmSplash(this);
                splashScreen.lblVersion.Text = "Version " + this.GetType().Assembly.GetName().Version.ToString();
                splashScreen.BackColor = Color.FromArgb(255, 153, 204, 255); // "{Name=ff659abb, ARGB=(255, 101, 154, 187)}"
                splashScreen.Refresh();
                if (debugEnabled) WriteText("DEBUG: Splash screen created", string.Empty); // settings.WriteToLog);

                if (debugEnabled) WriteText("DEBUG: Showing splash screen", string.Empty);
                if (Properties.Settings.Default.ShowSplash)
                {
                    try
                    {
                        splashScreen.Show();
                    }
                    catch (Exception ex)
                    {
                        if (debugEnabled) WriteText("DEBUG: Error Showing Splash Screen: \n" + ex.Message + "\n" + ex.StackTrace, string.Empty);
                        throw;
                    }

                }
                if (debugEnabled) WriteText("DEBUG: Splash Screen Shown", string.Empty);

            }
            catch (Exception ex)
            {
                ProcessError(ex, ErrorMessageType.Initialisation, true, false, string.Format(""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

        }

        private void MainFormLoad(String vSettingsPath = "")
        {

            if (debugEnabled) WriteText("DEBUG: MainFormLoad() started", string.Empty);

            try
            {
                applyingSettings = true;
                Startup(applyingSettings, vSettingsPath);
                applyingSettings = false;

            }
            catch (FileLoadException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.StartUp, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            Application.DoEvents();
            HideChangeWallpaperWorking(); // 
        }


        private void Startup(Boolean vFirstRun, String vSettingsPath = "")
        {
            // do this here incase this function is called directly/outside of the startup processes
            applyingSettings = vFirstRun;

            SetIconWorking();
            
            if (debugEnabled) WriteText("DEBUG: LAWC Startup() started", string.Empty); //settings.WriteToLog);

            refreshSplashScreen("Creating forms");
            notifyIcon1.Text = "Creating forms";
            createForms();
            if (debugEnabled) WriteText("DEBUG: CreateForms() finished", string.Empty);

            if (debugEnabled) WriteText("DEBUG: Starting LoadSettings()", string.Empty);
            refreshSplashScreen("Loading Settings");
            notifyIcon1.Text = "LAWC - Loading";
            LoadSettings(vSettingsPath);
            if (debugEnabled) WriteText("DEBUG: settings loaded", string.Empty);
            if (splashScreen.cancel == true) Quit(false);
            if (debugEnabled) WriteText("DEBUG: Finished LoadSettings()", string.Empty);

            refreshSplashScreen("Initializing");
            notifyIcon1.Text = "LAWC - Initializing";
            Initialise(vFirstRun); // 
            if (debugEnabled) WriteText("DEBUG: initialised", string.Empty);
            if (splashScreen.cancel == true) Quit(false);

            refreshSplashScreen("Getting available Images");
            notifyIcon1.Text = "LAWC - Getting available Images";

            refreshSplashScreen("Startup Done");

            splashScreen.Hide();
            if (splashScreen.cancel == true) Quit(false);

            notifyIcon1.Text = "LAWC - Wallpaper Changer";

            refreshSplashScreen("Checking Startup Wizard"); // 

            setSettingsFilePath(string.Empty);

            InformUserAboutWallpaperWebsiteManager();

            if (settings.FirstRunDone == false)
            {
                Properties.Settings.Default.Reset();

                ShowChangeWallpaperWorking(true);

                System.Windows.Forms.Timer timerStartWizard = new System.Windows.Forms.Timer
                {
                    Interval = 2000 // 2 seconds - just to make sure the form is visible before starting the wizard
                };
                timerStartWizard.Tick += TimerStartWizard_Tick;
                timerStartWizard.Start();
            }

            if (debugEnabled) WriteText("DEBUG: startup() finished", string.Empty);

            SetIconReady();

            applyingSettings = false;

        }


        protected override void WndProc(ref Message message)
        {
            //https://docs.microsoft.com/en-us/windows/desktop/menurc/wm-syscommand
            if (message.Msg == 0x0112) // WM_SYSCOMMAND
            {

                if (message.Msg == SingleInstanceWinApi.WMSHOWFIRSTINSTANCE) // || SingleInstanceWinApi.IsAlreadyRunning())
                {
                    SetFormNormal(); // (true);
                }
            }

            // Check your window state here
            if (message.WParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
            {
                // THe window is being maximized
            }
            else if (message.WParam == new IntPtr(0xF020)) // Minimize event - SC_MINIMIZE from Winuser.h
            {
                // The window is being minimized
                SetFormMinimized();
            }
            else if (message.WParam == new IntPtr(0xF120)) // Normal / Restore event - SC_RESTORE from Winuser.h
            {
                // THe window is being restored
                SetFormNormal(); //(false);
            }

            base.WndProc(ref message);
        }

        private void createForms()
        {
            this.frmAbout = new frmAbout(this);
            this.frmWebsites = new frmWebsites(this);
            this.frmSettingsAdvanced = new frmSettingsAdvanced(this);
            this.frmSettings = new frmSettings(this);
            this.frmMetaData = new frmImageMetadata();
            this.frmThanks = new frmThanks();
        }

        private void LoadSettings(String vSettingsPath = "")
        {
            settings = new Setting(this);

            try
            {
                // LOAD SETTINGS to forms and values
                DataSet dsMain;
                dsMain = Setting.ReadSettings(settings.FirstRunDone, vSettingsPath);
                if (dsMain != null)
                {
                    // add the list items to the settings
                    Setting.LoadDSTablesToArray(dsMain, this.settings);
                    // load and apply the main settings in the config
                    Setting.LoadDSToSettings(dsMain, ref this.settings, out Boolean vNewInstall, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    dsMain.Dispose();
                }
                else
                {
                    if (this.splashScreen.Visible) this.splashScreen.Hide();
                    //Settings file cannot be loaded 
                    int backupCount = FrmMain.BackupFileCount();
                    DialogResult dr;
                    if (backupCount > 0)
                    {
                        dr = MessageBox.Show("There was a problem loading the Settings for LAWC.\n\nDo you want to load the last backup?\n(You have " + backupCount + " backups)", "Settings Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Yes)
                        {
                            doRestoreBackup(string.Empty);
                            MessageBox.Show("Restore Complete.", "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (dr == DialogResult.No)
                        {
                            resetSettings();
                        }
                        else
                        {
                            Quit(false);
                        }
                    }
                    else
                    {
                        // NO backup settings
                        dr = MessageBox.Show("There was a problem loading the setting for LAWC.\n\nDo you want create a new Settings file?", "Settings Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Yes)
                        {
                            resetSettings();
                        }
                        else
                        {
                            Quit(false);
                        }
                    }
                    
                }

            }
            catch (EndOfStreamException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.LoadingSettings, true, false, string.Empty, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            try
            {
                applyingSettings = true;
                this.frmSettingsAdvanced.SetFormValues();

                this.frmSettings.SetFormValues();
                applyingSettings = false;

                SetKeyPress();
            }
            catch (InternalBufferOverflowException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.SettingFormValues, true, false, string.Empty, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            
        }

        
        /// <summary>
        /// Redraws / updates the splash screen information
        /// </summary>
        /// <param name="vText"></param>
        private void refreshSplashScreen(String vText)
        {
            splashScreen.lblCurrentStep.Text = vText;
            splashScreen.refresh();

            Application.DoEvents();

            if (splashScreen.cancel == true)
            {
                this.Hide();
                this.Quit(false);
            }

            splashScreen.Refresh();
        }

        /// <summary>
        /// Update / collect the screen information
        /// </summary>
        /// <param name="vFirstRun"></param>
        private void initDisplaySettings(Boolean vFirstRun)
        {
            applyingSettings = vFirstRun;

            refreshSplashScreen("Initializing - Updating Screen State");
            UpdateScreenState();

            refreshSplashScreen("Initializing - Setting Monitor Bounds"); //
            UpdateMonitorInfo();

            this.frmSettingsAdvanced.SetScreenImageCheckBoxes();

            applyingSettings = false;
        }

        /// <summary>
        /// capture and display what version of windows is running
        /// </summary>
        private void setOSInfo()
        {
            string OSName;
            try
            {
                OSName = MainFunctions.getOSName();
            }
            catch (System.Security.SecurityException ex)
            {
                OSName = "Windows 10"; //ASSUME: Windows 10
                ProcessError(ex, ErrorMessageType.UnknownDataRead, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            if (OSName.StartsWith("Windows 10"))
            {
                OSVersion = 10;
            }
            if (OSName.StartsWith("Windows 8"))
            {
                OSVersion = 8;
            }
            if (OSName.StartsWith("Windows 7"))
            {
                OSVersion = 7;
            }

            //lblOS.Text = OSName;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vFirstRun">if the screen count has changed we need to re-set the settings form, this saves the app from setting the settings screen twice on startup</param>
        private void Initialise(Boolean vFirstRun)
        {
            try
            {
                applyingSettings = vFirstRun;

                refreshSplashScreen("Initializing - Checking for an internet connection");
                checkInternetConnection(false); // false = because this is the startup 
                                                //  and init of sensors is done in the already, so we dont need to do it twice

                refreshSplashScreen("Initializing - Display Settings");
                initDisplaySettings(vFirstRun);
                applyingSettings = vFirstRun;

                refreshSplashScreen("Initializing - Applying Settings");
                ApplySettings(vFirstRun);
                applyingSettings = vFirstRun;

                refreshSplashScreen("Initializing - Setting Handlers");
                SetHandlers();

                refreshSplashScreen("Initializing - Getting chosen Sensors");
                initHardwareSensors(true, vFirstRun, true);

                refreshSplashScreen("Initializing - Setting View"); //
                SetView(vFirstRun, true);

                refreshSplashScreen("Initializing - Checking AutoBackup");
                if (settings.FirstRunDone)
                {
                    AutoBackup();
                    CheckLog();
                }
                refreshSplashScreen("Initializing - Set Splash Screen");
                SetPreviewImages(string.Empty);

                refreshSplashScreen("Initializing - Loading Screen Position");
                LoadScreenPosition(false); 

                refreshSplashScreen("Initializing - Checking wallpaper folders");
                CheckFolders(false);

                SetDarkModeState();

                refreshSplashScreen("Initializing - Setting the screen position"); 
                if (settings.StartMinimized == true && vFirstRun)
                {
                    SetFormMinimized();
                }
                else if (settings.StartMinimized == false && vFirstRun)
                {
                    SetFormNormal();
                }

                cancelProcess = false;

                refreshSplashScreen("Initializing - Getting the Lowest View Count");
                lowestViewCount = GetLowestViewCountVisibleOnly(out lowestViewCountIndex); 

                refreshSplashScreen("Initializing - Processing Events");
                ProcessAllEvents();

                //reset the multi mon mode if only 1 screen is present
                if (settings.MultiMonitorMode == MultiMonitorModes.DifferentOnAll && Screen.AllScreens.Length == 1)
                {
                    settings.MultiMonitorMode = MultiMonitorModes.SameOnAll;
                }

                applyingSettings = false;
            }
            catch (InvalidOperationException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.Initialisation, true, false, String.Empty, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

        }

        protected override void Dispose(bool disposing)
        {
            keyboardHook.Dispose();
            frmSettings.Dispose();
            frmSettingsAdvanced.Dispose();
            frmMetaData.Dispose();
            frmThanks.Dispose();
            frmWebsites.Dispose();
            frmAbout.Dispose();
            splashScreen.Dispose();
            if (wizardHost != null) wizardHost.Dispose();
            doubleClickTimer.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetHandlers()
        {
            // Error Handling / Exception Handling
            // from here: https://stackoverflow.com/questions/14973642/how-using-try-catch-for-exception-handling-is-best-practice

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Dispatcher.CurrentDispatcher.UnhandledException += CurrentDispatcher_UnhandledException;
            
            SystemEvents.DisplaySettingsChanged += new EventHandler(FrmMain_DisplaySettingsChanged);

            // HOTKEY STUFF:
            // register the event that is fired after the key press.
            this.keyboardHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(Hook_KeyPressed);

            this.FormClosing += new FormClosingEventHandler(FrmMain_FormClosing);
            this.Resize += new EventHandler(FrmMain_Resize);
            this.ResizeEnd += FrmMain_ResizeEnd;

            pbPreviewImage.MouseUp += new MouseEventHandler(PbPreviewImage_MouseUp);
            pbPreviewImage.MouseDoubleClick += new MouseEventHandler(PbPreviewImage_DoubleClick);

            this.olvImages.CheckBoxes = false;
            olvImages.MouseUp += new MouseEventHandler(olvImages_MouseUp);
            olvImages.ColumnClick += new ColumnClickEventHandler(olvImages_ColumnClick);
            olvImages.MouseDoubleClick += new MouseEventHandler(olvImages_MouseDoubleClick);
            olvImages.MouseHover += olvImages_MouseHover;
            olvImages.KeyDown += olvImages_KeyDown;
            //The ItemSelectionChanged event occurs when the item state changes from selected to deselected or deselected to selected. 
            olvImages.ItemSelectionChanged += olvImages_ItemSelectionChanged;
            olvImages.CellEditStarting += OlvImages_CellEditStarting;
            olvImages.CellEditFinishing += OlvImages_CellEditFinishing;
            olvImages.Scroll += OlvImages_Scroll;

            olvImages.Resize += OlvImages_Resize;

            this.olvImages.UseOverlays = false;
            this.olvFolders.UseOverlays = false;

            olvFolders.MouseUp += new MouseEventHandler(LvFolders_MouseUp);
            olvFolders.MouseDoubleClick += new MouseEventHandler(LvFolders_MouseDoubleClick);
            olvFolders.MouseHover += LvFolders_MouseHover;
            olvFolders.FormatRow += OlvFolders_FormatRow;

            olvFolders.DragEnter += DragDropFolderEnter;
            olvFolders.DragDrop += DragDropFolder;

            cmPreviewImage.ItemClicked += CmPreviewImage_ItemClicked;

            OnWorkerThreadChange += new threadScanFolderChangeDelegate(FrmMain_OnWorkerThreadChange);
            OnWorkerThreadComplete += new threadScanFolderCompleteDelegate(FrmMain_OnWorkerThreadComplete);
            OnWallpaperChangeThreadComplete += FrmMain_OnWallpaperChangeThreadComplete;

            //tmrRefresh.Tick += TmrRefresh_Tick;
            tmrRefresh.Interval = refreshDelaySeconds * 1000;
            tmrRefresh.Enabled = false;

            // sort out the notifyicon click and double click stuff:
            this.notifyIcon1.MouseDown += NotifyIcon1_MouseDown;
            hitTestRectangle.Location = new Point(30, 20);
            hitTestRectangle.Size = new Size(100, 40);
            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += DoubleClickTimer_Tick;
        }

        /// <summary>
        /// This handles all unhandled exceptions
        /// https://stackoverflow.com/questions/14973642/how-using-try-catch-for-exception-handling-is-best-practice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (!AppDomain.CurrentDomain.FriendlyName.EndsWith("vshost.exe")) { showCrashInfo(e); }
        }

        private void CurrentDispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!AppDomain.CurrentDomain.FriendlyName.EndsWith("vshost.exe")) { showCrashInfo(e); }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!AppDomain.CurrentDomain.FriendlyName.EndsWith("vshost.exe")) { showCrashInfo(e); }
        }

        private void FrmMain_OnWallpaperChangeThreadComplete()
        {
            HideWorkingMessage();
        }

        internal void showCrashInfo(ThreadExceptionEventArgs e)
        {
            String ex = string.Empty;
            ex += "=============================";
            ex += "Exception:";
            ex += e.Exception.Message + "\n\n";
            ex += "=============================";
            if (e.Exception.InnerException != null)
            {
                ex += "Inner Exception:";
                ex += e.Exception.InnerException.Message.ToString() + "\n\n";
            }
            showCrashInfo(e.Exception.ToString());
        }

        internal void showCrashInfo(System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            String ex = string.Empty;
            ex += "=============================";
            ex += "Exception:";
            ex += e.Exception.Message + "\n\n";
            ex += "=============================";
            if (e.Exception.InnerException != null) ex += "Inner Exception:";
            if (e.Exception.InnerException != null) ex += e.Exception.InnerException.Message.ToString() + "\n\n";

            showCrashInfo(e.Exception.ToString());
        }

        internal void showCrashInfo(UnhandledExceptionEventArgs e)
        {
            String ex = string.Empty;
            ex += "=============================";
            ex += "Exception:";
            ex += e.ToString() + "\n\n";
            ex += "=============================";
            ex += "Exception Object:";
            ex += e.ExceptionObject.ToString() + "\n\n";

            showCrashInfo(e.ExceptionObject.ToString());
        }

        internal void showCrashInfo(String vMessage)
        {
            ProcessError(null, ErrorMessageType.Crash, true, false, vMessage, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            
            Quit(false);
        }

        private void imageSelectionChanged(Boolean vScrollToEntry, Boolean vSelectEntry)
        {
            try
            {
                if (this.applyingSettings == false)
                {
                    if (this.olvImages.SelectedItem == null) //  .SelectedItems.Count == 0)
                    {
                        SetSampleImage();
                        return;
                    }
                    if (!System.IO.File.Exists(
                        ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath)
                        ) 
                    {
                        SetImageNotFound(); // SetSampleImage();
                        return;
                    }

                    applyingSettings = false;
                    SelectCurrentWallpaper(true); 
                    ScrollToSelectedImageItem(settings.ImageLastSelected, vScrollToEntry, vSelectEntry);

                    // force SelectCurrentWallpaper to not start calling itsself
                    System.Windows.Forms.Timer selectionTimer = new System.Windows.Forms.Timer
                    {
                        Interval = 130 // min time between user clicks on images
                    };
                    selectionTimer.Tick += selectionTimer_Tick;
                    selectionTimer.Start();

                }

            }
            catch (FileFormatException ex)
            {
                ProcessError(ex, ErrorMessageType.SelectingImage, true, false, string.Empty, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
        }

        internal void InitWallpaperFilenames()  //Boolean vForceUpdate)
        {
            // set the initial wallpaper file names
            if (wallpaperFilenames == null) 
                wallpaperFilenames = new List<string>(ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode));

            ImageInfo[] ImagesFound = GetNextImages(true, settings.WallpaperOrder);

            int screenCount = ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode);//Screen.AllScreens.Length;

            for (int y = 0; y < screenCount; y++)
            {
                if (ImagesFound[y] != null)
                {
                    ChangeImageFilename(ImagesFound[y].FullPath, y, true);
                }
            }
        }

        private void ApplySettings(Boolean vFirstRun)
        {
            applyingSettings = vFirstRun;

            SetImageCompressionRegistry(this.settings.CompressionQuality);

            SetNextWallpaperChangeTime();
            SetNextWallpaperAdjustTime();

            if (this.settings.CheckForUpdate && internetAvailable && settings.FirstRunDone)
                this.CheckForUpdate(false);

            RefreshDownloadUpdateIcon();

            // Set Wallpaper Changer Timer
            tmrWallpaper.Start();

            SetPortable(Properties.Settings.Default.Portable);

            // if the screen count has changed we need to re-set the settings form
            // if this is NOT the first run, then we can reapply the settings (saves it running twice on startup)
            if (vFirstRun == false)
            {
                this.frmSettingsAdvanced.SetFormValues();
                this.frmSettings.SetFormValues();
            }

            applyingSettings = false;
        }

        internal void SetPortable(Boolean vIsPortable)
        {
            Properties.Settings.Default.Portable = vIsPortable; 
            String newSettingsPath = string.Empty;

            // save the settings in the current location - make sure its up-to-date
            if (!applyingSettings) SaveSettings(newSettingsPath, this.settings);

            if (Properties.Settings.Default.Portable)
            {
                // PORTABLE
                newSettingsPath = MainFunctions.GetAppFullPath(Properties.Settings.Default.Portable);
                newSettingsPath += MainFunctions.SettingsFilenameOnly;
            }
            else
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.SettingsFilePath))
                {
                    // Custom
                    newSettingsPath = Properties.Settings.Default.SettingsFilePath;
                }
                else
                {
                    // Standard
                    newSettingsPath = Setting.getSettingsFullPathFixed(Properties.Settings.Default.Portable);
                    newSettingsPath += MainFunctions.SettingsFilenameOnly;
                }
            }

            Properties.Settings.Default.SettingsFilePath = newSettingsPath;

            // save new file
            if (!applyingSettings) SaveSettings(newSettingsPath, this.settings);
        }

        /// <summary>
        /// 1: None
        /// 2: Text Colour
        /// 3: Border
        /// 4: Translucent
        /// 5: Light Box
        /// </summary>
        /// <param name="olv"></param>
        /// <param name="vStyle"></param>
        internal static void ChangeHotItemStyle(ObjectListView olv, int vStyle)
        {
            olv.UseTranslucentHotItem = false;
            olv.UseHotItem = true;
            olv.UseExplorerTheme = false;

            switch (vStyle)
            {
                case 0:
                    olv.UseHotItem = false;
                    break;
                case 1:
                    HotItemStyle hotItemStyle = new HotItemStyle
                    {
                        ForeColor = Color.AliceBlue,
                        BackColor = Color.FromArgb(255, 64, 64, 64)
                    };
                    olv.HotItemStyle = hotItemStyle;
                    break;
                case 2:
                    RowBorderDecoration rbd = new RowBorderDecoration
                    {
                        BorderPen = new Pen(Color.SeaGreen, 2),
                        FillBrush = null,
                        CornerRounding = 4.0f
                    };
                    HotItemStyle hotItemStyle2 = new HotItemStyle
                    {
                        Decoration = rbd
                    };
                    olv.HotItemStyle = hotItemStyle2;
                    break;
                case 3:
                    olv.UseTranslucentHotItem = true;
                    break;
                case 4:
                    HotItemStyle hotItemStyle3 = new HotItemStyle
                    {
                        Decoration = new LightBoxDecoration()
                    };
                    olv.HotItemStyle = hotItemStyle3;
                    break;
                case 5:
                    olv.FullRowSelect = true;
                    olv.UseHotItem = false;
                    olv.UseExplorerTheme = true;
                    break;
            }
            olv.Invalidate();
        }

        #endregion


        #region Sensors 


        internal void initHardwareSensors(Boolean vResetTimers, Boolean vStartup, Boolean vCheckSensors)
        {
            try
            {
                //await initHardwareSensorsAsync(vResetTimers);
                initHardwareSensorsAsync(vResetTimers, vStartup, vCheckSensors);
            }
            catch (IOException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.InitialisingSensors, true, false, string.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
        }

        //async Task initHardwareSensorsAsync(Boolean vResetTimers)
        internal void initHardwareSensorsAsync(Boolean vResetTimers, Boolean vStartup, Boolean vCheckSensors)
        {

            String output = string.Empty;

            thisComputer = new Computer()
            {
                FanControllerEnabled = false,
                GPUEnabled = false,
                CPUEnabled = false,
                HDDEnabled = false,
                MainboardEnabled = false,
                RAMEnabled = false,
            };

            thisComputer.FanControllerEnabled = settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("FAN");
            thisComputer.GPUEnabled = (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("GPU"));
            thisComputer.CPUEnabled = (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("CPU"));
            thisComputer.HDDEnabled = (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("HDD"));
            thisComputer.MainboardEnabled = (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("MAINBOARD"));
            thisComputer.RAMEnabled = (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("RAM"));

            HWSensors.Clear();

            // dont bother if there are no sensors selected
            if (!string.IsNullOrEmpty(settings.HWSensorCategoriessUsed))
            {
                if (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("FAN")
                     || settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("GPU")
                      || settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("CPU")
                       || settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("HDD")
                        || settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("MAINBOARD")
                         || settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("RAM")
                    )
                {
                    thisComputer.Open();
                    foreach (IHardware hardware in thisComputer.Hardware)
                    {
                        hardware.Update();
                        foreach (IHardware subHardware in hardware.SubHardware)
                            subHardware.Update();

                        refreshSplashScreen("Finding Sensors - Hardware");
                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (sensor.Hardware.HardwareType != HardwareType.HDD)
                            {
                                Boolean hasNull = false;

                                SensorSummary s = new SensorSummary();

                                if (sensor.Name.ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                                    || sensor.Name.ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                                    || sensor.Name.ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                                    || sensor.Name.ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION")
                                    ) // was && ??????? if any wierd stuff, check this
                                {
                                    s.Name = sensor.Name;
                                    s.DataType = SensorType.Data;
                                    s.Value = sensor.Value;
                                    s.Category = SensorSource.Hardware;
                                    if (sensor.Value == null)
                                    {
                                        hasNull = true;
                                        s.Value = "[no value]";
                                    }
                                }
                                else
                                {
                                    s.Name = sensor.Name;
                                    s.DataType = sensor.SensorType;
                                    s.Value = sensor.Value;
                                    s.Category = SensorSource.Hardware;
                                    if (sensor.Value == null)
                                    {
                                        hasNull = true;
                                        s.Value = 0;
                                    }
                                }

                                HWSensors.Add(s);

                                output += String.Format(CultureInfo.InvariantCulture, "Hardware - {0} ({1}) - {2}", sensor.Name, sensor.SensorType, sensor.Value) + System.Environment.NewLine;
                                if (hasNull)
                                {
                                    output += "    [Run LAWC as Administrator to get this value]" + System.Environment.NewLine;
                                    hasNull = false;
                                }
                            }
                        }
                    }
                }
                output += System.Environment.NewLine;
            }


            // Add HDD Sensors to the list ///////////////////////////////
            if (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("HDD"))
            {
                refreshSplashScreen("Finding Sensors - HDD");

                DriveInfo[] allDrives = DriveInfo.GetDrives();
                double percent;// = 0;
                String name;

                foreach (DriveInfo d in allDrives)
                {
                    if (!d.IsReady)
                    {
                        //await PutTaskDelay(5);
                        Thread.Sleep(5000);
                        // if its STILL not ready...
                        if (!d.IsReady)
                        {
                            output += d.Name + " (" + d.VolumeLabel + ") is not ready." + System.Environment.NewLine;
                        }
                    }

                    name = d.Name.Replace(@":\", "");
                    SensorSummary usedSpace = new SensorSummary
                    {
                        Name = "HDD Used MB - " + name + " (" + d.VolumeLabel + ")",
                        Category = SensorSource.HDD,
                        DataType = SensorType.Temperature,
                        Value = Math.Round(MathExtra.ConvertBytesToMegabytes(d.TotalSize - d.TotalFreeSpace), 1)
                    };
                    HWSensors.Add(usedSpace);
                    output += String.Format(CultureInfo.InvariantCulture, "{0}:  {1}", usedSpace.Name, usedSpace.Value) + System.Environment.NewLine;

                    // Used Space - Percent
                    name = d.Name.Replace(@":\", "");
                    SensorSummary usedSpacePercent = new SensorSummary
                    {
                        Name = "HDD Used % - " + name + " (" + d.VolumeLabel + ")",
                        Category = SensorSource.HDD,
                        DataType = SensorType.Load
                    };
                    percent = ((double)usedSpace.Value / MathExtra.ConvertBytesToMegabytes(d.TotalSize)) * 100; //MathExtra.ConvertBytesToMegabytes(d.AvailableFreeSpace / (long)d.TotalSize);
                    usedSpacePercent.Value = Math.Round(percent, 1);
                    HWSensors.Add(usedSpacePercent);
                    output += String.Format(CultureInfo.InvariantCulture, "{0}:  {1}", usedSpacePercent.Name, usedSpacePercent.Value) + System.Environment.NewLine;

                    // Free Space - MB
                    name = d.Name.Replace(@":\", "");
                    SensorSummary freeSpace = new SensorSummary
                    {
                        Name = "HDD Free MB - " + name + " (" + d.VolumeLabel + ")",
                        Category = SensorSource.HDD,
                        DataType = SensorType.Temperature,
                        Value = Math.Round(MathExtra.ConvertBytesToMegabytes(d.AvailableFreeSpace), 1)
                    };
                    HWSensors.Add(freeSpace);
                    output += String.Format(CultureInfo.InvariantCulture, "{0}:  {1}", freeSpace.Name, freeSpace.Value) + System.Environment.NewLine;

                    // Free Space - Percent
                    name = d.Name.Replace(@":\", "");
                    SensorSummary freeSpacePercent = new SensorSummary
                    {
                        Name = "HDD Free % - " + name + " (" + d.VolumeLabel + ")",
                        Category = SensorSource.HDD,
                        DataType = SensorType.Load
                    };
                    percent = ((double)freeSpace.Value / MathExtra.ConvertBytesToMegabytes(d.TotalSize)) * 100; //MathExtra.ConvertBytesToMegabytes(d.AvailableFreeSpace / (long)d.TotalSize);
                    freeSpacePercent.Value = Math.Round(percent, 1);
                    HWSensors.Add(freeSpacePercent);
                    output += String.Format(CultureInfo.InvariantCulture, "{0}:  {1}", freeSpacePercent.Name, freeSpacePercent.Value) + System.Environment.NewLine;

                }
                output += System.Environment.NewLine;
            }


            // Add LOCATION Sensors to the list //////////////////////////////
            refreshSplashScreen("Finding Sensors - Location");
            if (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("LOCATION"))
            {
                // if no location is set 
                if (settings.Latitude == 0 && settings.Longitude == 0)
                {
                    // NO LOCATION
                    output += "Location: Sorry, you need to set a Location in Advanced Settings." + System.Environment.NewLine;
                    output += System.Environment.NewLine;
                }
                else if (!internetAvailable)
                {
                    output += "Location: Sorry, the internet connection is unavailable." + System.Environment.NewLine;
                    output += System.Environment.NewLine;
                }
                else
                {
                    SensorSummary sunrise = new SensorSummary();
                    DateTime val = settings.LightSunriseTime;
                    sunrise.Name = "Sunrise";
                    sunrise.DataType = SensorType.SmallData;
                    sunrise.Value = val;
                    sunrise.Category = SensorSource.Location;
                    HWSensors.Add(sunrise);

                    output += String.Format(CultureInfo.InvariantCulture, "Location {0}:  {1}", "Sunrise", val.ToString("HH:mm", CultureInfo.InvariantCulture)) + System.Environment.NewLine;

                    SensorSummary sunset = new SensorSummary();
                    val = settings.DarkSunsetTime;
                    sunset.Name = "Sunset";
                    sunset.DataType = SensorType.SmallData;
                    sunset.Value = val;
                    sunset.Category = SensorSource.Location;
                    HWSensors.Add(sunset);

                    output += String.Format(CultureInfo.InvariantCulture, "Location {0}:  {1}", "Sunset", val.ToString("HH:mm", CultureInfo.InvariantCulture)) + System.Environment.NewLine;
                    output += System.Environment.NewLine;
                }
            }

            // Add POWER Sensors to the list //////////////////////////////
            if (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("POWER"))
            {
                refreshSplashScreen("Finding Sensors - Power");

                SensorSummary powerPercent = new SensorSummary();
                float powerVal = float.Parse(GetPowerDetails(BatteryInfoCategory.BatteryLifePercent).ToString(), CultureInfo.InvariantCulture);

                powerPercent.Name = "BatteryLifePercent";
                powerPercent.DataType = SensorType.Load;
                powerPercent.Value = powerVal;
                powerPercent.Category = SensorSource.Power;
                HWSensors.Add(powerPercent);
                output += String.Format(CultureInfo.InvariantCulture, "Power {0}:  {1}%", "BatteryLifePercent", powerVal * 100) + System.Environment.NewLine;

                SensorSummary powerFullLifetime = new SensorSummary();
                powerVal = (float.Parse(GetPowerDetails(BatteryInfoCategory.BatteryFullLifetime).ToString(), CultureInfo.InvariantCulture)) * 100f;
                powerFullLifetime.Name = "BatteryFullLifetime";
                powerFullLifetime.DataType = SensorType.Load;
                powerFullLifetime.Value = powerVal;
                powerFullLifetime.Category = SensorSource.Power;
                HWSensors.Add(powerFullLifetime);
                output += String.Format(CultureInfo.InvariantCulture, "Power {0}:  {1}%", "BatteryFullLifetime", powerVal) + System.Environment.NewLine;

                SensorSummary powerLifeRemaining = new SensorSummary();
                powerVal = (float.Parse(GetPowerDetails(BatteryInfoCategory.BatteryLifeRemaining).ToString(), CultureInfo.InvariantCulture)) * 100f;
                powerLifeRemaining.Name = "BatteryLifeRemaining";
                powerLifeRemaining.DataType = SensorType.Load;
                powerLifeRemaining.Value = powerVal;
                powerLifeRemaining.Category = SensorSource.Power;
                HWSensors.Add(powerLifeRemaining);
                output += String.Format(CultureInfo.InvariantCulture, "Power {0}:  {1}%", "BatteryLifeRemaining", powerVal) + System.Environment.NewLine;

                output += System.Environment.NewLine;
            }

            // WEATHER ///////////////////
            refreshSplashScreen("Finding Sensors - Weather");
            if (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("WEATHER")
                )
            {
                if (internetAvailable)
                {
                    // if no location is set 
                    if (settings.Latitude == 0 && settings.Longitude == 0)
                    {
                        // NO LOCATION
                        //if (settings.HWSensorsUsed.ToUpper().Contains("WEATHER")) output += "Weather: Sorry, you need to set a Location in Advanced Settings." + System.Environment.NewLine;
                        //if (settings.HWSensorsUsed.ToUpper().Contains("LOCATION")) output += "Location: Sorry, you need to set a Location in Advanced Settings." + System.Environment.NewLine;
                    }
                    else
                    {
                        // a location is set
                        float value = getSensorValue("Temperature", SensorType.Temperature, string.Empty, out string textData); // txtValue.Text
                        if (String.IsNullOrEmpty(textData.Trim()) && value != 0)
                        {
                            output += String.Format(CultureInfo.InvariantCulture, "Weather Sensor Test: OK") + System.Environment.NewLine;
                        } else
                        {
                            output += String.Format(CultureInfo.InvariantCulture, "Weather Sensor Test: (PROBLEM) {0}", textData) + System.Environment.NewLine;
                            output += String.Format(CultureInfo.InvariantCulture, "Sensors available: NONE" + System.Environment.NewLine);
                        }

                        if (settings.HasOwnWeatherKey())
                        {
                            output += String.Format(CultureInfo.InvariantCulture, "Weather Sensor using your own OpenWeather Key", textData) + System.Environment.NewLine;
                        } else
                        {
                            output += String.Format(CultureInfo.InvariantCulture, "Weather Sensor using the Default OpenWeather Key", textData) + System.Environment.NewLine;
                        }

                        if (String.IsNullOrEmpty(textData.Trim()) && value != 0)
                        {
                            output += System.Environment.NewLine;
                            output += String.Format(CultureInfo.InvariantCulture, "Sensors available:" + System.Environment.NewLine);
                        }
                        var weatherSensorCount = Enum.GetNames(typeof(OpenWeatherAPI.OpenWeatherAPI.WeatherSensors)).Length;
                        
                        //if the key is invalid, dont list the sensors
                        if ((textData.Trim().Equals("[Server is Overloaded]") || textData.Trim().Equals("[Invalid Key]")) 
                            && value == 0) weatherSensorCount = -1;

                        for (int j = 0; j < weatherSensorCount; j++)
                        {
                            // Add in Weather
                            SensorSummary sWeather = new SensorSummary
                            {
                                Name = ((OpenWeatherAPI.OpenWeatherAPI.WeatherSensors)j).ToString()
                            };

                            switch (((OpenWeatherAPI.OpenWeatherAPI.WeatherSensors)j))
                            {
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Humidity:
                                    sWeather.DataType = SensorType.Load;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Pressure:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.CelsiusCurrent:
                                    sWeather.Name = "Temperature";
                                    sWeather.DataType = SensorType.Temperature;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Rain:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Snow:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Visibility:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.WindDegree:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.WindGust:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.WindSpeedMPS:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Clouds:
                                    sWeather.DataType = SensorType.SmallData;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Sunrise:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.Sunset:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;

                                case OpenWeatherAPI.OpenWeatherAPI.WeatherSensors.WindDirection:
                                    sWeather.DataType = SensorType.SmallData;
                                    sWeather.Category = SensorSource.Weather;
                                    break;

                                default:
                                    sWeather.DataType = SensorType.Clock;
                                    sWeather.Category = SensorSource.Weather;
                                    break;
                            }

                            // filter out the unnecessary ones
                            if (((OpenWeatherAPI.OpenWeatherAPI.WeatherSensors)j) != WeatherSensors.Sunrise
                                && ((OpenWeatherAPI.OpenWeatherAPI.WeatherSensors)j) != WeatherSensors.Sunset
                                )
                            {
                                HWSensors.Add(sWeather);
                                //output += String.Format(CultureInfo.InvariantCulture, "Weather {0}:  {1}", sWeather.Name, sWeather.Value) + System.Environment.NewLine;
                                output += String.Format(CultureInfo.InvariantCulture, "Weather {0}", sWeather.Name) + System.Environment.NewLine;
                            }
                        }

                        if (weatherSensorCount != -1)
                        {
                            // manually add in entry for Wind Speed KpH
                            SensorSummary ss = new SensorSummary();
                            double windSpeedKPH = 0;
                            windSpeedKPH = Math.Round((windSpeedKPH * 60 * 60) / 1000f, 2);
                            ss.Name = "WindSpeedKPH";
                            ss.DataType = SensorType.Clock;
                            ss.Value = windSpeedKPH;
                            ss.Category = SensorSource.Weather;
                            HWSensors.Add(ss);

                            output += String.Format(CultureInfo.InvariantCulture, "Weather {0}", "WindSpeedKPH") + System.Environment.NewLine;
                            output += System.Environment.NewLine;
                        }
                        
                    }
                }
                else
                {
                    // internet NOT available 
                    output += "Weather: Sorry, the internet connection is unavailable." + System.Environment.NewLine;
                    output += System.Environment.NewLine;
                }
            }
            if (settings.HWSensorCategoriessUsed.ToUpper(CultureInfo.InvariantCulture).Contains("INTERNET"))
            {
                // manually add in entry for Internet
                SensorSummary ssInternet = new SensorSummary
                {
                    Name = "InternetConnection",
                    DataType = SensorType.SmallData,
                    Value = MainFunctions.CheckForInternetConnection(),
                    Category = SensorSource.Internet
                };
                HWSensors.Add(ssInternet);

                output += String.Format(CultureInfo.InvariantCulture, "InternetConnection: {0}", this.internetAvailable) + System.Environment.NewLine;

                // manually add in entry for Internet Website
                SensorSummary ssInternetWebsite = new SensorSummary
                {
                    Name = "InternetWebsite",
                    DataType = SensorType.SmallData,
                    Value = MainFunctions.CheckForWebsite("8.8.8.8", pingTimeout), 
                    Category = SensorSource.Internet
                };
                HWSensors.Add(ssInternetWebsite);

                output += String.Format(CultureInfo.InvariantCulture, "InternetWebsite (Google): {0}", ssInternetWebsite.Value) + System.Environment.NewLine;
                output += System.Environment.NewLine;
            }

            output = HWSensors.Count().ToString(CultureInfo.InvariantCulture)
                + " Sensors Found:" + System.Environment.NewLine
                + output;

            this.frmSettingsAdvanced.txtHardwareSensorsFound.Text = output;

            if (vResetTimers == true)
            {
                // Create timers for each event
                CreateEventTimers();
            }

            // re-check sensors and enable ones that are now available
            if (vCheckSensors) checkEventSensors(true, vStartup);

        }


        

        internal object getCurrentSensorValue(String vName, SensorType vType, Object vData)
        {
            object value = string.Empty;
            String name = vName;

            try
            {
                if (name.Contains("Temperature"))
                {
                    name = "CelsiusCurrent";
                }

                if (name.Contains("WindSpeedKPH"))
                {
                    name = "WindSpeedMPS";
                }

                switch (getSensorSource(name))
                {
                    case SensorSource.Hardware:
                        value = getHWSensorValue(name, vType).ToString();
                        break;
                    case SensorSource.Weather:
                        if (CheckForInternetConnection() == true)
                        {
                            if (settings.Latitude != 0 && settings.Longitude != 0)
                            {
                                // we have a real location?
                                try
                                {
                                    // try and find the location from the gps coords
                                    WeatherSensors ws = getWeatherSensorName(name);
                                    if (settings.HasOwnWeatherKey())
                                    {
                                        value = OpenWeatherAPI.OpenWeatherAPI.GetWeatherValue(
                                            settings.Latitude, settings.Longitude,
                                            ws,
                                            settings.OpenWeatherAPIKey.Trim());
                                    }
                                    else
                                    {
                                        value = OpenWeatherAPI.OpenWeatherAPI.GetWeatherValue(
                                            settings.Latitude, settings.Longitude,
                                            ws,
                                            Constants.OpenWeatherAPIKey);
                                    }
                                }
                                catch (ArithmeticException)
                                {
                                    // location isnt valid
                                    value = string.Empty;
                                }
                                catch (System.Net.WebException ex)
                                {
                                    if (ex.Message.Contains("(401)") && ex.Message.Contains("Unauthorized")) //{"The remote server returned an error: (401) Unauthorized."}
                                    {
                                        value = "[Invalid Key]";//string.Empty;
                                    }
                                    else
                                    {
                                        // server overloaded probably
                                        value = "[Server is Overloaded]";//string.Empty;
                                    }
                                }
                                catch (NullReferenceException)
                                {
                                    value = string.Empty;
                                }
                                catch (Exception)
                                {
                                    value = string.Empty;
                                }

                            }
                            else
                            {
                                // location not set
                                value = string.Empty;
                            }
                        }
                        break;

                    case SensorSource.HDD:
                        value = getHWSensorValue(name, vType).ToString();
                        break;

                    case SensorSource.Power:
                        value = GetPowerDetails(BatteryInfoCategory.BatteryLifePercent);
                        break;

                    case SensorSource.Internet:
                        if (name.ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
                        {
                            value = MainFunctions.CheckForInternetConnection();
                        }
                        else if (name.ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
                        {
                            value = MainFunctions.CheckForWebsite(vData.ToString(), pingTimeout);
                        }
                        break;

                    case SensorSource.Location:
                        if (vType == SensorType.SmallData)
                        {
                            DateTime rise = DateTime.Now;
                            DateTime set = DateTime.Now;
                            getSunriseSunsetValue(out rise, out set);
                            if (vName.ToUpper(CultureInfo.InvariantCulture).Equals("SUNRISE", StringComparison.InvariantCulture))
                            {
                                value = rise;
                            }
                            else if (vName.ToUpper(CultureInfo.InvariantCulture).Equals("SUNSET", StringComparison.InvariantCulture))
                            {
                                value = set;
                            }

                        }
                        break;

                    default:
                        break;
                }

                // if we are using the WindSpeedKPH, we need to calculate the final value
                if (vName.Contains("WindSpeedKPH"))
                {
                    double kph = (((double)value) * 60 * 60) / 1000f;
                    value = Math.Round(kph, 2);
                }

            }
            catch (IOException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                value = string.Empty;

                ProcessError(ex, ErrorMessageType.GetSensorValue, true, false, string.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            return value;

        }

        internal float? getHWSensorValue(String vName, SensorType vType)
        {
            try
            {
                foreach (IHardware hardware in thisComputer.Hardware)
                {
                    hardware.Update();
                    foreach (IHardware subHardware in hardware.SubHardware)
                        subHardware.Update();

                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == vType
                            && sensor.Name == vName)
                        {
                            return sensor.Value;
                        }
                    }
                }

            }
            catch (IOException ex)
            {
                //throw new ApplicationException(string.Format("I cannot write the file {0} to {1}", fileName, directoryName), ex);
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.GetSensorValue, true, false, string.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                //throw new ApplicationException(string.Format("Problem getting HWSenorValue"), ex);
            }

            return 0;

        }

        internal float getSensorValue(String vSensorName, SensorType vSensorType, Object vSensorDataToCheck, out String vTextData)
        {
            float currentValue = 0;
            vTextData = string.Empty;

            try
            {
                object val = getCurrentSensorValue(vSensorName, vSensorType, vSensorDataToCheck.ToString());

                if ((vSensorType == SensorType.Data
                    || vSensorType == SensorType.SmallData)
                    &&
                    (vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                    || vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                    || vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                    || vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION")
                    )
                    )
                {
                    vTextData = val.ToString();
                    currentValue = -999999; 
                }
                else if (vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
                {
                    val = MainFunctions.CheckForWebsite(vSensorDataToCheck.ToString(), pingTimeout);
                    currentValue = (int)val;
                    vTextData = val.ToString(); // return text is the state of the website
                }
                else if (vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
                {
                    currentValue = (Boolean.Parse(val.ToString()) ? 1 : 0);
                }
                else if (vSensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("HDD"))
                {
                    // HDD
                    string driveLetter = string.Empty;
                    driveLetter = vSensorName.Substring(vSensorName.IndexOf(" - ", StringComparison.InvariantCulture) + 3, vSensorName.Length - vSensorName.IndexOf(")", StringComparison.InvariantCulture));
                    currentValue = GetHDDValue(driveLetter, vSensorName);
                    vTextData = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(val.ToString()))
                    {
                        //Boolean result = float.TryParse(val.ToString(), out currentValue);
                        //currentValue = float.Parse(val.ToString(), CultureInfo.InvariantCulture);
                        //vTextData = string.Empty;

                        if (val.ToString() == "[Server is Overloaded]" || val.ToString() == "[Invalid Key]")
                        {
                            vTextData = val.ToString();
                            currentValue = 0;
                        } else
                        {
                            Boolean result = float.TryParse(val.ToString(), out currentValue);
                            if (result)
                            {
                                currentValue = float.Parse(val.ToString(), CultureInfo.InvariantCulture);
                                vTextData = string.Empty;
                            }
                            else
                            {
                                currentValue = 0;
                                vTextData = val.ToString();
                            }
                        }

                        
                    }
                    else
                    {
                        currentValue = 0;
                        vTextData = string.Empty;
                    }
                }

            }
            catch (IOException ex)
            {
                if (this.splashScreen.Visible) this.splashScreen.Hide();
                ProcessError(ex, ErrorMessageType.SelectingImage, true, false, String.Format(CultureInfo.InvariantCulture, "Sensor Value:{0}   Type:{1}", vSensorName, vSensorType.ToString()), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            return currentValue;
        }


        #endregion


        #region LAWC Events
        
        internal void CreateEventTimers()
        {
            ClearEventTimers();

            foreach (EventInfo e in settings.Events)
            {
                if (e.Enabled)
                {
                    System.Windows.Forms.Timer t;
                    t = new System.Windows.Forms.Timer
                    {
                        Interval = e.CheckSeconds * 1000,
                        Tag = (EventInfo)e
                    };
                    t.Tick += DoEventTimerTick;
                    t.Start();

                    EventTimers.Add(t);
                }
            }

        }


        private void ClearEventTimers()
        {
            foreach (System.Windows.Forms.Timer tmr in EventTimers)
            {
                tmr.Stop();
                tmr.Dispose();
            }

            EventTimers = new List<System.Windows.Forms.Timer>();
        }


        internal void addEventTimer(EventInfo vEvent)
        {
            System.Windows.Forms.Timer t;
            t = new System.Windows.Forms.Timer
            {
                Interval = vEvent.CheckSeconds * 1000,
                Tag = (EventInfo)vEvent
            };
            t.Tick += DoEventTimerTick;
            t.Start();

            EventTimers.Add(t);
        }


        /// <summary>
        /// fire off each event to clear / update messages and images displayed
        /// </summary>
        internal void ProcessAllEvents()
        {
            // clear the messages as we may have just disabled the last event that was active (no active events = no timer_tick events firing)
            EventMessages = string.Empty;

            foreach (System.Windows.Forms.Timer tmr in EventTimers)
            {
                // if the timer has to be run, or if it has previously run and is displayed. 
                // In that case the call to the _tick will clean up and hide the event
                if (tmr.Enabled || ((EventInfo)tmr.Tag).Displayed == true)
                {
                    DoEventTimerTick(tmr, new EventArgs());
                }
            }
        }


        internal void editEventTimer(EventInfo vEvent)
        {
            for (int i = 0; i < EventTimers.Count; i++)
            {
                if (EventTimers[i].Tag == vEvent)
                {
                    EventTimers[i].Enabled = vEvent.Enabled;
                    EventTimers[i].Interval = vEvent.CheckSeconds * 1000;
                    EventTimers[i].Tag = vEvent;
                    break;
                }
            }
        }

        private Boolean isSensorInList(String vSensorName)
        {
            if (String.Equals(vSensorName.ToUpperInvariant(), "NONE", StringComparison.InvariantCulture))
                return true;

            for (int i = 0; i < HWSensors.Count; i++)
            {
                if (HWSensors[i].Name == vSensorName) // list any of the exceptions )
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Will check the sensors in the listed Events, and if any sensors are not found in the current set, then display a message
        /// </summary>
        /// returns a boolean - true = all is okay, false = event(s) disabled
        internal Boolean checkEventSensors(Boolean vCheckifEnabled, Boolean vStartup)
        {
            Boolean output = true;
            String result = string.Empty;
            String categoryList = string.Empty;

            for (int i = 0; i < settings.Events.Count; i++)
            {
                if (checkEventSensorAvailable(settings.Events[i], vCheckifEnabled, ref categoryList, ref result) == false)
                {
                    // disable that event until the sensor is fixed
                    settings.Events[i].Enabled = false;
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                output = false;
                result = "There are Sensors missing, and these Events are Disabled: \n\n" + result;
                result += "\n\n";
                result += "Go in to Advanced Settings -> Events tab, and click on the sensors you want to use, then press the Set button.\n";
                result += "\n\n";
                result += "An active Sensor Category may still be reported as unavailable for an Event, if the device has been removed or disabled.";
                result += "\n\n";
                result += "These Sensor Categories need to be enabled for all current sensors to work: \n\n" + categoryList;

                // if it is the startup and we want to check sensors, or we are not starting up and want to check
                if ((vStartup && settings.CheckSensorsOnStartup) || (!vStartup))
                    MessageBox.Show(result, "Sensors are Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return output;
        }


        internal Boolean checkEventSensorAvailable(EventInfo vEvent, Boolean vCheckifEnabled, ref String vCategoryList, ref String vResult)
        {
            Boolean output = true;
            String sensor;// = string.Empty;
            String joiner = string.Empty;

            sensor = vEvent.SensorName;

            //if (sensor.Contains("Temperature"))
            //{
            //    sensor = "CelsiusCurrent";
            //}
            if (sensor.Contains("WindSpeedKPH"))
            {
                sensor = "WindSpeedMPS";
            }

            if ((isSensorInList(sensor) == false))
            {
                //if the event is enabled, and we want to check if its enabled, OR its disabled
                if ((vEvent.Enabled == false && vCheckifEnabled == true)
                    || vEvent.Enabled == true)
                {
                    string cat = getSensorSource(vEvent.SensorName).ToString();
                    vResult += " * " + cat + " - " + vEvent.SensorName + "\n";

                    vCategoryList += joiner + cat;
                    joiner = ", ";

                    output = false;
                    // disable that event until the sensor is fixed
                }
            }
            else
            {
                // dont enable it automatically, as it may enable events that were manually disabled
            }

            return output;
        }


        /// <summary>
        /// Launches the Event Timer process via a thread
        /// </summary>
        internal void DoEventTimerTick(object sender, EventArgs e)
        {
            if (debugEnabled) WriteText("DEBUG: Start DoEventTimerTick", string.Empty);

            void work() { EventTimerFired(sender, e); }
            Thread thr = new Thread(work)
            {
                IsBackground = true,
                Priority = MainFunctions.threadPriority 
            };
            thr.Start();

            if (debugEnabled) WriteText("DEBUG: End Event Fired", string.Empty);
        }


        private void EventTimerFired(object sender, EventArgs e)
        {

            if (settings.Images.Count <= 0)
            {
                return;
            }

            try
            {
                checkInternetConnection(false);
                int wallpaperIndex = settings.Images.FindIndex(r => this.settings.ImageLastWallpaper.Contains(r.FullPath));  

                EventInfo eI = (EventInfo)((System.Windows.Forms.Timer)sender).Tag;

                // break if its not enabled, but allow it to keep going if we need to cleanup / finish displaying an event
                if (eI.Enabled == false && eI.Displayed == false) return;

                object currentValue = 0;
                String textData = string.Empty;

                try
                {
                    currentValue = getSensorValue(eI.SensorName, EventInfo.StringToSensorType(eI.TypeOfEvent), eI.CheckValueString, out textData);
                }
                catch (IOException ex)
                {
                    ProcessError(ex, ErrorMessageType.GetSensorValue, true, false, string.Format(CultureInfo.InvariantCulture, "{0} of {1}", eI.SensorName, eI.TypeOfEvent), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }

                Boolean actionResult = false;

                // return TEXT fields here
                if (eI.SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                    || eI.SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION"))
                {
                    // Strings
                    //if (!string.IsNullOrEmpty(eI.CheckValueString.Trim()))
                    {
                        try
                        {
                            actionResult = getActionResultString(eI.SensorName, EventInfo.StringToSensorType(eI.TypeOfEvent), eI.CheckAction, textData, eI.CheckValueString, eI);
                        }
                        catch (IOException ex)
                        {
                            ProcessError(ex, ErrorMessageType.ActionResult, false, false, string.Format(CultureInfo.InvariantCulture, "for {0} of {1} with {2}", eI.SensorName, eI.TypeOfEvent, eI.CheckAction), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        }

                    }
                }
                else
                {
                    // default - numeric
                    try
                    {
                        actionResult = getActionResult(eI.SensorName, EventInfo.StringToSensorType(eI.TypeOfEvent), eI.CheckAction, (float?)currentValue, (float)eI.CheckValueDecimal);
                    }
                    catch (IOException ex)
                    {
                        ProcessError(ex, ErrorMessageType.ActionResult, false, false, string.Format(CultureInfo.InvariantCulture, "for {0} of {1} with {2}", eI.SensorName, eI.TypeOfEvent, eI.CheckAction), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    }
                }

                // success, not displayed before, and enabled
                if (actionResult == true && eI.Displayed == false && eI.Enabled == true)
                {
                    // a new Event was fired
                    EventMessages = getEventMessages();

                    eI.LastRun = DateTime.Now;

                    // set the image from the Event
                    if (System.IO.File.Exists(eI.ImagePath) == true && eI.OverrideWallpaper == true)
                    {
                        SetAsWallpaper(eI.ImagePath);
                    }

                    eI.Displayed = true;

                    if (!string.IsNullOrEmpty(eI.Message.Trim()) && eI.ShowNotification == true)
                        showNotificationBalloon("LAWC Event", ProcessEventMessage(eI.Message, (float)currentValue, textData, eI.SensorName, wallpaperIndex)); //, eI));

                    AdjustDesktopImages(); // displays the text if any is present

                }
                if (actionResult == true && eI.Displayed == true && eI.Enabled == true)
                {
                    // currently displaying, update the message
                    // Need to check if we are trying to re-display the same images again (ie. so it doesnt keep doing it over and over)
                    eI.LastRun = DateTime.Now;

                    if ((settings.RecentImages.Contains(eI.ImagePath) == false && !string.IsNullOrEmpty(eI.ImagePath))
                        || EventMessages != getEventMessages()
                        )
                    {
                        // set the image from Event
                        if (System.IO.File.Exists(eI.ImagePath) == true && eI.OverrideWallpaper == true)
                        {
                            EventMessages = getEventMessages();
                            SetAsWallpaper(eI.ImagePath);
                        }
                    }
                    AdjustDesktopImages(); // Update / display the text
                } //update

                if (actionResult == false && eI.Displayed == true && eI.Enabled == true) 
                {
                    // the event has been fired, but now is finished firing
                    EventMessages = getEventMessages();
                    // only change it if it was showing the event image
                    if (!string.IsNullOrEmpty(eI.ImagePath)) ChangeWallpaperNow(true, false);
                    eI.Displayed = false;
                }

                // update again, incase none of the calls above were called
                EventMessages = getEventMessages();


            }
            catch (IOException ex)
            {
                ProcessError(ex, ErrorMessageType.FiringEvent, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
        }


        internal void RenderEventMessages(Graphics g)
        {
            String message = string.Empty;
            float fontSize = Screen.AllScreens[0].Bounds.Height / 90f;
            Color fontColour = Color.White;
            float gap = fontSize * 0.9f; //% font hight for the gap 

            float currentValue = 0;
            String textDataResult = string.Empty;
            int posY = 0;
            int lineNum = 0; // line pos, because some Events will not be fired, and we dont want a gap for those entries

            posY = 10; // start at 10px down

            int indexWallpaper = settings.Images.FindIndex(r => this.settings.ImageLastWallpaper.Contains(r.FullPath)); // (r => r.FullPath == settings.ImageLastWallpaper);

            for (int i = 0; i < settings.Events.Count; i++)
            {
                if (settings.Events[i].Enabled)
                {
                    currentValue = getSensorValue(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckValueString, out textDataResult);

                    if (settings.Events[i].SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                        || settings.Events[i].SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION")
                        )
                    {
                        // Strings
                        if (getActionResultString(settings.Events[i].SensorName,
                            EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent),
                            settings.Events[i].CheckAction, textDataResult,
                            settings.Events[i].CheckValueDecimal.ToString(CultureInfo.InvariantCulture),
                            settings.Events[i]) == true)
                        {
                            // Event firing
                            message = settings.Events[i].Message + "\n";
                            message = ProcessEventMessage(message, currentValue, textDataResult, settings.Events[i].SensorName, indexWallpaper); //, settings.Events[i]);
                            fontSize = settings.Events[i].FontSize;
                            // for old versions - do this check - change value to percent
                            int transparency;
                            if (settings.Events[i].Transparency > 100)
                            {
                                settings.Events[i].Transparency = (int)((255 - settings.Events[i].Transparency) / 255f * 100f); 
                            }
                            transparency = (int)((100 - settings.Events[i].Transparency) / 100f * 255f); 
                                                                                                         
                            fontColour = Color.FromArgb(transparency, settings.Events[i].FontColour.R, settings.Events[i].FontColour.G, settings.Events[i].FontColour.B);

                            RenderCaption(g,
                                new Rectangle(10, posY, 1000, (int)(fontSize + gap)),
                                fontSize,
                                message.ToString(CultureInfo.InvariantCulture),
                                fontColour, transparency);

                            lineNum++;
                            posY = (int)(lineNum * (fontSize + gap)) + 10;
                        }
                        else
                        {
                            // Event not firing
                        }
                    }
                    else
                    {
                        // Numeric:
                        if (getActionResult(settings.Events[i].SensorName,
                            EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent),
                            settings.Events[i].CheckAction, currentValue,
                            (float)settings.Events[i].CheckValueDecimal) == true)
                        {
                            // Event firing
                            message = settings.Events[i].Message + "\n";
                            message = ProcessEventMessage(message, currentValue, textDataResult, settings.Events[i].SensorName, indexWallpaper); //, settings.Events[i]);
                            fontSize = settings.Events[i].FontSize;

                            // for old versions - do this check - change value to percent
                            int transparency;
                            if (settings.Events[i].Transparency > 100)
                            {
                                settings.Events[i].Transparency = (int)((255 - settings.Events[i].Transparency) / 255f * 100f);
                            }
                            transparency = (int)((100 - settings.Events[i].Transparency) / 100f * 255f); 
                                                                                                         
                            fontColour = Color.FromArgb(transparency, settings.Events[i].FontColour.R, settings.Events[i].FontColour.G, settings.Events[i].FontColour.B);

                            RenderCaption(g,
                                new Rectangle(10, posY, 1000, (int)(fontSize + gap)),
                                fontSize,
                                message.ToString(CultureInfo.InvariantCulture),
                                fontColour, transparency);

                            lineNum++;
                            posY = (int)(lineNum * (fontSize + gap)) + 10;
                        }
                        else
                        {
                            // Event not firing
                        }
                    }
                }
            }

        }


        internal String ProcessEventMessage(String vMessage, float vCurrentValue, String vTextDataResult, String vSensor, int vIndexWallpaper)//, EventInfo vEvent)
        {
            string output = vMessage;
            // = -1;

            if (vIndexWallpaper >= settings.Images.Count)
                return output;

            if (float.TryParse(vCurrentValue.ToString(CultureInfo.InvariantCulture), out float value))
            {
                value = (float)Math.Round(value, 1);
            }

            if (vSensor.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                || vSensor.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                )
            {
                // Sunrise / sunset
                if (!string.IsNullOrEmpty(vTextDataResult))
                {
                    output = output.Replace("<<Value>>", DateTime.Parse(vTextDataResult.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToString("HH:mm", CultureInfo.InvariantCulture));  //("yyyy/MM/dd HH:mm"));
                }
                else
                {
                    output = output.Replace("<<Value>>", "12:00");  //("yyyy/MM/dd HH:mm"));
                }
            }
            else if (vSensor.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                || vSensor.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION"))
            {
                // other general text data
                output = output.Replace("<<Value>>", vTextDataResult.ToString(CultureInfo.InvariantCulture).Replace('_', ' '));
            }
            else if (vSensor.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
            {
                Boolean result = vCurrentValue == 1 ? true : false; 
                // Boolean data
                if (result)
                {
                    output = output.Replace("<<Value>>", "Yes");
                }
                else
                {
                    output = output.Replace("<<Value>>", "No");
                }
            }
            else if (vSensor.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
            {
                output = output.Replace("<<Value>>", vCurrentValue.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                if (vTextDataResult == "[Server is Overloaded]")
                {
                    // 
                    output = output.Replace("<<Value>>", vTextDataResult.ToString(CultureInfo.InvariantCulture));
                }
                else if (vTextDataResult == "[Invalid Key]")
                {
                    // 
                    output = output.Replace("<<Value>>", vTextDataResult.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    // everything else 
                    output = output.Replace("<<Value>>", value.ToString(CultureInfo.InvariantCulture));
                }

                
            }

            output = output.Replace("<<Time>>", DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture));
            output = output.Replace("<<Date>>", DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
            output = output.Replace("<<DateTime>>", DateTime.Now.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture));
            if (vIndexWallpaper >= 0 && vIndexWallpaper < settings.Images.Count) 
                output = output.Replace("<<Filename>>", settings.Images[vIndexWallpaper].Filename);

            if (vIndexWallpaper >= 0 && vIndexWallpaper < settings.Images.Count)
                output = output.Replace("<<Category>>", settings.Images[vIndexWallpaper].FullPath);

            if (vIndexWallpaper >= 0 && vIndexWallpaper < settings.Images.Count)
            {
                String[] path = settings.Images[vIndexWallpaper].FullPath.ToString().Split('\\');
                String pathDisplay = settings.Images[vIndexWallpaper].FullPath.ToString();
                if (path.Length >= 2)
                {
                    pathDisplay = path[path.Length - 3] + "\\" + path[path.Length - 2];
                }

                output = output.Replace("<<Category2>>", pathDisplay);
            }

            if (output.ToUpperInvariant().Contains("<<META_"))
            {
                string imageMetaTitle = ImageFunctions.GetImageMetaData(settings.Images[vIndexWallpaper].FullPath, "exif", "title");
                output = output.Replace("<<meta_title>>", imageMetaTitle);

                string imageMetaSubject = ImageFunctions.GetImageMetaData(settings.Images[vIndexWallpaper].FullPath, "exif", "subject");
                output = output.Replace("<<meta_subject>>", imageMetaSubject);

                string imageMetaComment = ImageFunctions.GetImageMetaData(settings.Images[vIndexWallpaper].FullPath, "exif", "comment");
                output = output.Replace("<<meta_comment>>", imageMetaComment);
            }

            return output;
        }


        internal String getEventMessages()
        {
            String output = string.Empty;

            float currentValue = 0;
            String textData = string.Empty;

            int wallpaperIndex = settings.Images.FindIndex(r => this.settings.ImageLastWallpaper.Contains(r.FullPath));

            for (int i = 0; i < settings.Events.Count; i++)
            {

                if (settings.Events[i].Enabled)
                {
                    if (settings.Events[i].SensorName.ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                        || settings.Events[i].SensorName.ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                        )

                    {
                        // make the dates the same, (we are only comparing the time on the current day) then compare the ticks
                        long checkValTicks = (long)settings.Events[i].CheckValueDecimal;
                        DateTime checkValDate = new DateTime(checkValTicks);
                        checkValDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, checkValDate.Hour, checkValDate.Minute, checkValDate.Second);

                        DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                        // Is a Date Value - use Ticks to compare
                        if (getActionResult(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckAction, now.Ticks, checkValDate.Ticks) == true)
                        {
                            // Event firing
                            // use the data for the value
                            currentValue = getSensorValue(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckValueString, out textData);
                            output += settings.Events[i].Message + "\n";
                            output = ProcessEventMessage(output, currentValue, textData, settings.Events[i].SensorName, wallpaperIndex); //, settings.Events[i]);
                        }
                        
                    }

                    // TEXT OPTIONS HERE
                    //CLOUDS
                    //WINDDIRECTION
                    else if (settings.Events[i].SensorName.ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                        || settings.Events[i].SensorName.ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION")
                        )

                    {
                        currentValue = getSensorValue(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), string.Empty, out textData);

                        if (getActionResultString(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckAction, null, settings.Events[i].CheckValueString, settings.Events[i]) == true)
                        //if (textData.Contains(settings.Events[i].CheckValueString))
                        {
                            // Event firing
                            output += settings.Events[i].Message + "\n";
                            output = ProcessEventMessage(output, currentValue, textData, settings.Events[i].SensorName, wallpaperIndex); //, settings.Events[i]);
                        }

                    }
                    else if (settings.Events[i].SensorName.ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
                    {
                        currentValue = getSensorValue(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent),
                            settings.Events[i].CheckValueString, out textData);

                        Boolean boolCurrentValue = false;
                        if (settings.Events[i].CheckValueDecimal == (int)currentValue)
                        {
                            boolCurrentValue = true;
                        }
                        if (getActionResultString(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckAction, boolCurrentValue.ToString(CultureInfo.InvariantCulture), textData, settings.Events[i]) == true)
                        {
                            // Event firing
                            currentValue = (boolCurrentValue ? 1 : 0);
                            output += settings.Events[i].Message + "\n";
                            output = ProcessEventMessage(output, currentValue, textData, settings.Events[i].SensorName, wallpaperIndex); 
                        }

                    }
                    else if (settings.Events[i].SensorName.ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
                    {
                        currentValue = getSensorValue(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent),
                            settings.Events[i].CheckValueString, out textData); // this gets the current state / ping of the website: 0 = no ping, 1 = returned

                        //now check that the state the user chose, matches the current website state

                        if (getActionResult(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckAction, currentValue, (float)settings.Events[i].CheckValueDecimal) == true)
                        {
                            // Event firing
                            //currentValue = (boolCurrentValue ? 1 : 0);
                            output += settings.Events[i].Message + "\n";
                            output = ProcessEventMessage(output, currentValue, textData, settings.Events[i].SensorName, wallpaperIndex); 
                        }

                    }
                    else
                    {
                        // NOT A Date or Text Field 
                        if (getActionResult(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckAction, null, (float)settings.Events[i].CheckValueDecimal) == true)
                        {
                            // Event firing
                            currentValue = getSensorValue(settings.Events[i].SensorName, EventInfo.StringToSensorType(settings.Events[i].TypeOfEvent), settings.Events[i].CheckValueDecimal.ToString(CultureInfo.InvariantCulture), out textData);
                            output += settings.Events[i].Message + "\n";
                            output = ProcessEventMessage(output, currentValue, textData, settings.Events[i].SensorName, wallpaperIndex); //, settings.Events[i]);

                        }
                        else
                        {
                            // Event not firing
                        }
                    }
                }
            }

            return output;
        }


        private Boolean getActionResultString(String vSensorName, SensorType vSensorType, EventInfo.CheckActionType vAction, String vCurrentValue, String vCheckValue, EventInfo vEvent)
        {
            float? currentValue;// = 0;

            String textData;// = string.Empty;
            String checkValue = vCheckValue;

            if (vCurrentValue != null)
            {
                textData = vCurrentValue;
            }
            else
            {
                currentValue = getSensorValue(vSensorName, vSensorType, checkValue, out textData);
            }

            // get rid of underscores before comparisons etc
            textData = textData.Replace('_', ' ');

            if (vSensorName.ToUpper(CultureInfo.InvariantCulture) == "INTERNETCONNECTION")
            {
                textData = MainFunctions.CheckForInternetConnection().ToString(CultureInfo.InvariantCulture);
            }
            else if (vSensorName.ToUpper(CultureInfo.InvariantCulture) == "INTERNETWEBSITE")
            {
                textData = vCurrentValue;
            }


            // this is NOT a date
            switch (vAction)
            {
                case EventInfo.CheckActionType.None:
                    if (settings.FirstRunDone) ProcessError(null, ErrorMessageType.EventActionNotSet, false, false, String.Format(CultureInfo.InvariantCulture, "Event: " + vEvent.Message), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    return false;

                case EventInfo.CheckActionType.GreaterThan:
                    return false; // not used with strings

                case EventInfo.CheckActionType.LessThan:
                    return false;

                case EventInfo.CheckActionType.EqualTo:
                    if (textData.ToString(CultureInfo.InvariantCulture).Equals(vCurrentValue, StringComparison.InvariantCulture)) { return true; } else { return false; }

                case EventInfo.CheckActionType.NotEqualTo:
                    if (textData != checkValue) { return true; } else { return false; }

                case EventInfo.CheckActionType.Contains:
                    if (textData.Contains(checkValue)) { return true; } else { return false; }

                case EventInfo.CheckActionType.DisplayAlways:
                    return true;

                default:
                    if (settings.FirstRunDone) ProcessError(null, ErrorMessageType.EventActionNotSet, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    return false;
            }
        }


        private Boolean getActionResult(String vSensorName, SensorType vSensorType, EventInfo.CheckActionType vAction, float? vCurrentValue, float? vCheckValue)
        {
            float? currentValue;

            if (vCurrentValue != null)
            {
                currentValue = vCurrentValue;
            }
            else
            {
                currentValue = getSensorValue(vSensorName, vSensorType, string.Empty, out _);
            }

            // this is NOT a date
            switch (vAction)
            {
                case EventInfo.CheckActionType.None:
                    if (settings.FirstRunDone) ProcessError(null, ErrorMessageType.EventActionNotSet, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    return false;

                case EventInfo.CheckActionType.GreaterThan:
                    if (currentValue > vCheckValue) { return true; } else { return false; }

                case EventInfo.CheckActionType.LessThan:
                    if (currentValue < vCheckValue) { return true; } else { return false; }

                case EventInfo.CheckActionType.EqualTo:
                    if (currentValue == vCheckValue) { return true; } else { return false; }

                case EventInfo.CheckActionType.NotEqualTo:
                    if (currentValue != vCheckValue) { return true; } else { return false; }

                case EventInfo.CheckActionType.Contains:
                    return false; // shouldnt ever get here - contains is for Strings - getActionResultString

                case EventInfo.CheckActionType.DisplayAlways:
                    return true;

                default:
                    if (settings.FirstRunDone) ProcessError(null, ErrorMessageType.EventActionNotSet, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    return false;
            }
        }



        #endregion


        #region Wallpaper Functions
                
        /// <summary>
        /// set the given wallpaperpath image as the current wallpaper
        /// </summary>
        /// <param name="vWallpaperPath"></param>
        private void SetAsWallpaper(String vWallpaperPath)
        {
            if (!string.IsNullOrEmpty(vWallpaperPath))
            {
                int screenCount = ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); // Screen.AllScreens.Length;

                for (int i = 0; i < screenCount; i++)
                {

                    if (settings.RecentImages.Count < ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode))
                    {
                        settings.RecentImages.Add(vWallpaperPath);
                    }
                    else
                    {
                        settings.RecentImages[i] = vWallpaperPath;
                    }

                    ChangeImageFilename(vWallpaperPath, i, true);

                }

                AdjustDesktopImages(); 

                // reset change time
                SetNextWallpaperChangeTime();
                SetNextWallpaperAdjustTime();

            }
        }

        internal void ChangeWallpaperNow(Boolean vGetNewImages, Boolean vInitializing)
        {
            try
            {
                Application.DoEvents();

                // dont try and change again, if we are already in the middle of a change of wallpaper
                if (btnWallpaperChange.Enabled == false && !applyingSettings) return;

                ShowChangeWallpaperWorking(false);

                // flip the image randomly.... this is where it is determined
                // 50% chance
                int r = (int)Randomizer.Next(0, 100);
                if (settings.RandomFlipImage && r >= 50)
                {
                    FlipX = true;
                }
                else
                {
                    FlipX = false;
                }

                IncrementWallpaperFileNumber();

                // Change wallpaper here
                DoNextWallpaperChange(vGetNewImages, vInitializing);
                Application.DoEvents();

                // start next cycle
                SetNextWallpaperChangeTime();
                SetNextWallpaperAdjustTime();

                if (!applyingSettings)
                    SaveSettings(string.Empty, this.settings);

                Application.DoEvents();

            }
            catch (InsufficientMemoryException ex)
            {
                ProcessError(ex, ErrorMessageType.ChangeWallpaper, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }



        }

        internal void IncrementWallpaperFileNumber()
        {
            // change the file number to avoid "file in use" error
            wallpaperFileNum++;
            if (wallpaperFileNum > wallpaperFileCount) { wallpaperFileNum = 0; }
        }

        internal int GetLowestViewCountVisibleOnly(out int vIndex)
        {
            int output = 999999;

            vIndex = 0;

            Boolean found = false;

            for (int i = 0; i < settings.Images.Count; i++)
            {
                if (settings.Images[i] != null)
                {
                    if (settings.Images[i].ViewCount <= output)
                    {
                        if (IsImageOK(settings.Images[i], false))
                        {
                            output = settings.Images[i].ViewCount;
                            found = true;
                            vIndex = i;

                            // if it finds Zero we can exit - cant get lower!
                            if (output == 0)
                                break;
                        }
                    }

                }
            }

            if (found == false)
            {
                if (settings.FirstRunDone && settings.Images.Count > 0) ProcessError(null, ErrorMessageType.LowestViewCountNotFound, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            return output;
        }

        private ImageInfo[] GetNextImages(Boolean vInitializing, ImageOrder vWallpaperOrder)
        {
            ImageInfo[] ImagesFound = new ImageInfo[ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode)];

            if (this.settings.Images.Count > 0)
            {
                lowestViewCount = GetLowestViewCountVisibleOnly(out lowestViewCountIndex); 

                if (vWallpaperOrder == Wallpaper.ImageOrder.Random)
                {
                    ImagesFound = GetRandomImages();
                } // if random
                else if (vWallpaperOrder == Wallpaper.ImageOrder.Ordered)
                {
                    ImagesFound = GetNextImagesFromSettings(vInitializing, -1, false);
                }
                else if (vWallpaperOrder == Wallpaper.ImageOrder.LowestViewCountOrdered)
                {
                    ImagesFound = GetNextImagesFromSettings(vInitializing, lowestViewCount, true);
                }
                else if (vWallpaperOrder == Wallpaper.ImageOrder.LowestViewCountRandom)
                {
                    ImagesFound = GetLowestViewCountRandomImages(vInitializing);
                }

                else
                {
                    ProcessError(null, ErrorMessageType.OrderNotFound, true, false, String.Format(CultureInfo.InvariantCulture, "{0}", vWallpaperOrder), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }
            else
            {
                // NO IMAGES IN LIST - may be okay if during first run, or starting up
                if (settings.FirstRunDone && olvImages.Items.Count > 0)
                    ProcessError(null, ErrorMessageType.NoImages, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            return ImagesFound;
        }


        private void DoNextWallpaperChange(Boolean vGetNewImages, Boolean vInitializing)
        {

            if (formClosing) return;

            if (CheckImagesAvailability(false) == false)
            {
                if (settings.FirstRunDone && olvImages.Items.Count > 0)
                    ProcessError(null, ErrorMessageType.NoImages, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                return;
            }

            if (vGetNewImages)
            {
                ImageInfo[] ImagesFound = GetNextImages(vInitializing, settings.WallpaperOrder);

                if (ImagesFound == null)
                {
                    if (settings.FirstRunDone) ProcessError(null, ErrorMessageType.NoImagesFound, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }

                // check the found images
                foreach (ImageInfo s in ImagesFound)
                {
                    if (s != null)
                    {
                        //if (s.FullPath != string.Empty)
                        {
                            if (string.IsNullOrEmpty(s.FullPath))
                            {
                                ProcessError(null, ErrorMessageType.NoImagesFound, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                                break;
                            }

                            if (System.IO.File.Exists(s.FullPath) == false)
                            {
                                ProcessError(null, ErrorMessageType.FileDoesntExist, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                            }
                        }
                    }
                    else
                    {
                        // The entry in ImagesFound is NULL - do nothing
                    }
                }

                int screenCount = ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); 

                // Change images, and update the history of recently used
                for (int y = 0; y < screenCount; y++)
                {
                    if (ImagesFound[y] != null)
                    {
                        // add to recent history
                        // if a first run / not matching, add items, else update the content
                        if (settings.RecentImages.Count < ImagesFound.Count())
                        {
                            settings.RecentImages.Add(ImagesFound[y].FullPath);
                        }
                        else
                        {
                            settings.RecentImages[y] = ImagesFound[y].FullPath;
                        }

                        ChangeImageFilename(ImagesFound[y].FullPath, y, true);
                        IncreaseViewCountList(ImagesFound[y].FullPath, 1);// update viewcount
                        addToHistory(ImagesFound[y].FullPath);
                    }
                    else
                    {
                        ProcessError(null, ErrorMessageType.NoImagesFound, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    }
                }

            } // end get new images

            // we have the filenames to load, now load them                
            AdjustDesktopImages();

            Application.DoEvents();
        }

        private ImageInfo[] GetRandomImages()
        {
            ImageInfo[] output = new ImageInfo[ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode)]; 

            for (int y = 0; y < ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); y++)
            {
                output[y] = GetRandomImage(out _);
            }

            return output;
        }


        private ImageInfo GetRandomImage(out int vIndex)
        {
            ImageInfo output;
            // check if we have images to work with
            if (settings.Images.Count() == 0)
            {
                if (settings.FirstRunDone) ProcessError(null, ErrorMessageType.NoImages, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

                output = new ImageInfo();
                vIndex = -1;
                return output;
            }

            int r = (int)Randomizer.Next(0, olvImages.GetItemCount());

            if (r < olvImages.GetItemCount())
            {
                output = settings.Images[r]; 
                vIndex = r;
            }
            else
            {
                output = new ImageInfo();
                vIndex = -1;
            }

            return output;
        }

        internal Boolean IsImageOK(ImageInfo vImage, Boolean vUseViewCount)
        {

            if (settings.UseFilters == false) return true;

            // if there are no items in the list yet (its an early call to the function probably)
            if (lowestViewCount == 999999)
            {
                lowestViewCount = 0;
            }

            if (string.IsNullOrEmpty(vImage.FullPath)) return false;

            // is this the lowest viewcount
            if (vUseViewCount == true)
            {
                if (vImage.ViewCount > lowestViewCount) return false;
            }

            // if the folder linked to the file does NOT exists in the list
            if (settings.GetFolderByID(vImage.FolderID) == null)
            {
                return false;
            }

            // Is the Folder Enabled:
            if (settings.GetFolderByID(vImage.FolderID).Enabled == false)
            {
                return false;
            }

            // FILTERS:
            //if ((vImage.ViewCount > lowestViewCount && vUseViewCount == true)) return false;
            if (vImage.Aspect < settings.AspectMin) return false;
            if (vImage.Aspect > settings.AspectMax) return false;
            if (vImage.Width < settings.MinImageWidth) return false;
            if (vImage.Height < settings.MinImageHeight) return false;
            if (vImage.AverageBrightness >= this.settings.BrightnessMax/100f) return false;
            if (vImage.AverageBrightness <= this.settings.BrightnessMin/100f) return false;
            if ((vImage.SizeBytes / 1024 < this.settings.SizeKBytesMin)) return false;

            return true;
        }

        internal int GetImageIndex(String vFullPath)
        {
            int index = -1;

            //this sucks, but it works
            String filename = Path.GetFileName(vFullPath);
            for (int i = 0; i < olvImages.Items.Count; i++)
            {
                if (olvImages.Items[i].Text.Contains(filename))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }



        internal void ScrollToSelectedImageItem(String vFullPath, Boolean vScrollToEntry, Boolean vSelectEntry) //int vListViewIndex, Boolean vScrollToEntry)
        {
            Boolean wasApplyingsettings = applyingSettings;
            applyingSettings = true;
            int index = -1;

            if (!string.IsNullOrEmpty(vFullPath))
            {
                if (olvImages.Items.Count > 0)
                {
                    //get the index of the selected item
                    index = GetImageIndex(vFullPath);
                }
            }

            if (index == -1)
            {
                applyingSettings = wasApplyingsettings;
                return;
            }

            // if the index is over the amount of available images (we have probably just disabled/enabled a folder)
            if (index >= olvImages.Items.Count)
            {
                index = 0;
            }

            if (index - 1 >= 0)
            {
                if (vScrollToEntry)
                {                    
                    // preferred way apparently: https://stackoverflow.com/questions/30585747/how-do-i-properly-select-a-row-in-objectlistview
                    olvImages.SelectObject(olvImages.GetModelObject(index));

                    // but still need to do this to make it the top/second top item
                    olvImages.TopItem = olvImages.Items[index - 1]; GetImageIndex(vFullPath);
                }
                if (vSelectEntry)
                {
                    olvImages.SelectObject(olvImages.GetModelObject(index));
                }

            }

            // set it back to how it was before
            applyingSettings = wasApplyingsettings;
        }


        private int GetolvImagesIndexByPath(string vPath)
        {
            int output = 0;

            for (int i = 0; i < olvImages.GetItemCount(); i++)
            {
                if (vPath == olvImages.Items[i].ToolTipText)
                {
                    output = i;
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Get the next image(s) IN ORDER
        /// </summary>
        /// <param name="vInitializing"></param>
        /// <returns></returns>
        private ImageInfo[] GetNextImagesFromSettings(Boolean vInitializing, int vListViewIndex, Boolean vUseViewCount)
        {
            int lastPos = settings.Images.FindIndex(r => r.FullPath == settings.ImageLastSelected);
            ImageInfo[] output = new ImageInfo[ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode)]; //Screen.AllScreens.Length];//settings.screens.Count()];

            if (vListViewIndex > -1) lastPos = vListViewIndex;

            if (lastPos >= settings.Images.Count)
            {
                // got to the top of the list
                lastPos = 0;
            }

            settings.ImageLastWallpaper = string.Empty;

            for (int i = 0; i < ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); i++) //Screen.AllScreens.Length; i++)
            {

                int count = 0;
                while (IsImageOK(settings.Images[lastPos], vUseViewCount) == false
                    && count < settings.Images.Count)
                {
                    lastPos++;

                    if (lastPos >= settings.Images.Count)
                    {
                        // got back to the top of the list
                        lastPos = 0;
                    }
                    count++;                    
                }

                if (lastPos >= settings.Images.Count)
                {
                    ProcessError(null, ErrorMessageType.LoopBreak, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }

                if (!vInitializing && lastPos < settings.Images.Count)
                {
                    settings.ImageLastWallpaper += settings.Images[lastPos].FullPath + ","; 
                }
                else
                {
                    settings.ImageLastWallpaper += settings.Images[lastPos].FullPath + ",";
                }

                output[i] = settings.Images[lastPos]; 

                //move to the next entry
                lastPos++;

                if (lastPos >= settings.Images.Count)
                {
                    // got back to the top of the list
                    lastPos = 0;
                }
            }
            return output;
        }


        /// <summary>
        /// Get the next image(s) RANDOMLY
        /// </summary>
        /// <param name="vInitializing"></param>
        /// <returns></returns>
        private ImageInfo[] GetLowestViewCountRandomImages(Boolean vInitializing)
        {
            ImageInfo[] output = new ImageInfo[ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode)];

            int lastPos = 0;
            Boolean switchToOrdered = false;

            for (int i = 0; i < ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); i++) 
            {
                int count = 0;
                if (settings.Images.Count > 0)
                {
                    lastPos = (int)Randomizer.Next(0, settings.Images.Count - 1);

                    while (IsImageOK(settings.Images[lastPos], true) == false 
                        && count < settings.Images.Count)      
                    {
                        lastPos = (int)Randomizer.Next(0, settings.Images.Count - 1);

                        count++;
                        
                    }
                }

                if (switchToOrdered)
                {
                    // dont want random function to keep hoping to find an lower view count, so switch to ordered when over 90% are not okay
                    output = GetNextImagesFromSettings(vInitializing, lowestViewCountIndex, true);
                }

                output[i] = settings.Images[lastPos];

            }

            return output;

        }


        private void ChangeImageFilename(String vImagePath, int vScreenIndex, Boolean vAddToLastWallpaper)
        {

            if (vScreenIndex >= wallpaperFilenames.Count)
            {
                wallpaperFilenames.Add(vImagePath);
                ////http://stackoverflow.com/questions/4840802/change-array-size
            }
            else
            {
                wallpaperFilenames[vScreenIndex] = vImagePath;
            }

            if (vAddToLastWallpaper) settings.ImageLastWallpaper = vImagePath; //TODO: make this work for multiple screens

        }

        private void IncreaseViewCountList(String vPath, int vAmount)
        {
            int index = settings.Images.FindIndex(r => r.FullPath == vPath);

            bool result = settings.ViewCountAdd(vPath, vAmount);
            if (olvImages.GetItem(index) != null) this.olvImages.RefreshItem(olvImages.GetItem(index));

            return;

        }

        internal static int GetBlurValue(int vVal)
        {
            // Convert the SETTING to the correct Blur value
            switch (vVal)
            {
                case 1: return 4;
                case 2: return 8;
                case 3: return 12;
                case 4: return 20;
                case 5: return 32;
                case 6: return 52;
                case 7: return 84;
                case 8: return 126;
                case 9: return 180;
                case 10: return 200;

                default:
                    return 32;
            }
        }

        internal int GetBlurSetting(int vVal)
        {
            // Convert the Blur VALUE to the Setting value
            switch (vVal)
            {
                case 4: return 1;
                case 8: return 2;
                case 12: return 3;
                case 20: return 4;
                case 32: return 5;
                case 52: return 6;
                case 84: return 7;
                case 126: return 8;
                case 180: return 9;
                case 200: return 10;

                default:
                    return 5;

            }
        }

        /// <summary>
        /// Delete all of the wallpaper files saved in the Pictures folder
        /// </summary>
        internal void DeleteOldWallpaperFiles()
        {
            if (debugEnabled) WriteText("DEBUG: DeleteWallpaperFiles() Started", string.Empty);

            string path;// = "";

            for (int i = 0; i <= wallpaperFileCount; i++)
            {
                path = Wallpaper.GetWallpaperPath(i, FrmMain.getWallpaperExtension(settings.WallpaperFormat));
                try
                {
                    //delete
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path); // delete permanently
                            //FileFunctions.DeleteFileToRecycleBin(path); // causes "file in use" issues
                            if (debugEnabled) WriteText("DEBUG: Deleted file: " + path + "", string.Empty);
                        }
                        catch (IOException)
                        {
                            // ignore
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // ignore
                        }
                    }

                }
                catch (IOException ex)
                {
                    if (debugEnabled) WriteText("DEBUG: FAILED on deleting " + path + "\n" + ex.Message + "", string.Empty);

                    ProcessError(ex, ErrorMessageType.DeletingFiles, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            if (debugEnabled) WriteText("DeleteWallpaperFiles() Finished", string.Empty);

        }


        private Boolean checkWallpaperFilenames()
        {
            Boolean output = false;

            try
            {
                // check if filenames are valid
                for (int i = 0; i < wallpaperFilenames.Count; i++)
                {
                    Application.DoEvents();

                    if (wallpaperFilenames[i] == null)
                    {
                        ProcessError(null, ErrorMessageType.FilenameEmpty, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        return output;
                    }
                    else
                    {
                        if (System.IO.File.Exists(wallpaperFilenames[i].ToString(CultureInfo.InvariantCulture)) == false)
                        {
                            ProcessError(null, ErrorMessageType.FileDoesntExist, false, false, String.Format(CultureInfo.InvariantCulture, "File Doesnt Exist: " + wallpaperFilenames[i].ToString(CultureInfo.InvariantCulture)), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                            return output;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                ProcessError(ex, ErrorMessageType.FileProblem, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            output = true;

            return output;
        }


        internal void AddImage(Graphics g, ref int vIndex, Rectangle vAllBounds, int vScreenCount, String vSettingsFullPath)
        {
            // sort the screen array
            Array.Sort(Screen.AllScreens, delegate (Screen x, Screen y) {
                return x.Bounds.X.CompareTo(y.Bounds.X) + x.Bounds.Y.CompareTo(y.Bounds.Y);
            });

            List<Rectangle> screensBoundsAdjusted = GetScreenBoundsAdjusted();
            Rectangle bounds = screensBoundsAdjusted[vIndex];
            Rectangle allBounds = vAllBounds;

            try
            {

                switch (settings.WallpaperMode)
                {
                    case Wallpaper.WallpaperModes.None:
                        break;
                    case Wallpaper.WallpaperModes.Centre:
                        AddImageToDesktopCentre(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), bounds, vIndex);
                        break;
                    case Wallpaper.WallpaperModes.FillWidth:
                        AddImageToDesktopFillWidthHeight2(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), bounds, vIndex, WallpaperModes.FillWidth);
                        break;
                    case Wallpaper.WallpaperModes.FillHeight:
                        AddImageToDesktopFillWidthHeight2(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), bounds, vIndex, WallpaperModes.FillHeight);
                        break;
                    case Wallpaper.WallpaperModes.Stretch:
                        AddImageToDesktopStretch(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), bounds, vIndex);
                        break;
                    case Wallpaper.WallpaperModes.Tile:
                        AddImageToDesktopTile(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), allBounds, vIndex, settings.ImageSizeScalePercent);
                        break;
                    case Wallpaper.WallpaperModes.Span:
                        float percentUsed = AddImageToDesktopSpan(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), allBounds, bounds, vIndex);
                        // if the image has taken more than half the span
                        if (percentUsed > 1f) //TODO: account for 3+ screens... need to change this (0.5f) if we have 3 screens?
                        {
                            vIndex = vScreenCount; //break out of loop without stopping the function
                        }
                        break;
                    case WallpaperModes.LAWC:
                        Boolean isSpan = false;
                        AddImageToDesktopLAWC(g, wallpaperFilenames[vIndex].ToString(CultureInfo.InvariantCulture), allBounds, bounds, vIndex, ref isSpan);
                        if (isSpan)
                        {
                            vIndex = vScreenCount; //break out of loop without stopping the function
                        }
                        break;
                    default:
                        ProcessError(null, ErrorMessageType.ImageScalingUnknown, false, false, String.Format(CultureInfo.InvariantCulture, "{0}", settings.WallpaperMode), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        break;
                }

                if (debugWallpaperEnabled)
                {
                    //draw rectangle - screen bounds
                    // looping this for ALL screens because some (Tile so far) only runs once
                    for (int i = 0; i < Screen.AllScreens.Count(); i++)
                    {
                        g.DrawRectangle(Pens.HotPink, new Rectangle(Screen.AllScreens[i].Bounds.X, Screen.AllScreens[i].Bounds.Y, Screen.AllScreens[i].Bounds.Width - 1, Screen.AllScreens[i].Bounds.Height));
                    }
                }

                // do this here so that the debugWallpaper boxes works properly
                if (settings.WallpaperMode == WallpaperModes.Tile)
                {
                    vIndex = vScreenCount; //break out of loop without stopping the function
                }

            }
            catch (InsufficientMemoryException ex)
            {
                ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
            }

        }


        /// <summary>
        /// Launches the adjustment via a thread
        /// </summary>
        int adjCount = 0;
        internal void AdjustDesktopImages()
        {
            if (formClosing) return;

            if (debugEnabled) WriteText("DEBUG: Start DoEventTimerTick", string.Empty);

            ShowChangeWallpaperWorking(false);
            adjCount++;
            try
            {
                // if lastadjusted was less than 1000ms ago, dont adjust again
                // this is now the MINIMUM update speed for the WHOLE APP
                if (lastAdjusted.AddMilliseconds(1000) > DateTime.Now)
                {
                    return;
                }

                lastAdjusted = DateTime.Now;
                //adjustcount++;
                
                void work() { doAdjustDesktopImages(); }
                threadChangeWallpaperFolder = new Thread(work)
                {
                    IsBackground = true,
                    Priority = MainFunctions.threadPriority //ThreadPriority.AboveNormal
                };
                threadChangeWallpaperFolder.Start();

            }
            catch (InsufficientMemoryException ex)
            {
                ProcessError(ex, ErrorMessageType.AdjustDesktopImageThread, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            if (debugEnabled) WriteText("DEBUG: End Event Fired", string.Empty);
        }

        /// <summary>
        /// Returns a rectangle of the size of all screens combined, and zero-ed so there are no negative values
        /// </summary>
        /// <returns></returns>
        private List<Rectangle> GetScreenBoundsAdjusted()
        {
            List<Rectangle> screensBounds = new List<Rectangle>();
            Point lowestPoint = new Point(0, 0);

            //get the list of screens, and the lowest screen positions x and y
            for (int i = 0; i < Screen.AllScreens.Count(); i++)
            {
                Rectangle bounds = Screen.AllScreens[i].Bounds;
                screensBounds.Add(bounds);

                Point check = new Point(bounds.X, bounds.Y);

                //get the zero point (the lowest in all screens)
                if (check.X < lowestPoint.X)
                {
                    lowestPoint.X = check.X;
                }

                if (check.Y < lowestPoint.Y)
                {
                    lowestPoint.Y = check.Y;
                }

            }

            //adjust positions
            for (int i = 0; i < screensBounds.Count; i++)
            {
                screensBounds[i] = new Rectangle(screensBounds[i].X + Math.Abs(lowestPoint.X), screensBounds[i].Y + Math.Abs(lowestPoint.Y),
                    screensBounds[i].Width, screensBounds[i].Height);
            }

            return screensBounds;

        }

        private void doAdjustDesktopImages()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int primaryScreenIndex;// = -1;

            DeleteOldWallpaperFiles();

            try
            {
                if (splashScreen.cancel) return;

                // only LAWC mode will use allBounds as a combo of all of the bounds. see below
                Rectangle allBounds = Screen.AllScreens[0].Bounds; //BoundsUtilities.GetAllMonitorsBounds();

                if (debugEnabled) WriteText("DEBUG: Start adjustDesktopImage", string.Empty);

                if (debugEnabled) WriteText("DEBUG: adjustDesktopImage() Started", string.Empty);

                if (wallpaperFilenames == null || wallpaperFilenames.Count == 0)
                {
                    return;
                }

                if (checkWallpaperFilenames() == false)
                {
                    return;
                }

                if (debugEnabled) WriteText("DEBUG: Checking Files DONE = OK", string.Empty);

                // assume just 1 screen for all modes except LAWC
                int screenCount = ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); //1;
                allBounds = Screen.AllScreens[0].Bounds;

                if (settings.WallpaperMode == WallpaperModes.LAWC)
                {
                    allBounds = BoundsUtilities.GetAllMonitorsBounds();
                }
                else if (settings.WallpaperMode == WallpaperModes.Span)
                {
                    allBounds = BoundsUtilities.GetAllMonitorsBounds();
                }
                else if (settings.WallpaperMode == WallpaperModes.Tile)
                {
                    allBounds = BoundsUtilities.GetAllMonitorsBounds();
                }

                if (settings.MultiMonitorMode == MultiMonitorModes.DifferentOnAll)
                {
                    allBounds = BoundsUtilities.GetAllMonitorsBounds();
                    screenCount = ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); //Screen.AllScreens.Length;
                }
                else
                {
                    // find the primary screen and use its bounds
                    for (int i = 0; i < Screen.AllScreens.Count(); i++)
                    {
                        if (Screen.AllScreens[i].Primary)
                        {
                            allBounds = Screen.AllScreens[i].Bounds;
                            screenCount = 1;
                            break;
                        }
                    }
                }

                string fn = string.Empty;
                Boolean result = false;

                Application.DoEvents();

                using (Bitmap desktopBitmapTransp = new Bitmap(allBounds.Width, allBounds.Height, Wallpaper.CurrentPixelFormat)) // PixelFormat.Format32bppArgb))
                {
                    int currentIndex = 0;

                    using (Graphics g = Graphics.FromImage(desktopBitmapTransp))
                    {
                        if (settings.BlurImageEdges == true) 
                            g.Clear(Color.Transparent);
                        else
                            g.Clear(GetCurrentColour());

                        try
                        {
                            
                            // Go through all screens and add an image for each
                            for (int indexScreen = 0; indexScreen < screenCount; indexScreen++)
                            {
                                // for error handling - store the index
                                currentIndex = indexScreen;

                                if (Screen.AllScreens[indexScreen].Primary == true) primaryScreenIndex = indexScreen; 

                                if (wallpaperFilenames.Count < Screen.AllScreens.Length) InitWallpaperFilenames();

                                if (indexScreen < wallpaperFilenames.Count)
                                {
                                    if (string.IsNullOrEmpty(wallpaperFilenames[indexScreen].ToString(CultureInfo.InvariantCulture)) || !System.IO.File.Exists(wallpaperFilenames[indexScreen].ToString(CultureInfo.InvariantCulture)))
                                    {
                                        ProcessError(null, ErrorMessageType.FileDoesntExist, false, false, String.Format(CultureInfo.InvariantCulture, wallpaperFilenames[indexScreen].ToString(CultureInfo.InvariantCulture)), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                                        return;
                                    }
                                }

                                if (debugEnabled) WriteText("DEBUG: Add Image to screen #" + indexScreen.ToString(CultureInfo.InvariantCulture), string.Empty);

                                AddImage(g, ref indexScreen, allBounds, screenCount, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

                                if (settings.MultiMonitorMode == MultiMonitorModes.SameOnAll)
                                    indexScreen = screenCount; // break out of loop

                                if (indexScreen == screenCount)
                                    break; // exit the loop

                            }  // END OF FOR, LOOP THROUGH SCREENS


                        }
                        catch (IOException ex)
                        {
                            string filename = string.Empty;
                            if (wallpaperFilenames != null)
                            {
                                if (currentIndex < wallpaperFilenames.Count) filename = wallpaperFilenames[currentIndex];
                            }
                            ProcessError(ex, ErrorMessageType.AddingImage, false, false, String.Format(CultureInfo.InvariantCulture, "Mode: {0} Filename: {1}", settings.WallpaperMode, filename), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        }

                        Application.DoEvents();
                        RenderEventMessages(g);

                    } // using graphics


                    if (debugEnabled) WriteText("DEBUG: Adding Images Done", string.Empty);

                    Application.DoEvents();

                    if (settings.BlurImageEdges == false)
                    {
                        if (debugEnabled) WriteText("DEBUG: Do NON Blurred", string.Empty);

                        try
                        {
                            // Save the wallpaper 
                            IncrementWallpaperFileNumber();
                            // Save image to a file:
                            fn = Wallpaper.SaveWallpaper(desktopBitmapTransp, wallpaperFileNum, settings.WallpaperFormat, settings.CompressionQuality, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

                            // DEBUG
                            if (debugEnabled) WriteText("DEBUG: Set Wallpaper filename: " + fn + " Started", string.Empty);

                            if (File.Exists(fn) == false)
                            {
                                ProcessError(null, ErrorMessageType.FileDoesntExist, false, false, String.Format(CultureInfo.InvariantCulture, "File: {0}", fn), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                            }

                        }
                        catch (IOException ex)
                        {
                            ProcessError(ex, ErrorMessageType.SavingWallpaper, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        }

                        try
                        {
                            // Set created wallpaper
                            if (!string.IsNullOrEmpty(fn))
                            {
                                if (settings.MultiMonitorMode == MultiMonitorModes.DifferentOnAll && settings.WallpaperMode != WallpaperModes.LAWC)
                                {
                                    //Multi mode - FORCE Tile
                                    result = SetWallpaper(fn, WallpaperModes.Tile);
                                }
                                else
                                {
                                    result = SetWallpaper(fn, settings.WallpaperMode);
                                }

                                Application.DoEvents();

                                // set registry entry for the new wallpaper
                                if (result == false)  
                                {
                                    ProcessError(null, ErrorMessageType.SettingWallpaper, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                                }
                            }
                            else
                            {
                                // the fn IS EMPTY
                                if (settings.MultiMonitorMode == MultiMonitorModes.DifferentOnAll && settings.WallpaperMode != WallpaperModes.LAWC)
                                {
                                    //Multi mode - FORCE Tile
                                    result = SetWallpaper(Wallpaper.GetWallpaperPath(wallpaperFileNum,
                                        FrmMain.getWallpaperExtension(settings.WallpaperFormat)),
                                        WallpaperModes.Tile 
                                        );
                                }
                                else
                                {
                                    result = SetWallpaper(Wallpaper.GetWallpaperPath(wallpaperFileNum,
                                        FrmMain.getWallpaperExtension(settings.WallpaperFormat)),
                                        settings.WallpaperMode
                                        ); 
                                }

                                Application.DoEvents();

                                if (result == false)
                                {
                                    ProcessError(null, ErrorMessageType.SettingWallpaper, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                                }
                            }
                            if (debugEnabled) WriteText("DEBUG: Finished Set Wallpaper", string.Empty);
                            
                        }
                        catch (IOException ex)
                        {
                            ProcessError(ex, ErrorMessageType.AdjustDesktopImage, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        }
                    }
                } // using bitmap
                
                if (debugEnabled) WriteText("DEBUG: adjustDesktopImage() Finished", string.Empty);

                if (debugEnabled) WriteText("DEBUG: ", string.Empty); 
                for (int d = 0; d < debugText.Count; d++)
                {
                    if (debugEnabled) WriteText("DEBUG: " + debugText[d].ToString(CultureInfo.InvariantCulture), string.Empty);
                }

                HideChangeWallpaperWorking();

                this.OnWallpaperChangeThreadComplete();

                sw.Stop();
                Debug.WriteLine("AdjustDesktopImages() = " + (sw.ElapsedMilliseconds / 1000f).ToString(CultureInfo.InvariantCulture) + " seconds");
            }
            catch (OutOfMemoryException ex)
            { 
                ProcessError(ex, ErrorMessageType.AdjustDesktopImage, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
        }


        internal static String getWallpaperExtension(ImageFormat vFormat)
        {
            String output = ".png";

            if (vFormat.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("BMP"))
            {
                output = ".bmp";
            }
            if (vFormat.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("JPG")
                || vFormat.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("JPEG"))
            {
                output = ".jpg";
            }
            if (vFormat.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("PNG"))
            {
                output = ".png";
            }

            return output;
        }


        internal static Boolean SetWallpaper(String vFilename, WallpaperModes vWallpaperMode) //, Wallpaper.Style vStyle)
        {
            Boolean output;// = false;

            output = Wallpaper.SetWallpaper(vFilename, vWallpaperMode);// vStyle);

            return output;
        }

        /// <summary>
        /// Choosing a Center fit centers your wallpaper on the screen. 
        /// Smaller images will set with a border on your screen 
        /// whereas the larger images will display only the center part of the image leaving the rest out of view.
        /// 
        /// Adds the given image to the bitmap, but wraps images as needed based on monitor configuration
        /// </summary>
        /// <param name="g">The Graphics of the composite Desktop </param>
        /// <param name="filename">The filename of the image to add</param>
        /// <param name="bounds">The bounds for this screen</param>
        public void AddImageToDesktopCentre(Graphics g, string filename, Rectangle vScreenBounds, int index)
        {

            if (string.IsNullOrEmpty(filename) || !System.IO.File.Exists(filename))
            {
                return;
            }

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, filename, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            // randomly flip the image - 50% chance
            if (settings.RandomFlipImage && FlipX)
            {
                try
                {
                    // there has been 1 occurrence of "generic error occurred in GDI+" in testing... so catch just in case 
                    currentImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            double ratio = settings.ImageSizeScalePercent / 100f; // ImageFunctions.MaintainAspectRatio(currentImage, ref boundsAdjustedScreen, settings.ImageSizeScalePercent, settings.WallpaperMode, settings.AdjustX, settings.AdjustY);
            currentImage = ImageFunctions.ResizeBitmap(currentImage, (float)ratio, InterpolationMode.HighQualityBicubic);

            int x = vScreenBounds.X;
            int y = vScreenBounds.Y;

            int imageX = (currentImage.Width - vScreenBounds.Width) / 2;
            int imageY = (currentImage.Height - vScreenBounds.Height) / 2;

            currentImage = (Bitmap)doCrop(currentImage, new Rectangle(imageX, imageY, (int)(vScreenBounds.Width), (int)(vScreenBounds.Height)));

            x += settings.AdjustX;
            y += settings.AdjustY;

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {
                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);

                Rectangle src = new Rectangle(0, 0, currentImage.Width, currentImage.Height); 
                Rectangle dest = new Rectangle(x, y, currentImage.Width, currentImage.Height); 

                // the +1 on the width/height is to stop the one pixel width lines that appear on the right and bottom, and rounding issues (maybe?)
                g.DrawImage(currentImage,
                    dest,  // DEST    
                    src,   // SRC 
                    GraphicsUnit.Pixel);
            }

            int ScreenCornerX = vScreenBounds.X;
            int ScreenCornerY = vScreenBounds.Y;

            if (settings.ShowScreenID) RenderCaption(g, new Rectangle(ScreenCornerX + vScreenBounds.Width - 70, ScreenCornerY, 200, 200), Screen.AllScreens[index].GetID().ToString(CultureInfo.InvariantCulture));  //index.ToString(CultureInfo.InvariantCulture), 255);

            currentImage.Dispose();
        }



        private static Bitmap doCrop(Bitmap vImage, Rectangle vCropRec)
        {
            return (Bitmap)ImageFunctions.CropImage(vImage, vCropRec.X, vCropRec.Y, vCropRec.Width, vCropRec.Height); //.Clone(); 
        }


        internal void AddImageToDesktopFillWidthHeight2(Graphics g, string filename, Rectangle vScreenBounds, int index, WallpaperModes vMode)
        {

            if (string.IsNullOrEmpty(filename) || !System.IO.File.Exists(filename))
            {
                return;
            }

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Rectangle boundsAdjustedScreen = vScreenBounds;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, filename, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            // randomly flip the image - 50% chance
            if (settings.RandomFlipImage && FlipX)
            {
                try
                {
                    // there has been 1 occurrence of "generic error occurred in GDI+" in testing... so catch just in case 
                    currentImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            double ratio = ImageFunctions.MaintainAspectRatio(currentImage, ref boundsAdjustedScreen, (settings.ImageSizeScalePercent / 100f), vMode, settings.AdjustX, settings.AdjustY);
            currentImage = ImageFunctions.ResizeBitmap(currentImage, (float)ratio, InterpolationMode.HighQualityBicubic);

            int x = vScreenBounds.X;
            int y = vScreenBounds.Y;

            int imageX = (currentImage.Width - vScreenBounds.Width) / 2;
            int imageY = (currentImage.Height - vScreenBounds.Height) / 2;

            currentImage = (Bitmap)doCrop(currentImage, new Rectangle(imageX, imageY, (int)(vScreenBounds.Width), (int)(vScreenBounds.Height)));

            x += settings.AdjustX;
            y += settings.AdjustY;

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {
                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);

                Rectangle src = new Rectangle(0, 0, currentImage.Width, currentImage.Height); 
                Rectangle dest = new Rectangle(x, y, currentImage.Width, currentImage.Height); 

                // the +1 on the width/height is to stop the one pixel width lines that appear on the right and bottom, and rounding issues (maybe?)
                g.DrawImage(currentImage,
                    dest,  // DEST    
                    src,  // SRC
                    GraphicsUnit.Pixel);
            }

            int ScreenCornerX = vScreenBounds.X;
            int ScreenCornerY = vScreenBounds.Y;

            if (settings.ShowScreenID) RenderCaption(g, new Rectangle(ScreenCornerX + vScreenBounds.Width - 70, ScreenCornerY, 200, 200), Screen.AllScreens[index].GetID().ToString(CultureInfo.InvariantCulture));  //index.ToString(CultureInfo.InvariantCulture), 255);

            currentImage.Dispose();
        }


        /// <summary>
        /// Choosing a Center fit centers your wallpaper on the screen. 
        /// Smaller images will set with a border on your screen 
        /// whereas the larger images will display only the center part of the image leaving the rest out of view.
        /// 
        /// Adds the given image to the bitmap, but wraps images as needed based on monitor configuration
        /// </summary>
        /// <param name="g">The Graphics of the composite Desktop </param>
        /// <param name="filename">The filename of the image to add</param>
        /// <param name="bounds">The bounds for this screen</param>
        public void AddImageToDesktopStretch(Graphics g, string filename, Rectangle newBounds, int index)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, filename, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            Rectangle boundsScreen = newBounds;
            float screenScale = 1;

            ImageFunctions.MaintainAspectRatioRectangle(currentImage, ref newBounds, settings.ImageSizeScalePercent / 100f, screenScale);

            int lowestBoundX = Screen.AllScreens[0].Bounds.X; // because the list is sorted by bounds
            int lowestBoundY = Screen.AllScreens[0].Bounds.Y; // because the list is sorted by bounds

            // Draw the first half
            int x = boundsScreen.X + ((boundsScreen.Width - (int)(boundsScreen.Width * settings.ImageSizeScalePercent / 100f)) / 2);
            int y = boundsScreen.Y + ((boundsScreen.Height - (int)(boundsScreen.Height * settings.ImageSizeScalePercent / 100f)) / 2);

            x += Math.Abs(lowestBoundX);
            y += Math.Abs(lowestBoundY);

            // randomly flip the image - 50% chance
            if (settings.RandomFlipImage && FlipX)
            {
                try
                {
                    // there has been 1 occurrence of "generic error occurred in GDI+" in testing... so catch just in case 
                    currentImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            x += settings.AdjustX;
            y += settings.AdjustY;

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {

                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);

                // Draw with corrected Y coordinate
                g.DrawImage(currentImage,
                    new Rectangle(x, y, (int)((boundsScreen.Width) * settings.ImageSizeScalePercent / 100f), // DEST
                        (int)((boundsScreen.Height) * settings.ImageSizeScalePercent / 100f) ),  // DEST
                    new Rectangle(0, 0, currentImage.Width, currentImage.Height), // SRC
                    GraphicsUnit.Pixel);

            }

            //Draw the Screen Number (if enabled)
            int ScreenCornerY = boundsScreen.Y + Math.Abs(lowestBoundY);
            if (settings.ShowScreenID) RenderCaption(g, new Rectangle(x + boundsScreen.Width - 70, ScreenCornerY, 200, 200), Screen.AllScreens[index].GetID().ToString(CultureInfo.InvariantCulture));  //index.ToString(CultureInfo.InvariantCulture), 255); // top right corner

            currentImage.Dispose();
        }
        

        /// <summary>
        /// Adds the given image to the bitmap, but wraps images as needed based on monitor configuration
        /// Works with ASPECTRATIO, FIT
        /// </summary>
        /// <param name="g">The Graphics of the composite Desktop </param>
        /// <param name="filename">The filename of the image to add</param>
        /// <param name="boundsSingleScreen">The bounds for this screen</param>
        public void AddImageToDesktopTile(Graphics g, string filename, Rectangle bounds, int index, float vScale)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, filename, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            Rectangle boundsFull = bounds;
            ImageFunctions.MaintainAspectRatio(currentImage, ref bounds, vScale / 100f, settings.WallpaperMode, settings.AdjustX, settings.AdjustY);

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {

                int COLS = (int)Math.Round(boundsFull.Width / (float)bounds.Width) + 1; 
                if (COLS % 2 == 0) COLS += 1; // must be odd so it starts in the centre

                int levl;
                int c = COLS / 2;
                int x, y;
                x = y = c;
                Size centre = new Size(0, 0);

                // Printing inwards to outwards. Works only for odd sized 2D array with one element in center
                // Spiral loop
                for (levl = 1; c + levl <= COLS; levl++)
                {
                    for (; y <= c + levl && y < COLS; y++) // go right
                        g.DrawImage(currentImage,
                            new Rectangle((x * bounds.Width) + centre.Width, (y * bounds.Height) + centre.Height, bounds.Width, bounds.Height), // DEST
                            new Rectangle(0, 0, currentImage.Width, currentImage.Height), // SRC
                            GraphicsUnit.Pixel);

                    // Since we always start from the center going towards right, top row (going left to right)
                    // will always be the last remaining row to print
                    if (x == 0 && y == COLS) // we are done
                        break;

                    for (x++, y--; x <= c + levl && x < COLS; x++)  // go down
                        g.DrawImage(currentImage,
                            new Rectangle((x * bounds.Width) + centre.Width, (y * bounds.Height) + centre.Height, bounds.Width, bounds.Height), // DEST
                            new Rectangle(0, 0, currentImage.Width, currentImage.Height), // SRC
                            GraphicsUnit.Pixel);
                    for (x--, y--; y >= c - levl; y--)    // go left
                        g.DrawImage(currentImage,
                            new Rectangle((x * bounds.Width) + centre.Width, (y * bounds.Height) + centre.Height, bounds.Width, bounds.Height), // DEST
                            new Rectangle(0, 0, currentImage.Width, currentImage.Height), // SRC
                            GraphicsUnit.Pixel);
                    for (x--, y++; x >= c - levl; x--)     // go up
                        g.DrawImage(currentImage,
                            new Rectangle((x * bounds.Width) + centre.Width, (y * bounds.Height) + centre.Height, bounds.Width, bounds.Height), // DEST
                            new Rectangle(0, 0, currentImage.Width, currentImage.Height), // SRC
                            GraphicsUnit.Pixel);
                    x++;
                    y++;
                }

                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);
            }

            int ScreenCornerX = boundsFull.X + boundsFull.Width - 20;
            int ScreenCornerY = boundsFull.Y + boundsFull.Height;
            if (settings.ShowScreenID)
                RenderCaption(g, new Rectangle(ScreenCornerX + boundsFull.Width - 70, ScreenCornerY, 200, 200), Screen.AllScreens[index].GetID().ToString(CultureInfo.InvariantCulture));  //index.ToString(CultureInfo.InvariantCulture), 255);

            currentImage.Dispose();
        }

        /// <summary>
        /// Adds the given image to the bitmap
        /// </summary>
        /// <param name="g">The Graphics of the composite Desktop </param>
        /// <param name="filename">The filename of the image to add</param>
        /// <param name="boundsSingleScreen">The bounds for this screen</param>
        public void AddImageToDesktopByPosition(Graphics g, string filename, Rectangle vScreenBounds, int index)//, Rectangle AllBounds)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, filename, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            Rectangle boundsScreen = vScreenBounds;
            double ratio = ImageFunctions.MaintainAspectRatio(currentImage, ref boundsScreen, settings.ImageSizeScalePercent / 100f, settings.WallpaperMode, settings.AdjustX, settings.AdjustY);

            currentImage = ImageFunctions.ResizeBitmap(currentImage, (float)ratio, InterpolationMode.HighQualityBicubic);

            int lowestBoundX = Screen.AllScreens[0].Bounds.X; // because the list is sorted by bounds
            int lowestBoundY = Screen.AllScreens[0].Bounds.Y; // because the list is sorted by bounds

            int x = boundsScreen.X;
            int y = boundsScreen.Y;

            x += Math.Abs(lowestBoundX);
            y += Math.Abs(lowestBoundY);

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {
                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);

                Rectangle src = new Rectangle(0, 0, currentImage.Width, currentImage.Height);// vScreenBounds.Width, vScreenBounds.Height); // currentImage.Width, currentImage.Height);
                Rectangle dest = new Rectangle(x, y, boundsScreen.Width, boundsScreen.Height); //Rectangle(x - 1, y - 1, vScreenBounds.Width + 1, vScreenBounds.Height + 1); // boundsScreen

                // Draw with corrected Y coordinate
                // the +1 on the width/height is to stop the one pixel width lines that appear on the right and bottom
                g.DrawImage(currentImage,
                    dest, 
                    src,  
                    GraphicsUnit.Pixel);
            }

            // Draw with corrected X coordinate
            int ScreenCornerX = boundsScreen.X + Math.Abs(lowestBoundX);
            int ScreenCornerY = boundsScreen.Y + Math.Abs(lowestBoundY);
            if (settings.ShowScreenID) RenderCaption(g, new Rectangle(ScreenCornerX + boundsScreen.Width - 70, ScreenCornerY, 200, 200), Screen.AllScreens[index].GetID().ToString(CultureInfo.InvariantCulture));  //index.ToString(CultureInfo.InvariantCulture), 255);

            currentImage.Dispose();
        }


        /// <summary>
        /// Adds the given image to the bitmap, but wraps images as needed based on monitor configuration
        /// Works with ASPECTRATIO, FIT
        /// </summary>
        /// <param name="g">The Graphics of the composite Desktop </param>
        /// <param name="filename">The filename of the image to add</param>
        /// <param name="boundsSingleScreen">The bounds for this screen</param>
        /// <returns>percentage of span taken by this image</returns>
        public float AddImageToDesktopSpan(Graphics g, string filename, Rectangle AllBounds, Rectangle bounds, int index)
        {
            float outputPercent = 1;
            String file = filename;
            Boolean wideFound = false;
            // find if there is a wide image in the current set
            for (int i = 0; i < wallpaperFilenames.Count; i++)
            {
                file = wallpaperFilenames[i];
                int wallpaperIndex = settings.Images.FindIndex(r => file.Contains(r.FullPath));

                // if not a valid index
                if (wallpaperIndex == -1)
                {
                    return 0;
                }

                if (settings.Images[wallpaperIndex].Width / bounds.Width > 1
                    && settings.Images[wallpaperIndex].Height / bounds.Height <= 1
                    )
                {
                    wideFound = true;
                    break; // the "file" variable holds the found wide filename
                }
            }

            // if we didnt find a wide image, go back to using the passed filename
            if (!wideFound)
            {
                file = filename;
            }

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, file, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            Rectangle boundsScreen = bounds;

            float imageScreenRatioWidth = currentImage.Width / boundsScreen.Width;
            float imageScreenRatioHeight = currentImage.Height / boundsScreen.Height;

            // if a tall image, it needs to be reduced to the right height
            if (imageScreenRatioHeight > 1)
            {
                double ratio = ImageFunctions.MaintainAspectRatio(currentImage, ref bounds, settings.ImageSizeScalePercent / 100f, settings.WallpaperMode, settings.AdjustX, settings.AdjustY);
                outputPercent = (float)ratio;
            }
            // else if its a wide image
            else if (imageScreenRatioWidth > 1)
            {
                bounds = new Rectangle(0, 0, AllBounds.Width, AllBounds.Height);
                double ratio = ImageFunctions.MaintainAspectRatio(currentImage, ref bounds, settings.ImageSizeScalePercent / 100f, settings.WallpaperMode, settings.AdjustX, settings.AdjustY);
                outputPercent = imageScreenRatioWidth;
            }
            else
            {
                double ratio = ImageFunctions.MaintainAspectRatio(currentImage, ref bounds, settings.ImageSizeScalePercent / 100f, settings.WallpaperMode, settings.AdjustX, settings.AdjustY);
                outputPercent = (float)ratio;
            }

            // randomly flip the image - 50% chance
            if (settings.RandomFlipImage && FlipX)
            {
                try
                {
                    // there has been 1 occurrence of "generic error occurred in GDI+" in testing... so catch just in case 
                    currentImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {
                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);

                Rectangle src = new Rectangle(0, 0, currentImage.Width, currentImage.Height);
                // Draw with corrected Y coordinate
                // the +1 on the width/height is to stop the one pixel width lines that appear on the right and bottom, and rounding issues (maybe?)
                g.DrawImage(currentImage,
                    new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height),  // DEST    
                    src, 
                    GraphicsUnit.Pixel);
            }
            int lowestBoundX = Screen.AllScreens[0].Bounds.X; // because the list is sorted by bounds
            int lowestBoundY = Screen.AllScreens[0].Bounds.Y; // because the list is sorted by bounds

            int x = bounds.X;
            int y = bounds.Y;

            x += Math.Abs(lowestBoundX);
            y += Math.Abs(lowestBoundY);

            int ScreenCornerX = boundsScreen.X + Math.Abs(lowestBoundX);
            int ScreenCornerY = boundsScreen.Y + Math.Abs(lowestBoundY);

            if (settings.ShowScreenID) RenderCaption(g, new Rectangle(ScreenCornerX + boundsScreen.Width - 70, ScreenCornerY, 200, 200), Screen.AllScreens[index].GetID().ToString(CultureInfo.InvariantCulture));  //index.ToString(CultureInfo.InvariantCulture), 255);

            currentImage.Dispose();
            currentImage = null;

            return (float)outputPercent;
        }


        public void AddImageToDesktopLAWC(Graphics g, string filename, Rectangle boundsAll, Rectangle boundsDest, int index, ref Boolean vIsSpan)
        {
            float screenScale = 1;

            if (string.IsNullOrEmpty(filename) || !System.IO.File.Exists(filename))
            {
                ProcessError(null, ErrorMessageType.FileDoesntExist, false, false, String.Format(CultureInfo.InvariantCulture, "File: {0}", filename), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                return;
            }

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Bitmap currentImage = null;
            ImageFunctions.BitmapFromFile(ref currentImage, filename, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
            currentImage.SetResolution(96, 96); //this stops some GDI+ errors

            // randomly flip the image - 50% chance
            if (settings.RandomFlipImage && FlipX)
            {
                try
                {
                    // there has been 1 occurrence of "generic error occurred in GDI+" in testing... so catch just in case 
                    currentImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            Rectangle destBounds = new Rectangle(0, 0, boundsDest.Width, boundsDest.Height);
            Rectangle zeroRect;

            if (settings.ExtraScreenInfo.Count() <= index)
            {
                initDisplaySettings(false);
            }

            if (settings.ExtraScreenInfo[index].ShowImageOnScreen)
            {
                ImageFunctions.MaintainAspectRatioRectangle(currentImage, ref destBounds, settings.ImageSizeScalePercent / 100f, screenScale);

                // adjust the image as per user settings
                SetAdjustedWallpaper(ref currentImage);

                //centre image
                Size centreImage = new Size(
                    destBounds.Width / 2,
                    destBounds.Height / 2
                    );
                Size centreScreen = new Size(
                    Screen.AllScreens[index].Bounds.Width / 2,
                    Screen.AllScreens[index].Bounds.Height / 2
                    );

                float horizPercent = (currentImage.Width / (float)(Screen.AllScreens[index].Bounds.Width));
                float vertPercent = (currentImage.Height / (float)(Screen.AllScreens[index].Bounds.Height));

                zeroRect = BoundsUtilities.ZeroRectangle(Screen.AllScreens[index].Bounds, BoundsUtilities.GetRefPoint(boundsAll));

                //centre the image
                int x = zeroRect.X + centreScreen.Width - centreImage.Width;
                int y = zeroRect.Y + centreScreen.Height - centreImage.Height;

                // if the image is a span sized one
                float aspect = (currentImage.Width / (float)(currentImage.Height));

                if (aspect < settings.AspectThresholdNarrow)
                {  // Portrait shaped images - Best Choice = FillWidth
                    AddImageToDesktopFillWidthHeight2(g, wallpaperFilenames[index].ToString(CultureInfo.InvariantCulture), Screen.AllScreens[index].Bounds, index, WallpaperModes.FillWidth);
                }
                else if (aspect >= settings.AspectThresholdNarrow && aspect < settings.AspectThresholdWide)
                {
                    int margin = settings.MarginToEnlarge;
                    float marginWidth = settings.MarginToEnlarge / 100f;
                    float marginHeight = (settings.MarginToEnlarge / 100f) * 0.66f; //Smaller value for vertical cause we have less pixels to work with

                    // if mostly square and small, tile it
                    //TODO: Re-implement Tiling in LAWC mode for squareish images
                    //if ( Math.Abs(horizPercent - vertPercent) < 0.25f
                    //    //horizPercent < 0.5f && vertPercent < 0.5f // check size
                    //    //&& (ratio > 0.8f && ratio < 1.2f) // check square
                    //    )
                    //{
                    //    AddImageToDesktopTile(g, filename, boundsAll, index, settings.ImageSizeScalePercent);
                    //}

                    // Standard to Wide Screen shaped images (eg. around 1920x1080) - Best Choice = FillHeight
                    // if the image is only slightly wider or narrower, then use fill width, otherwise use fill height
                    if (horizPercent > (1 - marginWidth) && horizPercent < (1 + marginWidth) //0.85  1.15
                            && vertPercent > (1 - marginHeight) && vertPercent < (1 + marginHeight))
                    {
                        AddImageToDesktopFillWidthHeight2(g, wallpaperFilenames[index].ToString(CultureInfo.InvariantCulture), boundsDest, index, WallpaperModes.FillWidth);
                    }
                    else
                    {
                        AddImageToDesktopFillWidthHeight2(g, wallpaperFilenames[index].ToString(CultureInfo.InvariantCulture), Screen.AllScreens[index].Bounds, index, WallpaperModes.FillHeight);
                    }
                }
                else if (aspect >= settings.AspectThresholdWide)
                {  // Ultra Wide Screen images
                    float wideThreshold = settings.WideThreshold;
                    wideThreshold = 1 + (wideThreshold / 100f);
                    // is the image big enough to span more than one screen, and its the FIRST one on the list
                    if (index == 0 && horizPercent > wideThreshold)
                    {
                        // Spannable Image
                        AddImageToDesktopSpan(g, filename, boundsAll, boundsAll, index);
                        vIsSpan = true;
                        return;
                    }
                    else
                    {
                        // not really that wide.... Standard Image
                        AddImageToDesktopFillWidthHeight2(g, filename, boundsDest, index, WallpaperModes.FillHeight);
                    }
                }
                else
                {
                    // shouldnt get here
                    g.DrawImage(currentImage,
                        new Rectangle(x - 1, y - 1, destBounds.Width + 1, destBounds.Height + 1),
                        new Rectangle(0, 0, currentImage.Width, currentImage.Height), GraphicsUnit.Pixel);
                }
            }
            
            currentImage.Dispose();
        }


        private void adjustHSVBlue(ref Bitmap vBitmap, String vSettingsFullPath)
        {
            if (settings.UseHSV)
            {
                float sat = 1.2f;
                float hue = 0f;
                float val = 0.03f;
                float red = 1f;
                float green = 1f; // 0.8f;
                float blue = 1.2f;

                // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
                vBitmap.SetResolution(96, 96); //this stops some GDI+ errors

                try
                {
                    ImageFunctions.ImageHSVAdjust(ref vBitmap, sat, hue, val, red, green, blue, true);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                }

            }
        }


        private void adjustHSVOrange(ref Bitmap vBitmap, String vSettingsFullPath)
        {
            if (settings.UseHSV)
            {
                // https://stackoverflow.com/questions/2681813/c-sharp-winforms-graphics-drawimage-problem
                vBitmap.SetResolution(96, 96); //this stops some GDI+ errors

                float sat = 1.1f;
                float hue = 0f;
                float val = -0.03f;
                float red = 1.1f;
                float green = 1.1f;
                float blue = 0.8f;
                try
                {

                    ImageFunctions.ImageHSVAdjust(ref vBitmap, sat, hue, val, red, green, blue, true);
                }
                catch (OutOfMemoryException ex)
                {
                    ProcessError(ex, ErrorMessageType.GraphicsGDI, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                }

            }
        }



        private void SetAdjustedWallpaper(ref Bitmap vBitmap)
        {
            DateTime timeEndLightening = this.settings.LightSunriseTime.AddMinutes(this.settings.DurationMins);

            if (settings.UseDarkLightTimes)
            {
                if (screenStateCurrent == ScreenState.Light)
                {
                    if (settings.UseHSV)
                        adjustHSVBlue(ref vBitmap, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
                else if (screenStateCurrent == ScreenState.Dark)
                {
                    AdjustWallpaperImages(ref vBitmap, 1.0f, true);
                    if (settings.UseHSV) adjustHSVOrange(ref vBitmap, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
                else if (screenStateCurrent == ScreenState.GettingDarker)
                {
                    TimeSpan Duration = DateTime.Now - this.settings.DarkSunsetTime;
                    float percent = (float)(Duration.TotalMinutes / (float)settings.DurationMins);
                    AdjustWallpaperImages(ref vBitmap, percent, true);
                    if (settings.UseHSV) adjustHSVOrange(ref vBitmap, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
                else if (screenStateCurrent == ScreenState.GettingLighter)
                {
                    TimeSpan Duration = timeEndLightening - DateTime.Now;
                    float percent = (float)(Duration.TotalMinutes / (float)settings.DurationMins);
                    AdjustWallpaperImages(ref vBitmap, percent, true);
                    if (settings.UseHSV) adjustHSVBlue(ref vBitmap, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }
            else
            if (settings.AdjustmentsUseAlways || settings.DarkMode)
            {
                // STAYING DARK
                AdjustWallpaperImages(ref vBitmap, 1.0f, true);
            }
        }

        private void AdjustWallpaperImages(ref Bitmap vImage, float vPercent, Boolean vGettingDarker)
        {
            float contrast;
            float gamma;
            float brightness;
            float alpha;

            float percent = vPercent;
            
            // TESTED MODES: DARK, LIGHT
            alpha = 1f; // solid
            float alphaChange = this.settings.Alpha * (1f - percent);
            alpha = alpha + (this.settings.Alpha - alphaChange);
            alpha /= 100f;
            alpha -= 1f;
            alpha = Math.Abs(alpha);

            brightness = 1f; // 100% bright
            float brightnessChange = this.settings.Brightness * (1f - percent);
            brightness = brightness + (this.settings.Brightness - brightnessChange);
            brightness /= 100f;
            brightness -= 1f;
            brightness = Math.Abs(brightness);

            contrast = 1f; // 100% bright
            float contrastChange = this.settings.Contrast * (1f - percent);
            contrast = contrast + (this.settings.Contrast - contrastChange);
            contrast /= 100f;
            contrast -= 1f;
            contrast = Math.Abs(contrast);

            gamma = 0f; // 100% bright
            float gammaChange = (this.settings.Gamma - 100f) * (1f - percent);
            gamma += gammaChange;

            gamma+= 100; //100% = no change (this makes it 1xx percent)
            // convert to a percent
            gamma /= 100;
            
            // LIGHT TO DARK
            if (vGettingDarker)
            {
                //percent = 1 - vPercent;

                
                //if (percent > 0)
                //{
                //    float alphaChange = this.settings.Alpha * percent;
                //    alpha += 100f - alphaChange;
                //    //alpha = (this.settings.Alpha - alphaChange);
                //}
                //alpha += 1;

                //brightness = 0;
                //if (percent > 0)
                //{
                //    float brightnessChange = this.settings.Brightness * percent;
                //    brightness = (brightnessChange + this.settings.Brightness);
                //}
                //brightness += 1;

                //gamma = 0;
                //if (percent > 0)
                //{
                //    float gammaChange = this.settings.Gamma * percent;
                //    gamma = (gammaChange + this.settings.Gamma);
                //}
                //else
                //{
                //    gamma += 100; // the default
                //}
                //gamma /= 100;

                //contrast = 0;
                //if (percent > 0)
                //{
                //    float contrastChange = this.settings.Contrast * percent;
                //    contrast = (contrastChange + this.settings.Contrast);
                //}
                //contrast += 1;


                //float alphaChange = (1 - this.settings.Alpha) * percent;
                //alpha = (alphaChange + this.settings.Alpha);

                //percent = 1 - vPercent;

                //float alphaChange = Math.Abs(1 - this.settings.Alpha) * percent;
                //alpha = (alphaChange + this.settings.Alpha);

                //float brightnessChange = Math.Abs(1 - this.settings.Brightness) * percent;
                //brightness = (brightnessChange + this.settings.Brightness);

                //float gammaChange = Math.Abs(1 - this.settings.Gamma) * percent;
                //gamma = (gammaChange + this.settings.Gamma);

                //float contrastChange = Math.Abs(1 - this.settings.Contrast) * percent;
                //contrast = (contrastChange + this.settings.Contrast);

                //float tintChange = this.settings.TintStrength * percent;
                //tint = (tintChange + this.settings.TintStrength);

                //float alphaChange = (1 - this.settings.Alpha) * percent;
                //alpha = (alphaChange + this.settings.Alpha);

                //float brightnessChange = Math.Abs(1 - this.settings.Brightness) * percent;
                //brightness = (brightnessChange + this.settings.Brightness);

                //float gammaChange = Math.Abs(1 - this.settings.Gamma) * percent;
                //gamma = (gammaChange + this.settings.Gamma);

                //float contrastChange = Math.Abs(1 - this.settings.Contrast) * percent;
                //contrast = (contrastChange + this.settings.Contrast);

            }
            else
            {

                // DARK TO LIGHT: (Percentage is different)
                float alphaChange2 = (1 - this.settings.Alpha) * percent;
                alpha = (alphaChange2 + this.settings.Alpha);

                float brightnessChange2 = Math.Abs(1 - this.settings.Brightness) * percent;
                brightness = (brightnessChange2 + this.settings.Brightness);

                float gammaChange2 = Math.Abs(1 - this.settings.Gamma) * percent;
                gamma = (gammaChange2 + this.settings.Gamma);

                float contrastChange2 = Math.Abs(1 - this.settings.Contrast) * percent;
                contrast = (contrastChange + this.settings.Contrast);

            }

            ImageFunctions.AdjustImage(ref vImage, brightness, contrast, gamma);  
            ImageFunctions.ImageTint(ref vImage, settings.TintColour, vPercent, settings.TintStrength); // strength = 1-100, percent 0-1f
            ImageFunctions.SetImageOpacity(ref vImage, alpha); // 1f = SOLID
            

        }

        /// <summary>
        /// Render text over a Graphics object
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <param name="caption"></param>
        private static void RenderCaption(Graphics g, Rectangle bounds, string caption)
        {
            Font captionFont = new Font(FontFamily.GenericSansSerif, bounds.Height / 4);
            Rectangle layoutRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            GraphicsPath path = new GraphicsPath();
            Pen p = new Pen(Brushes.Black, 5);

            path.AddString(caption, captionFont.FontFamily,
                (int)captionFont.Style, (float)captionFont.Height,
                layoutRect, StringFormat.GenericDefault);

            g.DrawPath(p, path);
            g.FillPath(Brushes.White, path);

            p.Dispose();
            captionFont.Dispose();
            path.Dispose();
        }


        /// <summary>
        /// Render text over a Graphics object
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <param name="caption"></param>
        private static void RenderCaption(Graphics g, Rectangle bounds, float vFontSize, string caption, Color vColour1, int vTransparency)
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;            

            Color outlineColour;
            outlineColour = Color.FromArgb(vTransparency, 0, 0, 0); // BLACK

            Font captionFont = new Font("Arial", (int)vFontSize, FontStyle.Regular, GraphicsUnit.Point);
            Font captionOutlineFont = new Font("Arial", (int)vFontSize, FontStyle.Regular, GraphicsUnit.Point);
            Rectangle layoutRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            GraphicsPath path = new GraphicsPath();
            Pen pB = new Pen(Brushes.Black, 3);
            //Pen pW = new Pen(Brushes.Black, 2);
            SolidBrush fontBrush = new SolidBrush(vColour1);

            path.AddString(caption, captionFont.FontFamily,
                (int)captionFont.Style, (int)vFontSize, 
                layoutRect, StringFormat.GenericTypographic);   

            g.DrawString(caption, captionOutlineFont, new SolidBrush(outlineColour), new PointF(bounds.X - 1, bounds.Y - 1));
            g.DrawString(caption, captionFont, fontBrush, new PointF(bounds.X, bounds.Y));

            pB.Dispose();
            captionFont.Dispose();
        }

        internal void DoTimerAdjust()
        {
            if (formClosing) return;

            if (debugEnabled) WriteText("DEBUG: doTimerAdjust() Started", string.Empty);

            if (screenStateCurrent == ScreenState.GettingDarker || screenStateCurrent == ScreenState.GettingLighter)
            {
                AdjustDesktopImages(); //(true); // run the adjustment in its own thread
            }

            if (debugEnabled) WriteText("DEBUG: doTimerAdjust() Finished", string.Empty);

        }

        internal void UpdateScreenState()
        {
            ScreenState newState = GetScreenState(DateTime.Now);

            // check if we need to do this here.  HACK cause it shouldnt be done here
            if (screenStateCurrent != newState)
            {
                screenStateCurrent = newState;

                try
                {
                    olvFolders.Invoke((Action)delegate
                    {
                        SetInterfaceColour();
                    });
                }
                catch (Exception)
                {
                    // Do Nothing
                }
                
                
            }

            screenStateCurrent = newState;
        }

        private void DoTimerWallpaper()
        {
            if (formClosing) return;

            UpdateScreenState();

            this.SetTimeToGoText();

            if (olvImages.GetItemCount() == 0) return; // dont bother doing anything if we dont have images

            if (settings.NextWallpaperChange > DateTime.Now.AddSeconds(-1)) return; // if we are NOT up to the next change return

            ChangeWallpaperNow(true, false);
        }

        internal void SelectCurrentWallpaper(Boolean vGetSelectedImage) //Boolean vScrollToEntry)
        {
            if (vGetSelectedImage)
            {
                if (this.olvImages.SelectedItem != null)
                    if (this.olvImages.SelectedItem.RowObject != null)
                        this.settings.ImageLastSelected = ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath; 

            }

            SetPreviewImages(this.settings.ImageLastSelected);
        }


        #endregion


        #region LAWC Actions


        // Or if you wanted to get fancy ping multiple sources
        private async Task<List<PingReply>> PingAsync(List<string> listOfIPs)
        {
            Ping pingSender = new Ping();
            var tasks = listOfIPs.Select(ip => pingSender.SendPingAsync(ip, 2000));
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.ToList();
        }


        internal Boolean IsLightTimeOvernight()
        {
            // check what state 
            TimeSpan dt = settings.DarkSunsetTime - settings.LightSunriseTime;

            if (dt.TotalMinutes < 0)
            {
                return true;
            }

            return false;            
        }


        private void SetFolderState(Boolean vEnabled, Boolean vDoSelectCurrent)
        {
            ((FolderInfo)(this.olvFolders.SelectedItem.RowObject)).Enabled = vEnabled;
            SetView(false, vDoSelectCurrent); //true
            CheckImagesAvailability(true);
        }

        /// <summary>
        /// Go through the found images, and 1) delete the invalid entries 2) update the current list's info  3) redraw the image list
        /// </summary>
        private void ProcessFoundImages()
        {
            ShowChangeWallpaperWorking(true);  

            // if we are scanning ALL FOLDERS
            if (this.foldersToScan.Count > 0)
            {
                DoRescanAllFolders();
                return;
            }

            CleanImageList(); // remove invalid entries etc

            // Add new images
            //Update the Image info already recorded, with the new file/image info
            if (this.settings.FoundImages.Count() > 0)
            {
                UpdateImageInfo();
            }

            lblStatus.Visible = true;
            lblStatus.Text = "Refreshing Wallpaper List";
            statusStrip1.Refresh();

            redrawLists();

            pbarStatus.Visible = false;
            lblStatus.Text = "";
            lblStatus.Visible = false;
            SetView(false, true);

            // if we have 1 item (ie. this is the first item added to the folders), then we need to reset the change time
            if (this.olvFolders.Items.Count == 0)
            {
                SetNextWallpaperChangeTime();
                SetNextWallpaperAdjustTime();
            }

            InitWallpaperFilenames();

            SaveSettings(string.Empty, this.settings);
            HideChangeWallpaperWorking(); 

            // if images are found, then change the wallpaper now
            if (settings.Images.Count > 0)
                ChangeWallpaperNow(true, false);

            rescanFolderToolStripMenuItem.Enabled = true;
            rescanAllFoldersToolStripMenuItem1.Enabled = true;
            addFolderToolStripMenuItem.Enabled = true;
            addImageFolderToolStripMenuItem.Enabled = true;
            rescanAllFoldersToolStripMenuItem2.Enabled = true;
            checkFoldersToolStripMenuItem.Enabled = true;
            removeFolderToolStripMenuItem.Enabled = true;

            isScanningFolder = false;
            HideChangeWallpaperWorking();

        }

        // this is used to see what percentage of progress we are at when getting DARKER or LIGHTER
        internal float GetPercentThroughChange()
        {
            if (this.screenStateCurrent == ScreenState.GettingDarker)
            {
                TimeSpan Duration = DateTime.Now - this.settings.DarkSunsetTime;
                float percent = (float)(Duration.TotalMinutes / (float)settings.DurationMins);
                return percent;
            }
            else if (this.screenStateCurrent == ScreenState.GettingLighter)
            {
                DateTime timeEndLightening = this.settings.LightSunriseTime.AddMinutes(this.settings.DurationMins);
                TimeSpan Duration = timeEndLightening - DateTime.Now;
                float percent = (float)((this.settings.DurationMins - Duration.TotalMinutes) / (float)settings.DurationMins);
                return percent;
            }
            else if (this.screenStateCurrent == ScreenState.Light)
            {
                return 1.0f;
            }
            else if (this.screenStateCurrent == ScreenState.Dark)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Check the log file, and if its over the autobackup period and over 0kb, rename it
        /// </summary>
        private void CheckLog()
        {
            long secondsOld = 0; // how long since the last backup
            String logFilePath = Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

            if (!string.IsNullOrEmpty(logFilePath))
            {
                if (System.IO.File.Exists(logFilePath))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFilePath);
                    secondsOld = (long)DateTime.Now.Subtract(fileInfo.CreationTime).TotalSeconds;
                    int days = (int)(secondsOld / 60 / 60 / 24);

                    if (days > FrmMain.AutobackupDays && fileInfo.Length > 0)
                    {
                        // rename the log file
                        String newFilename = System.IO.Path.GetDirectoryName(logFilePath) +
                            String.Format(CultureInfo.InvariantCulture, "\\Error Log to {0}-{1}-{2} {3}-{4}.log",
                            DateTime.Now.Year.ToString("D2", CultureInfo.InvariantCulture),
                            DateTime.Now.Month.ToString("D2", CultureInfo.InvariantCulture),
                            DateTime.Now.Day.ToString("D2", CultureInfo.InvariantCulture),
                            DateTime.Now.Hour.ToString("D2", CultureInfo.InvariantCulture),
                            DateTime.Now.Minute.ToString("D2", CultureInfo.InvariantCulture)
                            );

                        try
                        {
                            System.IO.File.Move(logFilePath, newFilename);
                        }
                        catch (IOException ex)
                        {
                            ProcessError(ex, ErrorMessageType.FileProblem, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                        }
                        
                    }
                }
            }
            
        }


        private void AutoBackup()
        {
            if (!settings.AutoBackup) return;

            string LatestBackup = getLastBackupFilename();
            long secondsOld = 0; // how long since the last backup

            if (!string.IsNullOrEmpty(LatestBackup))
            {
                if (System.IO.File.Exists(LatestBackup))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(LatestBackup);
                    secondsOld = (long)DateTime.Now.Subtract(fileInfo.CreationTime).TotalSeconds;
                    int days = (int)(secondsOld / 60 / 60 / 24);
                    if (days >= settings.AutoBackupDays) //FrmMain.AutobackupDays)
                    {
                        BackupSettings(false);
                    }
                }
            }
            else
            {
                // no previous backup found - backup now
                BackupSettings(false);
            }

            // delete older backup files
            String settingsFullPath = Setting.getSettingsFullPath(Properties.Settings.Default.Portable);

            DirectoryInfo di = new DirectoryInfo(Setting.getSettingsFullPath(Properties.Settings.Default.Portable));//@"C:\Music");
            FileInfo[] files = di.GetFiles("Settings_BAK*.xml", SearchOption.TopDirectoryOnly)
             //.Select(x => new FileInfo(x))
             .OrderByDescending(x => x.LastWriteTime)
             //.Take(5)
             .ToArray();

            for (int i = 0; i < files.Count(); i++)
            {
                if (i >= BackupsToKeep)
                {
                    FileFunctions.DeleteFileToRecycleBin(files[i].FullName);
                }
            }
        }

        internal void SetStartAutomatically(Boolean vSet)
        {
            RegistryKey rkApp;
            rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string appName = this.GetType().Assembly.GetName().Name.ToString(CultureInfo.InvariantCulture);

            if (vSet)
            {
                // ADD KEY
                rkApp.SetValue(appName, Application.ExecutablePath.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                // REMOVE KEY
                try
                {
                    rkApp.DeleteValue(appName);
                }
                catch (IOException)
                {
                    // entry probably doesnt exist, ignore it
                }

            }

            rkApp.Close();
        }

        private void getSunriseSunsetValue(out DateTime vSunrise, out DateTime vSunset)
        {
            vSunrise = settings.LightSunriseTime;
            vSunset = settings.DarkSunsetTime;

        }

        private void DragDropFolderEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;

            this.Focus();
            this.BringToFront();

            olvFolders.Focus();
            olvFolders.BringToFront();

        }

        private void DragDropFolder(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                if (Directory.Exists(path))
                {
                    AddFolder(path);
                    settings.LastFolderPath = path;
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    // do nothing
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void AddSampleEventsWebsites()
        {
            if (MessageBox.Show(
                    "Would you like to import sample Events, and a list\n"
                            + "of popular Wallpaper Websites?\n",
                            "Sample Events and Wallpaper Websites",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                startWebsiteImport();
                addDefaultEvents();

                MessageBox.Show("Now please go into Advanced Settings -> Events tab, \n"
                    + "and Enable the CPU and Weather sensors. \n"
                    + "You will also need to set your Location in Advanced Settings. \n"
                    , "Add Samples"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information
                    );
            }
        }

        private void CheckStartWizard()
        {
            // do the Startup Wizard
            if (settings.FirstRunDone == false)
            {
                MessageBox.Show("Note: This is FREE software.\n\nIf you have paid for this software then the supplier has violated the terms of the license.", "LAWC is Free", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                ShowChangeWallpaperWorking(true);

                StartSetupWizard();
                AddSampleEventsWebsites();

                HideChangeWallpaperWorking();

                settings.FirstRunDone = true;
                SaveSettings(string.Empty, this.settings);

            }
        }

        private void CheckDonateMessage()
        {
            this.settings.UseCount++;

            if (this.settings.UseCount % 100 == 0)
            {
                MessageBox.Show("Please donate to help support Light Adjusting Wallpaper Changer.", "Donate to LAWC", MessageBoxButtons.OK, MessageBoxIcon.Question);

                frmAbout.ShowDialog();

            }

            SaveSettings(string.Empty, this.settings);
        }

        // High DPI Support
        // https://docs.microsoft.com/en-us/dotnet/framework/winforms/high-dpi-support-in-windows-forms
        private void ShowMonitorInfo()
        {
            //update lowest view count info
            lowestViewCount = GetLowestViewCountVisibleOnly(out lowestViewCountIndex);

            String text = "";

            text += ("Screen Count: " + SystemInformation.MonitorCount.ToString(CultureInfo.InvariantCulture)) + System.Environment.NewLine;// + System.Environment.NewLine;
            text += ("Screen Count By Mode: " + ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode).ToString(CultureInfo.InvariantCulture)) + System.Environment.NewLine + System.Environment.NewLine;

            for (int i = 0; i < Screen.AllScreens.Length; i++) 
            {
                Screen screen = Screen.AllScreens[i];
                //screen.GetDpi(DpiType.RAW, out uint dpiX, out uint dpiY);
                //screen.DPItoPercent(dpiX, out float dpiPercent);
                //int.TryParse(screen.DeviceName.ToUpper().Replace("DISPLAY", "").Replace(@"\", "").Replace(".", ""), out screenID);dd;
                //Scale Percentage - DPI Values
                //100%       - 96
                //125%       - 120
                //150%       - 144
                //200%       - 192

                text += ("Screen #" + screen.GetID() + System.Environment.NewLine);
                text += "Device Name: " + screen.DeviceName + System.Environment.NewLine;
                text += ("Primary Screen: " + screen.Primary.ToString(CultureInfo.InvariantCulture)) + System.Environment.NewLine;
                text += ("Bounds: " + screen.Bounds.ToString() + System.Environment.NewLine); // RESOLUTION IN WIDTH AND HEIGHT
                text += ("Working Area: " + screen.WorkingArea.ToString() + System.Environment.NewLine);
                text += ("Bits Per Pixel: " + screen.BitsPerPixel.ToString(CultureInfo.InvariantCulture)) + System.Environment.NewLine;

                text += System.Environment.NewLine;
            }

            text += System.Environment.NewLine;
            text += "=====================================================" + System.Environment.NewLine;
            text += "Other Information" + System.Environment.NewLine + System.Environment.NewLine;

            text += "* Settings Path: " + Properties.Settings.Default.SettingsFilePath.ToString(CultureInfo.InvariantCulture);
            text += System.Environment.NewLine;
            text += "* Lowest View Count: " + lowestViewCount.ToString(CultureInfo.InvariantCulture) + " @ Index: " + lowestViewCountIndex.ToString(CultureInfo.InvariantCulture) + " " 
                + "File: " + settings.Images[lowestViewCountIndex].FullPath; 
            text += System.Environment.NewLine;

            text += "* Screen Colour Change Times: " + System.Environment.NewLine;
            text += GetScreenChangeTimes();
            text += System.Environment.NewLine;

            int num;// = 0;
            if (wallpaperFilenames.Count > 0)
            {
                text += "* Filename(s): ";
                text += System.Environment.NewLine;
                num = 0;
                foreach (String filename in wallpaperFilenames)
                {
                    text += "\tScreen #" + num + ": " + filename; 
                    text += System.Environment.NewLine;
                    num++;
                }
                text += System.Environment.NewLine;
            }
            else
            {
                // filenames count = 0
                text += "* Filename(s): (None Selected)";
                text += System.Environment.NewLine;
            }

            text += "* Portable Installation: " + Properties.Settings.Default.Portable.ToString(CultureInfo.InvariantCulture);
            text += System.Environment.NewLine;
            text += "* LAWC Executable Path: " + Application.ExecutablePath.ToString(CultureInfo.InvariantCulture);
            text += System.Environment.NewLine;
            text += "* Administrator Access: " + MainFunctions.isAdministrator().ToString(CultureInfo.InvariantCulture);
            text += System.Environment.NewLine;
            text += "* Applying Settings: " + this.applyingSettings.ToString(CultureInfo.InvariantCulture);
            text += System.Environment.NewLine;
            text += System.Environment.NewLine;
            text += "* Recent Images: " + System.Environment.NewLine;
            num = 0;
            foreach (String imagePath in settings.ImageHistory)
            {
                num++;
                text += "\t#" + String.Format(CultureInfo.InvariantCulture, "{0}", num.ToString("D2", CultureInfo.InvariantCulture)) + ": " + imagePath + System.Environment.NewLine;
            }

            frmShowText showInfo = new frmShowText(this)
            {
                Text = "LAWC Technical Information"
            };
            showInfo.lblDetails.Text = "This information will show you about how LAWC is working, and from where. \n"
                + "This information may be useful if you have a problem with LAWC.\n ";
            showInfo.lblHeading.Text = "LAWC Technical Information";
            showInfo.richText.Text = text;
            showInfo.btnCancel.Visible = false;
            showInfo.Width += 200;
            showInfo.ShowDialog();
            DialogResult result = showInfo.DialogResult;

        }

        internal ScreenState GetScreenState(DateTime vDateTime)
        {
            UpdateTimes(); // update the settings time to be the current day, but the same times

            DateTime timeStartLightening = this.settings.LightSunriseTime;
            DateTime timeEndLightening = timeStartLightening.AddMinutes(this.settings.DurationMins);
            DateTime timeStartDimming = this.settings.DarkSunsetTime;
            DateTime timeEndDimming = timeStartDimming.AddMinutes(this.settings.DurationMins);

            if (vDateTime < timeStartLightening) // before the start of the lightening time
            {
                tslblScreenMode.Text = "Screen Mode: Dark";
                return ScreenState.Dark;
            }
            else if (vDateTime >= timeStartLightening && vDateTime <= timeEndLightening) // 
            {
                tslblScreenMode.Text = "Screen Mode: Getting Lighter " + ((int)(GetPercentThroughChange() * 100)).ToString(CultureInfo.InvariantCulture) + "%";
                return ScreenState.GettingLighter;
            }
            else if (vDateTime >= timeEndLightening && vDateTime <= timeStartDimming) // 
            {
                tslblScreenMode.Text = "Screen Mode: Light";
                return ScreenState.Light;
            }
            else if (vDateTime >= timeStartDimming && vDateTime <= timeEndDimming) // 
            {
                tslblScreenMode.Text = "Screen Mode: Getting Darker " + ((int)(GetPercentThroughChange() * 100)).ToString(CultureInfo.InvariantCulture) + "%";
                return ScreenState.GettingDarker;
            }
            else if (vDateTime > timeEndDimming) // after darkening
            {
                tslblScreenMode.Text = "Screen Mode: Dark";
                return ScreenState.Dark;
            }
            else
            {
                MessageBox.Show("Doh! line 5492");
                return ScreenState.Light;
            }
        }


        private void NotifyIconMouseDoubleClick()
        {

            if (olvImages.GetItemCount() > 0)
            {
                ChangeWallpaperNow(true, false);
            }
            else
            {
                // NO IMAGES
                MessageBox.Show(GetImagesAvailableMessage(), "No Images Available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        internal void DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (settings.AdjustInterfaceColour == true)
            {
                using (Brush b = new SolidBrush(colourDarker))
                {

                    e.Graphics.FillRectangle(b, e.Bounds);

                    using (StringFormat sf = new StringFormat())
                    {
                        //Set the brush color.
                        using (SolidBrush brush = new SolidBrush(colourLightest)) 
                        {
                            //Draw the text.
                            e.Graphics.DrawString(e.Header.Text, e.Font, brush, e.Bounds, sf);
                        }
                    }
                }
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.ControlDark, e.Bounds);
                e.DrawText();
            }

        }

        internal void DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (((ListView)sender).Name == "olvImages")
            {
                //((DrawListViewSubItemEventArgs)e).Item.SubItems[1].BackColor = Color.Red;
            }

            if (settings.AdjustInterfaceColour == false)
            {
                if ((e.ItemState & ListViewItemStates.Selected) == 0)
                {
                    // NOT highlighted
                    e.DrawBackground();
                    e.DrawText();

                }
                else
                {
                    // highlighted
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    e.Graphics.DrawString(e.SubItem.Text, olvImages.Font, SystemBrushes.HighlightText, e.Bounds);
                }

                return;
            }

            TextFormatFlags flags = TextFormatFlags.Left;

            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default 
                // to Left if it has not been set to Center or Right. 
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        flags = TextFormatFlags.HorizontalCenter;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        flags = TextFormatFlags.Right;
                        break;
                }

                // Unless the item is selected, draw the standard  
                // background to make it stand out from the gradient. 
                if ((e.ItemState & ListViewItemStates.Selected) == 0)
                {
                    e.DrawBackground();
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    //e.Graphics.DrawString(e.Item.Text, olvImages.Font, SystemBrushes.HighlightText, e.Bounds);

                    // Draw the subitem text in red to highlight it. 
                    e.Graphics.DrawString(e.SubItem.Text,
                        olvImages.Font, Brushes.Red, e.Bounds, sf);
                    return;

                }

                // Draw normal text for a subitem with a nonnegative  
                // or nonnumerical value.
                e.DrawText(flags);

            }

        }

        internal void SaveSettings(String vSettingsFullPath, Setting vSetting)
        {
            Setting.SaveSettings(vSettingsFullPath, vSetting);
            Properties.Settings.Default.Save();
            setRestoreBackupState();
        }


        internal static void setListsNoEntriesLabels()
        {
            //do nothing
        }


        private void doResizeForm(Boolean vToggleWindowState)
        {
            setListsNoEntriesLabels();

            if (this.applyingSettings == false)
            {
                if (vToggleWindowState)
                {
                    // Toggle
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        SetFormNormal();
                    }
                    else if (this.WindowState == FormWindowState.Normal)
                    {
                        SetFormMinimized();
                    }
                }
                else
                {
                    //NON TOGGLE
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        SetFormMinimized();
                    }
                    else if (this.WindowState == FormWindowState.Normal)
                    {
                        SetFormNormal(); 
                    }
                }
            }
        }


        internal Boolean CheckAutoRunState()
        {

            Boolean output;
            RegistryKey rkApp;

            rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //this.GetType().Assembly.GetName().Name.ToString(CultureInfo.InvariantCulture) == APP NAME
            if (rkApp.GetValue(this.GetType().Assembly.GetName().Name.ToString(CultureInfo.InvariantCulture)) == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                output = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                output = true;

            }

            rkApp.Close();
            return output;
        }


        internal void ShowChangeWallpaperWorking(Boolean vShowMessage)
        {
            if (btnWallpaperChange.Disposing) return;

            if (!applyingSettings && vShowMessage)
                ShowWorkingMessage();

            try
            {
                btnWallpaperChange.Invoke((Action)delegate
                {
                    btnWallpaperChange.Enabled = false;
                    changeWallpaperNowToolStripMenuItem.Enabled = false;
                });                
            }
            catch (Exception)
            {
                // do nothing
            }


            SetIconWorking();
            //try
            //{
            //    this.Invoke((Action)delegate
            //    {
            //        notifyIcon1.Icon = new System.Drawing.Icon(Application.StartupPath + @"\Images\LAWCWorking.ico");
            //    });
            //}
            //catch (Exception)
            //{
            //    //Do Nothing
            //}

            Application.DoEvents();

        }


        internal void SetIconWorking()
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    notifyIcon1.Icon = new System.Drawing.Icon(Application.StartupPath + @"\Images\LAWCWorking.ico");
                });
            }
            catch (Exception)
            {
                //Do Nothing
            }
        }

        internal void SetIconReady()
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    notifyIcon1.Icon = new System.Drawing.Icon(Application.StartupPath + @"\Images\LAWC.ico");
                });
            }
            catch (Exception)
            {
                //Do Nothing
            }
        }


            internal void HideChangeWallpaperWorking()
        {
            // if we arent quitting the app 
            //if (btnWallpaperChange.IsDisposed == false)
            if (formClosing == false)
            {
                HideWorkingMessage();
                
                btnWallpaperChange.Invoke((Action)delegate
                {
                    btnWallpaperChange.Enabled = true;
                });

                btnWallpaperChange.Invoke((Action)delegate
                {
                    changeWallpaperToolStripMenuItem.Enabled = true;
                    changeWallpaperNowToolStripMenuItem.Enabled = true;
                });

                //notifyIcon1.Icon = new System.Drawing.Icon(Application.StartupPath + @"\Images\LAWC.ico");
                //SetIconReady();
            }

            SetIconReady();

        }

        internal void SetPreviewImages(String vFilePath)
        {
            Bitmap preview;

            if (!string.IsNullOrEmpty(vFilePath))
            {
                preview = (Bitmap)ImageFunctions.LoadImage(vFilePath, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            else
            {
                if (!string.IsNullOrEmpty(settings.ImageLastSelected))
                {
                    preview = (Bitmap)ImageFunctions.LoadImage(settings.ImageLastSelected, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
                else
                {
                    preview = (Bitmap)ImageFunctions.LoadImage(GetSampleImagePath(), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }

            // if the file doesnt exist preview will be null
            if (preview == null)
            {
                return;
            }

            Rectangle destBounds = new Rectangle(-1, -1, pbPreviewImage.Width + 1, pbPreviewImage.Height + 1); // extra pixels are to account for rounding differences in the aspect ratio calculation

            ImageFunctions.MaintainAspectRatioRectangle(preview, ref destBounds, settings.ImageSizeScalePercent / 100f, 1);

            // duplicate the preview, and add the blur
            if (settings.BlurImageEdges)
            {
                Bitmap blurredCopy = new Bitmap(preview);
                ImageFunctions.BlurFast(ref blurredCopy, GetBlurValue(settings.BlurAmount));
                preview = ImageFunctions.Superimpose(blurredCopy, preview);
                blurredCopy.Dispose();
            }

            // set image previews here
            this.pbPreviewImage.BackColor = GetCurrentColour();
            this.pbPreviewImage.Image = (Image)preview.Clone();

            this.frmSettingsAdvanced.SetPreviewBMP(preview); // cloning done in the function

            preview.Dispose();


        }


        private void setRestoreBackupState()
        {
            string LatestBackup = getLastBackupFilename();
            if (string.IsNullOrEmpty(LatestBackup))
            {
                restoreLastBackupToolStripMenuItem.Enabled = false;
            }
            else
            {
                restoreLastBackupToolStripMenuItem.Enabled = true;
            }
        }

        internal void SetView(Boolean vFirstRun, Boolean vDoSelectCurrent)
        {
            applyingSettings = vFirstRun;

            setRestoreBackupState();

            SetInterfaceColour();

            DrawFolderList();
            DrawImageList(settings.ImageSortColumnNum);

            this.frmSettingsAdvanced.SetView();

            if (vDoSelectCurrent) SelectCurrentWallpaper(true); //true
            ScrollToSelectedImageItem(settings.ImageLastSelected, true, true);
            applyingSettings = vFirstRun;

            if (vFirstRun)
            {
                InitWallpaperFilenames();
            }

            if (settings.ChangeOnStartup == true && vFirstRun)// )
            {
                // change on startup
                ChangeWallpaperNow(true, false);
            }

            SetTimeToGoText(); // set the text for how long till next wallpaper change

            olvImages.Columns[settings.ImageSortColumnNum].Text = olvImages.Columns[settings.ImageSortColumnNum].Text;

        }

        internal void SetNextWallpaperAdjustTime()
        {
            tmrAdjust.Interval = settings.ImageAdjustFrequencySecs * 1000;
        }

        internal void SetNextWallpaperChangeTime()
        {
            if (applyingSettings && settings.NextWallpaperChange > DateTime.Now)
            {
                //Do nothing
                //settings.NextWallpaperChange = ; 
            }
            else
            {
                settings.NextWallpaperChange = DateTime.Now.AddMinutes(this.settings.WallpaperChangeFrequencyMins);
            }

        }


        private void adjustMyObjectListViewHeader()
        {
            foreach (OLVColumn item in olvFolders.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(colourDarker);
                headerstyle.SetForeColor(colourLightest);
                item.HeaderFormatStyle = headerstyle;
            }

            foreach (OLVColumn item in olvImages.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(colourDarker);
                headerstyle.SetForeColor(colourLightest);
                item.HeaderFormatStyle = headerstyle;
            }
        }

        internal void DrawFolderList()
        {

            settings.setFoldersImageCounts();

            adjustMyObjectListViewHeader();

            olvFolders.CheckBoxes = false;

            settings.Folders.Sort(FolderInfo.CompareByPath);

            this.olvFolders.SetObjects(settings.Folders);

            if (this.settings.Folders.Count == 0)
            {
                lblNoEntriesFolders.Visible = true;
            }
            else
            {
                lblNoEntriesFolders.Visible = false;
            }

        }


        internal void ShowWorkingMessage()
        {
            // cross thread function 
            //https://stackoverflow.com/questions/43741059/cross-thread-operation-not-valid-control-textbox-accessed-from-a-thread-other

            if (InvokeRequired)
            {
                lblWorkingImages.Invoke((Action)delegate
                {
                    Cursor.Current = Cursors.WaitCursor;
                    lblWorkingImages.Visible = true;
                    lblWorkingImages.Refresh();

                    this.olvImages.UseOverlays = true;
                    this.olvFolders.UseOverlays = true;
                    if (this.Visible && !applyingSettings) this.olvImages.ShowOverlays();
                    if (this.Visible && !applyingSettings) this.olvFolders.ShowOverlays();
                    olvImages.RefreshOverlays();
                    olvFolders.RefreshOverlays();

                });
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                lblWorkingImages.Visible = true;
                lblWorkingImages.BringToFront();
                lblWorkingImages.Refresh();

                this.olvImages.UseOverlays = true;
                this.olvFolders.UseOverlays = true;
                if (this.Visible && !applyingSettings) this.olvImages.ShowOverlays();
                if (this.Visible && !applyingSettings) this.olvFolders.ShowOverlays();
                olvImages.RefreshOverlays();
                olvFolders.RefreshOverlays();
            }

            if (frmSettingsAdvanced != null)
            {
                if (frmSettingsAdvanced.InvokeRequired && frmSettingsAdvanced.IsHandleCreated)
                {
                    frmSettingsAdvanced.lblWorkingImages.Invoke((Action)delegate
                    {
                        frmSettingsAdvanced.lblWorkingImages.Visible = true;
                        frmSettingsAdvanced.lblWorkingImages.Refresh();
                    });
                }
                else
                {
                    frmSettingsAdvanced.lblWorkingImages.Visible = true;
                    frmSettingsAdvanced.lblWorkingImages.Refresh();
                }

            }
        }

        internal void HideWorkingMessage()
        {
            //https://stackoverflow.com/questions/43741059/cross-thread-operation-not-valid-control-textbox-accessed-from-a-thread-other

            try
            {
                if (InvokeRequired)
                {
                    lblWorkingImages.Invoke((Action)delegate
                    {
                        lblWorkingImages.Visible = false;
                        lblWorkingImages.Refresh();

                        this.olvImages.HideOverlays();
                        this.olvFolders.HideOverlays();
                        this.olvImages.UseOverlays = false;
                        this.olvFolders.UseOverlays = false;
                        olvImages.RefreshOverlays();
                        olvFolders.RefreshOverlays();

                        Cursor.Current = Cursors.Default;

                    });
                }
                else
                {
                    lblWorkingImages.Visible = false;
                    lblWorkingImages.Refresh();

                    this.olvImages.HideOverlays();
                    this.olvFolders.HideOverlays();
                    this.olvImages.UseOverlays = false;
                    this.olvFolders.UseOverlays = false;
                    olvImages.RefreshOverlays();
                    olvFolders.RefreshOverlays();

                    Cursor.Current = Cursors.Default;
                }

                if (frmSettingsAdvanced != null)
                {
                    if (frmSettingsAdvanced.lblWorkingImages.InvokeRequired && frmSettingsAdvanced.lblWorkingImages.IsHandleCreated)
                    {
                        frmSettingsAdvanced.lblWorkingImages.Invoke((Action)delegate
                        {
                            frmSettingsAdvanced.lblWorkingImages.Visible = false;
                            frmSettingsAdvanced.lblWorkingImages.Refresh();
                        });
                    }
                    else
                    {
                        frmSettingsAdvanced.lblWorkingImages.Visible = false;
                        frmSettingsAdvanced.lblWorkingImages.Refresh();
                    }
                }
            }
            catch (System.ObjectDisposedException)
            {
                return;
            }
        }


        internal void SortImages(int vSortBy, Boolean vIsAutoRefresh)
        {
            if (vIsAutoRefresh) return; // dont sort if its just resetting

            /* sorting */
            if (vSortBy == 0)
            {
                // TITLE
                if (settings.ImageSortOrderDESC == false)
                {
                    settings.Images.Sort(ImageInfo.CompareByTitle);
                }
                else
                {
                    settings.Images.Sort(ImageInfo.CompareByTitleDESC);
                }

            }
            else if (vSortBy == 1)
            {
                //  BRIGHTNESS
                if (settings.ImageSortOrderDESC == false)
                {
                    settings.Images.Sort(ImageInfo.CompareByBrightness);
                }
                else
                {
                    settings.Images.Sort(ImageInfo.CompareByBrightnessDESC);
                }
            }
            else if (vSortBy == 2)//SORT = 2 //Viewcount
            {
                if (settings.ImageSortOrderDESC == false)
                {
                    settings.Images.Sort(ImageInfo.CompareByViewCount);
                }
                else
                {
                    settings.Images.Sort(ImageInfo.CompareByViewCountDESC);
                }
            }
            else if (vSortBy == 3)//SORT = 3 //Aspect
            {
                if (settings.ImageSortOrderDESC == false)
                {
                    settings.Images.Sort(ImageInfo.CompareByAspectRatio);
                }
                else
                {
                    settings.Images.Sort(ImageInfo.CompareByAspectRatioDESC);
                }
            }
        }

        internal interface IModelFilter
        {
            bool Filter(object modelObject);
        }


        internal void DrawImageList(int vSortBy)
        {
            Boolean useViewCount = false;

            adjustMyObjectListViewHeader();

            ChangeHotItemStyle(olvImages, 4); // set hover to translucent
            ChangeHotItemStyle(olvFolders, 4); // set hover to translucent
            this.olvImages.UseFiltering = true;
            this.olvImages.TintSortColumn = true;

            this.olvImages.ModelFilter = new ModelFilter(delegate (object x) {
                return IsImageOK((ImageInfo)x, useViewCount);
            });

            this.olvImages.SetObjects(settings.Images);
            this.olvImages.UseNotifyPropertyChanged = true;

            SortOrder order = SortOrder.Ascending;
            if (settings.ImageSortOrderDESC)
            {
                order = SortOrder.Descending;
            }
            this.olvImages.Sort(olvImages.GetColumn(vSortBy), order);

            setImageCountText(); 

            if (this.settings.Images.Count == 0)
            {
                lblNoEntriesImages.Visible = true;
                pnlSearch.Enabled = false;
                lblSearch.Enabled = false;
                lblClear.Enabled = false;
                txtSearch.ReadOnly = true;
            }
            else
            {
                lblNoEntriesImages.Visible = false;
                pnlSearch.Enabled = true;
                lblSearch.Enabled = true;
                lblClear.Enabled = true;
                txtSearch.ReadOnly = false;
            }

            setScrollbars();
        }

        internal void setScrollbars()
        {
            // set scroll bars
            sbImagesVert.Maximum = olvImages.GetItemCount() - 1;
            if (((olvImages.RowHeightEffective + 3) * olvImages.Items.Count) > olvImages.Height) //Fixed Value = 4 for padding of rows
            {
                sbImagesVert.Visible = true;
            } else
            {
                sbImagesVert.Visible = false;
            }

        }

        private void SetTimeToGoText()
        {
            if (olvImages.GetItemCount() > 0)
            {
                // finally set the text for when wallpaper changes next
                TimeSpan toGo = settings.NextWallpaperChange - DateTime.Now;
                tslblTimeToNextChange.Text = "Change in " + String.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}",
                    Math.Abs(toGo.Hours).ToString("D2", CultureInfo.InvariantCulture),
                    Math.Abs(toGo.Minutes).ToString("D2", CultureInfo.InvariantCulture),
                    Math.Abs(toGo.Seconds).ToString("D2", CultureInfo.InvariantCulture) + "\t");
            }
            else
            {
                tslblTimeToNextChange.Text = "Change Disabled. No Images." + "\t";
            }
        }

        internal System.Drawing.Color GetCurrentColour()
        {
            UpdateScreenState();

            float percent = GetPercentThroughChange();

            if (percent < 0) { percent = 0; }
            if (percent > 1) { percent = 1; }

            if (this.settings.AdjustmentsUseAlways == false && settings.DarkMode == false)
            {
                if (screenStateCurrent == ScreenState.Light)
                {
                    return settings.BackgroundColourLight;
                }
                else if (screenStateCurrent == ScreenState.Dark)
                {
                    return settings.BackgroundColourDark;
                }
                else if (screenStateCurrent == ScreenState.GettingDarker)
                {                    
                    return CalcCurrentColour(settings.BackgroundColourLight, settings.BackgroundColourDark, percent);
                }
                else if (screenStateCurrent == ScreenState.GettingLighter)
                {
                     return CalcCurrentColour(settings.BackgroundColourDark, settings.BackgroundColourLight, percent);
                }
            }
            else
            {
                // ALWAYS USE FULL ADJUSTMENTS
                return CalcCurrentColour(settings.BackgroundColourDark, settings.BackgroundColourLight, 0.0f);
            }
            return settings.BackgroundColourLight;
        }

        internal static System.Drawing.Color CalcCurrentColour(Color vColourFrom, Color vColourTo, float vPercent)
        {
            int r = vColourFrom.R;
            int g = vColourFrom.G;
            int b = vColourFrom.B;

            int diffRed = vColourTo.R - vColourFrom.R;
            int diffGreen = vColourTo.G - vColourFrom.G;
            int diffBlue = vColourTo.B - vColourFrom.B;

            r += (int)(diffRed * vPercent);
            g += (int)(diffGreen * vPercent);
            b += (int)(diffBlue * vPercent);

            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;

            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;

            return Color.FromArgb(255, r, g, b);
        }

        //internal Color GetCurrentInterfaceColour(InterfaceColours vType, ScreenState vScreenState)//, float vPercent)
        //{
        //    float factor = 1.0f;

        //    Color currentCol = GetCurrentColour();

        //    if (settings.AdjustInterfaceColour
        //        &&
        //        (vScreenState == ScreenState.Light
        //        || (vScreenState == ScreenState.GettingDarker)
        //        ))
        //    {
        //        currentCol = settings.BackgroundColourLight;
        //        if (vType == InterfaceColours.none) factor = 1.0f;
        //        if (vType == InterfaceColours.darkest) factor = 0.6f;
        //        if (vType == InterfaceColours.darker) factor = 0.75f;
        //        if (vType == InterfaceColours.dark) factor = 0.9f;
        //        if (vType == InterfaceColours.medium) factor = 1.0f;
        //        if (vType == InterfaceColours.lighter) factor = 1.2f;
        //        if (vType == InterfaceColours.lightest) factor = 1.4f;
        //    }

        //    if (vScreenState == ScreenState.Dark
        //        || (vScreenState == ScreenState.GettingLighter)
        //        )
        //    {
        //        currentCol = settings.BackgroundColourDark;
        //        if (vType == InterfaceColours.none) factor = 1.0f;
        //        if (vType == InterfaceColours.darkest) factor = 2.0f;
        //        if (vType == InterfaceColours.darker) factor = 3f;
        //        if (vType == InterfaceColours.dark) factor = 5f;
        //        if (vType == InterfaceColours.medium) factor = 6f;
        //        if (vType == InterfaceColours.lighter) factor = 7f;
        //        if (vType == InterfaceColours.lightest) factor = 14.0f;
        //    }

        //    int r;
        //    int g;
        //    int b;

        //    r = (int)(currentCol.R * (factor));
        //    g = (int)(currentCol.G * (factor));
        //    b = (int)(currentCol.B * (factor));

        //    if (r > 255) r = 255;
        //    if (g > 255) g = 255;
        //    if (b > 255) b = 255;

        //    if (r < 0) r = 0;
        //    if (g < 0) g = 0;
        //    if (b < 0) b = 0;

        //    return Color.FromArgb(255, r, g, b);

        //}

        internal void SetInterfaceColour()
        {
            // set the Taskbar color
            if (this.settings.AdjustTaskbarColour)
            {
                Taskbar.ColorizationColor = GetCurrentColour(); 
            }

            Boolean darkMode = ScreenFunctions.isWinDarkModeEnabled();

            if (this.settings.AdjustInterfaceColour)
            {
                if (darkMode)
                {
                    colourDarkest = Color.FromArgb(255, 35, 35, 35);   
                    colourDarker = Color.FromArgb(255, 49, 49, 49); 
                    colourDark = Color.FromArgb(255, 60, 60, 60);  
                    colourMedium = Color.FromArgb(255, 132, 132, 132); 
                    colourLighter = Color.FromArgb(255, 176, 176, 176);  
                    colourLightest = Color.FromArgb(255, 250, 250, 250);  
                }
                else
                {
                    // LIGHT MODE
                    colourDarkest = Color.White; 
                    colourDarker = Color.LightGray; 
                    colourDark = Color.DarkGray; 
                    colourMedium = Color.Gray; 
                    colourLighter = Color.DimGray; 
                    colourLightest = Color.Black; 
                }

                colourAlert = Color.FromArgb(255, 255, 192, 192);

                lblWorkingImages.BackColor = colourAlert;
                frmSettingsAdvanced.lblWorkingImages.BackColor = colourAlert;

                menuStrip1.BackColor = colourDarkest;
                menuStrip1.ForeColor = colourLightest;
                foreach (ToolStripMenuItem toolItem in GetMenuStripItems(menuStrip1))
                {
                    toolItem.BackColor = colourDarkest;
                    toolItem.ForeColor = colourLightest;
                }

                cmNotifyIcon.BackColor = colourDark;
                cmNotifyIcon.ForeColor = colourLightest;
                foreach (ToolStripMenuItem toolItem in GetMenuStripItems()) 
                {
                    toolItem.BackColor = colourDarker;
                    toolItem.ForeColor = colourLightest;
                }


                cmFolders.BackColor = colourDarker;
                cmFolders.ForeColor = colourLightest;
                toolStripMenuItem4.ForeColour = colourLightest;
                toolStripMenuItem8.ForeColour = colourLightest;

                cmImages.BackColor = colourDarker;
                cmImages.ForeColor = colourLightest;
                toolStripMenuItem1.ForeColour = colourLightest;
                toolStripMenuItem3.ForeColour = colourLightest;
                toolStripMenuItem14.ForeColour = colourLightest;
                extendedToolStripSeparator2.ForeColor = colourLightest;
                toolStripMenuItem6.ForeColour = colourLightest;
                extendedToolStripSeparator3.ForeColour = colourLightest;

                cmPreviewImage.BackColor = colourDarker;
                cmPreviewImage.ForeColor = colourLightest;
                toolStripMenuItem5.ForeColor = colourLightest;

                //cmNotify
                toolStripMenuItem2.ForeColour = colourLightest;
                toolStripMenuItem13.ForeColour = colourLightest;
                toolStripMenuItem11.ForeColour = colourLightest;
                extendedToolStripSeparator2.ForeColour = colourLightest;
                extendedToolStripSeparator3.ForeColor = colourLightest;
                toolStripMenuItem6.ForeColour = colourLightest;

                //File Menu fileToolStripMenuItem
                toolStripMenuItem10.BackColour = colourDarkest;
                toolStripMenuItem6.BackColour = colourDarkest;
                toolStripMenuItem7.BackColour = colourDarkest;
                toolStripMenuItem9.BackColour = colourDarkest;
                toolStripMenuItem15.BackColour = colourDarkest;
                toolStripMenuItem10.ForeColour = colourLightest;
                toolStripMenuItem6.ForeColour = colourLightest;
                toolStripMenuItem7.ForeColour = colourLightest;
                toolStripMenuItem9.ForeColour = colourLightest;
                toolStripMenuItem15.ForeColour = colourLightest;
                extendedToolStripSeparator3.BackColor = colourDarkest;
                extendedToolStripSeparator3.ForeColour = colourLightest;

                foreach (ToolStripItem toolItem in GetMenuStripItems()) 
                {
                    toolItem.BackColor = colourDarkest;
                    toolItem.ForeColor = colourLightest;
                }

                statusStrip1.BackColor = colourDarkest;
                statusStrip1.ForeColor = colourLightest;

                olvFolders.BackColor = colourDarkest; 
                olvImages.BackColor = colourDarkest;
                olvFolders.ForeColor = colourLightest;
                olvImages.ForeColor = colourLightest;

                olvFolders.AlternateRowBackColor = colourDarker;
                olvImages.AlternateRowBackColor = colourDarker;

                txtSearch.BackColor = colourDarker;
                txtSearch.ForeColor = colourLightest;
                pnlSearch.BackColor = colourDark; 
                pnlSearch.ForeColor = Color.Black;
                lblClear.ForeColor = colourLightest;
                lblSearch.ForeColor = colourLightest;

                sbImagesVert.BackColor = colourDarkest;
                sbImagesVert.BorderColour = colourMedium;
                sbImagesVert.ThumbColour = colourDark;
                sbImagesVert.TrackColour = Color.MediumPurple;  // click on track 
                sbImagesVert.ArrowColour = colourDark;
                sbImagesVert.HoverColour = System.Drawing.Color.Silver;
                sbImagesVert.ShowGrip = false;


                // FORMS =============================
                this.BackColor = colourDarker;
                this.ForeColor = colourLightest;

                foreach (Control c in this.frmSettings.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }
                this.frmSettings.BackColor = colourDarker;
                this.frmSettings.ForeColor = colourLightest;


                foreach (Control c in this.frmSettingsAdvanced.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }
                this.frmSettingsAdvanced.BackColor = colourDarker;
                this.frmSettingsAdvanced.ForeColor = colourLightest;

                foreach (Control c in this.frmSettingsAdvanced.tabSettings.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }

                foreach (TabPage tp in frmSettingsAdvanced.tabSettings.TabPages)
                {
                    var controls = tp.Controls;

                    tp.BackColor = colourDarker;
                    tp.ForeColor = colourLightest;

                    foreach (Control control in controls)
                    {

                        control.BackColor = colourDarker;
                        control.ForeColor = colourLightest;

                    }
                }
                frmWebsites.olvWebsites.UseAlternatingBackColors = true;
                frmWebsites.olvWebsites.AlternateRowBackColor = colourDarker;
                foreach (Control c in this.frmWebsites.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }
                this.frmWebsites.BackColor = colourDarker;
                this.frmWebsites.ForeColor = colourLightest;
                foreach (Control c in this.frmAbout.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }
                this.frmAbout.BackColor = colourDarker;
                this.frmAbout.ForeColor = colourLightest;

                if (darkMode)
                {
                    // dont change the SPLASH SCREEN colour if its light mode - keeps the splash background as sky blue
                    foreach (Control c in this.splashScreen.Controls)
                    {
                        c.BackColor = colourDarkest;
                        c.ForeColor = colourLightest;
                    }

                    this.splashScreen.BackColor = colourDarkest;
                    this.splashScreen.ForeColor = colourLightest;
                }

                foreach (Control c in this.frmMetaData.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }
                this.frmMetaData.BackColor = colourDarkest;
                this.frmMetaData.ForeColor = colourLightest;

                foreach (Control c in this.frmThanks.Controls)
                {
                    c.BackColor = colourDarker;
                    c.ForeColor = colourLightest;
                }
                this.frmThanks.BackColor = colourDarker;
                this.frmThanks.ForeColor = colourLightest;

                btnSettings.FlatStyle = FlatStyle.Flat;
                btnSettings.BackColor = colourDarker;
                btnSettings.FlatAppearance.BorderColor = colourDarkest;
                btnSettings.ForeColor = colourLightest;

                btnAdvancedSettings.FlatStyle = FlatStyle.Flat;
                btnAdvancedSettings.BackColor = colourDarker;
                btnAdvancedSettings.FlatAppearance.BorderColor = colourDarkest;
                btnAdvancedSettings.ForeColor = colourLightest;

                btnWallpaperChange.FlatStyle = FlatStyle.Flat;
                btnWallpaperChange.BackColor = colourDarker;
                btnWallpaperChange.FlatAppearance.BorderColor = colourDarkest;
                btnWallpaperChange.ForeColor = colourLightest;

                btnWebsites.FlatStyle = FlatStyle.Flat;
                btnWebsites.BackColor = colourDarker;
                btnWebsites.FlatAppearance.BorderColor = colourDarkest;
                btnWebsites.ForeColor = colourLightest;

                lblDonate.BackColor = colourLightest;

                frmSettingsAdvanced.pnlBackgroundColourDark.BackColor = this.settings.BackgroundColourDark;
                frmSettingsAdvanced.pnlBackgroundColourLight.BackColor = this.settings.BackgroundColourLight;

                frmSettings.pnlBackgroundColourDark.BackColor = this.settings.BackgroundColourDark;
                frmSettings.pnlBackgroundColourLight.BackColor = this.settings.BackgroundColourLight;

            }

        }


        private static string getLastBackupFilename()
        {
            String settingsFullPath = Setting.getSettingsFullPath(Properties.Settings.Default.Portable);

            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(settingsFullPath);
            long seconds = 99999;
            String foundFilename = string.Empty;

            if (dirInfo.Exists == false) return string.Empty;

            foreach (FileSystemInfo fileObject in dirInfo.GetFileSystemInfos())
            {
                if (fileObject.Attributes != FileAttributes.Directory)
                {
                    if (fileObject.FullName.Contains("Settings_BAK"))
                    {
                        if (seconds > (long)DateTime.Now.Subtract(fileObject.LastWriteTime).TotalSeconds)
                        {
                            seconds = (long)DateTime.Now.Subtract(fileObject.LastWriteTime).TotalSeconds;
                            foundFilename = fileObject.FullName;
                        }
                    }
                }
            }

            return foundFilename;
        }


        //https://stackoverflow.com/questions/22158278/wait-some-seconds-without-blocking-ui-execution
        async Task PutTaskDelay(int vSeconds)
        {
            await Task.Delay(vSeconds * 1000).ConfigureAwait(false);
        }

        private void DoWorkerThreadChange(String vFolder, int vCount)
        {
            try
            {
                if (vCount > this.pbarStatus.Maximum) this.pbarStatus.Maximum = vCount;

                this.pbarStatus.Value = vCount;

                DirectoryInfo folderInfo = new DirectoryInfo(vFolder);

                lblStatus.Text = folderInfo.Parent.Name + "\\" + folderInfo.Name + ": Reading file #" + vCount + " of " + this.pbarStatus.Maximum;
                
            }
            catch (IOException ex)
            {
                ProcessError(ex, ErrorMessageType.ThreadUpdate, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
        }


        internal void startWebsiteImport()
        {
            frmWebsites.importWebsites(MainFunctions.GetAppFullPath(Properties.Settings.Default.Portable) + "WallpaperWebsites.xml");

            SaveSettings(string.Empty, this.settings);
        }

        internal void StartSetupWizard()
        {
            try
            {
                if (MessageBox.Show("Welcome to the Light Adjusting Wallpaper Changer (LAWC)." + System.Environment.NewLine + System.Environment.NewLine
                    + "Would you like to run the setup Wizard?", "Start Setup Wizard for LAWC", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    wizardHost = new WizardHost
                    {
                        Text = "LAWC Setup Wizard",
                        Icon = new Icon(@".\Images\LAWC.ico"),
                    };
                    wizardHost.WizardCompleted += new WizardHost.WizardCompletedEventHandler(Host_WizardCompleted);
                    wizardHost.WizardPages.Add(1, new Page1(this));
                    wizardHost.WizardPages.Add(2, new Page2(this));
                    wizardHost.WizardPages.Add(3, new Page3(this));
                    wizardHost.WizardPages.Add(4, new Page4());
                    wizardHost.LoadWizard();
                    wizardHost.ShowDialog();
                }
                else
                {
                    // NO to running the wizard
                    settings.FirstRunDone = true;
                    SaveSettings(string.Empty, this.settings);

                }

            }
            catch (ApplicationException ex)
            {
                ProcessError(ex, ErrorMessageType.WizardStart, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

        }

        private Boolean CheckImagesAvailability(Boolean vShowMessage)
        {
            if (ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode) > olvImages.GetItemCount())
            {                
                if (vShowMessage) MessageBox.Show("There is a problem.  You only have " + olvImages.GetItemCount().ToString(CultureInfo.InvariantCulture) + " images available, and " + ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode).ToString(CultureInfo.InvariantCulture) + " screens." + System.Environment.NewLine
                    + "Please add another folder with more images.", "Not Enough Images!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Boolean anyEnabled = false;
            for (int i = 0; i < settings.Folders.Count; i++)
            {
                if (settings.Folders[i].Enabled == true) anyEnabled = true;
            }

            if (anyEnabled == false && settings.Folders.Count > 0)
            {
                if (vShowMessage) MessageBox.Show("There is a problem.  You do not have any folders enabled." + System.Environment.NewLine
                    + "Please Enable one of the folders, or add another folder with more images.", "Not Enough Images!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            return true;
        }

        private void ClearPreviewImageMenu()
        {
            for (int i = cmPreviewImage.Items.Count - 1; i > 0; i--) // skipping Zero as its the default item that always stays
            {
                if (cmPreviewImage.Items[i].Text.Contains(cWallpaperOnScreen))
                {
                    cmPreviewImage.Items.RemoveAt(i);
                }
            }
        }

        private void ClearNotifyIconMenu()
        {
            showWallpaperToolStripMenuItem.DropDownItems.Clear();
            openImageToolStripMenuItem.DropDownItems.Clear();
            deleteImageToolStripMenuItem.DropDownItems.Clear();
        }

        /// <summary>
        /// Determine the overall bounds for all monitors together and create a single Bitmap
        /// </summary>
        internal void UpdateMonitorInfo()
        {
            try
            {
                // Where is 0,0 in the composite Desktop image?
                Point refPoint;
                Rectangle overallBounds = new Rectangle();

                refPoint = new Point();

                ClearPreviewImageMenu();
                ClearNotifyIconMenu();

                int count = 0;

                // sort the screens from right to left, top to bottom
                Array.Sort(Screen.AllScreens, delegate (Screen x, Screen y) {
                    return x.Bounds.X.CompareTo(y.Bounds.X) + x.Bounds.Y.CompareTo(y.Bounds.Y);
                });

                this.openImageToolStripMenuItem.DropDownItems.Clear();
                this.showWallpaperToolStripMenuItem.DropDownItems.Clear();
                this.deleteImageToolStripMenuItem.DropDownItems.Clear();
                this.selectCurrentWallpaperToolStripMenuItem.DropDownItems.Clear();

                for (int i = 0; i < ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode); i++)
                {
                    overallBounds = System.Drawing.Rectangle.Union(overallBounds, Screen.AllScreens[0].Bounds);

                    // add menu options
                    cmPreviewImage.Items.Add("Wallpaper on Screen " + count);

                    //***** MENU: Add Open File ****                    
                    ToolStripMenuItem newItem = new System.Windows.Forms.ToolStripMenuItem
                    {
                        Text = cWallpaperOnScreen + " " + count
                    };
                    newItem.Click += NewItem_Click;

                    // add the new to the menu
                    this.openImageToolStripMenuItem.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[]
                        { newItem });


                    //***** MENU: Add Open in Explorer ****                    
                    ToolStripMenuItem newItemExp = new System.Windows.Forms.ToolStripMenuItem
                    {
                        Text = cWallpaperOnScreen + " " + count
                    };
                    newItemExp.Click += NewMenuItemExp_Click;

                    // add the new to the menu
                    this.showWallpaperToolStripMenuItem.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[]
                        { newItemExp });


                    //***** MENU: Add Delete in Explorer ****                    
                    ToolStripMenuItem newItemDel = new System.Windows.Forms.ToolStripMenuItem
                    {
                        Text = cWallpaperOnScreen + " " + count
                    };
                    newItemDel.Click += NewMenuItemDel_Click;

                    // add the new to the menu
                    this.deleteImageToolStripMenuItem.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[]
                        { newItemDel });




                    //***** MENU: Add Select Wallpaper to Images ****                    
                    ToolStripMenuItem selectCurrent = new System.Windows.Forms.ToolStripMenuItem
                    {
                        Text = cWallpaperOnScreen + " " + count
                    };
                    selectCurrent.Click += SelectCurrent_Click;

                    // add the new to the menu
                    this.selectCurrentWallpaperToolStripMenuItem.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[]
                        { selectCurrent });

                    
                    count++;

                }

                // Screens to the left or above the primary screen cause 0,0 to be other
                // than the top/left corner of the Bitmap
                if (overallBounds.X < 0) refPoint.X = Math.Abs(overallBounds.X);
                if (overallBounds.Y < 0) refPoint.Y = Math.Abs(overallBounds.Y);


            }
            catch (ApplicationException ex)
            {
                ProcessError(ex, ErrorMessageType.UpdatingMonitorInfo, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
        }


        private void SelectCurrent_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            if (item.Text.Contains(cWallpaperOnScreen))
            {
                string screenIDText = new string(item.Text.Where(char.IsDigit).ToArray());

                Boolean result = int.TryParse(screenIDText, out int screenID);

                settings.ImageLastSelected = wallpaperFilenames[screenID];
                SelectCurrentWallpaper(false); //false
                ScrollToSelectedImageItem(settings.ImageLastSelected, true, true);
            }

        }


        #endregion


        #region Handlers Actions

        private void TimerStartWizard_Tick(object sender, EventArgs e)
        {
            // only run this once on first startup
            ((System.Windows.Forms.Timer)sender).Stop();
            CheckStartWizard();
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            MainFormLoad(Properties.Settings.Default.SettingsFilePath);
        }


        private void OlvFolders_FormatRow(object sender, FormatRowEventArgs e)
        {
            FolderInfo folder = (FolderInfo)e.Model;
            if (folder.Enabled == false)
                e.Item.ForeColor = colourMedium;
        }


        private void olvImages_MouseUp(object sender, MouseEventArgs e)
        {

            // first click in rename
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (RenameFirstClickStart == MainFunctions.DateNull || renameRowNum != olvImages.SelectedIndex)
                {
                    RenameFirstClickStart = DateTime.Now;
                    renameRowNum = olvImages.SelectedIndex;
                }
            }

            // show context menu
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //reset the renaming
                RenameFirstClickStart = MainFunctions.DateNull;
                renameRowNum = -1;

                openFileLocationToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                removeToolStripMenuItem.Enabled = false;
                setAsWallpaperToolStripMenuItem.Enabled = false;
                selectCurrentWallpaperToolStripMenuItem.Enabled = false;
                renameToolStripMenuItem.Enabled = false;
                viewMetadataToolStripMenuItem.Enabled = false;
                resetViewCountToolStripMenuItem.Enabled = false;
                refreshToolStripMenuItem.Enabled = false;

                if (this.olvImages.SelectedItem != null 
                    && System.IO.File.Exists(
                        ((ImageInfo)(olvImages.SelectedItem.RowObject)).FullPath
                        ) 
                    )
                {
                    openFileLocationToolStripMenuItem.Enabled = true;
                    deleteToolStripMenuItem.Enabled = true;
                    removeToolStripMenuItem.Enabled = true;
                    setAsWallpaperToolStripMenuItem.Enabled = true;
                    selectCurrentWallpaperToolStripMenuItem.Enabled = true;
                    renameToolStripMenuItem.Enabled = true;
                    viewMetadataToolStripMenuItem.Enabled = true;
                    resetViewCountToolStripMenuItem.Enabled = true;
                    refreshToolStripMenuItem.Enabled = true;
                } 
                else if (!System.IO.File.Exists(  ((ImageInfo)olvImages.SelectedItem.RowObject).FullPath)
                    )
                {
                    SetImageNotFound(); //SetSampleImage();
                }

                this.cmImages.Show(olvImages, e.Location);

            }
        }


        private void OlvImages_CellEditStarting(object sender, CellEditEventArgs e)
        {
            // if not the filename column, or the edit timer has not elapsed yet, then cancel
            if (e.Column.AspectName != "Filename")
            {
                e.Cancel = true;
                return;
            }

            TimeSpan ts = DateTime.Now - RenameFirstClickStart;
            if (ts.TotalMilliseconds < renameDelayMS)
            {
                e.Cancel = true;
                return;
            }
            else if (renameRowNum != olvImages.SelectedIndex)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                //starting up... clear datetime
                RenameFirstClickStart = MainFunctions.DateNull;
            }

        }

        private void OlvImages_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            // PROCESS FILE RENAME
            if (e.Column.AspectName == "Filename")
            {

                if (this.olvImages.SelectedItem == null) return;

                String filenameNew = e.NewValue.ToString().Trim();
                String filenameExtNew = System.IO.Path.GetExtension(filenameNew);

                String filenameOldPath = ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath;
                String filenameOld = Path.GetFileName(filenameOldPath);
                String filenameExtOld = System.IO.Path.GetExtension(filenameOldPath);

                // if there is no extension on the new filename, add it back
                if (String.IsNullOrEmpty(filenameExtNew) 
                    && !filenameNew.ToUpper().Contains(filenameExtOld)
                    )
                {
                    filenameNew += filenameExtOld;
                }

                String filenameNewPath = filenameOldPath.Replace(filenameOld, ""); // get the path only
                filenameNewPath += filenameNew;

                // if they are the same, dont bother doing anything
                if (filenameOldPath == filenameNewPath)
                {
                    return;
                }

                // Validate the filename                
                if (filenameNew.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    MessageBox.Show("Sorry, you have entered invalid characters for the filename.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
                if (System.IO.File.Exists(filenameNewPath))
                {
                    MessageBox.Show("Sorry, this filename already exists.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                // DO THE RENAME
                String destination = RenameWallpaperFile(filenameOldPath, filenameNew);
                if (string.IsNullOrEmpty(destination))
                {
                    MessageBox.Show("Sorry, there was a problem renaming the filename. Please rescan the folder contents and try again.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
                settings.ImageLastSelected = destination;

                //refresh the entry
                int index = settings.Images.FindIndex(r => r.FullPath == destination);
                if (olvImages.GetItem(index) != null) this.olvImages.RefreshItem(olvImages.GetItem(index));

                SaveSettings(string.Empty, this.settings);
            }
            else
            {
                e.Cancel = true;
            }
        }



        private void OlvImages_Resize(object sender, EventArgs e)
        {
            setListsNoEntriesLabels();
        }

        private void olvImages_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void olvImages_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected) 
            {
                imageSelectionChanged(this.applyingSettings, false); // do this on startup, but not after
            }
        }

        private void selectionTimer_Tick(object sender, EventArgs e)
        {
            ((System.Windows.Forms.Timer)sender).Stop();
            ((System.Windows.Forms.Timer)sender).Dispose();
        }

        private void Host_WizardCompleted()
        {
            try
            {
                setSettingsFilePath(Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable));

                frmSettingsAdvanced.SetFormValues();
                frmSettings.SetFormValues();

                wizardHost.Hide();

                SetInterfaceColour();

                if (!string.IsNullOrEmpty(this.wizardInitialAddFolder))
                {
                    ShowChangeWallpaperWorking(true);
                    int newID = AddFolderToSettings(this.wizardInitialAddFolder);
                    RescanFolder(this.wizardInitialAddFolder, newID);
                }
                else
                {
                    // just in case - depending on user choices in wizard
                    HideChangeWallpaperWorking();
                }

            }
            catch (ApplicationException ex)
            {
                ProcessError(ex, ErrorMessageType.WizardFinished, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            HideChangeWallpaperWorking();
        }

        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;

            // The timer has reached the double click time limit.
            if (milliseconds >= SystemInformation.DoubleClickTime)
            {
                doubleClickTimer.Stop();

                if (isDoubleClick)
                {
                    // DOUBLE CLICK
                    NotifyIconMouseDoubleClick();
                }
                else
                {
                    // SINGLE CLICK
                    doResizeForm(true);
                }

                // Allow the MouseDown event handler to process clicks again.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }

        private void NotifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            // Verify that the mouse click is in the main hit
            // test rectangle. NOT NEEDED AS FIRING FROM BUTTON
            //if (!hitTestRectangle.Contains(e.Location))
            //{
            //    return;
            //}

            // only use left mouse button for this
            if (e.Button != MouseButtons.Left) return;

            // This is the first mouse click.
            if (isFirstClick)
            {
                isFirstClick = false;

                // Determine the location and size of the double click 
                // rectangle area to draw around the cursor point.
                doubleClickRectangle = new Rectangle(
                    e.X - (SystemInformation.DoubleClickSize.Width / 2),
                    e.Y - (SystemInformation.DoubleClickSize.Height / 2),
                    SystemInformation.DoubleClickSize.Width,
                    SystemInformation.DoubleClickSize.Height);
                Invalidate();

                // Start the double click timer.
                doubleClickTimer.Start();
            }

            // This is the second mouse click.
            else
            {
                // Verify that the mouse click is within the double click
                // rectangle and is within the system-defined double 
                // click period.
                if (doubleClickRectangle.Contains(e.Location) &&
                    milliseconds < SystemInformation.DoubleClickTime)
                {
                    isDoubleClick = true;
                }
            }
        }

        private void TmrRefresh_Tick(object sender, EventArgs e)
        {
            if (tmrRefresh.Enabled == true)
            {
                if (debugEnabled) WriteText("DEBUG: TmrRefresh Tick", string.Empty);

                // re-detect the screens as a displaysettingschanged event fired
                initDisplaySettings(false);

                this.frmSettingsAdvanced.setMultiMonitorControls();
                this.frmSettings.setMultiMonitorControls();

                AdjustDesktopImages(); // refresh the wallpaper 

                tmrRefresh.Enabled = false;
            }
        }

        private void olvImages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Delete))
            {
                DeleteSelectedImage(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);
            }
        }

        private void LvFolders_MouseHover(object sender, EventArgs e)
        {
            //olvFolders.Focus();
        }

        private void olvImages_MouseHover(object sender, EventArgs e)
        {
            //olvImages.Focus();
        }

        /// <summary>
        /// Event handled invoked when display settings are changed.  Updates
        /// the composite images automatically.
        /// </summary>
        private void FrmMain_DisplaySettingsChanged(object sender, EventArgs e)
        {

            // if the screens have NOT changed, dont bother updating
            // some res changing in windows can make this fire more than once... so wait for a bit before running
            if (tmrRefresh.Enabled == false)
            {
                tmrRefresh.Enabled = true;
            }

            // update the Last Changed time if it is past due
        }

        private void olvImages_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            DrawColumnHeader(sender, e);
        }

        private void LvFolders_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            DrawColumnHeader(sender, e);
        }

        private void olvImages_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            DrawSubItem(sender, e);
        }

        private void LvFolders_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            DrawSubItem(sender, e);
        }

        private void FrmMain_OnWorkerThreadChange(String vFolder, int vCount)
        {
            if (this.InvokeRequired)
            {
                // Reinvoke the same method if necessary        
                BeginInvoke(new MethodInvoker(delegate () { this.DoWorkerThreadChange(vFolder, vCount); }));
            }
            else
            {
                this.DoWorkerThreadChange(vFolder, vCount);
            }
        }

        private void FrmMain_OnWorkerThreadComplete()
        {
            try
            {
                ProcessFoundImages();
            }
            catch (IOException ex)
            {
                ProcessError(ex, ErrorMessageType.ProcessingFoundImages, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
             }
        }


        private void LvFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFolder();
        }

        private void LvFolders_MouseUp(object sender, MouseEventArgs e)
        {
            // show context menu
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // defaults
                addFolderToolStripMenuItem.Enabled = true;
                removeFolderToolStripMenuItem.Enabled = false;
                if (!lblWorkingImages.Visible) rescanFolderToolStripMenuItem.Enabled = false;
                if (!lblWorkingImages.Visible) rescanAllFoldersToolStripMenuItem2.Enabled = false;
                if (!lblWorkingImages.Visible) rescanAllFoldersToolStripMenuItem1.Enabled = false;
                openFolderToolStripMenuItem.Enabled = false;
                if (!lblWorkingImages.Visible) checkFoldersToolStripMenuItem.Enabled = false;
                enableToolStripMenuItem.Visible = false;
                disableToolStripMenuItem.Visible = false;
                resetViewCountToolStripMenuItem1.Enabled = false;

                // only allow enable/disable if we have more than one image folder
                if (this.settings.Folders.Count() > 0 && !lblWorkingImages.Visible) // only do this if we are not working on something - scanning folders
                {
                    if (!lblWorkingImages.Visible) checkFoldersToolStripMenuItem.Enabled = true;
                    if (!lblWorkingImages.Visible) rescanAllFoldersToolStripMenuItem2.Enabled = true;
                    if (!lblWorkingImages.Visible) rescanAllFoldersToolStripMenuItem1.Enabled = true;
                }

                if (this.olvFolders.SelectedItem != null)
                {
                    if (((FolderInfo)(this.olvFolders.SelectedItem.RowObject)).Enabled)
                    {
                        disableToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        enableToolStripMenuItem.Visible = true;
                    }

                    resetViewCountToolStripMenuItem1.Enabled = true;
                    openFolderToolStripMenuItem.Enabled = true;
                    addFolderToolStripMenuItem.Enabled = true;
                    removeFolderToolStripMenuItem.Enabled = true;
                    if (!lblWorkingImages.Visible) rescanFolderToolStripMenuItem.Enabled = true;

                    this.cmFolders.Show(olvFolders, e.Location);


                }
                else
                {
                    // show with defaults
                    this.cmFolders.Show(olvFolders, e.Location);
                }

                //if we are currently scanning a folder, then disable the appropriate controls
                if (isScanningFolder)
                {
                    rescanFolderToolStripMenuItem.Enabled = false;
                    rescanAllFoldersToolStripMenuItem1.Enabled = false;
                    addFolderToolStripMenuItem.Enabled = false;
                    addImageFolderToolStripMenuItem.Enabled = false;
                    rescanAllFoldersToolStripMenuItem2.Enabled = false;
                    checkFoldersToolStripMenuItem.Enabled = false;
                    removeFolderToolStripMenuItem.Enabled = false;

                }
            }
        }

        private void olvImages_MouseDoubleClick(object sender, EventArgs e)
        {
            FileFunctions.PlayExternalFile(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);
        }

        private void olvImages_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // if new column is different, default to Ascending, otherwise swap the sort order
            if (e.Column != this.settings.ImageSortColumnNum)
            {
                settings.ImageSortOrderDESC = false;
            }
            else
            {
                settings.ImageSortOrderDESC = !settings.ImageSortOrderDESC;
            }

            this.settings.ImageSortColumnNum = e.Column;

            SaveSettings(string.Empty, this.settings);

        }

        private void CmPreviewImage_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Text.Contains(cWallpaperOnScreen))
            {
                string screenIDText = new string(e.ClickedItem.Text.Where(char.IsDigit).ToArray());
                Boolean result = int.TryParse(screenIDText, out int screenID);
                FileFunctions.PlayExternalFile(this.wallpaperFilenames[screenID].ToString(CultureInfo.InvariantCulture));
            }
        }        

        private void PbPreviewImage_DoubleClick(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath))
            {
                FileFunctions.PlayExternalFile(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);
            }            
        }

        private void PbPreviewImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.cmPreviewImage.Show(pbPreviewImage, e.Location);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown && formClosing == false)
            {
                formClosing = true;
                Quit(true);
                return;
            }
            else
            {
                if (formClosing == false)
                {
                    formClosing = true;

                    if (MessageBox.Show("Are you sure you want to close LAWC?\n\nNote: To hide the main window, press the Minimize button on the top right.", "Close Application",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        formClosing = false;
                    }
                    else
                    {
                        Quit(true);
                    }
                }
                else
                {
                    // form is closing
                }
            }
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            setListsNoEntriesLabels();
            setScrollbars();
        }

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            // end of moving form position
            //do nothing
        }

        private void NewMenuItemDel_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            if (item.Text.Contains(cWallpaperOnScreen))
            {
                string screenIDText = new string(item.Text.Where(char.IsDigit).ToArray());

                Boolean result = int.TryParse(screenIDText, out int screenID);

                if (MessageBox.Show("Are you sure you want to delete this file?" + System.Environment.NewLine + this.wallpaperFilenames[screenID].ToString(CultureInfo.InvariantCulture) + "", "Delete Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //delete file
                    FileFunctions.DeleteFileToRecycleBin(this.wallpaperFilenames[screenID].ToString(CultureInfo.InvariantCulture));
                    //remove from list
                    this.settings.RemoveImagesByFilename(System.IO.Path.GetFileName(this.wallpaperFilenames[screenID].ToString(CultureInfo.InvariantCulture)));
                    ChangeWallpaperNow(true, false);
                }
            }
        }

        private void NewMenuItemExp_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            if (item.Text.Contains(cWallpaperOnScreen))
            {
                string screenIDText = new string(item.Text.Where(char.IsDigit).ToArray());

                Boolean result = int.TryParse(screenIDText, out int screenID);

                string argument = @"/select, " + this.wallpaperFilenames[screenID].ToString(CultureInfo.InvariantCulture);

                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
        }

        private void NewItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            if (item.Text.Contains(cWallpaperOnScreen))
            {
                string screenIDText = new string(item.Text.Where(char.IsDigit).ToArray());
                //int screenID = 0;
                Boolean result = int.TryParse(screenIDText, out int screenID);
                FileFunctions.PlayExternalFile(this.wallpaperFilenames[screenID].ToString(CultureInfo.InvariantCulture));
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit(true);
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAdvancedSettings();
        }

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        private void ChangeWallpaperNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeWallpaperNow(true, false);
        }

        private void BtnWallpaperChange_Click(object sender, EventArgs e)
        {
            if (olvImages.GetItemCount() > 0)
            {
                ChangeWallpaperNow(true, false);
            }
            else
            {
                // NO IMAGES
                MessageBox.Show(GetImagesAvailableMessage(), "No Images Available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ExitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Quit(true);
        }

        private void RestoreWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doResizeForm(true);
        }
        private void HideLAWCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doResizeForm(true);
        }

        private void OpenWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileFunctions.PlayExternalFile(settings.ImageLastSelected); //Wallpaper.GetWallpaperPath(wallpaperFileNum));
        }

        private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        private void AddFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dont allow add if already scanning... the disabled Add Folder menu item doesnt want to disable :(
            if (isScanningFolder) return;
            AddFolder(string.Empty);
            redrawLists();
        }

        private void RemoveFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveFolders();
        }

        private void RescanFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RescanSelectedFolders();
        }

        private void OpenFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string argument = @"/select, " + ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath; 

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedImage(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Remove this file?" + System.Environment.NewLine + ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath + "", "Remove Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RemoveSelectedImage();
                redrawLists();
            }

        }

        private void AddImageFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFolder(string.Empty);
            redrawLists();
        }

        private void SetAsWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedAsWallpaper(false, true);
        }

        private void TimerWallpaper_Tick(object sender, EventArgs e)
        {
            DoTimerWallpaper();
        }

        private void RunSetupWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSetupWizard();
            AddSampleEventsWebsites();
        }

        private void TslblMonitorInfo_Click(object sender, EventArgs e)
        {
            ShowMonitorInfo();
        }

        private void LblDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenDonateURL();
        }

        private void TmrWizard_Tick(object sender, EventArgs e)
        {
            //do nothing
        }

        private void EnableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.olvFolders.SelectedItem != null)
            {
                ShowChangeWallpaperWorking(true);

                SetFolderState(true, false);
                //refresh listview
                redrawLists();

                HideChangeWallpaperWorking();
            }
        }

        private void DisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.olvFolders.SelectedItem != null)
            {
                ShowChangeWallpaperWorking(true);

                SetFolderState(false, false);
                redrawLists();

                HideChangeWallpaperWorking();
            }
        }


        private void TmrAdjust_Tick(object sender, EventArgs e)
        {
            DoTimerAdjust();

            // check if we should show the new version available icon
            RefreshDownloadUpdateIcon();

        }

        private void ChangeWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeWallpaperNow(true, false);
        }

        private void RescanAllFoldersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RescanAllFolders(true);
        }

        private void CheckForUpdateToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CheckForUpdate(true);
        }

        private void ResetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = String.Format(CultureInfo.InvariantCulture, "Settings_BAK_{0}-{1}-{2} {3}-{4}.xml",
                    DateTime.Now.Year.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Month.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Day.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Hour.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Minute.ToString("D2", CultureInfo.InvariantCulture)
                    );

            
            if (MessageBox.Show("Are you sure you want to RESET YOUR SETTINGS?\n\nNote: This cannot be undone.  Your settings will be backed up to:\n" + filename, "Reset Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Backup Settings first
                SaveSettings(Setting.getSettingsFullPath(Properties.Settings.Default.Portable) + "\\" + filename, this.settings);

                resetSettings();

            }
        }

        private void resetSettings()
        {
            createForms();

            // Reset Settings
            this.settings.Reset();

            ResetPosition();

            // save new / empty settings
            SaveSettings(string.Empty, this.settings);

            SetView(false, true);

            SetSampleImage();

            StartSetupWizard();
            AddSampleEventsWebsites();

        }


        private void CheckFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFolders(true);
            redrawLists();
        }

        private void SaveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings(string.Empty, this.settings);
        }

        private void BackupSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupSettings(true);
            setRestoreBackupState();
        }

        private void TslblImageCount_Click(object sender, EventArgs e)
        {

        }


        private void TslblScreenMode_Click(object sender, EventArgs e)
        {


        }

        private String GetScreenChangeTimes()
        {
            DateTime timeStartLight = this.settings.LightSunriseTime;
            DateTime timeEndLightening = timeStartLight.AddMinutes(this.settings.DurationMins);
            DateTime timeStartDark = this.settings.DarkSunsetTime;
            DateTime timeEndDimming = timeStartDark.AddMinutes(this.settings.DurationMins);

            String text = string.Empty;

            //if the current time is after Darkening (ie. DARK), then the lightening times are tomorrow
            if (IsLightTimeOvernight())
            {
                // "IS LIGHTENING" is OVERNIGHT!!!
                timeStartLight = timeStartLight.AddDays(1);
                timeEndLightening = timeEndLightening.AddDays(1);
                text += "\t(Note: Light time is overnight)\n";
            }

            text += "\tStart Lightening: " + timeStartLight + "\n";
            text += "\tEnd Lightening: " + timeEndLightening + "\n";
            text += "\tStart Darkening: " + timeStartDark + "\n";
            text += "\tEnd Darkening: " + timeEndDimming + "\n";

            return text;

            // Extra tests for debugging:
            //text = "";
            //text += " Mode at 04:23 = " + GetScreenState(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 4, 23, 0)).ToString(CultureInfo.InvariantCulture) + "\n";
            //text += " Mode at 07:07 = " + GetScreenState(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 7, 0)).ToString(CultureInfo.InvariantCulture) + "\n";
            //text += " Mode at 13:55 = " + GetScreenState(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 55, 0)).ToString(CultureInfo.InvariantCulture) + "\n";
            //text += " Mode at 18:01 = " + GetScreenState(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 01, 0)).ToString(CultureInfo.InvariantCulture) + "\n";
            //text += " Mode at 23:33 = " + GetScreenState(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 33, 0)).ToString(CultureInfo.InvariantCulture) + "\n";


        }

        private void ShowWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //do nothing
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowChangeWallpaperWorking(true); 
            //redraw olvImages
            redrawLists();

            HideChangeWallpaperWorking();
        }

        private void PbarStatus_Click(object sender, EventArgs e)
        {
            doCancelScan();
        }
               
        private void DarkModeEnableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeCurrentMode(true);
        }

        private void DarkModeDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeCurrentMode(false);
        }

        private void SelectCurrentWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //do nothing
        }

        private void BtnWebsites_Click(object sender, EventArgs e)
        {
            ShowWebsites();
        }

        private void ResetViewCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetSelectedViewCount();
            //refresh listview
            redrawLists();
        }

        private void restoreLastBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreLastBackup();
        }

        private void viewMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.olvImages.SelectedItem != null)
            {
                ShowImageData(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);  
            }
        }

        void Hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            ChangeWallpaperNow(true, false);
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void btnAdvancedSettings_Click(object sender, EventArgs e)
        {

            ShowAdvancedSettings();
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            //Uncomment to react to clicking on the notification
            //NotifyIcon sent = (NotifyIcon)sender;
        }

        private void retryInternetConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetConnection(false);
        }

        private void guideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAppURL();
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void resetPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPosition();
        }

        private void deleteImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redrawLists();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lblClear.Enabled = (((TextBox)sender).Text.Length > 0);

            SearchFilter(this.olvImages, ((TextBox)sender).Text);
            setImageCountText();

            txtSearch.Focus();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {

        }



        private void lblClear_Click_1(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;

        }

        private void deleteWallpaperFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {
            doCancelScan();
        }

        private void deleteWallpaperFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteOldWallpaperFiles();
        }

        private void rescanAllFoldersToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RescanAllFolders(true);
        }

        private void resetAllViewcountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the View Count / Display Count for ALL Images in LAWC?", "Reset View Count",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                settings.ResetAllViewCount();
                redrawLists();
            }
        }

        private void checkFolderPathsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckFolders(true);
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {

        }

        private void wallpaperWebsitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWebsites();
        }

        private void olvFolders_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sendErrorReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayErrorForm();//, this);
        }

        private void loadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog
                {
                    Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                    InitialDirectory = settings.LastSettingsFolderPath,
                };
                DialogResult result = openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShowChangeWallpaperWorking(true);

                    setSettingsFilePath(openFileDialog1.FileName);

                    MainFormLoad(openFileDialog1.FileName);

                    settings.LastSettingsFolderPath = Path.GetDirectoryName(openFileDialog1.FileName);

                    MessageBox.Show("Settings loaded.\n\n    " + Properties.Settings.Default.SettingsFilePath  //Setting.getSettingsFullPath(Properties.Settings.Default.Portable)
                        , "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (FileLoadException ex)
            {
                MessageBox.Show("There was a problem loading the selected settings file.\nAttempting to load the default settings...", "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessError(ex, ErrorMessageType.LoadingSettings, false, false, string.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                ShowChangeWallpaperWorking(true);
                MainFormLoad(); // load the default settings
                MessageBox.Show("Settings file loaded:\n\n    " + Setting.getSettingsFullPath(Properties.Settings.Default.Portable)
                    , "Load Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            HideChangeWallpaperWorking();

        }

        private void saveSettingsAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FileName = "SettingsCopy.xml"
            };
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(saveFileDialog1.FileName.Trim()))
                {
                    SaveSettings(saveFileDialog1.FileName, this.settings);
                    setSettingsFilePath(saveFileDialog1.FileName);
                    MessageBox.Show("Settings Saved.", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void pbDownloadLatest_Click(object sender, EventArgs e)
        {
            OpenAppURL();
        }

        private void renameToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //String fileToStartWith = string.Empty;

            if (this.olvImages.SelectedItem != null)
            {
                startRenameFile(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);
            }
            else
            {
                if (settings.Images.Count > 0)
                {
                    startRenameFile(settings.Images[0].FullPath);
                }
            }
        }

        private void pbNoInternet_Click(object sender, EventArgs e)
        {
            checkInternetConnection(true);
            showNoInternetMessage();
        }

        private void ResetViewCountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetSelectedViewCount();
            //refresh listview
            redrawLists();
        }

        private void sbImagesVert_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue < 0) return;
            olvImages.EnsureVisible(e.NewValue);


        }

        private void OlvImages_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue < 0) return;
            sbImagesVert.Value = e.NewValue;
        }


        private void rescanFolderContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.olvImages.SelectedItems.Count > 0)
            if (this.olvImages.SelectedItem != null)
            {
                RescanFolderFromSelectedImage(((ImageInfo)(olvImages.SelectedItem.RowObject)).FolderID);
            }

        }

        #endregion


        #region Form actions

        internal void SetFormMinimized()
        {
            //applyingSettings = true;
            //SaveScreenPosition(); // save position before minimize because we change location below

            //this.WindowState = FormWindowState.Minimized;
            //this.frmTaskbarIcon.ShowInTaskbar = false;
            //notifyIcon1.Visible = true;
            //if (this.Visible == true)
            //this.Hide();
            //bool b = this.Visible;
            //this.Visible = false;

            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.Visible = false;
            this.ShowInTaskbar = false;

            hideLAWCToolStripMenuItem.Visible = false;
            restoreWindowToolStripMenuItem.Visible = true;

            //this.Visible = false;

            //applyingSettings = false;
        }

        internal void SetFormNormal() //(Boolean vFirstRun)
        {
            //if (this.WindowState == FormWindowState.Minimized || vFirstRun == true)
            {
                this.Show();
                this.Visible = true;
                this.ShowInTaskbar = true;
                this.LoadScreenPosition(FormWindowState.Normal);
                this.BringToFront();
                this.Focus();

                hideLAWCToolStripMenuItem.Visible = true;
                restoreWindowToolStripMenuItem.Visible = false;

            }
        }


        internal void ShowAdvancedSettings()
        {
            frmSettingsAdvanced.SetPreview();
            frmSettingsAdvanced.DrawEventList(); 
            frmSettingsAdvanced.Location = new Point(frmSettingsAdvanced.Location.X + (this.Width), frmSettingsAdvanced.Location.Y);

            // if moving from Settings to AdvancedSettings.... 
            if (frmSettings.FormChanged == true)
            {
                frmSettingsAdvanced.FormChanged = true;
            }
            frmSettings.FormChanged = false;
            if (frmSettings.Visible) frmSettings.Hide();

            frmSettingsAdvanced.ShowDialog();

            // On Closing
            SaveSettings(string.Empty, this.settings);
            if (frmSettingsAdvanced.FormChanged == true)
                updateImageList();
            if (frmSettingsAdvanced.redrawWallpaper == true)
                AdjustDesktopImages();

            frmSettingsAdvanced.FormChanged = false;

        }

        internal void ShowSettings()
        {
            frmSettings.SetFormValues();
            frmSettings.ShowDialog();

            if (frmSettings.FormChanged == true)
            {
                updateImageList();
            }
            frmSettings.FormChanged = false;

            this.Focus();
            this.BringToFront();

            // On Closing
            SaveSettings(string.Empty, this.settings);
            if (frmSettings.FormChanged == true)
            {
                updateImageList();
                AdjustDesktopImages();
            }

            frmSettings.FormChanged = false;
        }

        /// <summary>
        /// This will show the image list, update the filtered count, set the hours and aspect ratio
        /// </summary>
        internal void updateImageList()
        {
            this.redrawLists();
            frmSettingsAdvanced.UpdateFilteredFileCount();
            frmSettingsAdvanced.SetHoursImages();
        }

        internal void updateImageList(int vColumn)
        {
            settings.ImageSortColumnNum = vColumn;
            updateImageList();
        }

        internal static void OpenDonateURL()
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ZY9EW2SVJ84NU&lc=AU&item_name=Strangetimez&item_number=LAWC&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted");
            }
            catch (System.Net.WebException)
            {
                // do nothing
            }
            
        }

        internal static void OpenGuideURL()
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.strangetimez.com/Blog/?p=712");
            }
            catch (System.Net.WebException)
            {
                // do nothing
            }

        }

        internal static void OpenModesURL()
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.strangetimez.com/Blog/?p=712");
            }
            catch (System.Net.WebException)
            {
                // do nothing
            }

        }

        internal static void OpenAppURL()
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.strangetimez.com/Blog/?page_id=478");
            }
            catch (System.Net.WebException)
            {
                // do nothing
            }

        }

        internal static void OpenOpenWeatherMapURL()
        {
            try
            {
                System.Diagnostics.Process.Start("https://openweathermap.org/");
            }
            catch (System.Net.WebException)
            {
                // do nothing
            }

        }

        private void ShowAbout()
        {
            frmAbout.ShowDialog();
        }

        private void OpenFolder()
        {
            if (this.olvFolders.SelectedItem != null) //s.Count > 0)
            {
                string argument = ((FolderInfo)(olvFolders.SelectedItem.RowObject)).Path;

                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
        }

        private void RemoveFolders()
        {
            if (olvFolders.SelectedItem != null)
            {
                String folder = olvFolders.SelectedItem.Text;

                if (MessageBox.Show("Are you sure you want to Remove this folder?" + System.Environment.NewLine + folder + "", "Remove Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    this.settings.RemoveImagesByPath(((FolderInfo)(olvFolders.SelectedItem.RowObject)).Path);
                    this.settings.RemoveFolder(((FolderInfo)(olvFolders.SelectedItem.RowObject)).ID);

                    setListsNoEntriesLabels();
                    redrawLists();
                    updateImageList(); 
                    DrawFolderList();

                }
            }

            SaveSettings(string.Empty, this.settings);
        }

        private void RescanSelectedFolders()
        {
            rescanFolderToolStripMenuItem.Enabled = false;
            rescanAllFoldersToolStripMenuItem1.Enabled = false;
            addFolderToolStripMenuItem.Enabled = false;
            addImageFolderToolStripMenuItem.Enabled = false;
            rescanAllFoldersToolStripMenuItem2.Enabled = false;
            checkFoldersToolStripMenuItem.Enabled = false;
            removeFolderToolStripMenuItem.Enabled = false;

            if (this.olvFolders.SelectedItem != null)
            {
                RescanFolder(((FolderInfo)(this.olvFolders.SelectedItem.RowObject)).Path, ((FolderInfo)(this.olvFolders.SelectedItem.RowObject)).ID);
            }
        }


        private void RescanFolder(String vPath, int vFolderID)
        {
            // these are reenabled in ProcessFoundImages()
            rescanFolderToolStripMenuItem.Enabled = false;
            rescanAllFoldersToolStripMenuItem1.Enabled = false;
            addFolderToolStripMenuItem.Enabled = false;
            addImageFolderToolStripMenuItem.Enabled = false;
            rescanAllFoldersToolStripMenuItem2.Enabled = false;
            checkFoldersToolStripMenuItem.Enabled = false;
            removeFolderToolStripMenuItem.Enabled = false;

            // Start a new thread only if the thread is stopped 
            // or the thread has not been created yet.
            if (threadScanFolder == null || threadScanFolder.ThreadState == System.Threading.ThreadState.Stopped)
            {
                ShowChangeWallpaperWorking(true);

                this.threadScanFolder = new Thread(() => DoRescanFolder(vPath, vFolderID));
                threadScanFolder.Name = "Scanning Folder";
                threadScanFolder.Priority = MainFunctions.threadPriority; //ThreadPriority.AboveNormal; //ThreadPriority.Highest;
                threadScanFolder.Start();
                isScanningFolder = true;

            }

        }

        private void DoRescanFolder(String vPath, int vFolderID)
        {
            rescanStarted = DateTime.Now;

            this.settings.LastFolderPath = vPath;

            cancelProcess = false;

            int count = 0;
            MainFunctions.RecursiveCount(vPath, ref count, FileFunctions.ImageFileTypes, false);

            this.settings.FoundImages.Clear();

            if (this.InvokeRequired)
            {
                // Reinvoke the same method if necessary        
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    pbarStatus.Maximum = count;
                    pbarStatus.Visible = true;
                    lblStatus.Visible = true;

                    count = 0;
                    RecursiveSearch(vPath, ref this.settings.FoundImages, FileFunctions.ImageFileTypes, false, ref count, vFolderID);

                    OnWorkerThreadComplete();

                }));
            }
            else
            {
                pbarStatus.Maximum = count;
                pbarStatus.Visible = true;
                lblStatus.Visible = true;

                count = 0;
                RecursiveSearch(vPath, ref this.settings.FoundImages, FileFunctions.ImageFileTypes, false, ref count, vFolderID);

                OnWorkerThreadComplete();
            }

        }

        private void CleanImageList() 
        {
            // Remove DELETED Images

            pbarStatus.Maximum = settings.Images.Count;
            pbarStatus.Value = 0;
            pbarStatus.Visible = true;
            lblStatus.Visible = true;
            lblStatus.Text = "Removing missing wallpapers";
            statusStrip1.Refresh();

            RemoveDeletedImages();

            pbarStatus.Value = settings.Images.Count;
            pbarStatus.Visible = false;
            lblStatus.Visible = false;
            statusStrip1.Refresh();
        }

        internal void RemoveDeletedImages()
        {
            int count = 0;

            for (int i = settings.Images.Count - 1; i >= 0; i--)
            {
                pbarStatus.Value = count;
                count++;

                if (cancelProcess == true) break;
               
                if (System.IO.File.Exists(settings.Images[i].FullPath) == false)
                {
                    settings.Images.RemoveAt(i);

                }
                Application.DoEvents();
            }

        }

        private void UpdateImageInfo()
        {
            lblStatus.Text = "Updating wallpaper info";
            this.pbarStatus.Maximum = settings.FoundImages.Count();
            this.pbarStatus.Value = 0;
            pbarStatus.Visible = true;
            lblStatus.Visible = true;

            for (int i = 0; i < settings.FoundImages.Count(); i++)
            {
                if (cancelProcess == true) break;

                lblStatus.Text = "Updating Wallpaper Information #" + i + " of " + this.pbarStatus.Maximum;
                settings.UpdateImageInfo(settings.FoundImages[i]);
                this.pbarStatus.Value = i;
                Application.DoEvents();
            }

            lblStatus.Text = "Updating Wallpapers Done.";
            this.pbarStatus.Value = settings.FoundImages.Count();
            pbarStatus.Visible = false;
            lblStatus.Visible = false;
            statusStrip1.Refresh();
        }

        private void RescanAllFolders(Boolean vFirstPass)
        {
            rescanFolderToolStripMenuItem.Enabled = false;
            rescanAllFoldersToolStripMenuItem1.Enabled = false;
            addFolderToolStripMenuItem.Enabled = false;
            addImageFolderToolStripMenuItem.Enabled = false;
            rescanAllFoldersToolStripMenuItem2.Enabled = false;
            checkFoldersToolStripMenuItem.Enabled = false;
            removeFolderToolStripMenuItem.Enabled = false;

            // copy the list of folders
            CheckFolders(false);

            if (vFirstPass)
            {
                foreach (FolderInfo item in settings.Folders)
                {
                    foldersToScan.Add(item);
                }
            }

            // Start a new thread only if the thread is stopped 
            // or the thread has not been created yet.
            if (threadScanAllFolders == null || threadScanAllFolders.ThreadState == System.Threading.ThreadState.Stopped)
            {
                this.threadScanAllFolders = new Thread(() => DoRescanAllFolders());
                threadScanAllFolders.Name = "Scanning All Folders";
                // Of course this only affects the main thread rather than child threads.
                threadScanAllFolders.Priority = MainFunctions.threadPriority; //ThreadPriority.Highest;
                threadScanAllFolders.Start();
                isScanningFolder = true;
            }
        }

        private void DoRescanAllFolders()
        {
            string folder;
            int id;

            if (foldersToScan.Count > 0)
            {
                ShowChangeWallpaperWorking(false);  

                folder = foldersToScan[foldersToScan.Count - 1].Path;
                id = foldersToScan[foldersToScan.Count - 1].ID;
                foldersToScan.RemoveAt(foldersToScan.Count - 1);

                DoRescanFolder(folder, id);
            }
        }

        private int AddFolderToSettings(String vFolder)
        {
            // add folder to the list / settings
            FolderInfo newItem = new FolderInfo
            {
                Path = vFolder,
                ID = this.settings.GetNextID()
            };
            this.settings.Folders.Add(newItem);

            return newItem.ID;
        }

        internal void AddFolder(String vPath)
        {
            DialogResult result = DialogResult.OK;
            String path = vPath;

            if (string.IsNullOrEmpty(path))
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.MyComputer //Environment.SpecialFolder.MyPictures;
                };
                if (!string.IsNullOrEmpty(settings.LastFolderPath)) dialog.SelectedPath = settings.LastFolderPath;

                result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    path = dialog.SelectedPath;
                    settings.LastFolderPath = path;
                }

                dialog.Dispose();

            }

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (settings.FolderExists(path) == false)
                {
                    int newID = AddFolderToSettings(path);
                    RescanFolder(path, newID);

                    InitWallpaperFilenames();
                }
                else
                {
                    MessageBox.Show("This folder has already been added.", "Folder Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void ResetPosition()
        {
            applyingSettings = true;

            Properties.Settings.Default.Location = new Point(300, 100);

            LoadScreenPosition(true); // set the form with new values
            SaveScreenPosition(); // save the values

            this.Focus();

            applyingSettings = false;
        }

        private void SaveScreenPosition()
        {
            if ((ScreenFunctions.IsOnScreenTopLeft(this) == false || ScreenFunctions.IsOnScreen(this) == false) && !applyingSettings)
            {
                ResetPosition();
            }
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.Location = RestoreBounds.Location;
                Properties.Settings.Default.Size = RestoreBounds.Size;
                Properties.Settings.Default.Maximised = true;
                Properties.Settings.Default.Minimised = false;
            }
            else if (WindowState == FormWindowState.Minimized)
            {
                Properties.Settings.Default.Location = RestoreBounds.Location;
                Properties.Settings.Default.Size = RestoreBounds.Size;
                Properties.Settings.Default.Maximised = false;
                Properties.Settings.Default.Minimised = true;
            }
            else //if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Location = Location;
                Properties.Settings.Default.Size = Size; //Size;
                Properties.Settings.Default.Maximised = false;
                Properties.Settings.Default.Minimised = false;
            }

        }


        private void LoadScreenPosition(Boolean vSetWindowState)
        {

            if (Properties.Settings.Default.Maximised)
            {
                if (vSetWindowState) WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
            else if (Properties.Settings.Default.Minimised)
            {
                if (vSetWindowState) WindowState = FormWindowState.Minimized;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
            else
            {
                // Normal
                if (vSetWindowState) WindowState = FormWindowState.Normal;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }

            if ((ScreenFunctions.IsOnScreenTopLeft(this) == false || ScreenFunctions.IsOnScreen(this) == false) && !applyingSettings)
            {
                ResetPosition();
            }

        }

        private void LoadScreenPosition(FormWindowState vWindowState)
        {

            if (vWindowState == FormWindowState.Maximized) 
            {
                WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
            else if (vWindowState == FormWindowState.Minimized) 
            {
                WindowState = FormWindowState.Minimized;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
            else
            {
                // Normal
                WindowState = FormWindowState.Normal;
            }
            if ((ScreenFunctions.IsOnScreenTopLeft(this) == false || ScreenFunctions.IsOnScreen(this) == false) && !applyingSettings)
            {
                ResetPosition();
            }
        }


        internal void GetSunriseSunset(double vLat, double vLong,
            SunTimes.LatitudeCoords.Direction vLatDirection, SunTimes.LongitudeCoords.Direction vLongDirection)
        {
            int latHours = 0;
            int latMins = 0;
            int latSecs = 0;

            int longHours = 0;
            int longMins = 0;
            int longSecs = 0;

            SunTimes.ConvertDecimalToDegrees(vLat, vLong, vLatDirection, vLongDirection,
                ref latHours, ref latMins, ref latSecs,
                ref longHours, ref longMins, ref longSecs
                );

            SunTimes.Instance.CalculateSunRiseSetTimes(new SunTimes.LatitudeCoords(latHours, latMins, latSecs, vLatDirection),
                                                new SunTimes.LongitudeCoords(longHours, longMins, longSecs, vLongDirection),
                                                DateTime.Now, ref sunrise, ref sunset,
                                 ref isSunrise, ref isSunset);
        }

        internal void GetSunriseSunset(double vLat, double vLong)
        {
            int latHours = 0;
            int latMins = 0;
            int latSecs = 0;

            int longHours = 0;
            int longMins = 0;
            int longSecs = 0;

            SunTimes.LongitudeCoords.Direction longDir;
            SunTimes.LatitudeCoords.Direction latDir;

            SunTimes.ConvertDecimalToDegrees(vLat, vLong,
                ref latHours, ref latMins, ref latSecs,
                ref longHours, ref longMins, ref longSecs
                );

            if (vLat >= 0) latDir = SunTimes.LatitudeCoords.Direction.North; else latDir = SunTimes.LatitudeCoords.Direction.South;
            if (vLong >= 0) longDir = SunTimes.LongitudeCoords.Direction.East; else longDir = SunTimes.LongitudeCoords.Direction.West;

            SunTimes.Instance.CalculateSunRiseSetTimes(new SunTimes.LatitudeCoords(latHours, latMins, latSecs, latDir),
                                                new SunTimes.LongitudeCoords(longHours, longMins, longSecs, longDir),
                                                DateTime.Now, ref sunrise, ref sunset,
                                 ref isSunrise, ref isSunset);
        }

        internal void CheckFolders(Boolean vShowMessage)
        {
            String message = string.Empty;

            foreach (FolderInfo item in settings.Folders)
            {
                if (System.IO.Directory.Exists(item.Path) == false)
                {
                    item.Enabled = false;

                    message += "* " + item.Path + "\n";
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                message = "The following folders are missing: \n\n" + message + "They have been disabled.";
            }
            else
            {
                if (settings.Images.Count() > 0)
                {
                    message = "All folders are okay.";
                }
                else
                {
                    message = "You have no folders.  Please add one.";
                }
            }

            if (vShowMessage)
            {
                MessageBox.Show(message, "Folder Check Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static int BackupFileCount()
        {
            DirectoryInfo di = new DirectoryInfo(Setting.getSettingsFullPath(Properties.Settings.Default.Portable));//@"C:\Music");

            int numBAK = di.GetFiles("Settings_BAK*.xml", SearchOption.TopDirectoryOnly).Length;

            return numBAK;
        }

        private void BackupSettings(Boolean vShowMessage)
        {
            String filename = String.Format(CultureInfo.InvariantCulture, "Settings_BAK_{0}-{1}-{2} {3}-{4}.xml",
                    DateTime.Now.Year.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Month.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Day.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Hour.ToString("D2", CultureInfo.InvariantCulture),
                    DateTime.Now.Minute.ToString("D2", CultureInfo.InvariantCulture)
                    );

            SaveSettings(Setting.getSettingsFullPath(Properties.Settings.Default.Portable) + "\\" + filename, this.settings);

            if (vShowMessage) MessageBox.Show("Settings Backed up to " + filename + "", "Settings Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateTimes()
        {
            this.settings.LightSunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, this.settings.LightSunriseTime.Hour, this.settings.LightSunriseTime.Minute, 0);
            this.settings.DarkSunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, this.settings.DarkSunsetTime.Hour, this.settings.DarkSunsetTime.Minute, 0);
        }

        private void ShowWebsites()
        {
            foreach (Control c in this.frmWebsites.Controls)
            {
                c.BackColor = colourDarkest;
                c.ForeColor = colourLightest;
            }
            this.frmWebsites.BackColor = colourDarkest;
            this.frmWebsites.ForeColor = colourLightest;

            frmWebsites.DrawWebsiteList();
            frmWebsites.ShowDialog();

            settings.WallpaperManagerOpened = true;
        }

        internal void ChangeCurrentMode(Boolean vEnable)
        {
            this.settings.DarkMode = vEnable;
            SetDarkModeState();
        }

        private void SetDarkModeState()
        {

            toolStripMenuItem11.Visible = true;

            if (this.settings.DarkMode == true) // if dark mode enabled // OLD used setting: AdjustmentsUseAlways 
            {
                darkModeDisabledToolStripMenuItem.Visible = true; // set the rt click menu for dark mode enable/disable
                darkModeEnableToolStripMenuItem.Visible = false;
            }
            else
            {
                darkModeDisabledToolStripMenuItem.Visible = false; // set the rt click menu for dark mode enable/disable
                darkModeEnableToolStripMenuItem.Visible = true;
            }


            if (!applyingSettings) 
            {
                AdjustDesktopImages();
            }
        }

        private void ResetSelectedViewCount()
        {
            if (this.olvImages.SelectedItem != null)
            {
                this.settings.ResetViewCount(((ImageInfo)(olvImages.SelectedItem.RowObject)).FullPath);
                //update lowest view count info
                lowestViewCount = GetLowestViewCountVisibleOnly(out lowestViewCountIndex);

            }
        }

        internal int TotalMinutesOfImages()
        {
            int output;

            // total minutes
            output = settings.Images.Count * settings.WallpaperChangeFrequencyMins;

            return output;
        }

        private void RestoreLastBackup()
        {
            string latestBackup = getLastBackupFilename();

            if (!string.IsNullOrEmpty(latestBackup))
            {
                if (System.IO.File.Exists(latestBackup))
                {
                    if (MessageBox.Show("Are you sure you want to RESTORE the previous settings backup for LAWC?\n\n"
                        + "Filename: " + latestBackup
                        + "\n\nYour current settings will be backed up.",
                        "Restore Last Backup Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // backup the current settings file now
                        BackupSettings(false);
                        doRestoreBackup(latestBackup);
                    }
                }
            }
        }

        private void doRestoreBackup(String vLatestBackup)
        {
            string latestBackup = vLatestBackup;
            if (String.IsNullOrEmpty(latestBackup)) latestBackup = getLastBackupFilename();

            //// backup the current settings file now
            //BackupSettings(false);

            // "rename" (delete/copy/load) the LatestBackup to be the settings filename (settings.xml)
            // delete the current settings
            FileFunctions.DeleteFileToRecycleBin(Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable));

            // copy the latestbackup to settings.xml
            System.IO.File.Copy(latestBackup, Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable));

            Properties.Settings.Default.SettingsFilePath = Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable);

            // load the new settings
            MainFormLoad(Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable));
        }

        private void addDefaultEvents()
        {
            // only add if none are listed currently 
            if (settings.Events.Count == 0)
            {
                EventInfo newEvent = new EventInfo
                {

                    // TEMPERATURE
                    CheckAction = EventInfo.CheckActionType.DisplayAlways,
                    CheckSeconds = 300,
                    CheckValueString = string.Empty,
                    CheckValueDecimal = 0,
                    Enabled = false,
                    ImagePath = string.Empty,
                    Message = "<<Value>>°C",
                    OverrideWallpaper = false,
                    Displayed = false,
                    ShowNotification = false,
                    FontSize = 12,
                    FontColour = Color.CornflowerBlue,
                    LastRun = MainFunctions.DateNull, 
                    TypeOfEvent = "Temperature",
                    SensorName = "Temperature"
                };
                settings.Events.Add(newEvent);


                // PC Overheating
                newEvent = new EventInfo
                {
                    CheckAction = EventInfo.CheckActionType.GreaterThan,
                    CheckSeconds = 60,
                    CheckValueString = string.Empty,
                    CheckValueDecimal = 0,
                    Enabled = false,
                    ImagePath = string.Empty,
                    Message = "The CPU is getting HOT! <<Value>>°C",
                    OverrideWallpaper = false,
                    Displayed = false,
                    ShowNotification = true,
                    FontSize = 12,
                    FontColour = Color.Red,
                    LastRun = MainFunctions.DateNull, 
                    TypeOfEvent = "Temperature",
                    SensorName = "CPU Core #1"
                };
                settings.Events.Add(newEvent);

                // add the hardware sensors? No, let the user do that if they want to use them (faster startup by default this way)
                //settings.HWSensorsUsed = "";
            }


        }


        internal void redrawLists()
        {
            lblStatus.Visible = true;
            lblStatus.Text = "Loading Wallpaper List";
            statusStrip1.Refresh();

            applyingSettings = true;

            olvImages.SetObjects(settings.Images, true);
            olvFolders.SetObjects(settings.Folders, true);
            applyingSettings = false;

            lblStatus.Visible = false;
            lblStatus.Text = "";
            setImageCountText();
            statusStrip1.Refresh();
        }


        internal void setImageCountText()
        {
            // Integer comma thousands example
            //String.Format(CultureInfo.InvariantCulture, "{0:n}", 1234);  // Output: 1,234.00
            //String.Format(CultureInfo.InvariantCulture, "{0:n0}", 9876); // No digits after the decimal point. Output: 9,876

            tslblImageCount.Text = "# Images: " + String.Format(CultureInfo.InvariantCulture, "{0:n0}", olvImages.GetItemCount())
                + " (" + String.Format(CultureInfo.InvariantCulture, "{0:n0}", (settings.Images.Count - olvImages.GetItemCount()).ToString(CultureInfo.InvariantCulture)) + " hidden)";
        }

        internal void SearchFilter(ObjectListView olv, string txt)
        {
            try
            {
                SearchFilter(olv, txt, 0); // choose "contains" as default search type
            }
            catch (ApplicationException ex)
            {
                ProcessError(ex, ErrorMessageType.Search, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

        }

        internal static void SearchFilter(ObjectListView olv, string txt, int matchKind)
        {
            TextMatchFilter filter = null;
            if (!String.IsNullOrEmpty(txt))
            {
                switch (matchKind)
                {
                    case 0:
                    default:
                        filter = TextMatchFilter.Contains(olv, txt);
                        break;
                    case 1:
                        filter = TextMatchFilter.Prefix(olv, txt);
                        break;
                    case 2:
                        filter = TextMatchFilter.Regex(olv, txt);
                        break;
                }
            }

            // Text highlighting requires at least a default renderer
            if (olv.DefaultRenderer == null)
                olv.DefaultRenderer = new HighlightTextRenderer(filter);

            olv.AdditionalFilter = filter;

        }

        private void doCancelScan()
        {
            if (MessageBox.Show("Are you sure you want to cancel the scan?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cancelProcess = true;

                rescanFolderToolStripMenuItem.Enabled = true;
                rescanAllFoldersToolStripMenuItem1.Enabled = true;
                addFolderToolStripMenuItem.Enabled = true;
                addImageFolderToolStripMenuItem.Enabled = true;
                rescanAllFoldersToolStripMenuItem2.Enabled = true;
                checkFoldersToolStripMenuItem.Enabled = true;
                removeFolderToolStripMenuItem.Enabled = true;

                HideChangeWallpaperWorking();

            }
        }

        internal void DisplayErrorForm()//, FrmMain vParentForm = null)
        {
            //MessageBox.Show(msg ?? ex.Message, "", MessageBoxButton.OK, img);

            frmShowText showInfo = new frmShowText
            {
                Text = "Unexpected Error"
            };
            showInfo.lblHeading.Text = "Send a Report/Error about LAWC";
            showInfo.lblDetails.Text = "Sorry, there was a problem. Please add any information that might be relevant. "
                + "Eg. What you were doing at the time. \nPlease only write above the horizontal line. Then press [Send Error].\n "
                + "Thank you :)";
            //showInfo.richText.Text = "\n\n_____________________^ Please only reply in the area above this line ^_____________________\n\n";
            showInfo.richText.ReadOnly = false;
            showInfo.AcceptButton = null;

            showInfo.btnCancel.Visible = false;
            showInfo.btnSendError.Location = showInfo.btnCancel.Location;
            showInfo.btnSendError.Visible = true;
            showInfo.btnOK.Text = "Close";
            showInfo.ShowDialog();

            //vIsDisplayed = true;
            //DialogResult result = showInfo.DialogResult;

            //if (result == DialogResult.Yes) // send error
            //{
            //    SendSMTPMail("@gmail.com", "me@here.com", "LAWC Error Received!", showInfo.richText.Text, MailPriority.High); 
            //}
        }

        internal void setSettingsFilePath(String vNewFullPath)
        {
            if (!string.IsNullOrEmpty(vNewFullPath.Trim()))
            {
                // set the settings filename
                Properties.Settings.Default.SettingsFilePath = vNewFullPath;
            }
            else
            {
                // blank path was passed in
                if (settings != null)
                {
                    // if a settings filename has not been set, then set the default
                    if (string.IsNullOrEmpty(Properties.Settings.Default.SettingsFilePath))
                    {
                        Properties.Settings.Default.SettingsFilePath = Setting.getCurrentSettingsFullPathWithFilename(Properties.Settings.Default.Portable);
                    }
                }
            }
            //set title text
            this.Text = "Light Adjusting Wallpaper Changer - LAWC  (" + Path.GetFileName(Properties.Settings.Default.SettingsFilePath) + ")";
        }


        private void RescanFolderFromSelectedImage(int vFolderID)
        {
            string path = settings.GetFolderByID(vFolderID).Path;
            RescanFolder(path, vFolderID);
        }

        #endregion


        #region Events and Sensors

        internal static float GetHDDValue(String vDriveLetter, String vSensorName)
        {

            float output = 0;
            Boolean found = false;

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            double percent;

            foreach (DriveInfo d in allDrives)
            {
                if (!d.IsReady)
                {
                    //await PutTaskDelay(5);
                    Application.DoEvents();
                    Thread.Sleep(5000);
                    Application.DoEvents();
                    // if its STILL not ready...
                    if (!d.IsReady)
                    {
                        MessageBox.Show(d.Name + " " + d.VolumeLabel + " is not ready.");
                    }
                }
                else if (vDriveLetter + @":\" == d.Name)
                {
                    float usedMB = (float)Math.Round(MathExtra.ConvertBytesToMegabytes(d.TotalSize - d.TotalFreeSpace), 1);
                    float usedPercent = (float)Math.Round(MathExtra.ConvertBytesToMegabytes(d.AvailableFreeSpace), 1);

                    if (vSensorName.Contains("HDD Used MB - "))
                    {
                        output = usedMB;
                        found = true;
                    }
                    else if (vSensorName.Contains("HDD Used % - "))
                    {
                        percent = ((double)usedMB / MathExtra.ConvertBytesToMegabytes(d.TotalSize)) * 100; 
                        output = (float)Math.Round(percent, 1);
                        found = true;
                    }
                    else if (vSensorName.Contains("HDD Free MB - "))
                    {
                        output = usedPercent;
                        found = true;
                    }
                    else if (vSensorName.Contains("HDD Free % - "))
                    {
                        percent = ((double)usedPercent / MathExtra.ConvertBytesToMegabytes(d.TotalSize)) * 100; 
                        output = (float)Math.Round(percent, 1);
                        found = true;
                    }
                    else
                    {
                        ProcessError(null, ErrorMessageType.SensorUnknown, true, false, String.Format(CultureInfo.InvariantCulture, "Sensor: {0}", vSensorName), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                    }
                }
            }

            if (found == false)
            {
                // probably a problem getting the value... unlikely that the value would be zero normally
                ProcessError(null, ErrorMessageType.HDDNotFound, true, false, String.Format(CultureInfo.InvariantCulture, "Problem getting value for Drive [{0}] and Sensor: {1}", vDriveLetter, vSensorName), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            return output;
        }

        //https://stackoverflow.com/questions/17832969/c-constantly-monitor-battery-level
        internal static object GetPowerDetails(BatteryInfoCategory vCategory)
        {

            PowerStatus ps = SystemInformation.PowerStatus;

            switch (vCategory)
            {
                case BatteryInfoCategory.BatteryChargeStatus:
                    return ps.BatteryChargeStatus.ToString();

                case BatteryInfoCategory.BatteryFullLifetime:
                    return ps.BatteryFullLifetime;

                case BatteryInfoCategory.BatteryLifePercent:
                    return ps.BatteryLifePercent;

                case BatteryInfoCategory.BatteryLifeRemaining:
                    return ps.BatteryLifeRemaining;

                case BatteryInfoCategory.PowerLineStatus:
                    return ps.PowerLineStatus.ToString();

                default:
                    return ps.BatteryLifePercent;
            }
            
        }

        #endregion


        #region Helpers

        private List<ToolStripMenuItem> GetMenuStripItems(MenuStrip menuStrip)
        {
            List<ToolStripMenuItem> toolSripItems = new List<ToolStripMenuItem>();

            foreach (ToolStripMenuItem toolStripItem in menuStrip.Items)
            {
                GetSubMenuStripItems(toolStripItem, ref toolSripItems);
            }

            return toolSripItems;
        }

        private List<ToolStripItem> GetMenuStripItems()
        {

            List<ToolStripItem> toolStripItems = new List<ToolStripItem>();

            if (ContextMenu != null)
            {
                foreach (ToolStripMenuItem toolStripItem in ContextMenu.MenuItems)
                {
                    GetSubMenuStripItems(toolStripItem, ref toolStripItems);
                }
            }

            return toolStripItems;
        }

        private void GetSubMenuStripItems(ToolStripMenuItem menuItem, ref List<ToolStripItem> vAllItems)
        {
            vAllItems.Add(menuItem);

            // if sub menu contain child dropdown items
            if (menuItem.HasDropDownItems)
            {
                foreach (ToolStripItem toolStripItem in menuItem.DropDownItems)
                {
                    //if (toolSripItem is ToolStripMenuItem)
                    {
                        //call recursively
                        GetSubMenuStripItems((ToolStripMenuItem)toolStripItem, ref vAllItems);
                    }
                }
            }
        }

        private void GetSubMenuStripItems(ToolStripMenuItem menuItem, ref List<ToolStripMenuItem> vAllItems)
        {
            vAllItems.Add(menuItem);

            // if sub menu contain child dropdown items
            if (menuItem.HasDropDownItems)
            {
                foreach (ToolStripItem toolStripItem in menuItem.DropDownItems)
                {
                    if (toolStripItem is ToolStripMenuItem)
                    {
                        //call recursively
                        GetSubMenuStripItems((ToolStripMenuItem)toolStripItem, ref vAllItems);
                    }
                }
            }
        }

        private Color InvertColour(Color vColour)
        {
            int r = 255 - vColour.R;
            int g = 255 - vColour.G;
            int b = 255 - vColour.B;

            return Color.FromArgb(255, r, g, b);
        }

        
        private void RecursiveSearch(String vPath, ref List<ImageInfo> rOutput, ArrayList vFileTypes, Boolean vCancel, ref int vCount, int vFolderID)
        {
            if (string.IsNullOrEmpty(vPath)) return;

            int pos = 0;

            string currentItemName;// = string.Empty;

            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(vPath);

            Boolean cancel = vCancel;

            foreach (FileSystemInfo fileObject in dirInfo.GetFileSystemInfos())
            {
                if (cancelProcess == true) cancel = true;
                if (cancel == true) break;

                Application.DoEvents();

                if (fileObject.Attributes == FileAttributes.Directory)
                {
                    System.Windows.Forms.Application.DoEvents();

                    RecursiveSearch(fileObject.FullName.ToString(CultureInfo.InvariantCulture), ref rOutput, vFileTypes, cancel, ref vCount, vFolderID);
                }
                else
                {
                    pos++;
                                        
                    currentItemName = fileObject.Name;

                    // are file types okay / appropriate
                    if (FileFunctions.IsFileTypeOK(fileObject.Name, vFileTypes))
                    {
                        OnWorkerThreadChange(vPath, vCount);

                        Bitmap bmp = null;

                        bmp = (Bitmap)ImageFunctions.LoadImage(fileObject.FullName, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

                        if (bmp != null)
                        {
                            int width = bmp.Width;
                            int height = bmp.Height;

                            float aspect = (float)bmp.Width / (float)bmp.Height;
                            ImageFunctions.ResizeFixedAspectRatio(ref bmp, ImageFunctions.ImageProcessSizeMedium.Width,
                                ImageFunctions.ImageProcessSizeMedium.Height, this.settings.BackgroundColourLight, InterpolationMode.Low);

                            ImageInfo newItemInfo = new ImageInfo
                            {
                                FullPath = fileObject.FullName,
                                FolderID = vFolderID
                            };

                            ImageStats stats = ImageFunctions.GetImageStats(bmp);
                            newItemInfo.AverageColour = stats.AverageColour;
                            newItemInfo.AverageBrightness = stats.Brightness;
                            newItemInfo.SizeBytes = ((FileInfo)fileObject).Length;
                            newItemInfo.Aspect = aspect;
                            newItemInfo.Width = width;
                            newItemInfo.Height = height;

                            rOutput.Add(newItemInfo);

                            vCount++;
                        }

                        if (bmp != null) bmp.Dispose();
                        Application.DoEvents();

                    } // end if file type OK                    
                } // if directory or file
            }

        }

        private void InformUserAboutWallpaperWebsiteManager()
        {
            int numDays = 14;
            int numImagesMinimum = 100;

            // if the settings were created/reset over 14 days ago
            if (settings.SettingsCreated.AddDays(numDays) < DateTime.Now)
            {
                if (settings.Images.Count < numImagesMinimum && settings.WallpaperManagerOpened == false)
                {
                    if (MessageBox.Show("It seems that you do not have a lot of wallpapers.\n\n"
                        + "LAWC has a Wallpaper Website manager built in, and has a\n"
                        + "number of suggested websites where you can get more free \n"
                        + "wallpapers. You can also add in your own websites, as"
                        + "well as share your list with friends.  \n\nWould you like to "
                        + "open the Wallpaper Website Manager? \n\n(You can always open "
                        + "it at any time with the Websites button at "
                        + "the top of the main window)",
                        "Open Wallpaper Website Manager",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ShowWebsites();
                    }

                    settings.WallpaperManagerOpened = true; // dont hassle the user again
                }

            }
        }

        internal void checkInternetConnection(Boolean vResetSensors)
        {
            internetAvailable = MainFunctions.CheckForInternetConnection();

            if (internetAvailable)
            {
                internetAvailable = true;

                if (vResetSensors) initHardwareSensorsAsync(true, false, false); // force refresh

                retryInternetConnectionToolStripMenuItem.Visible = false;
            }
            else
            {
                internetAvailable = false;

                retryInternetConnectionToolStripMenuItem.Visible = true;
            }

            if (pbNoInternet.InvokeRequired)
            {   
                pbNoInternet.Invoke((Action)delegate
                {
                    pbNoInternet.Visible = !internetAvailable;
                });
            }
            else
            {
                pbNoInternet.Visible = !internetAvailable;
            }
            

        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startRenameFile(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath);
        }

        private void startRenameFile(String vFullPathCurrent)
        {
            frmRenameImage frmRename = new frmRenameImage(this, vFullPathCurrent);

            frmRename.loadSizeLocation();
            frmRename.SetInterfaceColour();
            frmRename.load();
            frmRename.ShowDialog();

            frmRename.Dispose();

            this.BringToFront();
        }

        /// <summary>
        /// Returns the full path of the new file
        /// </summary>
        /// <param name="vSourceFullPath"></param>
        /// <param name="vDestFilename"></param>
        /// <returns></returns>
        internal String RenameWallpaperFile(String vSourceFullPath, String vDestFilename)
        {
            String output = string.Empty;

            if (!string.IsNullOrEmpty(vDestFilename) && System.IO.File.Exists(vSourceFullPath))
            {
                // (rename) move the file to the same folder, with the new name
                string destination = System.IO.Path.GetDirectoryName(vSourceFullPath) + "\\" + vDestFilename; 

                // if there is no change, cancel the rename
                if (vSourceFullPath == destination)
                {
                    return destination;
                }

                try
                {
                    System.IO.File.Move(vSourceFullPath, destination);

                    int index = settings.Images.FindIndex(r => r.FullPath == vSourceFullPath);

                    if (index >= 0 && settings.Images.Count >= index)
                    {
                        settings.Images[index].FullPath = destination;
                        output = destination;
                    }
                    else
                    {
                        MessageBox.Show("Error ABC: LAWC was unable to find the wallpaper you renamed. Please rescan your Folders.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem renaming this file. Please check the permissons on the file and folder. Also check if the file is Read-Only."
                        + System.Environment.NewLine + System.Environment.NewLine + ex.Message
                        , "Problem Renaming File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return output;
        }

        private void RefreshDownloadUpdateIcon()
        {
            int currentVersion = int.Parse("0" + this.GetType().Assembly.GetName().Version.ToString().Replace(".", ""), CultureInfo.InvariantCulture);
            int latestVersion = 0;
            string versionString;

            pbDownloadLatest.Visible = false;

            try
            {
                versionString = GetLatestVersionNumber();
                if (versionString.Trim() == string.Empty)
                {
                    versionString = "0.0.0.0";
                }
                latestVersion = int.Parse("0" + versionString.Replace(".", ""), CultureInfo.InvariantCulture);
            }
            catch (NetworkInformationException ex)
            {
                if (settings.ShowInternetError) MessageBox.Show("Error getting version info from the internet.  Check your internet connection.\n\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessError(ex, ErrorMessageType.CheckForUpdate, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            if (latestVersion > currentVersion)
            {
                pbDownloadLatest.Visible = true;
            }
        }

        private void CheckForUpdate(Boolean vShowWhenOkay)
        {
            if (internetAvailable == false)
            {
                showNoInternetMessage();
                return;
            }

            //http://www.csharp-station.com/HowTo/HttpWebFetch.aspx

            int currentVersion = int.Parse("0" + this.GetType().Assembly.GetName().Version.ToString().Replace(".", ""), CultureInfo.InvariantCulture);
            int latestVersion = 0;
            string versionString = string.Empty;
            string latestChanges = string.Empty;

            try
            {
                versionString = GetLatestVersionNumber();
                latestChanges = GetLatestChanges();

                if (versionString.Trim() == string.Empty)
                {
                    versionString = "0.0.0.0";
                }
                latestVersion = int.Parse("0" + versionString.Replace(".", ""), CultureInfo.InvariantCulture);
            }
            catch (NetworkInformationException ex)
            {
                if (settings.ShowInternetError) MessageBox.Show("Error getting version info from the internet.  Check your internet connection.\n\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessError(ex, ErrorMessageType.CheckForUpdate, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            if (latestVersion == 0)
            {
                MessageBox.Show("There was a problem getting the version number from the internet. Please check your connection.", "Problem Getting Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (latestVersion > currentVersion)
            {
                frmShowText showInfo = new frmShowText(this)
                {
                    Text = "New Version Available"
                };
                showInfo.lblHeading.Text = "There is a new version of LAWC available";
                showInfo.lblDetails.Text = "You can download it from my LAWC blog page where you can find extra info about the application.";
                showInfo.richText.Text = "Newer version available.\n\nWould you like visit the website for LAWC?\n\n"; // Recent Changes:\n\n" + latestChanges;
                showInfo.ShowDialog();
                DialogResult result = showInfo.DialogResult;

                if (result == System.Windows.Forms.DialogResult.Yes
                    || result == System.Windows.Forms.DialogResult.OK)
                {
                    FrmMain.OpenAppURL();
                }
            }
            else if (latestVersion < currentVersion)
            {
                if (vShowWhenOkay) MessageBox.Show("This version is NEWER than the official release.\n\nYou are very Cool :)\n\nRecent Changes:\n\n" + latestChanges, "LAWC Version " + versionString, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (vShowWhenOkay)
                {
                    frmShowText showInfo = new frmShowText(this)
                    {
                        Text = "LAWC Is Up-To-Date"
                    };
                    showInfo.lblHeading.Text = "LAWC Is Up-To-Date";
                    showInfo.lblDetails.Text = "You do not need to do anything.";
                    showInfo.richText.Text = "This version is Up-To-Date.\n\n"; 
                    showInfo.btnCancel.Visible = false;

                    showInfo.ShowDialog();
                }

            }
        }

        public string GetLatestVersionNumber()
        {
            if (!internetAvailable) return string.Empty;

            return GetWebData(new Uri("http://www.strangetimez.com/Apps/LAWC/latestversion.txt"));
        }

        public string GetLatestChanges()
        {
            if (!internetAvailable) return string.Empty; 

            return GetWebData(new Uri("http://www.strangetimez.com/Apps/LAWC/latestchanges.txt"));
        }

        public string GetWebData(Uri vURI)
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            try
            {
                checkInternetConnection(false);
                if (internetAvailable == false)
                {
                    return string.Empty;
                }

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(vURI);//("http://www.strangetimez.com/Apps/LAWC/latestversion.txt");

                HttpWebResponse response;

                try
                {
                    // execute the request
                    response = (HttpWebResponse)request.GetResponse();

                    // we will read data via the response stream
                    Stream resStream = response.GetResponseStream();

                    string tempString = null;
                    int count = 0;

                    do
                    {
                        // fill the buffer with data
                        count = resStream.Read(buf, 0, buf.Length);

                        // make sure we read some data
                        if (count != 0)
                        {
                            // translate from bytes to ASCII text
                            tempString = Encoding.ASCII.GetString(buf, 0, count);

                            // continue building the string
                            sb.Append(tempString);
                        }
                    }
                    while (count > 0); // any more data to read?
                }
                catch (System.Net.WebException)
                {
                    //do nothing
                }
            }
            catch (NetworkInformationException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            catch (TimeoutException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            catch (System.Net.WebException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            catch (ProtocolViolationException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            // print out page source
            return sb.ToString();

        }

        public string GetWebData(String vURL)
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            try
            {
                checkInternetConnection(false);
                if (internetAvailable == false)
                {
                    return string.Empty;
                }

                // used on each read operation
                byte[] buf = new byte[8192];

                Uri uri = new Uri(vURL);

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);//("http://www.strangetimez.com/Apps/LAWC/latestversion.txt");

                HttpWebResponse response;

                try
                {
                    // execute the request
                    response = (HttpWebResponse)request.GetResponse();

                    // we will read data via the response stream
                    Stream resStream = response.GetResponseStream();

                    string tempString = null;
                    int count = 0;

                    do
                    {
                        // fill the buffer with data
                        count = resStream.Read(buf, 0, buf.Length);

                        // make sure we read some data
                        if (count != 0)
                        {
                            // translate from bytes to ASCII text
                            tempString = Encoding.ASCII.GetString(buf, 0, count);

                            // continue building the string
                            sb.Append(tempString);
                        }
                    }
                    while (count > 0); // any more data to read?
                }
                catch (System.Net.WebException)
                {
                    //do nothing
                }
            }
            catch (NetworkInformationException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            catch (TimeoutException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            catch (System.Net.WebException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }
            catch (ProtocolViolationException ex)
            {
                ProcessError(ex, ErrorMessageType.GetWebData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            // print out page source
            return sb.ToString();
        }

        internal Color GetTaskbarColour()
        {
            int argbColor = (int)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", "00000000");

            return System.Drawing.Color.FromArgb(argbColor);
        }

        internal void SetTaskbarColourDisableAuto(Color vColour)
        {
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationGlassAttribute", 0);
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", vColour.ToArgb());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vQuality">1 to 100 - percent.  low of 60, default of 85, and a max of 100</param>
        internal static void SetImageCompressionRegistry(int vQuality)
        {
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", vQuality);
        }

        private string GetImagesAvailableMessage()
        {
            string output = string.Empty;
            if (settings.Images.Count() > 0)
            {
                // images available - do nothing
            }
            else
            {
                if (settings.Images.Count() == 0)
                {
                    output = "There are no folders of images in your list. Right Click on the list to add a Folder of images.";
                }
                else
                {
                    output = "There are no images available in your list.\n\nPlease check your Brightness values in the List tab of the Settings.";
                }
            }

            return output;
        }

        internal void SetSampleImage()
        {
            SetPreviewImages(GetSampleImagePath());
        }

        internal void SetImageNotFound()
        {
            SetPreviewImages(GetNotFoundImagePath());
        }

        internal static string GetSampleImagePath()
        {
            return Application.StartupPath + "\\Images\\Sample4k.jpg";
        }
        internal static string GetNotFoundImagePath()
        {
            return Application.StartupPath + "\\Images\\image-not-found.png";
        }

        internal Boolean DeleteSelectedImage(String vPath)
        {
            Boolean output = false;

            try
            {
                if (MessageBox.Show("Are you sure you want to delete this file?" + System.Environment.NewLine + vPath + "", "Delete Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    output = true; // user wants to delete
                                   //delete file
                    FileFunctions.DeleteFileToRecycleBin(vPath);

                    //select the next image on the list                    
                    for (int i = 0; i < settings.Images.Count; i++)
                    {
                        if (settings.Images[i].FullPath == vPath)
                        {
                            // remove from list
                            RemoveSelectedImage();

                            //Scroll to the next entry
                            // as the entry was removed, (i) now represents the next entry in the list
                            ScrollToSelectedImageItem(settings.Images[i].FullPath, false, true);
                            olvImages.Items[i].Selected = true;
                            settings.ImageLastSelected = ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath;

                            //refresh listview
                            redrawLists();

                            break;
                        }
                    }
                }

            }
            catch (IOException ex)
            {
                ProcessError(ex, ErrorMessageType.DeletingImage, true, false, string.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

            return output;
        }

        private void RemoveSelectedImage()
        {
            if (this.olvImages.SelectedItem != null)
            {
                if (!System.IO.File.Exists(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath)) return;

                this.settings.RemoveImagesByPath(((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath); 

                //refresh listview
                redrawLists();
            }
        }

        private void addToHistory(String vCurrentImage)
        {
            settings.ImageHistory.Add(vCurrentImage);
            
            if (settings.ImageHistory.Count > settings.MaxRecentImageHistoryCount)
            {
                try
                {
                    settings.ImageHistory.RemoveAt(0);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    ProcessError(ex, ErrorMessageType.ImageData, false, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                }
            }
            
        }

        private static int GetPrimaryScreenIndex()
        {
            for (int i = 0; i < Screen.AllScreens.Count(); i++)
            {
                if (Screen.AllScreens[i].Primary)
                {
                    return i;
                }
            }

            // not found?!
            return 0;
        }

        /// <summary>
        /// 
        /// Note: Ignores ViewCount
        /// </summary>
        /// <param name="vScrollToEntry"></param>
        private void SetSelectedAsWallpaper(Boolean vScrollToEntry, Boolean vSelectEntry)
        {
            if (this.olvImages.SelectedItem != null)
            {
                int screenCount = ScreenFunctions.GetScreenCount(settings.MultiMonitorMode, settings.WallpaperMode);

                String currentImageFile = ((ImageInfo)(this.olvImages.SelectedItem.RowObject)).FullPath;
                ImageInfo[] result = GetNextImages(false, this.settings.WallpaperOrder); //fresh set of images using the wallpaper order
                settings.ImageLastSelected = currentImageFile; // select the user chosen image (not the following ones that are selected following rules)

                for (int i = 0; i < screenCount; i++)
                {
                    // if we are not on the first screen, and if the first image added is different to the current one
                    // then increase the viewcount
                    // this stops the same image being counted multiple times if displayed on all screens
                    if (wallpaperFilenames[i] != currentImageFile)
                    {
                        settings.RecentImages.Add(currentImageFile);
                        ChangeImageFilename(currentImageFile, i, true);
                        IncreaseViewCountList(currentImageFile, 1);
                        addToHistory(currentImageFile);

                        if (((ImageInfo)(this.olvImages.SelectedItem.RowObject)).Aspect > settings.AspectThresholdWide && i == 0)
                        {
                            // a spanned FIRST image - only count this entry once                          
                            break;
                        }
                    }
                    currentImageFile = result[i].FullPath; // use one of the new images              
                }

                AdjustDesktopImages();

                SaveSettings(string.Empty, this.settings);

                //refresh the list
                redrawLists();
                ScrollToSelectedImageItem(settings.ImageLastSelected, vScrollToEntry, vSelectEntry);

                // reset change time
                SetNextWallpaperChangeTime();
                SetNextWallpaperAdjustTime();
            }
        }


        internal static void reSortOrderValues(ref List<String> vList, int vIndexToKeepPos) //, int vMaxCount)
        {
            List<String> testList = new List<string>(new string[] { "element1", "element3", "element2", "element4" });
            reSortOrderValues(ref testList, 3);

            if (vList.Count == 0) return;

            String valToKeep = vList[vIndexToKeepPos];

            String temp = string.Empty;
            //for (int x = 1; x < parentForm.parentForm.settings.Events.Count; x++)
            {
                for (int y = 0; y < vList.Count; y++)
                {
                    if (y + 1 < vList.Count)
                    {
                        if (vList[y] == vList[y + 1])
                        {
                            // the current and last entries have same position. one needs to change
                            if (y != vIndexToKeepPos)
                            {
                                vList[y + 1] = vList[y];
                            }

                            temp = string.Empty;
                        }
                    }
                }
            }

            //sort by the new order
            vList = vList.OrderBy(q => q).ToList();
        }


        internal void ShowImageData(String vPath)
        {
            try
            {
                String info = ImageFunctions.GetImageMetaData(vPath);
                frmMetaData.txtMetadata.Text = info.Replace("\n", System.Environment.NewLine);
                frmMetaData.ShowDialog();
            }
            catch (IOException ex)
            {
                ProcessError(ex, ErrorMessageType.ImageData, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

        }

        //internal void ShowImageDataMS(String vPath)
        //{

        //    BitmapMetadata metaData = ImageFunctions.GetImageMetaDataMS(vPath);
        //    string message = string.Empty;
        //    message += "File: " + vPath + "\n\n";
        //    message += "Title: " + metaData.Title + "\n";
        //    message += "Subject: " + metaData.Subject + "\n";
        //    message += "Author(s): ";
        //    if (metaData.Author != null) 
        //        foreach (String auth in metaData.Author)
        //        {
        //            message += metaData.Author + " ";
        //        }
        //    message += "\n";
        //    message += "CameraManufacturer: " + metaData.CameraManufacturer + "\n";
        //    message += "CameraModel: " + metaData.CameraModel + "\n";
        //    message += "CanFreeze: " + metaData.CanFreeze + "\n";
        //    message += "Copyright: " + metaData.Copyright + "\n";
        //    message += "DateTaken: " + metaData.DateTaken + "\n";
        //    message += "Format: " + metaData.Format + "\n";
        //    message += "IsFixedSize: " + metaData.IsFixedSize + "\n";
        //    message += "IsFrozen: " + metaData.IsFrozen + "\n";
        //    message += "IsDataReadOnly: " + metaData.IsReadOnly + "\n";
        //    message += "Keywords: " + metaData.Keywords + "\n";
        //    if (metaData.Keywords != null)
        //        foreach (String auth in metaData.Keywords)
        //        {
        //            message += metaData.Keywords + " ";
        //        }
        //    message += "Location: " + metaData.Location + "\n";
        //    message += "Rating: " + metaData.Rating + "\n\n";

        //    message += "File: " + vPath + "\n";

        //    MessageBox.Show(message, "Image Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //}

        internal void SetKeyPress()
        {
            Boolean convertOK;// = false;

            keyboardHook.UnRegisterHotKey(1);

            //Keys key1;
            convertOK = Enum.TryParse<Keys>(settings.ShortcutKey1, out Keys key1);
            if (!convertOK) return;

            //Keys key2;
            convertOK = Enum.TryParse<Keys>(settings.ShortcutKey2, out Keys key2);
            if (!convertOK) return;

            //Keys key3;
            convertOK = Enum.TryParse<Keys>(settings.ShortcutKey3, out Keys key3);
            if (!convertOK) return;


            if (key1 == Keys.None && key2 == Keys.None)
            {
                keyboardHook.RegisterHotKey(1, Common.ModifierKeys.None, key3);
                return;
            }

            if (key1 != Keys.None && key2 != Keys.None)
            {
                keyboardHook.RegisterHotKey(1, KeyboardHook.KeyToModifierKey(key1) | KeyboardHook.KeyToModifierKey(key2), key3);
                return;
            }

            if (key1 != Keys.None)
            {
                keyboardHook.RegisterHotKey(1, KeyboardHook.KeyToModifierKey(key1), key3);
                return;
            }
            if (key2 != Keys.None)
            {
                keyboardHook.RegisterHotKey(1, KeyboardHook.KeyToModifierKey(key2), key3);
                return;
            }
        }

        private void ClearKeyPress()
        {
            keyboardHook.UnRegisterHotKey(1);
        }
               
        internal void showNoInternetMessage()
        {
            if (internetAvailableCount == 0 && !this.internetAvailable)
            {
                internetAvailableCount++;
                MessageBox.Show(DateTime.Now.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture) + " - No internet connection available. Please reconnect and try again.");
                internetAvailableCount--;
            }
        }

        internal void getWeather(Boolean vAddMessage)
        {
            checkInternetConnection(false);

            if (internetAvailable == false)
            {
                showNoInternetMessage();
                return;
            }

            // Alternative Weather calls HERE:  https://www.obioberoi.com/2018/07/14/how-to-pull-weather-info-into-a-console-app/

            var client = new OpenWeatherAPI.OpenWeatherAPI(Constants.OpenWeatherAPIKey);

            var results = client.Query(settings.Latitude, settings.Longitude);

            if (results == null)
            {
                MessageBox.Show("There was a problem getting the weather. The internet connection may be unavailable.");
            }

            if (results.ValidRequest == false)
            {
                MessageBox.Show("Unable to show the weather for this location.");
                return;
            }

            showWeatherReport(results, vAddMessage);
        }

        internal static void showWeatherReport(Query vResults, Boolean vAddMessage)
        {
            String output = string.Empty;

            if (!string.IsNullOrEmpty(vResults.Name))
            {
                output += "City: " + vResults.Name + "\n";
            }
            output += "--------------------\n";
            output += "Temp (CelsiusCurrent): " + vResults.Main.Temperature.CelsiusCurrent + "°C\n";
            //output += "Temp (CelsiusMinimum): " + vResults.Main.Temperature.CelsiusMinimum + "\n";
            //output += "Temp (CelsiusMaximum): " + vResults.Main.Temperature.CelsiusMaximum + "\n";
            output += "Humidity: " + vResults.Main.Humdity + "\n";
            output += "Pressure: " + vResults.Main.Pressure + "\n";
            output += "Clouds: " + vResults.Clouds.All.ToString(CultureInfo.InvariantCulture) + "\n";
            if (vResults.Rain != null) output += "Rain: " + vResults.Rain.H3 + "\n"; else output += "Rain: 0" + "\n";
            if (vResults.Snow != null) output += "Snow: " + vResults.Snow.H3 + "\n"; else output += "Snow: 0" + "\n";
            output += "Sunrise: " + vResults.Sys.Sunrise + "\n";
            output += "Sunset: " + vResults.Sys.Sunset + "\n";
            output += "Visibility: " + vResults.Visibility + "\n";
            output += "Wind (Degree): " + vResults.Wind.Degree + "°\n";
            output += "Wind (Direction): " + vResults.Wind.Direction + "\n";
            output += "Wind (Gust): " + vResults.Wind.Gust + "\n";
            output += "Speed (MetersPerSecond): " + vResults.Wind.SpeedMetersPerSecond
                + " (" + Math.Round((vResults.Wind.SpeedMetersPerSecond * 60 * 60) / 1000f, 2) + " Kph)"
                + "\n";
            //output += "Weathers: " + vResults.Weathers.Count() + "\n";

            foreach (Weather w in vResults.Weathers)
            {
                output += "Main: " + w.Main + "\n";
                output += "Description: " + w.Description + "\n";
                //output += "  Weathers Icon: " + w.Icon.ToString(CultureInfo.InvariantCulture) + "\n";
            }
            output += "Latitude: " + vResults.Coord.Latitude.ToString(CultureInfo.InvariantCulture) + "\n";
            output += "Longitude: " + vResults.Coord.Longitude.ToString(CultureInfo.InvariantCulture) + "\n";

            if (vAddMessage)
            {
                output += "\n\n";
                output += "INSTRUCTIONS: If this is the correct city, press the Set button. \nOr, search for another city.";
            }

            MessageBox.Show(output);
        }

        private void showNotificationBalloon(string title, string body)
        {
            notifyIcon1.Icon = new Icon(MainFunctions.GetAppIconPath(Properties.Settings.Default.Portable));

            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipClicked += NotifyIcon_BalloonTipClicked;

            if (title != null)
            {
                notifyIcon1.BalloonTipTitle = title;
            }

            if (body != null)
            {
                if (body.Trim().Length > 0)
                {
                    notifyIcon1.BalloonTipText = body;
                }
                else
                {
                    notifyIcon1.BalloonTipText = " ";
                }
            }

            notifyIcon1.ShowBalloonTip(30000);

            // This will let the balloon close after it's 5 second timeout
            // for demonstration purposes. Comment this out to see what happens
            // when dispose is called while a balloon is still visible.1
            //Thread.Sleep(10000);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            //notifyIcon.Dispose();
        }


        #endregion


        private void Quit(Boolean vSaveSettings)
        {
            // clear event messages before closing
            EventMessages = string.Empty;
            AdjustDesktopImages();

            if (formClosing == true && vSaveSettings)
            {
                SaveScreenPosition();
            }
            if (vSaveSettings) SaveSettings(string.Empty, this.settings);

            formClosing = true;

            ClearEventTimers();

            // Unregister hotkey with id 0 before closing the form. You might want to call this more than once with different id values if you are planning to register more than one hotkey.
            ClearKeyPress();
            keyboardHook.Dispose(); // dispose keyboard shortcut

            // OpenhardwareMonitor
            if (thisComputer != null) thisComputer.Close();

            doubleClickTimer.Stop();
            doubleClickTimer.Enabled = false;
            doubleClickTimer.Dispose();

            tmrAdjust.Stop();
            tmrAdjust.Enabled = false;
            tmrAdjust.Dispose();

            tmrWallpaper.Stop();
            tmrWallpaper.Enabled = false;
            tmrWallpaper.Dispose();

            tmrRefresh.Stop();
            tmrRefresh.Enabled = false;
            tmrRefresh.Dispose();

            if (wizardHost != null) wizardHost.Dispose();

            if (threadScanFolder != null) if (threadScanFolder.ThreadState != System.Threading.ThreadState.Stopped) threadScanFolder.Abort();
            threadScanFolder = null;
            
            if (threadChangeWallpaperFolder != null) if (threadChangeWallpaperFolder.ThreadState != System.Threading.ThreadState.Stopped) threadChangeWallpaperFolder.Abort();
            threadChangeWallpaperFolder = null;


            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();

            if (frmSettingsAdvanced != null) frmSettingsAdvanced.Dispose();
            if (frmSettings != null) frmSettings.Dispose();
            if (settings != null) settings.Dispose();

            if (frmWebsites != null) frmWebsites.Dispose();
            if (frmAbout != null) frmAbout.Dispose();
            if (splashScreen != null) splashScreen.Dispose();
            if (frmMetaData != null) frmMetaData.Dispose();
            if (frmThanks != null) frmThanks.Dispose();
            if (frmAbout != null) frmAbout.Dispose();
            if (frmMetaData != null) frmMetaData.Dispose();

            //https://stackoverflow.com/questions/12977924/how-to-properly-exit-a-c-sharp-application
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }
        

    }

}
