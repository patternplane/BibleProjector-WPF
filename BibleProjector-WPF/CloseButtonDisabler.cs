using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace BibleProjector_WPF
{
    public static class CloseButtonDisabler
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;

        const uint SC_CLOSE = 0xF060;

        public static void DisableCloseButton(this System.Windows.Window window)
        {
            // Disable close button
            IntPtr hwnd = new WindowInteropHelper(window).EnsureHandle();
            IntPtr hMenu = GetSystemMenu(hwnd, false);

            if (hMenu != IntPtr.Zero)
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
        }
    }
}
