using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class KorString
    {
        // ============================ 특성 검사 함수 ============================ 

        public bool IsKorConsonant(char c)
        {
            return (c >= 'ㄱ' && c <= 'ㅎ');
        }

        public bool IsCombinatedKor(char c)
        {
            return (c >= '가' && c <= '힣');
        }

        /// <summary>
        /// 특수한글자모를 사용하지 않는 일반적인 사용환경에서
        /// 한글인지 확인하는 함수.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsKor(char c)
        {
            if (c >= 'ㄱ' && c <= 'ㅣ'
                || c >= '가' && c <= '힣')
                return true;
            else
                return false;
        }

        // ============================ 초성 중성 종성 반환함수 ============================ 

        char[] firstConsMap =
        {
            'ㄱ',
            'ㄲ',
            'ㄴ',
            'ㄷ',
            'ㄸ',
            'ㄹ',
            'ㅁ',
            'ㅂ',
            'ㅃ',
            'ㅅ',
            'ㅆ',
            'ㅇ',
            'ㅈ',
            'ㅉ',
            'ㅊ',
            'ㅋ',
            'ㅌ',
            'ㅍ',
            'ㅎ'
        };

        public char GetFirstCons_Kor(char c)
        {
            if (c >= 'ㄱ' && c <= 'ㅎ')
                return c;
            else if (IsCombinatedKor(c))
                return firstConsMap[((c - '가') / 588)];
            else
                return '\0';
        }

        public char GetVowel_Kor(char c)
        {
            if (c >= 'ㅏ' && c <= 'ㅣ')
                return c;
            else if (IsCombinatedKor(c))
            {
                return (char)('ㅏ' + ((c - '가') % 588 / 28));
            }
            else
                return '\0';
        }

        char[] lastConsMap =
        {
            '\0',
            'ㄱ',
            'ㄲ',
            'ㄳ',
            'ㄴ',
            'ㄵ',
            'ㄶ',
            'ㄷ',
            'ㄹ',
            'ㄺ',
            'ㄻ',
            'ㄼ',
            'ㄽ',
            'ㄾ',
            'ㄿ',
            'ㅀ',
            'ㅁ',
            'ㅂ',
            'ㅄ',
            'ㅅ',
            'ㅆ',
            'ㅇ',
            'ㅈ',
            'ㅊ',
            'ㅋ',
            'ㅌ',
            'ㅍ',
            'ㅎ'
        };

        public char GetLastCons_Kor(char c)
        {
            if (IsCombinatedKor(c))
                return lastConsMap[((c - '가') % 28)];
            else
                return '\0';
        }

        char[][] consSepMap =
        {
            new char[] { },
            new char[] { },
            new char[] { 'ㄱ', 'ㅅ'},
            new char[] { },
            new char[] { 'ㄴ', 'ㅈ'},
            new char[] { 'ㄴ', 'ㅎ'},
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { 'ㄹ', 'ㄱ'},
            new char[] { 'ㄹ', 'ㅁ'},
            new char[] { 'ㄹ', 'ㅂ'},
            new char[] { 'ㄹ', 'ㅅ'},
            new char[] { 'ㄹ', 'ㅌ'},
            new char[] { 'ㄹ', 'ㅍ'},
            new char[] { 'ㄹ', 'ㅎ'},
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { 'ㅂ', 'ㅅ'},
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { },
            new char[] { }
        };

        public char GetLastCons1_Kor(char c)
        {
            c = GetLastCons_Kor(c);
            if (c != '\0')
            {
                if (consSepMap[c - 'ㄱ'].Length == 2)
                    return consSepMap[c - 'ㄱ'][0];
                else
                    return c;
            }
            else
                return '\0';
        }

        public char GetLastCons2_Kor(char c)
        {
            c = GetLastCons_Kor(c);
            if (c != '\0'
                && consSepMap[c - 'ㄱ'].Length == 2)
                    return consSepMap[c - 'ㄱ'][1];
            else
                return '\0';
        }

        // ====================================== 문자 편집비용 계산 ====================================== 

        public int GetAddCost(string k)
        {
            int result = 0;
            foreach (char c in k)
            {
                if (IsKor(c))
                    result += GetAddCost(c);
                else
                    result += 1;
            }
            return result;
        }

        public int GetAddCost(char a)
        {
            if (IsKor(a))
            {
                if (GetLastCons2_Kor(a) != '\0')
                    return 4;
                else if (GetLastCons1_Kor(a) != '\0')
                    return 3;
                else if (GetVowel_Kor(a) != '\0')
                    return 2;
                else
                    return 1;
            }
            else
                return 4;
        }

        public int GetDistance_WithKor(char a, char b)
        {
            if (IsKor(a))
            {
                int distance = 0;
                bool isContain = true;
                if (GetFirstCons_Kor(a) != GetFirstCons_Kor(b))
                {
                    distance++;
                    isContain = false;
                }
                if (GetVowel_Kor(a) != GetVowel_Kor(b))
                {
                    distance++;
                    if (GetVowel_Kor(a) != '\0')
                        isContain = false;
                }
                if (GetLastCons1_Kor(a) != GetLastCons1_Kor(b))
                {
                    distance++;
                    if (GetLastCons1_Kor(a) != '\0')
                        isContain = false;
                }
                if (GetLastCons2_Kor(a) != GetLastCons2_Kor(b))
                {
                    distance++;
                    if (GetLastCons2_Kor(a) != '\0')
                        isContain = false;
                }

                if (isContain)
                    distance = 0;

                return distance;
            }
            else
                return ((a == b) ? 0 : 4);
        }

        // ============================ 한글 문자열 초성분해 ============================ 

        int[] popedLastConsMap =
        {
            0,
            0,
            0,
            1, // ㄳ
            0,
            4, // ㄵ
            4, // ㄶ
            0,
            0,
            8, // ㄺ
            8, // ㄻ
            8, // ㄼ
            8, // ㄽ
            8, // ㄾ
            8, // ㄿ
            8, // ㅀ
            0,
            0,
            17, // ㅄ
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };
        char[] popLastConsMap =
        {
            '\0',
            'ㄱ',
            'ㄲ',
            'ㅅ', // ㄳ
            'ㄴ',
            'ㅈ', // ㄵ
            'ㅎ', // ㄶ
            'ㄷ',
            'ㄹ',
            'ㄱ', // ㄺ
            'ㅁ', // ㄻ
            'ㅂ', // ㄼ
            'ㅅ', // ㄽ
            'ㅌ', // ㄾ
            'ㅍ', // ㄿ
            'ㅎ', // ㅀ
            'ㅁ',
            'ㅂ',
            'ㅅ', // ㅄ
            'ㅅ',
            'ㅆ',
            'ㅇ',
            'ㅈ',
            'ㅊ',
            'ㅋ',
            'ㅌ',
            'ㅍ',
            'ㅎ'
        };

        /// <summary>
        /// 문자열 내에서 한글 초성은 전부 분해하여 반환합니다.
        /// lastCons가 true일 때 마지막 한글 조합자의 받침문자 하나를 분리합니다.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="lastCons"></param>
        /// <returns></returns>
        public string DisassembleKorCons(string word, bool lastCons = false)
        {
            StringBuilder result = new StringBuilder(10);
            StringBuilder temp = new StringBuilder(10);
            char LC = '\0';
            bool hasLC = false;

            foreach (char c in word)
            {
                if (lastCons && GetLastCons_Kor(c) != '\0')
                {
                    if (hasLC)
                    {
                        result.Append(LC);
                        result.Append(temp);
                        temp.Clear();
                    }

                    LC = c;
                    hasLC = true;
                }
                else
                {
                    if (IsKorConsonant(c))
                    {
                        if (consSepMap[c - 'ㄱ'].Length == 2)
                        {
                            if (lastCons && hasLC)
                            {
                                temp.Append(consSepMap[c - 'ㄱ'][0]);
                                temp.Append(consSepMap[c - 'ㄱ'][1]);
                            }
                            else
                            {
                                result.Append(consSepMap[c - 'ㄱ'][0]);
                                result.Append(consSepMap[c - 'ㄱ'][1]);
                            }
                        }
                        else
                        {
                            if (lastCons && hasLC)
                                temp.Append(c);
                            else
                                result.Append(c);
                        }
                    }
                    else
                    {
                        if (lastCons && hasLC)
                            temp.Append(c);
                        else
                            result.Append(c);
                    }
                }
            }

            if (lastCons && hasLC)
            {
                result.Append((char)('가' + ((LC - '가') / 28 * 28) + popedLastConsMap[(LC - '가') % 28]));
                result.Append(popLastConsMap[(LC - '가') % 28]);
                result.Append(temp);
            }

            return result.ToString();
        }
    }
}
