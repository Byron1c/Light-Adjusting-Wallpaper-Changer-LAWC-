// FROM: http://www.codeproject.com/Articles/32908/C-Single-Instance-App-With-the-Ability-To-Restore
using System;

namespace LAWC.Common
{
    static public class WinApi
    {
        
        internal static void ShowToFront(IntPtr window)
        {
            _ = NativeMethods.ShowWindow(window, NativeMethods.SW_SHOWNORMAL);
            _ = NativeMethods.SetForegroundWindow(window);
        }


    }
}
