using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleModifyViewModel : NotifyPropertyChanged
    {
        // =============================== 속성 =============================

        private string OriginContent_in;
        public string OriginContent { get { return OriginContent_in; } set { OriginContent_in = value; OnPropertyChanged(); } }
        private string UserModifyContent_in;
        public string UserModifyContent { get { return UserModifyContent_in; } set { UserModifyContent_in = value; OnPropertyChanged(); } }

        // =============================== 세팅 =============================

        string currentKjjeul = null;

        public BibleModifyViewModel(string Kjjeul)
        {
            setData(Kjjeul);
        }

        public void setData(string Kjjeul)
        {
            currentKjjeul = Kjjeul;

            UserModifyContent = OriginContent = Database.getBible(currentKjjeul);
        }

        // ============================ 메서드 ============================ 

        public void save()
        {
            if (System.Windows.MessageBox.Show("변경값을 저장하시겠습니까?","변경값 저장 확인", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Cancel)
                return;

            Database.updateBible(currentKjjeul, UserModifyContent);
            OriginContent = Database.getBible(currentKjjeul);
        }

        public void reset()
        {
            if (System.Windows.MessageBox.Show("변경을 초기화하시겠습니까?", "수정 취소 확인", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Cancel)
                return;

            UserModifyContent = Database.getBible(currentKjjeul);
        }
    }
}
