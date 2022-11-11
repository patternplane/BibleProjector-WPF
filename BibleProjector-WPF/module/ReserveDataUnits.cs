using BibleProjector_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public enum ReserveType
    {
        NULL,
        Bible,
        Reading,
        Song,
        ExternPPT
    }

    // 예약항목 데이터 단위

    public abstract class ReserveDataUnit
    {
        public ReserveType reserveType { get { return getReserveType(); } private set { } }
        protected abstract ReserveType getReserveType();
        public abstract string getContentInfo();
        public abstract string getFileSaveText();
    }

    public class EmptyReserveDataUnit : ReserveDataUnit
    {
        public override string getContentInfo()
        {
            return "No Data";
        }

        protected override ReserveType getReserveType()
        {
            return ReserveType.NULL;
        }
        
        public override string getFileSaveText()
        {
            return getContentInfo();
        }
    }

    public class BibleReserveDataUnit : ReserveDataUnit
    {
        string displayInfo_in;
        public override string getContentInfo()
        {
            return displayInfo_in;
        }
        protected override ReserveType getReserveType()
        {
            return ReserveType.Bible;
        }

        public string Book;
        public string Chapter;
        public string Verse;

        public override string getFileSaveText()
        {
            return Book+ Chapter + Verse;
        }
        void Initializer(string Keun, string Jang, string Jeul)
        {
            this.displayInfo_in = Database.getTitle(Keun)
                    + " "
                    + int.Parse(Jang)
                    + "장 "
                    + int.Parse(Jeul)
                    + "절";

            this.Book = Keun;
            this.Chapter = Jang;
            this.Verse = Jeul;
        }
        public BibleReserveDataUnit(string Keun, string Jang, string Jeul)
        {
            Initializer(Keun, Jang, Jeul);  
        }
        public BibleReserveDataUnit(string SaveData)
        {
            Initializer(SaveData.Substring(0,2),SaveData.Substring(2,3),SaveData.Substring(5,3));
        }
    }

    public class ReadingReserveDataUnit : ReserveDataUnit
    {
        string displayInfo_in;
        public override string getContentInfo()
        {
            return displayInfo_in;
        }
        protected override ReserveType getReserveType()
        {
            return ReserveType.Reading;
        }

        public int readingIndex;

        public override string getFileSaveText()
        {
            return readingIndex.ToString();
        }
        void Initializer(int readingIndex)
        {
            this.displayInfo_in = Database.getReadingTitle(readingIndex);

            this.readingIndex = readingIndex;
        }
        public ReadingReserveDataUnit(int readingIndex)
        {
            Initializer(readingIndex);
        }
        public ReadingReserveDataUnit(string SaveData)
        {
            Initializer(int.Parse(SaveData));
        }
    }
    
    public class SongReserveDataUnit : ReserveDataUnit
    {
        string displayInfo_in;
        public override string getContentInfo()
        {
            return displayInfo_in;
        }
        protected override ReserveType getReserveType()
        {
            return ReserveType.Song;
        }

        public SingleLyric lyric;
        public bool isHymn;

        public override string getFileSaveText()
        {
            return 
        }
        void Initializer(SingleLyric lyric)
        {
            isHymn = (lyric.GetType() == typeof(SingleHymn));
            if (isHymn)
                displayInfo_in = "찬송가 " + lyric.title + "장";
            else
                displayInfo_in = lyric.title;

            this.lyric = lyric;
        }
        public SongReserveDataUnit(SingleLyric lyric)
        {
            Initializer(lyric);
        }
        public SongReserveDataUnit(string SaveData)
        {
            Initializer
        }
    }
    
    public class ExternPPTReserveDataUnit : ReserveDataUnit
    {
        string displayInfo_in;
        public override string getContentInfo()
        {
            return displayInfo_in;
        }
        protected override ReserveType getReserveType()
        {
            return ReserveType.ExternPPT;
        }

        public string PPTfilePath;

        public ExternPPTReserveDataUnit(string fullFilePath)
        {
            displayInfo_in = System.IO.Path.GetFileName(fullFilePath) + " : (" + fullFilePath + ")";

            PPTfilePath = fullFilePath;
        }
    }

}
