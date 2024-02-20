using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleSearchViewModel : ViewModel
    {

        string searchText_in = "";
        public string searchText { get { return searchText_in; } set { searchText_in = value; } }

        public ObservableCollection<module.Data.BibleSearchData> searchList { get; set; } = new ObservableCollection<module.Data.BibleSearchData>();

        public bool popupOpen { get; set; } = false;

        module.BibleSearcher searcher = new module.BibleSearcher();

        // ====================================== 메소드

        public void searchTextChanged()
        {
            searchList.Clear();
            if (searchText.Length != 0)
            {
                foreach (module.Data.BibleSearchData r in searcher.getSearchResult(searchText))
                    searchList.Add(r);

                popupOpen = true;
                OnPropertyChanged("popupOpen");
            }
        }
    }
}
