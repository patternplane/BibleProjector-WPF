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
        public abstract void ProcessBeforeDeletion();

        protected abstract bool checkSameData(object data);
        public bool isSameData(object data)
        {
            return checkSameData(data);
        }

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

        public override void ProcessBeforeDeletion()
        {
            return;
        }

        protected override bool checkSameData(object data)
        {
            return (data == null);
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

        public override void ProcessBeforeDeletion()
        {
            return;
        }

        protected override bool checkSameData(object data)
        {
            if (data.GetType() == typeof(string)
                && (Book + Chapter + Verse).CompareTo((string)data) == 0)
                return true;
            else
                return false;
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
        public override void ProcessBeforeDeletion()
        {
            return;
        }

        protected override bool checkSameData(object data)
        {
            if (data.GetType() == typeof(int)
                && (int)data == readingIndex)
               return true;
            else 
                return false;
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
        public override void ProcessBeforeDeletion()
        {
            return;
        }

        protected override bool checkSameData(object data)
        {
            if (data.GetType() == typeof(SingleLyric)
                && (SingleLyric)data == this.lyric)
                return true;
            else
                return false;
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

        public ExternPPTReserveDataUnit(string fullFilePath)
        {
            displayInfo_in = System.IO.Path.GetFileName(fullFilePath) + " : (" + fullFilePath + ")";

            PPTfilePath = fullFilePath;
            Powerpoint.ExternPPTs.setPresentation(fullFilePath);
        }

        public override void ProcessBeforeDeletion()
        {
            new ExternPPTManager().UnlinkPPT(PPTfilePath);
        }

        protected override bool checkSameData(object data)
        {
            if (data.GetType() == typeof(string)
                && ((string)data).CompareTo(PPTfilePath) == 0)
                return true;
            else
                return false;
        }
    }

}
