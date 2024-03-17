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

using System.ComponentModel;

namespace BibleProjector_WPF
{
    /// <summary>
    /// Reading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Reading : UserControl
    {
        public Reading()
        {
            InitializeComponent();
            ReadingInitialize();
        }

        void ReadingInitialize()
        {
            SetReadingList();
        }



        // 교독문

        // 교독문 리스트
        BindingList<String> ReadingList;




        // ========================================= 교독문 ============================================

        // ======================================== 세팅

        void SetReadingList()
        {
            ReadingList = new BindingList<string>(Database.getReadingTitles());
            ReadingListBox.ItemsSource = ReadingList;
        }

        // ======================================== 교독문 처리

        void ReadingListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ReadingOutput();
        }

        void ReadingListBox_DoubleClick(object sender, RoutedEventArgs e)
        {
            ReadingOutput();
        }

        void ReadingOutputButton_Click(object sender, RoutedEventArgs e)
        {
            ReadingOutput();
        }

        void ReadingOutput()
        {
            if (module.ProgramOption.ReadingFramePath == null)
                MessageBox.Show("교독문 출력 틀ppt를 등록해주세요!", "ppt틀 등록되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (ReadingListBox.SelectedIndex == -1)
                MessageBox.Show("출력할 교독문을 선택해주세요!", "교독문 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                return;// new module.ShowStarter().ReadingShowStart(ReadingListBox.SelectedIndex);
        }

        // ======================================== 교독문 예약처리
        void ReadingReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReadingListBox.SelectedIndex == -1)
                MessageBox.Show("출력할 교독문을 선택해주세요!", "교독문 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                // 예약창에 보내는 데이터
                ViewModel.ReserveManagerViewModel.instance.ReserveDataManager.addReserve(
                    new module.ReadingReserveDataUnit(ReadingListBox.SelectedIndex));
            }
        }
    }
}
