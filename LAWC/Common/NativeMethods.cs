using LAWC.Common;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using static LAWC.Common.ScreenExtensions;
using static LAWC.Objects.Taskbar;


namespace LAWC
{

    class NativeMethods
    {

        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        // SCREEN CALLS
        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510(v=vs.85).aspx
        [DllImport("Shcore.dll")]
        internal static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        //[DllImport("User32")]
        //public static extern IntPtr MonitorFromWindow(IntPtr hWnd, int dwFlags);

        [DllImport("user32", EntryPoint = "GetMonitorInfo", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetMonitorInfoEx(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        internal const int CCHDEVICENAME = 32;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MonitorInfoEx
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public UInt32 dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string szDeviceName;
            //            public Size dpi;
        }

        //https://stackoverflow.com/questions/5977445/how-to-get-windows-display-settings/14283331
        [DllImport("user32.dll")]
        internal static extern int GetDpiForWindow(IntPtr hWnd);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062(v=vs.85).aspx
        [DllImport("User32.dll")]
        internal static extern IntPtr MonitorFromPoint([In]System.Drawing.Point pt, [In]uint dwFlags);

        [DllImport("SHCore.dll", SetLastError = true)]
        internal static extern bool SetProcessDpiAwareness(ProcessDPIAwareness awareness);

        [DllImport("SHCore.dll", SetLastError = true)]
        internal static extern void GetProcessDpiAwareness(IntPtr hprocess, out ProcessDPIAwareness awareness);


        internal static readonly int WM_SHOWFIRSTINSTANCE =
            NativeMethods.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);

        [DllImport("user32.dll")]
        internal static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern int IsIconic(IntPtr hWnd);
                       
        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal UInt16[] Blue;
        }

        [DllImport("gdi32.dll")]
        internal static extern bool SetDeviceGammaRamp(IntPtr hDC, ref NativeMethods.RAMP lpRamp);

        [DllImport("gdi32.dll")]
        internal static extern bool GetDeviceGammaRamp(IntPtr hDC, ref NativeMethods.RAMP lpRamp);

        
        // PARTS FROM http://stackoverflow.com/questions/17848090/how-to-get-a-smooth-color-change
        //The colorization parameters can be found under the key "HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"

        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        internal static extern void DwmGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);

        [DllImport("dwmapi.dll", EntryPoint = "#131", PreserveSig = false)]
        internal static extern void DwmSetColorizationParameters(ref DWM_COLORIZATION_PARAMS parameters,
                                                                bool unknown);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        internal static extern bool DwmIsCompositionEnabled();
        // Gets or sets the current color used for DWM glass, based on the user's color scheme.
        
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
               

        // WIN API
        [DllImport("user32", CharSet = CharSet.Unicode)]
        internal static extern int RegisterWindowMessage(string message);

        internal static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(CultureInfo.InvariantCulture, format, args);
            return RegisterWindowMessage(message);
        }

        internal const int HWND_BROADCAST = 0xffff;
        internal const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        internal static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        
        [DllImport("user32")]
        internal static extern bool SendNotifyMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        
        // Program.cs
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();


        // ===============================================
        // Borderless Moveable Form

        internal const int WM_NCLBUTTONDOWN = 0xA1;
        internal const int HT_CAPTION = 0x2;

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern int SendMessageW(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        internal static extern bool ReleaseCapture();


        //==================================================




    }
}
