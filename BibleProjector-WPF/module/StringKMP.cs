using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace BibleProjector_WPF.module
{
    class StringKMP
    {
        /// <summary>
        /// 전처리 테이블
        /// </summary>
        static private int[] patternTable;

        /// <summary>
        /// 문자 비교함수의 명세
        /// 같으면 true, 다르면 false일 것
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public delegate bool StringCompare(char a, char b);
            
        static public bool DefaultStringCompaerFunc(char a, char b)
        {
            return (a == b);
        }

        /// <summary>
        /// KMP - 전처리 테이블 제작 함수
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        static private int MakeTable(string sample, StringCompare compareFunc)
        {
            int len = sample.Length;
            patternTable = new int[len];

            int setter = 0;
            int checker = 0;
            patternTable[setter++] = checker;

            while (setter < len)
            {
                if (compareFunc(sample[setter], sample[checker]))
                    patternTable[setter++] = ++checker;
                else
                {
                    if (checker == 0)
                        patternTable[setter++] = checker;
                    else
                        checker = patternTable[checker - 1];
                }
            }

            return 0;
        }

        /// <summary>
        /// KMP 검색<br/>
        /// 검색값이 존재하는지 알려줍니다.
        /// </summary>
        /// <param name="origin">
        /// 원본 문자열
        /// </param>
        /// <param name="sample">
        /// 찾을 문자열
        /// </param>
        /// <param name="compareFunc">
        /// 비교함수
        /// </param>
        /// <param name="doCareBlank">
        /// 공백을 신경쓸지 여부, 공백도 고려하여 검색하려면 true
        /// </param>
        /// <returns>
        /// 하나라도 검색이 되었다면 true, 그렇지 않다면 false를 반환
        /// </returns>
        static public bool HasPattern (string origin, string sample, StringCompare compareFunc, bool doCareBlank = true)
        {
            if (!doCareBlank)
            {
                origin = Regex.Replace(origin, "\\s", "");
                sample = Regex.Replace(sample, "\\s", "");
            }

            if (MakeTable(sample, compareFunc) == -1)
                return false;

            int o_len = origin.Length;
            int s_len = sample.Length;
            int o_i = 0;
            int s_i = 0;
            while (o_i < o_len)
            {
                if (compareFunc(origin[o_i], sample[s_i]))
                {
                    o_i++;
                    s_i++;

                    if (s_i == s_len)
                    {
                        return true;
                    }
                }
                else
                {
                    if (s_i == 0)
                        o_i++;
                    else
                        s_i = patternTable[s_i - 1];
                }
            }
            return false;
        }

        /// <summary>
        /// KMP 검색<br/>
        /// 모든 검색 성공 위치를 반환합니다.
        /// </summary>
        /// <param name="origin">
        /// 원본 문자열
        /// </param>
        /// <param name="sample">
        /// 찾을 문자열
        /// </param>
        /// <param name="compareFunc">
        /// 비교함수
        /// </param>
        /// <param name="doCareBlank">
        /// 공백을 신경쓸지 여부, 공백도 고려하여 검색하려면 true
        /// </param>
        /// <returns>
        /// 모든 발견위치를 반환합니다.<br/>
        /// 오류시 null을 반환
        /// </returns>
        static public int[] FindPattern(string origin, string sample, StringCompare compareFunc, bool doCareBlank = true)
        {
            string hasBlankOrigin = origin;
            if (!doCareBlank)
            {
                origin = Regex.Replace(origin,"\\s", "");
                sample = Regex.Replace(sample, "\\s", "");
            }

            if (MakeTable(sample, compareFunc) == -1)
                return null;

            int o_len = origin.Length;
            int s_len = sample.Length;
            int o_i = 0;
            int s_i = 0;

            List<int> positions = new List<int>(50);

            while (o_i < o_len)
            {
                if (compareFunc(origin[o_i], sample[s_i]))
                {
                    o_i++;
                    s_i++;

                    if (s_i == s_len)
                    {
                        positions.Add(o_i - s_len);
                        s_i = patternTable[s_i - 1];
                    }
                }
                else
                {
                    if (s_i == 0)
                        o_i++;
                    else
                        s_i = patternTable[s_i - 1];
                }
            }

            if (!doCareBlank)
                return getRealIndex(hasBlankOrigin, positions.ToArray());
            else
                return positions.ToArray();
        }

        /// <summary>
        /// 공백을 제외하고 검색된 KMP 검색 결과의 인덱스를 원래 문자열에서의 검색위치로 변경해줍니다.
        /// </summary>
        /// <param name="originalStr">원본 문자열</param>
        /// <param name="findPos">공백을 제외하여 검색한 KMP 검색결과</param>
        /// <returns>실제 원본 문자열에서 검색된 위치들</returns>
        static int[] getRealIndex(String originalStr, int[] findPos)
        {
            int posIndex = 0;
            int realIndex = 0;
            int nonBlankIndex = 0;
            while (posIndex < findPos.Length)
            {
                while (nonBlankIndex != findPos[posIndex])
                {
                    while (char.IsWhiteSpace(originalStr[++realIndex])) ;
                    nonBlankIndex++;
                }
                findPos[posIndex] = realIndex;
                posIndex++;
            }
            return findPos;
        }

        /// <summary>
        /// 한 문자열에서 찾은 패턴들을 주변 문맥을 덧붙여 가공하여 문자열로 출력합니다.<br/>
        /// 여러 패턴이 가까이 붙어있다면 한 문자열로 이어서 출력합니다.
        /// </summary>
        /// <param name="startPos">
        /// KMP 검색결과, 문자열 상에서 발견된 패턴들의 시작위치입니다.
        /// </param>
        /// <param name="content">
        /// 원본 문자열입니다.
        /// </param>
        /// <param name="patternLength">
        /// 검색한 패턴의 길이입니다.
        /// </param>
        /// <param name="skipMark">
        /// 생략을 표시할 문자
        /// </param>
        /// <param name="interval">
        /// 가져올 주변 글자 수
        /// </param>
        /// <returns>
        /// 가공된 패턴들을 담은 String 배열
        /// </returns>
        static public String[] makeResultPreview(int[] startPos, String content, int patternLength, string skipMark = "...", int interval = 6)
        {
            List<String> result = new List<string>(startPos.Length);
            int startIndex = ((startPos[0] - interval > 0) ? startPos[0] - interval : 0);
            int endPos;
            StringBuilder displayString = new StringBuilder(200);
            for (int i = 0; i < startPos.Length; i++)
            {
                endPos = startPos[i] + patternLength;
                if ((i + 1 == startPos.Length) || (endPos + interval < startPos[i + 1] - interval))
                {
                    endPos += interval;
                    if (endPos > content.Length)
                        endPos = content.Length;

                    displayString.Clear();

                    if (startIndex != 0)
                        displayString.Append(skipMark);
                    displayString.Append(content.Substring(startIndex, endPos - startIndex));
                    if (endPos != content.Length)
                        displayString.Append(skipMark);

                    result.Add(displayString.Replace("\r"," ").Replace("\n"," ").ToString());

                    if (i + 1 != startPos.Length)
                        startIndex = startPos[i + 1] - interval;
                }
            }

            return result.ToArray();
        }

    }
}
