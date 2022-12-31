using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class BibleSongBase : IReserveOptionViewModel, INotifyPropertyChanged
    {

        // ========================== 속성 ==========================

        enum previewMode
        {
            CurrentItem,
            SearchItem
        }
        previewMode _currentPreviewMode = previewMode.CurrentItem;
        previewMode currentPreviewMode { get { return _currentPreviewMode; } set { _currentPreviewMode = value; NotifyPropertyChanged(nameof(currentPreviewMode)); } }

        const int BibleViewModel = 0;
        const int SongViewModel = 1;
        IReserveOptionViewModel[] viewModels = { new Bible(), new Song()};

        // ========================== 바인딩 속성 ==========================

        public bool PreviewShowCurrentItem { get { return (currentPreviewMode == previewMode.CurrentItem); } }
        public bool PreviewShowSearchItem { get { return (currentPreviewMode == previewMode.SearchItem); } }

        IReserveOptionViewModel _SelectedViewModel = null;
        public IReserveOptionViewModel SelectedViewModel { get { return _SelectedViewModel; } set { _SelectedViewModel = value; NotifyPropertyChanged(nameof(SelectedViewModel)); } }

        string _PreviewText;
        public string PreviewText { get { return _PreviewText; } set { _PreviewText = value; previewTextChanged(); } }

        // ========================== 메소드 ==========================

        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            switch (data[0].reserveType)
            {
                case module.ReserveType.Bible:
                    SelectedViewModel = viewModels[BibleViewModel];
                    break;
                case module.ReserveType.Song:
                    SelectedViewModel = viewModels[SongViewModel];
                    break;
                default:
                    break;
            }

            SelectedViewModel.GiveSelection(data);
        }

        public void ShowContent()
        {
            SelectedViewModel.ShowContent();
        }

        public void setPreviewCurrentItem()
        {
            currentPreviewMode = previewMode.CurrentItem;
        }

        public void setPreviewSearchItem()
        {
            currentPreviewMode = previewMode.SearchItem;
        }

        void previewTextChanged()
        {
            Console.WriteLine("텍스트 변경됨");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
