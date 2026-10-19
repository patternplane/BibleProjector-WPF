using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMLyricAdd : ViewModel
    {
        // ============ Events ============

        public event EventHandler CloseEventHandler;
        public event Action<object, module.Data.SongData> NewItemAddedEventHandler;

        // ============ Binding Properties ============

        public ICommand CAddNewLyric { get; }
        public ICommand CClose { get; }
        public ICommand CRemoveLinefeed { get; }

        public string TitleText { get; set; } = "";

        public bool IsMultiLineDeleteButtonEnable { get; private set; } = false;

        private string _ContentText = "";
        public string ContentText
        {
            get { return _ContentText; }
            set
            {
                _ContentText = module.StringModifier.makeCorrectNewline(value);
                IsMultiLineDeleteButtonEnable = module.StringModifier.hasMultiLinefeeds(_ContentText);
                OnPropertyChanged(nameof(IsMultiLineDeleteButtonEnable));
            }
        }
        
        // ============ Properties ============

        private module.Data.SongManager songManager;

        // ============ Setup ============

        public VMLyricAdd(module.Data.SongManager songManager)
        {
            this.songManager = songManager;

            CAddNewLyric = new RelayCommand(obj => addNewLyric(), obj => canAddNewLyric());
            CClose = new RelayCommand(obj => close());
            CRemoveLinefeed = new RelayCommand(obj => removeLinefeedsInContent());
        }

        // ============ Methods ============

        private void close()
        {
            CloseEventHandler?.Invoke(this, null);
        }

        private bool canAddNewLyric()
        {
            if (TitleText.Trim().Length == 0)
            {
                System.Windows.MessageBox.Show("제목을 입력해주세요!", "곡 추가 오류", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void addNewLyric()
        {
            // 빈 제목
            if (TitleText.Trim().Length == 0)
                return;

            module.Data.SongData newSong = 
                new module.Data.SongData(
                    module.StringModifier.makeCorrectNewline(TitleText),
                    new module.Data.SongContent(module.StringModifier.makeCorrectNewline(ContentText)),
                    module.Data.SongDataTypeEnum.CCM,
                    null);

            songManager.AddSongInOrder(newSong);

            TitleText = "";
            OnPropertyChanged(nameof(TitleText));
            ContentText = "";
            OnPropertyChanged(nameof(ContentText));

            NewItemAddedEventHandler?.Invoke(this, newSong);
        }

        private void removeLinefeedsInContent()
        {
            _ContentText = module.StringModifier.RemoveMultiLinefeeds(_ContentText);
            OnPropertyChanged(nameof(ContentText));

            IsMultiLineDeleteButtonEnable = false;
            OnPropertyChanged(nameof(IsMultiLineDeleteButtonEnable));
        }
    }
}
