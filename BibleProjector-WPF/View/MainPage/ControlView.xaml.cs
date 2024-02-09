using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibleProjector_WPF.View.MainPage
{
    /// <summary>
    /// ControlView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ControlView : UserControl
    {
        public ControlView()
        {
            InitializeComponent();
        }

        private void testMouseMove(object sender, MouseEventArgs e)
        {
            ListBoxItem obj = sender as ListBoxItem;

            if (obj != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData(typeof(ListBoxItem), obj);
                DragDrop.DoDragDrop(obj,
                                     data,
                                     DragDropEffects.Move);
                Console.WriteLine("드래그의 시작");
            }
        }
        
        private void testDragOver(object sender, DragEventArgs e)
        {
            
            Point pos = e.GetPosition((IInputElement)sender);
            Console.WriteLine(e.Data.GetData(typeof(ListBoxItem)).ToString());
            ((ListBoxItem)e.Data.GetData(typeof(ListBoxItem))).RenderTransform = new TranslateTransform(pos.X, pos.Y);
            //Console.WriteLine("x : {0}, y : {1}",pos.X,pos.Y);
        }
    }
}
