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
        /// 문자열을 재단합니다.<br/>
        /// 줄별 문자의 갯수(단어 한 덩어리를 굳이 자르지는 않습니다)와 페이지별 줄 수를 입력받습니다.
        /// </summary>
        /// <param name="origin">원본 문자열</param>
        /// <param name="charPerLine">줄별 문자 수</param>
        /// <param name="linePerPage">페이지별 줄 수</param>
        /// <returns>재단된 문자열 페이지</returns>
        static public string[] makeStringPage(string origin, int charPerLine ,int linePerPage)
        {
            origin = origin.Trim();
            List<string> pages = new List<string>(10);
            StringBuilder line = new StringBuilder(50);
            int currentChar = 0;
            int currentLine = 0;
            // 글자를 한자 한자 읽어가면서
            //          공백이면 패스 (갯수 세면서
            //          단어 나오면 단어 끝까지 패스 (갯수 세면서
            //      만약 문자 수가 다 찼으면 새 라인 생성
            //      만약 끝났거나 줄 수가 다 찼으면 페이지 생성
            // 끝났으면 종료, 아니면 위를 반복
            for (int i = 0; i < origin.Length; i++)
            {
                for (; i < origin.Length && char.IsWhiteSpace(origin[i]); i++)
                {
                    line.Append(origin[i]);
                    currentChar++;
                }
                for (; i < origin.Length && !char.IsWhiteSpace(origin[i]); i++)
                {
                    line.Append(origin[i]);
                    if (origin[i] >= 0 && origin[i] < 128)
                        currentChar++;
                    else
                        currentChar += 2;
                }

                if (i == origin.Length) {
                    pages.Add(line.ToString());
                    break;
                }
                else if (currentChar >= charPerLine)
                {
                    if (currentLine == linePerPage)
                    {
                        pages.Add(line.ToString());
                        line.Clear();
                        currentChar = 0;
                        currentLine = 1;
                    }
                    else
                    {
                        line.Append("\r\n");
                        currentChar = 0;
                        currentLine++;
                    }
                    for (; i < origin.Length && char.IsWhiteSpace(origin[i]); i++) ;
                }
            }

            return pages.ToArray();
        }
    }
}
