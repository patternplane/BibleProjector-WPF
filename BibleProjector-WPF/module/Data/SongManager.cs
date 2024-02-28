﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class SongManager
    {
        public ICollection<SongData> CCMs;
        public ICollection<SongData> Hymns;

        public SongManager()
        {
            CCMs = getLyricList();
            Hymns = getHymnList();
        }

        // ============================================== 받아오기 ==============================================

        // 파일 입출력시 구분자
        public const string SEPARATOR = "∂";
        private const string HYMN_SEPARATOR = "∫";

        List<SongData> getLyricList()
        {
            List<SongData> PrimitiveLyricList = new List<SongData>(10);

            string rawData = ProgramData.getLyricData().TrimEnd();

            foreach (string data in rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] line = data.TrimStart().Split(new string[] { "\r\n" }, 2, StringSplitOptions.None);
                if (line.Length == 1)
                    PrimitiveLyricList.Add(new SongData(line[0], new SongContent("")));
                else if (line.Length == 2)
                    PrimitiveLyricList.Add(new SongData(line[0], new SongContent(line[1])));
            }

            return PrimitiveLyricList;
        }

        List<SongData> getHymnList()
        {
            List<SongData> PrimitiveHymnList = new List<SongData>(10);

            string rawData = module.ProgramData.getHymnData();

            string[] songs = rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < songs.Length; i++)
                PrimitiveHymnList.Add(
                    new SongData(
                        (i + 1).ToString()
                        , new SongContent (songs[i].Split(new string[] { HYMN_SEPARATOR }, StringSplitOptions.None))
                        )
                    );

            return PrimitiveHymnList;
        }

        // ============================================== 저장하기 ==============================================

        public string getSaveData_Lyric()
        {
            StringBuilder str = new StringBuilder(50).Clear();

            foreach (SongData lyric in CCMs)
            {
                str.AppendLine(lyric.songTitle);
                str.Append(lyric.songContent.getRawContent());
                str.AppendLine(SEPARATOR);
            }

            return str.ToString();
        }

        public string getSaveData_Hymn()
        {
            StringBuilder str = new StringBuilder(50).Clear();

            foreach (SongData hymn in Hymns)
            {
                str.Append(hymn.songContent.getContentByPage(1));
                for (int i = 2; i <= hymn.songContent.lyricCount; i++)
                {
                    str.Append(HYMN_SEPARATOR);
                    str.Append(hymn.songContent.getContentByPage(i));
                }
                str.Append(SEPARATOR);
            }

            return str.ToString();
        }
    }
}