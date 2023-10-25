using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class CharCompare
    {
        public bool IsContain_WithKor(string a, string b)
        {
            a = DisassembleKorLang(a);
            b = DisassembleKorLang(b);

            for (int i = 0; i < a.Length; i++)
            {
                if (i == b.Length)
                    return false;

                if (IsKor(a[i]) && IsKor(b[i]))
                {
                    if (!IsContain_WithKor(a[i], b[i]))
                        return false;
                }
                else if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        char[] korLastConsContainMap =
        {
            'ㄱ',
            'ㄲ',
            'ㄱ', // ㄳ
            'ㄴ',
            'ㄴ', // ㄵ
            'ㄴ', // ㄶ
            'ㄷ',
            'ㄸ',
            'ㄹ',
            'ㄹ', // ㄺ
            'ㄹ', // ㄻ
            'ㄹ', // ㄼ
            'ㄹ', // ㄽ
            'ㄹ', // ㄾ
            'ㄹ', // ㄿ
            'ㄹ', // ㅀ
            'ㅁ',
            'ㅂ',
            'ㅃ',
            'ㅂ', // ㅄ
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

        bool IsContain_WithKor(char a, char b)
        {
            if (IsKor(a) && IsKor(b))
                if (GetFirstCons_Kor(a) == GetFirstCons_Kor(b))
                {
                    if (GetVowel_Kor(a) == '\0')
                        return true;

                    if (GetVowel_Kor(b) != '\0'
                        && GetVowel_Kor(a) == GetVowel_Kor(b))
                    {
                        if (GetLastCons_Kor(a) == '\0')
                            return true;

                        if (GetLastCons_Kor(b) != '\0'
                            && GetLastCons_Kor(a) == korLastConsContainMap[GetLastCons_Kor(b) - 'ㄱ'])
                            return true;
                    }
                }

            return false;
        }

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

        char GetFirstCons_Kor(char c)
        {
            if (c >= 'ㄱ' && c <= 'ㅎ')
                return c;
            else if (IsCombinatedKor(c))
                return firstConsMap[((c - '가') / 588)];
            else
                return '\0';
        }

        char GetVowel_Kor(char c)
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

        char GetLastCons_Kor(char c)
        {
            if (IsCombinatedKor(c))
                return lastConsMap[((c - '가') % 28)];
            else
                return '\0';
        }
            
        string DisassembleKorLang(string word)
        {
            StringBuilder result = new StringBuilder(10);

            foreach (char c in word)
                switch (c)
                {
                    case 'ㄳ':
                        result.Append('ㄱ');
                        result.Append('ㅅ');
                        break;
                    case 'ㄵ':
                        result.Append('ㄴ');
                        result.Append('ㅈ');
                        break;
                    case 'ㄶ':
                        result.Append('ㄴ');
                        result.Append('ㅎ');
                        break;
                    case 'ㄺ':
                        result.Append('ㄹ');
                        result.Append('ㄱ');
                        break;
                    case 'ㄻ':
                        result.Append('ㄹ');
                        result.Append('ㅁ');
                        break;
                    case 'ㄼ':
                        result.Append('ㄹ');
                        result.Append('ㅂ');
                        break;
                    case 'ㄽ':
                        result.Append('ㄹ');
                        result.Append('ㅅ');
                        break;
                    case 'ㄾ':
                        result.Append('ㄹ');
                        result.Append('ㅌ');
                        break;
                    case 'ㄿ':
                        result.Append('ㄹ');
                        result.Append('ㅍ');
                        break;
                    case 'ㅀ':
                        result.Append('ㄹ');
                        result.Append('ㅎ');
                        break;
                    case 'ㅄ':
                        result.Append('ㅂ');
                        result.Append('ㅅ');
                        break;
                    default:
                        result.Append(c);
                        break;
                }

            return result.ToString();
        }

        bool IsCombinatedKor(char c)
        {
            return (c >= '가' && c <= '힣');
        }

        /// <summary>
        /// 특수한글자모를 사용하지 않는 일반적인 사용환경에서
        /// 한글인지 확인하는 함수.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool IsKor(char c)
        {
            if (c >= 'ㄱ' && c <= 'ㅣ'
                || c >= '가' && c <= '힣')
                return true;
            else
                return false;
        }
    }
}
