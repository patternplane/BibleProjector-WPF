﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class SongSearcher : ISearcher
    {
        SongManager songManager = null;

        public SongSearcher(SongManager songManager)
        {
            this.songManager = songManager;
        }

        public ICollection<SearchData> getSearchResult(string phrase)
        {
            ICollection<SongSearchData> searchList = new List<SongSearchData>();

            int[] findPos;
            foreach (SongData song in songManager.CCMs)
            {
                if (StringKMP.HasPattern(song.songTitle, phrase, StringKMP.IgnoreCaseStringCompareFunc, false))
                    searchList.Add(new SongSearchData(song, "(" + song.songTitle + ")", 0));
            }
            foreach (SongData song in songManager.Hymns)
            {
                if (StringKMP.HasPattern(song.songTitle, phrase, StringKMP.IgnoreCaseStringCompareFunc, false))
                    searchList.Add(new SongSearchData(song, "(새찬송가 " + song.songTitle + "장)", 0));
            }

            foreach (SongData song in songManager.CCMs)
            {
                findPos = StringKMP.FindPattern(song.songContent.getRawContent(), phrase, module.StringKMP.IgnoreCaseStringCompareFunc, false);
                if (findPos.Length > 0)
                    foreach (string s in StringKMP.makeResultPreview(findPos, song.songContent.getRawContent(), phrase.Length))
                        searchList.Add(new SongSearchData(song, "(" + song.songTitle + ") " + s, 0));
            }
            foreach (SongData song in songManager.Hymns)
            {
                findPos = StringKMP.FindPattern(song.songContent.getRawContent(), phrase, module.StringKMP.IgnoreCaseStringCompareFunc, false);
                if (findPos.Length > 0)
                    foreach (string s in StringKMP.makeResultPreview(findPos, song.songContent.getRawContent(), phrase.Length))
                        searchList.Add(new SongSearchData(song, "(새찬송가 " + song.songTitle + "장) " + s, 0));
            }

            return searchList.ToArray();
        }
    }
}
