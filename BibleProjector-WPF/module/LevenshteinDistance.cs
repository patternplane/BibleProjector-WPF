using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class LevenshteinDistance
    {
        KorString k = new KorString();
        int[,] dis;

        public int getLevenDis(string a, string b)
        {
            CalcLevenDis_WithKor(a, b);
            return dis[a.Length, b.Length];
        }

        /// <summary>
        /// b의 길이에 따른 편집거리 중 가장 적은 편집거리를 반환합니다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int getLevenDis_inShortestB(string a, string b)
        {
            CalcLevenDis_WithKor(a, b);

            int i = b.Length;
            int min = dis[a.Length, i];
            for (; i > 0; i--)
            {
                if (dis[a.Length, i - 1] <= min)
                    min = dis[a.Length, i - 1];
                else
                    break;
            }

            return min;
        }

        /// <summary>
        /// 문자열 a를 문자열 b까지 편집하는 거리를 구하는데,
        /// 편집비용이 가장 적은 b의 길이를 찾아 해당 편집비용을 계산합니다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int getLevenDis_min(string a, string b)
        {
            string a1 = k.DisassembleKorCons(a);
            string a2 = k.DisassembleKorCons(a, true);
            string b1 = k.DisassembleKorCons(b);

            int r1 = getLevenDis_inShortestB(a1, b1);
            int r2 = getLevenDis_inShortestB(a2, b1);

            return (r1 < r2 ? r1 : r2);
        }

        void CalcLevenDis_WithKor(string a, string b)
        {
            dis = new int[a.Length + 1, b.Length + 1];
            dis[0, 0] = 0;
            for (int i = 1; i < a.Length + 1; i++)
                dis[i, 0] = dis[i - 1, 0] + k.GetAddCost(a[i - 1]);
            for (int i = 1; i < b.Length + 1; i++)
                dis[0, i] = dis[0, i - 1] + k.GetAddCost(b[i - 1]);

            int distanceValue;
            int addValue, deleteValue, modifyValue;
            for (int i = 1; i <= a.Length; i++)
                for (int j = 1; j <= b.Length; j++)
                {
                    distanceValue = k.GetDistance_WithKor(a[i - 1], b[j - 1]);

                    if (distanceValue == 0)
                        dis[i, j] = dis[i - 1, j - 1];
                    else
                    {
                        addValue = dis[i, j - 1] + k.GetAddCost(b[j - 1]);
                        deleteValue = dis[i - 1, j] + k.GetAddCost(a[i - 1]);
                        modifyValue = dis[i - 1, j - 1] + distanceValue;

                        dis[i, j] = ((addValue < deleteValue) ?
                            (modifyValue < addValue ?
                            modifyValue :
                            addValue) :
                            (modifyValue < deleteValue ?
                            modifyValue :
                            deleteValue));
                    }
                }
        }
    }
}
