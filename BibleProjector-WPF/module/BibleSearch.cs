using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
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

        // 분석값들
        string title;
        string chapter;
        string verse;

        public BibleSearchData[] getSearchResult(string searchPrase)
        {

        }

        void trimPrase(string prase)
        {
            for(int i = 0, startindex = 0 ; i < prase.Length; i++)
            {
                if (isSeperator(prase[i]))
                {
                    prase.Substring(startindex, i - startindex); // 처리할 것

                    startindex = i + 1;
                }
                else if (i != startindex && char.IsDigit(prase[i - 1]) != char.IsDigit(prase[i]))
                {
                    prase.Substring(startindex, i - startindex); // 처리할 것

                    startindex = i--;
                }
                else if (i == prase.Length - 1)
                {
                    prase.Substring(startindex, i - startindex + 1); // 처리할 것
                }
            }
        }

        bool isSeperator(char a)
        {
            switch (a)
            {
                case ' ':
                case '/':
                case '.':
                case ':':
                case ';':
                case '\'':
                default:
                    return false;
            }
        }
    }
}
