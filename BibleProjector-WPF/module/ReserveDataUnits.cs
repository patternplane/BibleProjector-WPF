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
        Song_CCM,
        Song_Hymn,
        ExternPPT
    }

    // 예약항목 데이터 단위

    public abstract class ReserveDataUnit
    {
        public ReserveType reserveType { get { return getReserveType(); } private set { } }
        protected abstract ReserveType getReserveType();
        public abstract string getContentInfo();
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

        public BibleReserveDataUnit(string Keun, string Jang, string Jeul)
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

        public ReadingReserveDataUnit(int readingIndex)
        {
            this.displayInfo_in = Database.getReadingTitle(readingIndex);

            this.readingIndex = readingIndex;
        }
    }
    /*
    public class CCMReserveDataUnit : ReserveDataUnit
    {
    }
    
    public class HymnReserveDataUnit : ReserveDataUnit
    {
    }

    public class ExternPPTReserveDataUnit : ReserveDataUnit
    {
    }*/

}
