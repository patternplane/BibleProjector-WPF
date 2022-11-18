using BibleProjector_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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

        static public ReserveDataUnit ReserveDataUnitFactory(ReserveType type, string FileSaveData)
        {
            switch (type) 
            {
                case ReserveType.Bible:
                    return new BibleReserveDataUnit(FileSaveData);    
                case ReserveType.Reading:
                    return new ReadingReserveDataUnit(FileSaveData);
                case ReserveType.Song:
                    return new SongReserveDataUnit(FileSaveData);
                case ReserveType.ExternPPT:
                    if (!ExternPPTReserveDataUnit.isAvailData(FileSaveData))
                        return null;
                    return new ExternPPTReserveDataUnit(FileSaveData);
                default:
                    return new EmptyReserveDataUnit();
            }
        }
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
            if (isHymn)
                return "-" + lyric.title;
            else
                return lyric.getIndexInList().ToString();
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
            // viewModel 설계의 잘못으로
            // Model에서 가져와야 할 데이터를
            // viewModel에서 가져오고 있다.
            int songUID = int.Parse(SaveData);
            if (songUID < 0)
                Initializer(ViewModel.LyricViewModel.HymnList[(-songUID) - 1]);
            else
                Initializer(ViewModel.LyricViewModel.LyricList[songUID]);
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

        public override string getFileSaveText()
        {
            return PPTfilePath;
        }

        // 설계 마음에 안들어
        static public bool isAvailData(string fullFilePath)
        {
            if (new FileInfo(fullFilePath).Exists)
                return true;
            return false;
        }

        public ExternPPTReserveDataUnit(string fullFilePath)
        {
            displayInfo_in = System.IO.Path.GetFileName(fullFilePath) + " : (" + fullFilePath + ")";

            // 설계 마음에 안듦
            if (!isAvailData(fullFilePath))
                throw new Exception("없는 PPT 파일!");
            PPTfilePath = fullFilePath;
            Powerpoint.ExternPPTs.Initialize_Single(fullFilePath);
        }
    }

}
