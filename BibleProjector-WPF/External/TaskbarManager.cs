using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.External
{
    class TaskbarManager
    {
        private const string IID = "56FDF342-FD6D-11D0-958A-006097C9A090";
        private const string CLSID = "56FDF344-FD6D-11D0-958A-006097C9A090";

        private static readonly Guid CLSID_TaskbarList = new Guid(CLSID);
        private static readonly Guid IID_ITaskbarList = new Guid(IID);

        [System.Runtime.InteropServices.ComImport]
        [System.Runtime.InteropServices.Guid(IID)]
        [System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
        interface ITaskbarList
        {
            void HrInit();

            void AddTab(IntPtr hwnd);

            void DeleteTab(IntPtr hwnd);

            void ActivateTab(IntPtr hwnd);

            void SetActiveAlt(IntPtr hwnd);
        }

        [System.Runtime.InteropServices.DllImport("ole32")]
        private static extern int CoCreateInstance(
            ref Guid rclsid,
            IntPtr pUnkOuter,
            uint dwClsContext,
            ref Guid riid,
            out ITaskbarList ppv);
        private const uint CLSCTX_INPROC_SERVER = 0x1;

        /// <summary>
        /// 주어진 창을 작업 표시줄에서 숨깁니다.
        /// </summary>
        /// <param name="hwnd"></param>
        public static void DeleteFromTaskbar(IntPtr hwnd)
        {
            ITaskbarList taskbar = null;

            try
            {
                Guid clsid = CLSID_TaskbarList;
                Guid iid = IID_ITaskbarList;

                int hr = CoCreateInstance(
                    ref clsid,
                    IntPtr.Zero,
                    CLSCTX_INPROC_SERVER,
                    ref iid,
                    out taskbar);

                if (hr != 0)
                    System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);

                taskbar.HrInit();
                taskbar.DeleteTab(hwnd);
            }
            finally
            {
                if (taskbar != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(taskbar);
            }
        }
    }
}
