using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMModify : ViewModel
    {
        // ============ Events ============

        public event EventHandler CloseEventHandler;

        // ============ Binding Properties ============

        public ICommand CClose { get; }
        public ICommand CContentUpdated { get; }
        public ICommand CRemoveLinefeed { get; }

        public bool IsHymn { get; private set; }

        public bool CanNotModifyTitle { get; private set; } = false;

        private string _TitleText = "";
        public string TitleText 
        {
            get { return _TitleText; }
            set
            {
                titleModified = true;
                _TitleText = value;
                applyModifiedTitle(_TitleText); // Binding Update Trigger : LostFocus
            }
        }
        private void applyModifiedTitle(string title)
        {
            if (!titleModified)
                return;
            titleModified = false;

            updateTitle(title);
        }

        public BindingList<string> HymnList { get; set; } = new BindingList<string>();
        private int _HymnSelectedIdx;
        public int HymnSelectedIdx 
        {
            get { return _HymnSelectedIdx; }
            set { _HymnSelectedIdx = value; initialzeHymnData(_HymnSelectedIdx); }
        }

        public bool IsMultiLineDeleteButtonEnable { get; private set; } = false;

        public bool ContentWrapping { get; private set; } = false;

        private string _ContentText = "";
        public string ContentText
        {
            get { return _ContentText; }
            set
            {
                contentModified = true;
                _ContentText = module.StringModifier.makeCorrectNewline(value); // Binding Update Trigger : PropertyChanged
                IsMultiLineDeleteButtonEnable = module.StringModifier.hasMultiLinefeeds(_ContentText);
                OnPropertyChanged(nameof(IsMultiLineDeleteButtonEnable));
            }
        }
        private void applyModifiedContent(string content)
        {
            if (!contentModified)
                return;
            contentModified = false;

            updateContent(content);
        }

        // ============ Properties ============

        private bool titleModified = false;
        private bool contentModified = false;

        private module.Data.ShowData currentData;

        private module.Data.SongManager songManager;

        // ============ Setup ============

        public VMModify(module.Data.SongManager songManager)
        {
            this.songManager = songManager;
            
            CClose = new RelayCommand(obj => close());
            CContentUpdated = new RelayCommand(obj => applyModifiedContent(_ContentText));
            CRemoveLinefeed = new RelayCommand(obj => removeLinefeedsInContent());
        }

        // ============ Methods ============

        public void setupData(module.Data.ShowData data)
        {
            currentData = data;

            if (data.getDataType() == ShowContentType.Song)
            {
                module.Data.SongData typedData = (module.Data.SongData)data;
                if (typedData.songType == module.Data.SongDataTypeEnum.CCM)
                {
                    IsHymn = false;
                    OnPropertyChanged(nameof(IsHymn));
                    initalizeTitle(typedData.songTitle);
                    initalizeContent(typedData.songContent.getRawContent());
                    return;
                }

                if (typedData.songType == module.Data.SongDataTypeEnum.HYMN)
                {
                    IsHymn = true;
                    OnPropertyChanged(nameof(IsHymn));

                    HymnList.Clear();
                    int i = 1;
                    while (i <= typedData.songContent.lyricCount)
                        HymnList.Add(string.Format("{0}장-{1}절 ({2})", typedData.songTitle, i++, typedData.getTitle2()));

                    HymnSelectedIdx = 0;
                    OnPropertyChanged(nameof(HymnSelectedIdx));
                    return;
                }
            }

            if (data.getDataType() == ShowContentType.Bible)
            {
                module.Data.BibleData typedData = (module.Data.BibleData)data;
                string title = string.Format("{0} {1}장 {2}절", typedData.getBibleTitle(), typedData.chapter, typedData.verse);
                initalizeTitle(title, true);
                initalizeContent(typedData.getBibleContent(), true);
                return;
            }
        }

        private void close()
        {
            CloseEventHandler?.Invoke(this, null);
        }

        private void initialzeHymnData(int idx)
        {
            if (idx >= 0 && idx < HymnList.Count)
                initalizeContent(((module.Data.SongData)currentData).songContent.getContentByVerse(idx));
            else
                initalizeContent("");
        }

        private void initalizeTitle(string title, bool canNotModify = false)
        {
            CanNotModifyTitle = canNotModify;
            OnPropertyChanged(nameof(CanNotModifyTitle));
            _TitleText = title;
            OnPropertyChanged(nameof(TitleText));
        }

        private void initalizeContent(string content, bool doWrapContent = false)
        {
            ContentWrapping = doWrapContent;
            OnPropertyChanged(nameof(ContentWrapping));
            _ContentText = content;
            OnPropertyChanged(nameof(ContentText));

            IsMultiLineDeleteButtonEnable = module.StringModifier.hasMultiLinefeeds(_ContentText);
            OnPropertyChanged(nameof(IsMultiLineDeleteButtonEnable));
        }

        private void updateTitle(string title)
        {
            if (currentData.getDataType() == ShowContentType.Song
                && ((module.Data.SongData)currentData).songType == module.Data.SongDataTypeEnum.CCM)
            {
                ((module.Data.SongData)currentData).songTitle = title;
                songManager.saveCCMData(false);
            }
        }

        private void updateContent(string content)
        {
            if (currentData.getDataType() == ShowContentType.Song)
            {
                if (((module.Data.SongData)currentData).songType == module.Data.SongDataTypeEnum.CCM)
                {
                    ((module.Data.SongData)currentData).songContent.setContent(content, 0);
                    songManager.saveCCMData(false);
                }

                if (((module.Data.SongData)currentData).songType == module.Data.SongDataTypeEnum.HYMN)
                {
                    ((module.Data.SongData)currentData).songContent.setContent(content, HymnSelectedIdx);
                    songManager.saveHymnData(false);
                }
            }

            if (currentData.getDataType() == ShowContentType.Bible)
            {
                module.Data.BibleData typedData = (module.Data.BibleData)currentData;
                string address = typedData.book.ToString("D2") + typedData.chapter.ToString("D3") + typedData.verse.ToString("D3");
                Database.updateBible(address, content);
            }
        }

        private void removeLinefeedsInContent()
        {
            _ContentText = module.StringModifier.RemoveMultiLinefeeds(_ContentText);
            OnPropertyChanged(nameof(ContentText));
            updateContent(ContentText);

            IsMultiLineDeleteButtonEnable = false;
            OnPropertyChanged(nameof(IsMultiLineDeleteButtonEnable));
        }
    }
}
