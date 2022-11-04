using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    // 예약항목 데이터 한 단위
    public class ReserveDataUnit
    {
        string displayInfo;
        public String DisplayInfo
        {
            get { return displayInfo; }
            set { displayInfo = value; }
        }
    }

    internal class ReserveManagerViewModel
    {
        public ReserveManagerViewModel()
        {
            reserveList = new ObservableCollection<ReserveDataUnit>();

            // 테스트용 항목들
            reserveList.Add(new ReserveDataUnit() { DisplayInfo="dsaddddddddddddddfdsafsdfadsffdsaafs"});
            reserveList.Add(new ReserveDataUnit());
            reserveList.Add(new ReserveDataUnit());
            reserveList.Add(new ReserveDataUnit());
        }

        ObservableCollection<ReserveDataUnit> reserveList;
        public ObservableCollection<ReserveDataUnit> ReserveList
        {
            get { return reserveList; }
            set { reserveList = value; }
        }

        public void UpButtonClick()
        {
        }

        public void DownButtonClick()
        {
        }

        public void DeleteButtonClick()
        {
        }
    }
}
