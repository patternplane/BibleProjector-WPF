using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class CharCompare
    {
        KorString k = new KorString();

        // ====================================== 문자열 포함 확인 ====================================== 

        /// <summary>
        /// 문자열 a가 문자열 b에 포함되면 True, 그렇지 않으면 False를 반환합니다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool IsContain_WithKor(string a, string b)
        {
            a = k.DisassembleKorCons(a);
            b = k.DisassembleKorCons(b);

            for (int i = 0; i < a.Length; i++)
            {
                if (i == b.Length)
                    return false;

                if (k.IsKor(a[i]) && k.IsKor(b[i]))
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

        public bool IsContain_WithKor(char a, char b)
        {
            if (k.IsKor(a) && k.IsKor(b))
                if (k.GetFirstCons_Kor(a) == k.GetFirstCons_Kor(b))
                {
                    if (k.GetVowel_Kor(a) == '\0')
                        return true;

                    if (k.GetVowel_Kor(b) != '\0'
                        && k.GetVowel_Kor(a) == k.GetVowel_Kor(b))
                    {
                        if (k.GetLastCons_Kor(a) == '\0')
                            return true;

                        if (k.GetLastCons_Kor(b) != '\0'
                            && k.GetLastCons_Kor(a) == korLastConsContainMap[k.GetLastCons_Kor(b) - 'ㄱ'])
                            return true;
                    }
                }

            return (a == b);
        }
    }
}
