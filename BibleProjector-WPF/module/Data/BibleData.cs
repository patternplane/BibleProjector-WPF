using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class BibleData
    {
        public int book { get; } = -1;
        public int chapter { get; } = -1;
        public int verse { get; } = -1;

        string bibleTitle = null;
        string bibleTitle_abr = null;
        string bibleContent = null;

        public BibleData(int book, int chapter, int verse)
        {
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
            if (verse != -1
                && bibleContent == null)
                bibleContent = Database.getBible(book.ToString("D2") + chapter.ToString("D3") + verse.ToString("D3"));
            return bibleContent;
        }
    }
}
