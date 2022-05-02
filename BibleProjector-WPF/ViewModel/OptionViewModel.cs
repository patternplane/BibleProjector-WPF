using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class OptionViewModel
    {

        // ========================================== View 연결 속성들 ========================================== 

        public string LinePerSlide_Text { 
            get 
            {
                return module.ProgramOption.Bible_LinePerSlide.ToString();
            } 
            set
            {
                module.ProgramOption.Bible_LinePerSlide = int.Parse(value);
            }
        }
        public string CharPerLine_Text 
        {
            get
            {
                return module.ProgramOption.Bible_CharPerLine.ToString();
            }
            set
            {
                module.ProgramOption.Bible_CharPerLine = int.Parse(value);
            }
        }

        public string BibleFramePath_Text 
        {
            get
            {
                return module.ProgramOption.BibleFramePath;
            }
            set
            {
                module.ProgramOption.BibleFramePath = value;
            }
        }
        public string ReadingFramePath_Text
        {
            get
            {
                return module.ProgramOption.ReadingFramePath;
            }
            set
            {
                module.ProgramOption.ReadingFramePath = value;
            }
        }
        public List<String> SongFramePaths_List
        {
            get
            {
                return module.ProgramOption.SongFramePath;
            }
            set
            {
                module.ProgramOption.SongFramePath = value;
            }
        }

        // ========================================== 일반 속성들 ========================================== 

        System.Windows.Forms.OpenFileDialog FD_BibleFrame;
        System.Windows.Forms.OpenFileDialog FD_ReadingFrame;
        System.Windows.Forms.OpenFileDialog FD_SongFrame;

        // ========================================== 프로그램 세팅 ========================================== 

        public OptionViewModel()
        {
            SetFileDialogs();
        }

        void SetFileDialogs()
        {
            FD_BibleFrame = new System.Windows.Forms.OpenFileDialog();
            FD_ReadingFrame = new System.Windows.Forms.OpenFileDialog();
            FD_SongFrame = new System.Windows.Forms.OpenFileDialog();

            FD_BibleFrame.InitialDirectory = FD_ReadingFrame.InitialDirectory = FD_SongFrame.InitialDirectory
                = ".\\Frame\\";
            FD_BibleFrame.Multiselect = FD_ReadingFrame.Multiselect = FD_SongFrame.Multiselect
                = false;
            FD_BibleFrame.Filter = FD_ReadingFrame.Filter = FD_SongFrame.Filter
                = "PowerPoint파일(*.ppt,*.pptx,*.pptm)|*.ppt;*.pptx;*.pptm";
        }

        // ========================================== 메서드 ========================================== 

        bool isValidFrameFile(string path)
        {
            if (BibleFramePath_Text != null && BibleFramePath_Text.CompareTo(path) == 0)
                return false;
            if (ReadingFramePath_Text != null && ReadingFramePath_Text.CompareTo(path) == 0)
                return false;
            foreach (string s in SongFramePaths_List)
                if (s.CompareTo(path) == 0)
                    return false;

            return true;
        }

        public void setBibleFrame()
        {
            if (FD_BibleFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            while (!isValidFrameFile(FD_BibleFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.","중복된 파일 등록",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);
                if (FD_BibleFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_BibleFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_BibleFrame.FileName) + "\\";

            if (BibleFramePath_Text == null)
            {
                BibleFramePath_Text = FD_BibleFrame.FileName;
                Powerpoint.Bible.setPresentation(BibleFramePath_Text);
            }
            else
            {
                BibleFramePath_Text = FD_BibleFrame.FileName;
                Powerpoint.Bible.refreshPresentation(BibleFramePath_Text);
            }
        }

        public void setReadingFrame()
        {
            if (FD_ReadingFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            while (!isValidFrameFile(FD_ReadingFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.", "중복된 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                if (FD_ReadingFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_ReadingFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_ReadingFrame.FileName) + "\\";

            if (ReadingFramePath_Text == null)
            {
                ReadingFramePath_Text = FD_ReadingFrame.FileName;
                Powerpoint.Reading.setPresentation(ReadingFramePath_Text);
            }
            else
            {
                ReadingFramePath_Text = FD_ReadingFrame.FileName;
                Powerpoint.Reading.refreshPresentation(ReadingFramePath_Text);
            }
        }

        public void setSongFrame()
        {
            if (FD_SongFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            while (!isValidFrameFile(FD_SongFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.", "중복된 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                if (FD_SongFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_SongFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_SongFrame.FileName) + "\\";
            string newFilePath = FD_SongFrame.FileName;
            if (!SongFramePaths_List.Exists(x => x.CompareTo(newFilePath) == 0))
            {
                SongFramePaths_List.Add(newFilePath);
                Powerpoint.Song.setPresentation(newFilePath);
            }
        }

        public void deleteSongFrame(int[] itemIndex)
        {
            for (int i = itemIndex.Length - 1; i >= 0; i--)
            {
                Powerpoint.Song.closeSingle(SongFramePaths_List[itemIndex[i]]);
                SongFramePaths_List.Remove(SongFramePaths_List[itemIndex[i]]);
            }
        }
    }
}
