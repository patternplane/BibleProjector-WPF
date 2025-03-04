﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;

namespace BibleProjector_WPF.module
{
    public class SongFrameFile : ViewModel.ViewModel
    {
        public string DisplayName
        {
            get 
            { 
                return FileName + " [" + Path + "]";
            }
        }
        public string Path { get; set; }
        public string FileName { get; set; }
        bool _isCCMFrame;
        public bool isCCMFrame
        {
            get { return _isCCMFrame; }
            set
            {
                _isCCMFrame = value;
                ProgramOption.setThisFrameToCCM(this,value);
                OnPropertyChanged("isCCMFrame");
            }
        }
        bool _isHymnFrame;
        public bool isHymnFrame
        {
            get { return _isHymnFrame; }
            set
            {
                _isHymnFrame = value;
                ProgramOption.setThisFrameToHymn(this,value);
                OnPropertyChanged("isHymnFrame");
            }
        }

        public SongFrameFile(string path, bool isCCM = false, bool isHymn = false)
        {
            this.Path = path;
            this.FileName = System.IO.Path.GetFileName(path);
            this.isCCMFrame = isCCM;
            this.isHymnFrame = isHymn;
        }
    }

    class ProgramOption
    {

        // ======================================= 옵션값 =======================================

        static private int _Bible_CharPerLine = 20;
        static public int Bible_CharPerLine { get { return _Bible_CharPerLine; } set { _Bible_CharPerLine = value; saveData(false); } }
        static private int _Bible_LinePerSlide = 5; 
        static public int Bible_LinePerSlide { get { return _Bible_LinePerSlide; } set { _Bible_LinePerSlide = value; saveData(false); } }

        static private int _Song_LinePerSlide = 2;
        static public int Song_LinePerSlide { get { return _Song_LinePerSlide; } set { _Song_LinePerSlide = value; saveData(false); } }

        static public string _BibleFramePath = null;
        static public string BibleFramePath { get { return _BibleFramePath; } set { _BibleFramePath = value; saveData(false); } }
        static public string _ReadingFramePath = null;
        static public string ReadingFramePath { get { return _ReadingFramePath; } set { _ReadingFramePath = value; saveData(false); } }
        static public BindingList<SongFrameFile> SongFrameFiles { get; set; } = new BindingList<SongFrameFile>();

        static public SongFrameFile _DefaultCCMFrame = null;
        static public SongFrameFile DefaultCCMFrame { get { return _DefaultCCMFrame; } set { _DefaultCCMFrame = value; saveData(false); } }
        static public SongFrameFile _DefaultHymnFrame = null;
        static public SongFrameFile DefaultHymnFrame { get { return _DefaultHymnFrame; } set { _DefaultHymnFrame = value; saveData(false); } }

        // ======================================= PPT 틀 관리 관련 =======================================

        static public bool isValidFrameFile(string path)
        {
            string fileName = Path.GetFileName(path);
            if (BibleFramePath != null && Path.GetFileName(BibleFramePath).CompareTo(fileName) == 0)
                return false;
            if (ReadingFramePath != null && Path.GetFileName(ReadingFramePath).CompareTo(fileName) == 0)
                return false;
            foreach (SongFrameFile f in SongFrameFiles)
                if (f.FileName.CompareTo(fileName) == 0)
                    return false;

            return true;
        }

        static public void setBibleFrameFile(string newFilePath)
        {
            if (BibleFramePath == null)
                Powerpoint.Bible.setPresentation(newFilePath);
            else
                Powerpoint.Bible.refreshPresentation(newFilePath);
            BibleFramePath = newFilePath;
        }

        static public void setReadingFrameFile(string newFilePath)
        {
            if (ReadingFramePath == null)
                Powerpoint.Reading.setPresentation(newFilePath);
            else
                Powerpoint.Reading.refreshPresentation(newFilePath);
            ReadingFramePath = newFilePath;
        }

        static public void setSongFrameFile(string newFilePath, bool isCCM = false, bool isHymn = false)
        {
            SongFrameFiles.Add(new SongFrameFile(newFilePath, isCCM, isHymn));
            Powerpoint.Song.setPresentation(newFilePath);

            saveData(false);
        }

