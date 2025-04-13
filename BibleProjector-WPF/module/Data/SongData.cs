using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class SongData : ShowData
    {
        public string songTitle { get; set; }
        string subTitle;
        public SongContent songContent { get; set; }
        ShowContentData[] currentContents = null;
        public SongDataTypeEnum songType { get; private set; }
        public string pptFrameFullPath = null;
        int _linePerSlide = -1;
        public int linePerSlide { get { if (_linePerSlide == -1) return ProgramOptionManager.Song_LinePerSlide; else return _linePerSlide; } set { _linePerSlide = value; } }

        public SongData(string songTitle, SongContent songContent, SongDataTypeEnum songType, string subTitle)
        {
            songDataSetup(songTitle, songContent, songType, subTitle);
        }

        void songDataSetup(string songTitle, SongContent songContent, SongDataTypeEnum songType, string subTitle)
        {
            this.currentContents = null;

            this.songTitle = songTitle;
            this.songContent = songContent;
            this.songType = songType;
            if (songType == SongDataTypeEnum.HYMN
                && subTitle == null)
                throw new Exception("새찬송가 정보에는 subTitle이 포함되어야 합니다.");
            else
                this.subTitle = subTitle;
        }

        // ================ ShowData 메소드 ================

        public override string getdisplayName(bool isModified)
        {
            return getTitle2();
        }

        public override string getPreviewContent()
        {
            StringBuilder preview = new StringBuilder();
            string[] lyrics = songContent.getContents();
            for (int i = 0; i < lyrics.Length; i++)
            {
                preview.Append(" [ ").Append(i + 1).Append("절 ]").Append('\n');
                preview.Append(lyrics[i]);
                if (i < lyrics.Length - 1)
                    preview.Append("\n\n");
            }
            return preview.ToString();
        }

        public override string getTitle1()
        {
            if (songType == SongDataTypeEnum.CCM)
                return "복음성가(CCM)";
            else if (songType == SongDataTypeEnum.HYMN)
                return "새찬송가 " + songTitle + "장";
            else
                return "-";
        }

        public override string getTitle2()
        {
            if (songType == SongDataTypeEnum.CCM)
                return songTitle;
            else if (songType == SongDataTypeEnum.HYMN)
                return subTitle;
            else
                return "-";
        }

        public override ShowContentData[] getContents()
        {
            // 반드시 songData는 모든 슬라이드마다 0. 제목, 1. 가사 일 것!
            // songData 규격 : [정보 번호][커맨드문구(0)냐 내용(1)이냐]

            List<ShowContentData> songData = new List<ShowContentData>();

            for (int verse = 0; verse < songContent.lyricCount; verse++)
            {
                string[] pages = StringModifier.makePageWithLines(songContent.getContentByVerse(verse), linePerSlide);
                if (this.songType == SongDataTypeEnum.CCM)
                {
                    for (int j = 0; j < pages.Length; j++)
                    {
                        string[][] pageData = new string[2][];

                        pageData[0] = new string[2];
                        pageData[0][0] = "{t}";
                        pageData[0][1] = songTitle;

                        pageData[1] = new string[2];
                        pageData[1][0] = "{c}";
                        pageData[1][1] = pages[j];

                        songData.Add(new ShowContentData(pageData, pages[j], false));
                    }
                }
                else if (this.songType == SongDataTypeEnum.HYMN)
                {
                    for (int j = 0; j < pages.Length; j++)
                    {
                        string[][] pageData = new string[4][];

                        pageData[0] = new string[2];
                        pageData[0][0] = "{t}";
                        pageData[0][1] = songTitle;

                        pageData[1] = new string[2];
                        pageData[1][0] = "{c}";
                        pageData[1][1] = pages[j];

                        pageData[2] = new string[2];
                        pageData[2][0] = "{v}";
                        pageData[2][1] = (j == 0) ?
                            (verse + 1).ToString()
                            : "";

                        pageData[3] = new string[2];
                        pageData[3][0] = "{va}";
                        pageData[3][1] = (verse + 1).ToString();

                        if (j == 0)
                            songData.Add(new ShowContentData(pageData, "    " + (verse + 1) + " 절\r\n" + pages[j] + "\r\n", true));
                        else
                            songData.Add(new ShowContentData(pageData, pages[j], false));
                    }
                }
                else
                    throw new Exception("가사 처리 오류 : [" + this.songType.ToString() + "] 형식의 가사를 처리할 수 없습니다!");
            }

            currentContents = songData.ToArray();
            
            return currentContents;
        }

        public override ShowData getNextShowData()
        {
            return null;
        }

        public override ShowData getPrevShowData()
        {
            return null;
        }

        public override ShowContentType getDataType()
        {
            return ShowContentType.Song;
        }

        public override bool isSameData(ShowData data)
        {
            return (this == data);
        }

        public override ShowExcuteErrorEnum canExcuteShow()
        {
            if (!hasFrame())
                return ShowExcuteErrorEnum.NoneFrameFile;
            else
                return ShowExcuteErrorEnum.NoErrors;
        }

        public override bool isAvailData()
        {
            return true;
        }

        // ========================== 검증 ============================

        public void checkAndSetFrame()
        {
            if (pptFrameFullPath != null
                && ProgramOptionManager.canExcutableSongFrame(pptFrameFullPath))
                return;
            else if (songType == SongDataTypeEnum.CCM && ProgramOptionManager.DefaultCCMFrame != null)
                pptFrameFullPath = ProgramOptionManager.DefaultCCMFrame.Path;
            else if (songType == SongDataTypeEnum.HYMN && ProgramOptionManager.DefaultHymnFrame != null)
                pptFrameFullPath = ProgramOptionManager.DefaultHymnFrame.Path;
            else
                pptFrameFullPath = null;
        }

        public bool hasFrame()
        {
            checkAndSetFrame();
            return pptFrameFullPath != null;
        }
    }
}
