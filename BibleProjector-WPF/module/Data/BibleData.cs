using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class BibleData : ShowData
    {
        public int book { get; private set; } = -1;
        public int chapter { get; private set; } = -1;
        public int verse { get; private set; } = -1;

        string bibleTitle = null;
        string bibleTitle_abr = null;
        ShowContentData[] currentContents = null;

        // 메모리 누수의 문제 있음
        private static event Action<string> BibleUpdated;

        private void OnBibleUpdated()
        {
            BibleUpdated?.Invoke(getAddress());
        }

        private void bibleUpdatedHandler(string address)
        {
            if (getAddress().Equals(address))
                OnItemUpdated();
        }

        public BibleData(int book, int chapter, int verse)
        {
            BibleUpdated += bibleUpdatedHandler;
            setData(book, chapter, verse);
        }

        public void setData(int book, int chapter, int verse)
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
                if (chapter >= 1 && chapter <= Database.getChapterCount(getAddress(1)))
                {
                    this.chapter = chapter;
                    if (verse >= 1 && verse <= Database.getVerseCount(getAddress(2)))
                        this.verse = verse;
                }
            }

            OnItemUpdated();
        }

        public string getBibleTitle()
        {
            if (bibleTitle == null)
                bibleTitle = (book == -1
                ? null
                : Database.getTitle(getAddress(1)));
            return bibleTitle;
        }

        public string getBibleTitle_Abr()
        {
            if (bibleTitle_abr == null)
                bibleTitle_abr = (book == -1
                ? null
                : Database.getAbrTitle(getAddress(1)));
            return bibleTitle_abr;
        }

        /// <summary>
        /// 현재 절의 내용을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public string getBibleContent()
        {
            if (verse != -1)
                return Database.getBible(getAddress());
            return null;
        }

        /// <summary>
        /// 성경구절을 수정합니다.
        /// </summary>
        public void modifyContent(string content)
        {
            Database.updateBible(getAddress(), content);
            OnBibleUpdated();
            OnItemUpdated();
        }

        /// <summary>
        /// DB 검색 주소를 반환합니다.
        /// <br/> <paramref name="step"/>은 주소 범위 수준을 나타냅니다. (1:책, 2:책/장, 3:책/장/절)
        /// </summary>
        /// <param name="step"></param>
        private string getAddress(int step = 3)
        {
            if (step < 1 || step > 3)
                step = 3;

            string address = book.ToString("D2");
            if (step > 1)
                address += chapter.ToString("D3");
            if (step > 2)
                address += verse.ToString("D3");

            return address;
        }

        // ================ ShowData 메소드 ================

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
            string Kjjeul = Database.getBibleIndex_Next(getAddress());
            return
                new BibleData(
                    int.Parse(Kjjeul.Substring(0, 2)),
                    int.Parse(Kjjeul.Substring(2, 3)),
                    int.Parse(Kjjeul.Substring(5, 3)));
        }

        public override ShowData getPrevShowData()
        {

            string Kjjeul = Database.getBibleIndex_Previous(getAddress());
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
