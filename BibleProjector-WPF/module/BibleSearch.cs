using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
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

    class BibleSearch
    {
        // 검색 맵
        string[] bibleTitles_s =
        {
            "갈",
            "겔",
            "계",
            "고전",
            "고후",
            "골",
            "나",
            "눅",
            "느",
            "단",
            "대상",
            "대하",
            "딛",
            "딤전",
            "딤후",
            "레",
            "렘",
            "롬",
            "룻",
            "마",
            "막",
            "말",
            "몬",
            "미",
            "민",
            "벧전",
            "벧후",
            "빌",
            "사",
            "살전",
            "살후",
            "삼상",
            "삼하",
            "삿",
            "수",
            "스",
            "슥",
            "습",
            "시",
            "신",
            "아",
            "암",
            "애",
            "약",
            "에",
            "엡",
            "옵",
            "왕상",
            "왕하",
            "요",
            "요삼",
            "요이",
            "요일",
            "욘",
            "욜",
            "욥",
            "유",
            "잠",
            "전",
            "창",
            "출",
            "학",
            "합",
            "행",
            "호",
            "히"
        };
        int[] bibleTitleIndex_s =
        {
            48,
            26,
            66,
            46,
            47,
            51,
            34,
            42,
            16,
            27,
            13,
            14,
            56,
            54,
            55,
            3,
            24,
            45,
            8,
            40,
            41,
            39,
            57,
            33,
            4,
            60,
            61,
            50,
            23,
            52,
            53,
            9,
            10,
            7,
            6,
            15,
            38,
            36,
            19,
            5,
            22,
            30,
            25,
            59,
            17,
            49,
            31,
            11,
            12,
            43,
            64,
            63,
            62,
            32,
            29,
            18,
            65,
            20,
            21,
            1,
            2,
            37,
            35,
            44,
            28,
            58
        };
        string[] bibleTitles_l =
        {
            "갈라디아서",
            "고린도전서",
            "고린도후서",
            "골로새서",
            "나훔",
            "누가복음",
            "느헤미야",
            "다니엘",
            "데살로니가전서",
            "데살로니가후서",
            "디도서",
            "디모데전서",
            "디모데후서",
            "레위기",
            "로마서",
            "룻기",
            "마가복음",
            "마태복음",
            "말라기",
            "미가",
            "민수기",
            "베드로전서",
            "베드로후서",
            "빌레몬서",
            "빌립보서",
            "사도행전",
            "사무엘상",
            "사무엘하",
            "사사기",
            "스가랴",
            "스바냐",
            "시편",
            "신명기",
            "아가",
            "아모스",
            "야고보서",
            "에베소서",
            "에스겔",
            "에스더",
            "에스라",
            "여호수아",
            "역대상",
            "역대하",
            "열왕기상",
            "열왕기하",
            "예레미야",
            "예레미야애가",
            "오바댜",
            "요나",
            "요엘",
            "요한계시록",
            "요한복음",
            "요한삼서",
            "요한이서",
            "요한일서",
            "욥기",
            "유다서",
            "이사야",
            "잠언",
            "전도서",
            "창세기",
            "출애굽기",
            "하박국",
            "학개",
            "호세아",
            "히브리서"
        };
        int[] bibleTitleIndex_l =
        {
            48,
            46,
            47,
            51,
            34,
            42,
            16,
            27,
            52,
            53,
            56,
            54,
            55,
            3,
            45,
            8,
            41,
            40,
            39,
            33,
            4,
            60,
            61,
            57,
            50,
            44,
            9,
            10,
            7,
            38,
            36,
            19,
            5,
            22,
            30,
            59,
            49,
            26,
            17,
            15,
            6,
            13,
            14,
            11,
            12,
            24,
            25,
            31,
            32,
            29,
            66,
            43,
            64,
            63,
            62,
            18,
            65,
            23,
            20,
            21,
            1,
            2,
            35,
            37,
            28,
            58
        };

        // 분석값들 및 단계
        string title;
        int chapter;
        int verse;

        const int SEARCH_DISTANCE_LIMIT = 1;
        public BibleSearchData[] getSearchResult(string searchPrase)
        {
            setSearchData(searchPrase);

            List<BibleSearchData> result = new List<BibleSearchData>(5);
            LevenshteinDistance ld = new LevenshteinDistance();
            KorString ks = new KorString();

            int searchDis = 0;
            int titleLen;
            for (int i = 0; i < bibleTitles_l.Length; i++)
            {
                titleLen = ks.GetAddCost(title);
                if ((searchDis = ld.getLevenDis_min(title, bibleTitles_l[i])) <= ((titleLen / 2.5) * SEARCH_DISTANCE_LIMIT))
                    result.Add(makeSearchResult(bibleTitleIndex_l[i].ToString("00"), searchDis));
            }
            for (int i = 0; i < bibleTitles_s.Length; i++)
            {
                titleLen = ks.GetAddCost(title);
                if ((searchDis = ld.getLevenDis_min(title, bibleTitles_s[i])) <= ((titleLen / 2.5) * SEARCH_DISTANCE_LIMIT))
                    result.Add(makeSearchResult(bibleTitleIndex_s[i].ToString("00"), searchDis));
            }

            result.Sort((a, b) => (a.searchDistance > b.searchDistance ? 1 :
            (a.searchDistance == b.searchDistance ? 0 : -1)));

            return result.ToArray();
        }

        BibleSearchData makeSearchResult(string Kuen, int searchDis)
        {
            if (chapter != -1
                && chapter <= Database.getChapterCount(Kuen))
            {
                string Jang = chapter.ToString("000");

                if (verse != -1
                    && verse <= Database.getVerseCount(Kuen + Jang))
                {
                    string Jeul = verse.ToString("000");

                    return new BibleSearchData(Kuen, Jang, Jeul, searchDis);
                }
                else
                    return new BibleSearchData(Kuen, Jang, null, searchDis);
            }
            else
                return new BibleSearchData(Kuen, null, null, searchDis);
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
    }
}