        static public void deleteSongFrameFiles(int[] itemIndexes_sorted)
        {
            for (int i = itemIndexes_sorted.Length - 1; i >= 0; i--)
            {
                module.ProgramOption.process_deleteSongFrame(SongFrameFiles[itemIndexes_sorted[i]]);
                Powerpoint.Song.closeSingle(SongFrameFiles[itemIndexes_sorted[i]].Path);
                SongFrameFiles.RemoveAt(itemIndexes_sorted[i]);
            }

            saveData(false);
        }

        // ======================================= 찬양PPT틀 사용 가능성 확인 =======================================

        static public bool canExcutableSongFrame(string frameFilePath)
        {
            foreach (SongFrameFile f in SongFrameFiles)
                if (f.Path.CompareTo(frameFilePath) == 0)
                    return true;
            return false;
        }

        // ======================================= 찬양PPT틀 지울때 처리해야 하는... =======================================

        static public event Event.FrameDeletedEventHandler FrameDeletedEvent;

        static public void process_deleteSongFrame(SongFrameFile item)
        {
            if (item.isCCMFrame)
                DefaultCCMFrame = null;
            if (item.isHymnFrame)
                DefaultHymnFrame = null;

            FrameDeletedEvent.Invoke(null, new Event.FrameDeletedEventArgs(item.Path));
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

        static public string Initialize()
        {
            loadSavedData();

            StringBuilder pptFrameError = new StringBuilder(10);

            if (BibleFramePath == null)
                pptFrameError.Append("성경 ppt틀\r\n");

            // 교독문 미사용
            /*if (ReadingFramePath == null)
                pptFrameError.Append("교독문 ppt틀\r\n");*/

            if (SongFrameFiles.Count == 0)
                pptFrameError.Append("찬양 ppt틀\r\n");

            return (pptFrameError.Length == 0 ? null : pptFrameError.ToString());
        }

        const string SEPARATOR = "∂";
        const int IS_CCM_FRAME = 1;
        const int IS_HYMN_FRAME = 2;

        static private bool isLoading = false;

        static void loadSavedData()
        {
            string[] items = ProgramData.getOptionData().Split(new string[] { SEPARATOR }, StringSplitOptions.None);
            if (items.Length == 1)
                return;

            isLoading = true;
            
            if (items[0].CompareTo("") != 0)
                Bible_CharPerLine = int.Parse(items[0]);
            if (items[1].CompareTo("") != 0)
                Bible_LinePerSlide = int.Parse(items[1]);

            if (items[2].CompareTo("") != 0)
            {
                FileInfo file = new FileInfo(items[2]);
                if (file.Exists)
                    setBibleFrameFile(items[2]);
            }
            if (items[3].CompareTo("") != 0)
            {
                FileInfo file = new FileInfo(items[3]);
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
                        FileInfo file = new FileInfo(items[i + 1]);
                        if (file.Exists)
                            setSongFrameFile(
                                items[i + 1],
                                ((int.Parse(items[i]) & IS_CCM_FRAME) != 0),
                                ((int.Parse(items[i]) & IS_HYMN_FRAME) != 0)
                                );
                    }
                }

                // 구버전
                else
                {
                    for (int i = 4; i < items.Length; i++)
                    {
                        FileInfo file = new FileInfo(items[i]);
                        if (file.Exists)
                            setSongFrameFile(items[i]);
                    }
                }
            }

            isLoading = false;
        }

        static public void saveData(object sender, EventArgs e)
        {
            saveData(true);
        }

        static private void saveData(bool isImmidiate)
        {
            // 데이터 로드중이면 저장값을 즉시 읽어오고 변동은 없는 상태이므로
            // 이 저장과정은 불필요하다.(생략)
            if (isLoading)
                return;

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
                str.Append((f.isCCMFrame ? IS_CCM_FRAME : 0) | (f.isHymnFrame ? IS_HYMN_FRAME : 0));
                str.Append(SEPARATOR);
                str.Append(f.Path);
            }

            ProgramData.saveData(SaveDataTypeEnum.OptionData, str.ToString(), isImmidiate);
        }
    }
}
