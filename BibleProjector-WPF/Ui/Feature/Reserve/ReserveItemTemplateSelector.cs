using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BibleProjector_WPF.View.MainPage
{
    public class ReserveItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ViewModel.MainPage.VMReserveData dataContext = (ViewModel.MainPage.VMReserveData)item;

            if (dataContext.ViewType == ReserveViewType.NormalItem)
                return (DataTemplate)((FrameworkElement)container).FindResource("NormalItem");
            else if (dataContext.ViewType == ReserveViewType.DragPreview)
                return (DataTemplate)((FrameworkElement)container).FindResource("DragPreview");
            else if (dataContext.ViewType == ReserveViewType.DropPreview)
                return (DataTemplate)((FrameworkElement)container).FindResource("DropPreview");
            else
                return null;
        }
    }
}
