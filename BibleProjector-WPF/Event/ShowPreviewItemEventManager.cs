using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class ShowPreviewItemEventManager
    {
        public delegate void showPreviewItemEventHandler(ViewModel.MainPage.IVMPreviewData data);
        public delegate void showPreviewItemDefaultEventHandler(module.Data.IPreviewData data);
        public event showPreviewItemEventHandler showPreviewItemEvent;
        public event showPreviewItemDefaultEventHandler showPreviewItemDefaultEvent;

        public void InvokeShowPreviewItem(ViewModel.MainPage.IVMPreviewData data)
        {
            showPreviewItemEvent?.Invoke(data);
        }

        public void InvokeShowPreviewItem(module.Data.IPreviewData data)
        {
            showPreviewItemDefaultEvent?.Invoke(data);
        }
    }
}
