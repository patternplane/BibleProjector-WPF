using BibleProjector_WPF.module.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class SongManager : ISourceOfReserve
    {
        public List<SongData> CCMs;
        public List<SongData> Hymns;

        public SongManager()
        {
            CCMs = getLyricList();
            Hymns = getHymnList();
        }

        // ============================================== 관리 메소드 ==============================================

        public int AddSongInOrder(SongData data)
        {
            int pos = 0;
            for (; pos < CCMs.Count; pos++)
                if (data.songTitle.CompareTo(CCMs[pos].songTitle) <= 0)
                    break;
            CCMs.Insert(pos, data);

            saveCCMData(false);
            return pos;
        }

        public void DeleteSongItem(SongData item)
        {
            if (CCMs.Contains(item))
            {
                item.deleteProcess();
                CCMs.Remove(item);

                saveCCMData(false);
            }
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
                    PrimitiveLyricList.Add(new SongData(line[0], new SongContent(""), SongDataTypeEnum.CCM, null));
                else if (line.Length == 2)
                    PrimitiveLyricList.Add(new SongData(line[0], new SongContent(line[1]), SongDataTypeEnum.CCM, null));
            }

            return PrimitiveLyricList;
        }

        List<SongData> getHymnList()
        {
            List<SongData> PrimitiveHymnList = new List<SongData>(10);

            string rawData = ProgramData.getHymnData();
            string rawData_hymnSub = ProgramData.getHymnSubData();

            string[] songs = rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            string[] subTitles = rawData_hymnSub.Split(new string[] { HYMN_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < songs.Length; i++)
                PrimitiveHymnList.Add(
                    new SongData(
                        (i + 1).ToString()
                        , new SongContent (songs[i].Split(new string[] { HYMN_SEPARATOR }, StringSplitOptions.None))
                        , SongDataTypeEnum.HYMN
                        , subTitles[i])
                    );

            return PrimitiveHymnList;
        }

        // ============================================== 저장하기 ==============================================

        /// <summary>
        /// 시스템 이벤트에 의한 저장메소드입니다. (일반적으로 사용하지 않음)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void saveData_Lyric(object sender, EventArgs e)
        {
            saveCCMData(true);
        }

        public void saveCCMData(bool isImmediate)
        {
            CCMs.Sort((a, b) => a.songTitle.CompareTo(b.songTitle));

            StringBuilder str = new StringBuilder(50).Clear();

            foreach (SongData lyric in CCMs)
            {
                str.AppendLine(lyric.songTitle);
                str.Append(lyric.songContent.getRawContent());
                str.AppendLine(SEPARATOR);
            }

            ProgramData.saveData(SaveDataTypeEnum.LyricData, str.ToString(), isImmediate);
        }

        /// <summary>
        /// 시스템 이벤트에 의한 저장메소드입니다. (일반적으로 사용하지 않음)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void saveData_Hymn(object sender, EventArgs e)
        {
            saveHymnData(true);
        }

        public void saveHymnData(bool isImmediate)
        {
            StringBuilder str = new StringBuilder(50).Clear();

            foreach (SongData hymn in Hymns)
            {
                str.Append(hymn.songContent.getContentByVerse(0));
                for (int i = 1; i < hymn.songContent.lyricCount; i++)
                {
                    str.Append(HYMN_SEPARATOR);
                    str.Append(hymn.songContent.getContentByVerse(i));
                }
                str.Append(SEPARATOR);
            }

            ProgramData.saveData(SaveDataTypeEnum.HymnData, str.ToString(), isImmediate);
        }

        // ============================================== 예약값 추출 지원 ==============================================

        public ShowData getItemByReserveInfo(int ReserveInfo)
        {
            if (ReserveInfo < -Hymns.Count || ReserveInfo >= CCMs.Count)
                return null;

            if (ReserveInfo < 0)
                return Hymns.ElementAt(-ReserveInfo - 1);
            else
                return CCMs.ElementAt(ReserveInfo);
        }

        public int getReserveInfoByItem(ShowData item)
        {
            if (((SongData)item).songType == SongDataTypeEnum.CCM)
            {
                for (int i = 0; i < CCMs.Count; i++)
                    if (CCMs.ElementAt(i) == item)
                        return i;
            }
            else
            {
                for (int i = 0; i < Hymns.Count; i++)
                    if (Hymns.ElementAt(i) == item)
                        return -(i+1);
            }
            return int.MinValue;
        }
    }
}
