using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.External
{
    class WindowManager
    {
        // ===================== Win APIs =====================

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        private const int HWND_NOTOPMOST = -2;
        private const int HWND_TOPMOST = -1;
        private const int HWND_TOP = 0;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_NOZORDER = 0x0004;

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern Int32 SetWindowLong(int hWnd, Int32 nIndex, Int32 dwNewLong);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern Int32 GetWindowLong(int hWnd, Int32 nIndex);
        private const Int32 GWL_STYLE = -16;
        private const Int32 WS_VISIBLE = 0x10000000;
        private const Int32 WS_EX_TOOLWINDOW = 0x00000080;
        private const Int32 WS_EX_APPWINDOW = 0x00040000;
        private const Int32 WS_EX_NOACTIVATE = 0x08000000;

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int GetWindow(int hWnd, uint uCmd);
        private const uint GW_HWNDNEXT = 2;
        private const uint GW_HWNDPREV = 3;

        // ===================== Util Methods =====================

        /// <summary>
        /// 제시된 <paramref name="windowHWND"/>의 바로 전방 창 핸들을 조회합니다.
        /// </summary>
        /// <param name="windowHWND"></param>
        /// <returns></returns>
        public static int GetFrontWindowOf(int windowHWND)
        {
            return GetWindow(windowHWND, GW_HWNDPREV);
        }

        /// <summary>
        /// 기본 창 표시 정책에 따라 주어진 핸들의 창을 재설정 및 표시합니다.
        /// <br/> 정책 :
        /// <br/>- 작업표시줄에 Windows를 표시하지 않습니다.
        /// <br/>- 기존 위치를 유지하거나, <paramref name="frontWindowHWND"/>의 바로 뒤로 위치시킬 수 있습니다.
        /// </summary>
        /// <param name="windowHWND"></param>
        /// <param name="frontWindowHWND"></param>
        public static void ReshowWindow(int windowHWND, int? frontWindowHWND = null)
        {
            Int32 style = GetWindowLong(windowHWND, GWL_STYLE);

            style |= WS_VISIBLE;
            style &= ~(WS_EX_APPWINDOW);
            style |= WS_EX_TOOLWINDOW;
            style |= WS_EX_NOACTIVATE;

            ShowWindow(windowHWND, SW_HIDE);
            if (frontWindowHWND == null)
                SetWindowPos(windowHWND, 0, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_NOZORDER);
            else
                SetWindowPos(windowHWND, (int)frontWindowHWND, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            SetWindowLong(windowHWND, GWL_STYLE, style);
        }

        /// <summary>
        /// 창을 숨깁니다.
        /// </summary>
        /// <param name="windowHWND"></param>
        public static void hideWindow(int windowHWND)
        {
            ShowWindow(windowHWND, SW_HIDE);
        }

        /// <summary>
        /// 창을 가장 전방으로 표시합니다.
        /// </summary>
        /// <param name="windowHWND"></param>
        public static void TopMostWindow(int windowHWND)
        {
            SetWindowPos(windowHWND, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            SetWindowPos(windowHWND, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            //SetWindowPos(windowHWND, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
    }
}
