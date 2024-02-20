using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class BibleSearcher : ISearcher
    {

        // 검색 맵
        struct BibleTitleInfo{
            public string fullName;
            public string shortName;
            public int titleNumber;

            public BibleTitleInfo(string fullName, string shortName, int titleNumber)
            {
                this.fullName = fullName;
                this.shortName = shortName;
                this.titleNumber = titleNumber;
            }
        }

        BibleTitleInfo[] bibleTitles =
        {
            new BibleTitleInfo("창세기","창",1),
            new BibleTitleInfo("출애굽기","출",2),
            new BibleTitleInfo("레위기","레",3),
            new BibleTitleInfo("민수기","민",4),
            new BibleTitleInfo("신명기","신",5),
            new BibleTitleInfo("여호수아","수",6),
            new BibleTitleInfo("사사기","삿",7),
            new BibleTitleInfo("룻기","룻",8),
            new BibleTitleInfo("사무엘상","삼상",9),
            new BibleTitleInfo("사무엘하","삼하",10),
            new BibleTitleInfo("열왕기상","왕상",11),
            new BibleTitleInfo("열왕기하","왕하",12),
            new BibleTitleInfo("역대상","대상",13),
            new BibleTitleInfo("역대하","대하",14),
            new BibleTitleInfo("에스라","스",15),
            new BibleTitleInfo("느헤미야","느",16),
            new BibleTitleInfo("에스더","에",17),
            new BibleTitleInfo("욥기","욥",18),
            new BibleTitleInfo("시편","시",19),
            new BibleTitleInfo("잠언","잠",20),
            new BibleTitleInfo("전도서","전",21),
            new BibleTitleInfo("아가","아",22),
            new BibleTitleInfo("이사야","사",23),
            new BibleTitleInfo("예레미야","렘",24),
            new BibleTitleInfo("예레미야애가","애",25),
            new BibleTitleInfo("에스겔","겔",26),
            new BibleTitleInfo("다니엘","단",27),
            new BibleTitleInfo("호세아","호",28),
            new BibleTitleInfo("요엘","욜",29),
            new BibleTitleInfo("아모스","암",30),
            new BibleTitleInfo("오바댜","옵",31),
            new BibleTitleInfo("요나","욘",32),
            new BibleTitleInfo("미가","미",33),
            new BibleTitleInfo("나훔","나",34),
            new BibleTitleInfo("하박국","합",35),
            new BibleTitleInfo("스바냐","습",36),
            new BibleTitleInfo("학개","학",37),
            new BibleTitleInfo("스가랴","슥",38),
            new BibleTitleInfo("말라기","말",39),
            new BibleTitleInfo("마태복음","마",40),
            new BibleTitleInfo("마가복음","막",41),
            new BibleTitleInfo("누가복음","눅",42),
            new BibleTitleInfo("요한복음","요",43),
            new BibleTitleInfo("사도행전","행",44),
            new BibleTitleInfo("로마서","롬",45),
            new BibleTitleInfo("고린도전서","고전",46),
            new BibleTitleInfo("고린도후서","고후",47),
            new BibleTitleInfo("갈라디아서","갈",48),
            new BibleTitleInfo("에베소서","엡",49),
            new BibleTitleInfo("빌립보서","빌",50),
            new BibleTitleInfo("골로새서","골",51),
            new BibleTitleInfo("데살로니가전서","살전",52),
            new BibleTitleInfo("데살로니가후서","살후",53),
            new BibleTitleInfo("디모데전서","딤전",54),
            new BibleTitleInfo("디모데후서","딤후",55),
            new BibleTitleInfo("디도서","딛",56),
            new BibleTitleInfo("빌레몬서","몬",57),
            new BibleTitleInfo("히브리서","히",58),
            new BibleTitleInfo("야고보서","약",59),
            new BibleTitleInfo("베드로전서","벧전",60),
            new BibleTitleInfo("베드로후서","벧후",61),
            new BibleTitleInfo("요한일서","요일",62),
            new BibleTitleInfo("요한이서","요이",63),
            new BibleTitleInfo("요한삼서","요삼",64),
            new BibleTitleInfo("유다서","유",65),
            new BibleTitleInfo("요한계시록","계",66)
        };

        // 분석값들 및 단계
        string title;
        int chapter;
        int verse;

        const int SEARCH_DISTANCE_LIMIT = 1;
        public ICollection<ISearchData> getSearchResult(string searchPrase)
        {
            setSearchData(searchPrase);
            return makeSearchResult();
        }

        void setSearchData(string searchPrase)
        {
            string[] prases = new BibleTitleSeparator().trimPrase(searchPrase);

            title = null;
            chapter = -1;
            verse = -1;

            int temp;
            if (prases.Length > 0)
                title = prases[0];
            if (prases.Length > 1)
                if (int.TryParse(prases[1], out temp))
                {
                    chapter = temp;
                    if (prases.Length > 2)
                        if (int.TryParse(prases[2], out temp))
                            verse = temp;
                }
        }

        ICollection<ISearchData> makeSearchResult()
        {
            LevenshteinDistance ld = new LevenshteinDistance();
            KorString ks = new KorString();

            List<Data.BibleSearchData> result = new List<Data.BibleSearchData>(5);

            int searchDis_s = 0;
            int searchDis_l = 0;
            int searchDis;
            bool isShort;
            int titleLen;
            for (int i = 0; i < bibleTitles.Length; i++)
            {
                searchDis_s = ld.getLevenDis_min(title, bibleTitles[i].shortName);
                searchDis_l = ld.getLevenDis_min(title, bibleTitles[i].fullName);

                if (searchDis_s < searchDis_l
                    || searchDis_s == 0)
                {
                    searchDis = searchDis_s;
                    isShort = true;
                }
                else
                {
                    searchDis = searchDis_l;
                    isShort = false;
                }

                titleLen = ks.GetAddCost(title);
                if (searchDis <= ((titleLen / 2.5) * SEARCH_DISTANCE_LIMIT))
                    result.Add(
                        new Data.BibleSearchData(
                            new Data.BibleData(bibleTitles[i].titleNumber, this.chapter, this.verse)
                            , isShort
                            , searchDis));
            }

            result.Sort((a, b) => (a.searchDistance > b.searchDistance ? 1 : (a.searchDistance == b.searchDistance ? 0 : -1)));
            return result.ToArray();
        }
    }
    
    class BibleTitleSeparator
    {
        List<string> units = new List<string>(3);

        /// <summary>
        /// 구분자(대부분의 공백과 특수문자 전반)를 기준으로 단어를 분리하되,
        /// 숫자와 문자는 붙어있어도 따로 분리합니다.
        /// 또한 처음 단어 이후에는 숫자만 인식합니다.
        /// </summary>
        /// <param name="prase"></param>
        /// <returns></returns>
        public string[] trimPrase(string prase)
        {
            units.Clear();
            bool getOnlyNumber = false;
            bool onlyNumber = true;

            for (int i = 0, startindex = 0; i <= prase.Length; i++)
            {
                if (i == prase.Length)
                {
                    if (!getOnlyNumber || onlyNumber)
                        units.Add(prase.Substring(startindex, i - startindex));

                    break;
                }
                else if (isSeperator(prase[i]))
                {
                    if (i != startindex)
                    {
                        if (!getOnlyNumber || onlyNumber)
                        {
                            units.Add(prase.Substring(startindex, i - startindex));
                            getOnlyNumber = true;
                        }

                        onlyNumber = true;
                        startindex = i + 1;
                    }
                    else
                        startindex++;
                }
                else if (i != startindex && char.IsDigit(prase[i - 1]) != char.IsDigit(prase[i]))
                {
                    if (!getOnlyNumber || onlyNumber)
                    {
                        units.Add(prase.Substring(startindex, i - startindex));
                        getOnlyNumber = true;
                    }

                    onlyNumber = true;
                    startindex = i--;
                }
                else if (!char.IsDigit(prase[i]))
                    onlyNumber = false;
            }

            return units.ToArray();
        }

        bool isSeperator(char a)
        {
            if (char.IsSymbol(a)
                || char.IsWhiteSpace(a)
                || char.IsPunctuation(a)
                || char.IsSeparator(a))
                return true;

            return false;
        }
    }
}
