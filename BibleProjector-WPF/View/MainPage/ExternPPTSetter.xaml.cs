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
    /// ExternPPTSetter.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExternPPTSetter : UserControl
    {
        public ExternPPTSetter()
        {
            InitializeComponent();

            this.SetBinding(ShowRunCommandProperty, new Binding("CShowRun"));
            this.SetBinding(DoReserveCommandProperty, new Binding("CDoReserve"));
        }

        // ========== BindingProperties ==========

        System.Reflection.PropertyInfo _CAddPPTFileProperty = null;
        ICommand getCAddPPTFileProperty(object obj)
        {
            if (_CAddPPTFileProperty == null)
                _CAddPPTFileProperty = obj.GetType().GetProperty("CAddPPTFile")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CAddPPTFileProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CEditPPTFileProperty = null;
        ICommand getCEditPPTFileProperty(object obj)
        {
            if (_CEditPPTFileProperty == null)
                _CEditPPTFileProperty = obj.GetType().GetProperty("CEditPPTFile")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CEditPPTFileProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CRefreshPPTFileProperty = null;
        ICommand getCRefreshPPTFileProperty(object obj)
        {
            if (_CRefreshPPTFileProperty == null)
                _CRefreshPPTFileProperty = obj.GetType().GetProperty("CRefreshPPTFile")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CRefreshPPTFileProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CDeletePPTFileProperty = null;
        ICommand getCDeletePPTFileProperty(object obj)
        {
            if (_CDeletePPTFileProperty == null)
                _CDeletePPTFileProperty = obj.GetType().GetProperty("CDeletePPTFile")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CDeletePPTFileProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _AddPPTFileErrorProperty = null;
        int getAddPPTFileErrorProperty(object obj)
        {
            if (_AddPPTFileErrorProperty == null)
                _AddPPTFileErrorProperty = obj.GetType().GetProperty("AddPPTFileError")
                    ?? throw new Exception("Binding Error");

            return (int)_AddPPTFileErrorProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _RefreshPPTFileErrorProperty = null;
        int getRefreshPPTFileErrorProperty(object obj)
        {
            if (_RefreshPPTFileErrorProperty == null)
                _RefreshPPTFileErrorProperty = obj.GetType().GetProperty("RefreshPPTFileError")
                    ?? throw new Exception("Binding Error");

            return (int)_RefreshPPTFileErrorProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _MaxSlideSizeProperty = null;
        int getMaxSlideSizeProperty(object obj)
        {
            if (_MaxSlideSizeProperty == null)
                _MaxSlideSizeProperty = obj.GetType().GetProperty("MaxSlideSize")
                    ?? throw new Exception("Binding Error");

            return (int)_MaxSlideSizeProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _FilePathProperty = null;
        string getFilePathProperty(object obj)
        {
            if (_FilePathProperty == null)
                _FilePathProperty = obj.GetType().GetProperty("FilePath")
                    ?? throw new Exception("Binding Error");

            return (string)_FilePathProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _HasItemProperty = null;
        bool getHasItemProperty(object obj)
        {
            if (_HasItemProperty == null)
                _HasItemProperty = obj.GetType().GetProperty("HasItem")
                    ?? throw new Exception("Binding Error");

            return (bool)_HasItemProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _OnShiftProperty = null;
        bool getOnShiftProperty(object obj)
        {
            if (_OnShiftProperty == null)
                _OnShiftProperty = obj.GetType().GetProperty("OnShift")
                    ?? throw new Exception("Binding Error");

            return (bool)_OnShiftProperty.GetValue(obj);
        }

        public static readonly DependencyProperty ShowRunCommandProperty =
        DependencyProperty.Register(
            name: "ShowRunCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(ExternPPTSetter));

        public ICommand ShowRunCommand
        {
            get => (ICommand)GetValue(ShowRunCommandProperty);
            set => SetValue(ShowRunCommandProperty, value);
        }

        public static readonly DependencyProperty DoReserveCommandProperty =
        DependencyProperty.Register(
            name: "DoReserveCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(ExternPPTSetter));

        public ICommand DoReserveCommand
        {
            get => (ICommand)GetValue(DoReserveCommandProperty);
            set => SetValue(DoReserveCommandProperty, value);
        }

        // ========== EventHander ==========

        private void EH_MainButtonClick(object sender, RoutedEventArgs e)
        {
            if (getHasItemProperty(this.DataContext))
                ShowRunCommand.Execute(null);
        }

        private void EH_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            if (getOnShiftProperty(this.DataContext))
                DeletePPTTask();
            else
            {
                if (getHasItemProperty(this.DataContext))
                    RefreshPPTTask();
                else
                    AddPPTTask();
            }
        }

        private void EH_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            if (getOnShiftProperty(this.DataContext))
                DoReserveCommand.Execute(null);
            else
                EditPPTTask();
        }

        // ========== Sub Event Tasks ==========

        void AddPPTTask()
        {
            if (ExternPPTFileDialog.FD_ExternPPT.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (getCAddPPTFileProperty(this.DataContext).CanExecute(ExternPPTFileDialog.FD_ExternPPT.FileName))
                getCAddPPTFileProperty(this.DataContext).Execute(ExternPPTFileDialog.FD_ExternPPT.FileName);
            else
            {
                int errorCode = getAddPPTFileErrorProperty(this.DataContext);
                if (errorCode == 1)
                    MessageBox.Show("너무 큰 용량의 PPT를 등록하려 했습니다.\r\n한 PPT에 허용되는 최대 슬라이드 수는 " + getMaxSlideSizeProperty(this.DataContext) + "개 입니다.", "너무 큰 파일 등록", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (errorCode == 3)
                    MessageBox.Show("이미 등록된 파일입니다.", "중복 등록", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("알 수 없는 오류 : " + errorCode, "알 수 없는 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void RefreshPPTTask()
        {
            if (getCRefreshPPTFileProperty(this.DataContext).CanExecute(null))
                getCRefreshPPTFileProperty(this.DataContext).Execute(null);
            else
            {
                int errorCode = getRefreshPPTFileErrorProperty(this.DataContext);
                if (errorCode == 1)
                    MessageBox.Show("편집한 PPT의 슬라이드 수가 너무 많습니다.\r\n한 PPT에 허용되는 최대 슬라이드 수는 " + getMaxSlideSizeProperty(this.DataContext) + "개 입니다.", "슬라이드 수 초과", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (errorCode == 2)
                    MessageBox.Show("해당 PPT파일의 원본이 없어 새로고침하지 못했습니다!\n경로 : " + getFilePathProperty(this.DataContext), "파일 없음", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("알 수 없는 오류 : " + errorCode, "알 수 없는 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void DeletePPTTask()
        {
            if (getCDeletePPTFileProperty(this.DataContext).CanExecute(null))
                getCDeletePPTFileProperty(this.DataContext).Execute(null);
        }

        void EditPPTTask()
        {
            if (getHasItemProperty(this.DataContext))
            {
                if (!getCEditPPTFileProperty(this.DataContext).CanExecute(null))
                    MessageBox.Show("해당 PPT파일이 제거되어 열지 못했습니다!\n경로 : " + getFilePathProperty(this.DataContext), "파일 없음", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    getCEditPPTFileProperty(this.DataContext).Execute(null);
            }
        }
    }
}
