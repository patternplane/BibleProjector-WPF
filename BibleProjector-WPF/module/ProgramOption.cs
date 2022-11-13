using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;

namespace BibleProjector_WPF.module
{
    public class SongFrameFile : INotifyPropertyChanged
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        bool _isCCMFrame;
        public bool isCCMFrame
        {
            get { return _isCCMFrame; }
            set
            {
                ProgramOption.setThisFrameToCCM(this,value);
                _isCCMFrame = value;
                OnPropertyChanged("isCCMFrame");
            }
        }
        bool _isHymnFrame;
        public bool isHymnFrame
        {
            get { return _isHymnFrame; }
            set
            {
                ProgramOption.setThisFrameToHymn(this,value);
                _isHymnFrame = value;
                OnPropertyChanged("isHymnFrame");
            }
        }

        // INotifyPropertyChanged 인터페이스 관련

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    class ProgramOption
    {

        // ======================================= 옵션값 =======================================

        static public int Bible_CharPerLine { get; set; } = 20;
        static public int Bible_LinePerSlide { get; set; } = 5;


        static public string BibleFramePath { get; set; } = null;
        static public string ReadingFramePath { get; set; } = null;
        static public BindingList<SongFrameFile> SongFrameFiles { get; set; } = new BindingList<SongFrameFile>();

        static public void test(SongFrameFile f)
        {
            SongFrameFiles.Add(f);
        }

        static public SongFrameFile DefaultCCMFrame { get; set; } = null;
        static public SongFrameFile DefaultHymnFrame { get; set; } = null;

        // ======================================= 찬양PPT틀 지울때 처리해야 하는... =======================================
        // 너무 난잡한 구조다!
        // 한창 MVVM 공부하며 시간에 쫒겨 개발할 시절에 짠 코드라
        // 찬양곡 관련 부분들이 매우 개선 필요

        static public void process_deleteSongFrame(SongFrameFile item)
        {
            if (item.isCCMFrame)
                DefaultCCMFrame = null;
            if (item.isHymnFrame)
                DefaultHymnFrame = null;
        }

        // ======================================= 찬양PPT틀 선택가능 판단 =======================================

        static public void setThisFrameToCCM(SongFrameFile thisFrame, bool value)
        {
            if (value)
            {
                foreach (SongFrameFile file in SongFrameFiles)
                    if (file != thisFrame && file.isCCMFrame)
                        file.isCCMFrame = false;

                DefaultCCMFrame = thisFrame;
            }
            else
                if (DefaultCCMFrame == thisFrame)
                    DefaultCCMFrame = null;
        }

        static public void setThisFrameToHymn(SongFrameFile thisFrame, bool value)
        {
            if (value)
            {
                foreach (SongFrameFile file in SongFrameFiles)
                    if (file != thisFrame && file.isHymnFrame)
                        file.isHymnFrame = false;

                DefaultHymnFrame = thisFrame;
            }
            else
                if (DefaultHymnFrame == thisFrame)
                    DefaultHymnFrame = null;
        }

        // ======================================= 세팅 및 종료 =======================================

        const string SEPARATOR = "∂";
        const int IS_CCM_FRAME = 1;
        const int IS_HYMN_FRAME = 2;

        static public void Initialize()
        {
            string[] items = module.ProgramData.getOptionData().Split(new string[] { SEPARATOR }, StringSplitOptions.None);
            if (items.Length == 1)
                return;
            
            if (items[0].CompareTo("") != 0)
                Bible_CharPerLine = int.Parse(items[0]);
            if (items[1].CompareTo("") != 0)
                Bible_LinePerSlide = int.Parse(items[1]);

            if (items[2].CompareTo("") != 0)
            {
                System.IO.FileInfo file = new FileInfo(items[2]);
                if (file.Exists)
                    BibleFramePath = items[2];
            }
            if (items[3].CompareTo("") != 0)
            {
                System.IO.FileInfo file = new FileInfo(items[3]);
                if (file.Exists)
                    ReadingFramePath = items[3];
            }

            // 이전 버전의 저장값을 불러오면 오류 발생하므로 처리
            // 변경사항 : 찬양곡 ppt의 저장값에 ccm혹은 찬송가 프레임 선택 정보가 추가됨
            if (items.Length > 4)
            {
                int dummy;

                // 최근 버전
                if (int.TryParse(items[4], out dummy))
                {
                    for (int i = 4; i < items.Length; i += 2)
                    {
                        System.IO.FileInfo file = new FileInfo(items[i + 1]);
                        if (file.Exists)
                            SongFrameFiles.Add(new SongFrameFile()
                            {
                                Path = items[i + 1],
                                FileName = System.IO.Path.GetFileName(items[i + 1]),
                                isCCMFrame = int.Parse(items[i]) == IS_CCM_FRAME,
                                isHymnFrame = int.Parse(items[i]) == IS_HYMN_FRAME
                            });
                    }
                }

                // 구버전
                else
                {
                    for (int i = 4; i < items.Length; i++)
                    {
                        System.IO.FileInfo file = new FileInfo(items[i]);
                        if (file.Exists)
                            SongFrameFiles.Add(new SongFrameFile() { Path = items[i], FileName = System.IO.Path.GetFileName(items[i]) });
                    }
                }
            }

        }

        static public string getSaveData()
        {
            StringBuilder str = new StringBuilder(10);
            
            str.Append(Bible_CharPerLine.ToString());
            str.Append(SEPARATOR);
            str.Append(Bible_LinePerSlide.ToString());

            str.Append(SEPARATOR);
            str.Append(BibleFramePath);
            str.Append(SEPARATOR);
            str.Append(ReadingFramePath);
            foreach (SongFrameFile f in SongFrameFiles)
            {
                str.Append(SEPARATOR);
                str.Append(((f.isCCMFrame ? IS_CCM_FRAME : (f.isHymnFrame ? IS_HYMN_FRAME : 0))));
                str.Append(SEPARATOR);
                str.Append(f.Path);
            }

            return str.ToString();
        }
    }
}
