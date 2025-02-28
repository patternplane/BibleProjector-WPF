using BibleProjector_WPF.module.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMBiblePhraseSearchData : ViewModel, IVMPreviewData
    {
        public string address { get; set; }
        public HighlightedText contentInfo { get; set; }

        public string DisplayTitle { get { return (isModified ? "[수정됨] " : "") + data.getdisplayName(isModified); } }
        public string PreviewContent { get { return data.getBibleContent(); } }
        public ShowContentType Type { get { return ShowContentType.Bible; } }

        private BibleData data;
        private bool isModified;

        public VMBiblePhraseSearchData(string kjjeul, string content, (int, int)[] pos)
        {
            data = new BibleData(
                int.Parse(kjjeul.Substring(0, 2)),
                int.Parse(kjjeul.Substring(2, 3)),
                int.Parse(kjjeul.Substring(5, 3)));
            address = $"{data.getBibleTitle_Abr()} {data.chapter}:{data.verse} ";
            contentInfo = new HighlightedText(content, pos, HighlightType.SEARCH_RESULT);
        }

        public void update()
        {
            isModified = true;
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
            contentInfo = new HighlightedText("[수정됨] " + data.getBibleContent(), new (int, int, HighlightType)[] { (0, 4, HighlightType.DEFAULT_HIGHLIGHT) });
            OnPropertyChanged(nameof(contentInfo));
        }

        public ShowData getData()
        {
            return data;
        }
    }
}
