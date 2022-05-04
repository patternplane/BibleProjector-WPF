using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class StringModifier
    {
        /// <summary>
        /// 잘못된 개행문자를 윈도우 표준에 맞게 바꿔줍니다.
        /// </summary>
        /// <param name="original">
        /// 원본 문자열
        /// </param>
        /// <returns>
        /// 고쳐진 문자열
        /// </returns>
        static public string makeCorrectNewline(string original)
        {
            StringBuilder str = new StringBuilder(original);
            for (int i = 0, j = 0; i < original.Length; i++, j++)
            {
                if (original[i] == '\r')
                {
                    if ((i + 1 == original.Length) || (original[i + 1] != '\n'))
                    {
                        str = str.Insert(j + 1, '\n');
                        j++;
                    }
                    else
                    {
                        i++;
                        j++;
                    }
                }
                else if (original[i] == '\n')
                {
                    str = str.Insert(j, '\r');
                    j++;
                }
                else if (original[i] == '\v')
                {
                    str = str.Replace("\v", "\r\n", j, 1);
                    j++;
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 문자열을 페이지별 줄 수에 맞춰 재단합니다.
        /// </summary>
        /// <param name="origin">원본 문자열</param>
        /// <param name="linePerPage">페이지별 줄 수</param>
        /// <returns></returns>
        static public string[] makePageWithLines(string origin, int linePerPage)
        {
            List<string> pages = new List<string>(10);

            int startIndex = 0;
            int currentLine = 0;
            int i = 0;
            while (true)
            {
                if (i == origin.Length)
                {
                    if (startIndex != i)
                        pages.Add(origin.Substring(startIndex, i - startIndex));
                    else if (pages.Count == 0)
                        pages.Add("");
                    break;
                }
                else if (origin[i] == '\r')
                {
                    currentLine++;
                    if (currentLine >= linePerPage)
                    {
                        pages.Add(origin.Substring(startIndex, i - startIndex));
                        currentLine = 0;
                        i += 2;
                        startIndex = i;
                    }
                    else
                        i += 2;
                }
                else
                    i++;
            }

            return pages.ToArray();
        }

        /// <summary>
        /// 문자열을 재단합니다.<br/>
        /// 줄별 문자의 갯수(단어 한 덩어리를 굳이 자르지는 않습니다)와 페이지별 줄 수를 입력받습니다.
        /// </summary>
        /// <param name="origin">원본 문자열</param>
        /// <param name="charPerLine">줄별 문자 수</param>
        /// <param name="linePerPage">페이지별 줄 수</param>
        /// <returns>재단된 문자열 페이지</returns>
        static public string[] makeStringPage(string origin, int charPerLine, int linePerPage)
        {
            origin = origin.Trim();
            List<string> pages = new List<string>(10);
            StringBuilder page = new StringBuilder(50);
            StringBuilder newWord = new StringBuilder(50);
            int currentChar = 0;
            int newCharCount = 0;
            int currentLine = 0;
            int blankLength = 0;
            bool newLine = true;

            // 1. 공백과 단어를 하나 읽어들임
            // 2. 문자값 초과라면 저 단어는 승인목록에 안넣음
            // 3. 현재 승인목록들을 한 라인으로 만들기
            // 4. 라인이 다 찼다면 한 페이지 생성
            // 5. 승인목록에 방금 거절한 승인값 추가
            // 2. 초과가 아니라면 저 단어도 승인목록에 넣음
            for (int i = 0; i < origin.Length;)
            {
                if (newWord.Length == 0)
                {
                    for (; i < origin.Length && char.IsWhiteSpace(origin[i]); i++)
                    {
                        newWord.Append(origin[i]);
                        newCharCount++;
                        blankLength++;
                    }
                    for (; i < origin.Length && !char.IsWhiteSpace(origin[i]); i++)
                    {
                        newWord.Append(origin[i]);
                        if (origin[i] >= 0 && origin[i] < 128)
                            newCharCount++;
                        else
                            newCharCount += 2;
                    }
                }

                if (newLine)
                {
                    page.Append(newWord.ToString(blankLength, newWord.Length - blankLength));
                    newWord.Clear();
                    currentChar += newCharCount - blankLength;
                    blankLength = 0;
                    newCharCount = 0;
                    newLine = false;
                }
                else if (currentChar + newCharCount > charPerLine)
                {
                    currentLine++;
                    if (currentLine >= linePerPage)
                    {
                        pages.Add(page.ToString());
                        page.Clear();
                        currentLine = 0;
                    }
                    else
                        page.Append("\r\n");
                    currentChar = 0;
                    newLine = true;
                }
                else
                {
                    page.Append(newWord.ToString());
                    newWord.Clear();
                    blankLength = 0;
                    currentChar += newCharCount;
                    newCharCount = 0;
                }
            }
            if (newWord.Length > 0)
            {
                if (newLine)
                    page.Append(newWord.ToString(blankLength, newWord.Length - blankLength));
                else
                    page.Append(newWord.ToString());
            }
            if (page.Length > 0)
                pages.Add(page.ToString());

            if (pages.Count == 0)
                pages.Add("");

            return pages.ToArray();
        }

        /// <summary>
        /// 주어진 문자열에서 숫자만 남도록 재단합니다.
        /// </summary>
        /// <param name="origin">원본 문자열</param>
        /// <returns></returns>
        static public string makeOnlyNum(string origin)
        {
            StringBuilder output = new StringBuilder(origin);
            for (int i = 0; i < output.Length; i++)
                if (!char.IsDigit(output[i]))
                    output.Remove(i--, 1);

            return output.ToString();
        }
    }
}
