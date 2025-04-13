using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class BibleData : ShowData
    {
        public int book { get; set; } = -1;
        public int chapter { get; set; } = -1;
        public int verse { get; set; } = -1;

        string bibleTitle = null;
        string bibleTitle_abr = null;
        ShowContentData[] currentContents = null;

        public BibleData(int book, int chapter, int verse)
        {
            this.book = -1;
            this.chapter = -1;
            this.verse = -1;
            this.bibleTitle = null;
            this.bibleTitle_abr = null;
            this.currentContents = null;
            if (book >= 1 && book <= 66)
            {
                this.book = book;
                if (chapter >= 1 && chapter <= Database.getChapterCount(book.ToString("D2")))
                {
                    this.chapter = chapter;
                    if (verse >= 1 && verse <= Database.getVerseCount(book.ToString("D2") + chapter.ToString("D3")))
                        this.verse = verse;
                }
            }
        }

        public string getBibleTitle()
        {
            if (bibleTitle == null)
                bibleTitle = (book == -1
                ? null
                : Database.getTitle(book.ToString("D2")));
            return bibleTitle;
        }

        public string getBibleTitle_Abr()
        {
            if (bibleTitle_abr == null)
                bibleTitle_abr = (book == -1
                ? null
                : Database.getAbrTitle(book.ToString("D2")));
            return bibleTitle_abr;
        }

        public string getBibleContent()
        {
            if (verse != -1)
                return Database.getBible(book.ToString("D2") + chapter.ToString("D3") + verse.ToString("D3"));
            return null;
        }

        // ================ ShowData 메소드 ================

        public override string getdisplayName(bool isModified)
        {
            string displayName;

            displayName = getBibleTitle();
            if (chapter != -1)
                displayName += $" {chapter}장 ";
            if (verse != -1)
                displayName += $"{verse}절";

            return displayName;
        }

        public override string getPreviewContent()
        {
            return getBibleContent();
        }

        public override string getTitle1()
        {
            return getBibleTitle();
        }

        public override string getTitle2()
        {
            return chapter + "장 " + verse + "절";
        }

        public override ShowContentData[] getContents()
        {
            string[] contents = StringModifier.makeStringPage(
                getBibleContent()
                , ProgramOption.Bible_CharPerLine
                , ProgramOption.Bible_LinePerSlide);

            currentContents = new ShowContentData[contents.Length];
            int i = 0;
            foreach (string item in contents)
                currentContents[i++] = new ShowContentData(item, item, false);

            return currentContents;
        }

        public override ShowData getNextShowData()
        {
            string Kjjeul = Database.getBibleIndex_Next(book.ToString("D2") + chapter.ToString("D3") + verse.ToString("D3"));
            return
                new BibleData(
                    int.Parse(Kjjeul.Substring(0, 2)),
                    int.Parse(Kjjeul.Substring(2, 3)),
                    int.Parse(Kjjeul.Substring(5, 3)));
        }

        public override ShowData getPrevShowData()
        {

            string Kjjeul = Database.getBibleIndex_Previous(book.ToString("D2") + chapter.ToString("D3") + verse.ToString("D3"));
            return
                new BibleData(
                    int.Parse(Kjjeul.Substring(0, 2)),
                    int.Parse(Kjjeul.Substring(2, 3)),
                    int.Parse(Kjjeul.Substring(5, 3)));
        }

        public override ShowContentType getDataType()
        {
            return ShowContentType.Bible;
        }

        public override bool isSameData(ShowData data)
        {
            if (data.GetType() == this.GetType())
                if (book == ((BibleData)data).book
                    && chapter == ((BibleData)data).chapter
                    && verse == ((BibleData)data).verse)
                    return true;

            return false;
        }

        public override ShowExcuteErrorEnum canExcuteShow()
        {
            if (ProgramOption.BibleFramePath == null)
                return ShowExcuteErrorEnum.NoneFrameFile;
            else if (!isAvailData())
                return ShowExcuteErrorEnum.InvalidData;
            else
                return ShowExcuteErrorEnum.NoErrors;
        }

        public override bool isAvailData()
        {
            return (verse != -1);
        }
    }
}
