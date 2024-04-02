using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    public class OptionViewModel : ViewModel
    {

        // ========================================== View 연결 속성들 ========================================== 

        public string LinePerSlide_Text {
            get
            {
                return module.ProgramOption.Bible_LinePerSlide.ToString();
            }
            set
            {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    module.ProgramOption.Bible_LinePerSlide = 0;
                else
                    module.ProgramOption.Bible_LinePerSlide = int.Parse(res);
                OnPropertyChanged();
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
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    module.ProgramOption.Bible_CharPerLine = 0;
                else
                    module.ProgramOption.Bible_CharPerLine = int.Parse(res);
                OnPropertyChanged();
            }
        }

        public string SongLinePerSlide_Text
        {
            get
            {
                return module.ProgramOption.Song_LinePerSlide.ToString();
            }
            set
            {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    module.ProgramOption.Song_LinePerSlide = 0;
                else
                    module.ProgramOption.Song_LinePerSlide = int.Parse(res);
                OnPropertyChanged();
            }
        }

        public string BibleFramePath_Display
        {
            get
            {
                if (BibleFramePath_Text == null)
                    return null;
                else
                    return System.IO.Path.GetFileName(BibleFramePath_Text) + " [" + BibleFramePath_Text + "]";
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
                OnPropertyChanged();
            }
        }
        public string ReadingFramePath_Display
        {
            get
            {
                if (ReadingFramePath_Text == null)
                    return null;
                else
                    return System.IO.Path.GetFileName(ReadingFramePath_Text) + " [" + ReadingFramePath_Text + "]";
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
                OnPropertyChanged();
            }
        }
        public BindingList<module.SongFrameFile> SongFramePaths_List { get { return module.ProgramOption.SongFrameFiles; } set{}
     }

        // ========================================== 프로그램 세팅 ========================================== 

        public OptionViewModel()
        {
            SetFileDialogs();
        }

        System.Windows.Forms.OpenFileDialog FD_BibleFrame;
        System.Windows.Forms.OpenFileDialog FD_ReadingFrame;
        System.Windows.Forms.OpenFileDialog FD_SongFrame;

        void SetFileDialogs()
        {
            FD_BibleFrame = new System.Windows.Forms.OpenFileDialog();
            FD_ReadingFrame = new System.Windows.Forms.OpenFileDialog();
            FD_SongFrame = new System.Windows.Forms.OpenFileDialog();

            FD_BibleFrame.InitialDirectory = FD_ReadingFrame.InitialDirectory = FD_SongFrame.InitialDirectory
                = System.IO.Directory.GetCurrentDirectory();
            FD_BibleFrame.Multiselect = FD_ReadingFrame.Multiselect = false;
            FD_SongFrame.Multiselect = true;
            FD_BibleFrame.Filter = FD_ReadingFrame.Filter = FD_SongFrame.Filter
                = "PowerPoint파일(*.ppt,*.pptx,*.pptm)|*.ppt;*.pptx;*.pptm";
        }

        // ========================================== 메서드 ========================================== 

        public void setBibleFrame()
        {
            if (FD_BibleFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            while (!module.ProgramOption.isValidFrameFile(FD_BibleFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.","중복된 파일 등록",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);
                if (FD_BibleFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_BibleFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_BibleFrame.FileName) + "\\";

            module.ProgramOption.setBibleFrameFile(FD_BibleFrame.FileName);
            OnPropertyChanged("BibleFramePath_Text");
        }

        public void refreshBibleFrame()
        {
            if (BibleFramePath_Text == null)
                return;

            if (!new System.IO.FileInfo(BibleFramePath_Text).Exists)
            {
                System.Windows.MessageBox.Show("해당 틀 파일이 존재하지 않습니다!\r\n" + BibleFramePath_Text, "틀 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                BibleFramePath_Text = null;
            }
            else
                Powerpoint.Bible.refreshPresentation(BibleFramePath_Text);
        }

        public void setReadingFrame()
        {
            if (FD_ReadingFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            while (!module.ProgramOption.isValidFrameFile(FD_ReadingFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.", "중복된 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                if (FD_ReadingFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_ReadingFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_ReadingFrame.FileName) + "\\";

            module.ProgramOption.setReadingFrameFile(FD_ReadingFrame.FileName);
            OnPropertyChanged("ReadingFramePath_Text");
        }

        public void refreshReadingFrame()
        {
            if (ReadingFramePath_Text == null)
                return;

            if (!new System.IO.FileInfo(ReadingFramePath_Text).Exists)
            {
                System.Windows.MessageBox.Show("해당 틀 파일이 존재하지 않습니다!\r\n" + ReadingFramePath_Text, "틀 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                ReadingFramePath_Text = null;
            }
            else
                Powerpoint.Reading.refreshPresentation(ReadingFramePath_Text);
        }

        public void setSongFrame()
        {
            if (FD_SongFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            FD_SongFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_SongFrame.FileName) + "\\";

            StringBuilder overlappedFile = new StringBuilder();
            foreach (string newFilePath in FD_SongFrame.FileNames)
            {
                if (module.ProgramOption.isValidFrameFile(newFilePath))
                    module.ProgramOption.setSongFrameFile(newFilePath);
                else
                {
                    overlappedFile.Append(newFilePath);
                    overlappedFile.Append("\r\n");
                }
            }
            if (overlappedFile.Length > 0)
                System.Windows.MessageBox.Show("다음 파일들은 이미 사용중인 ppt 틀입니다.\r\n" + overlappedFile.ToString(), "중복된 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error); ;
        }

        public void refreshSongFrame(int[] itemIndex)
        {
            StringBuilder nonFrame = new StringBuilder(50);

            for (int i = itemIndex.Length - 1; i >= 0; i--)
            {
                if (!new System.IO.FileInfo(SongFramePaths_List[itemIndex[i]].Path).Exists)
                {
                    nonFrame.Append("\r\n");
                    nonFrame.Append(SongFramePaths_List[itemIndex[i]].Path);
                    Powerpoint.Song.closeSingle(SongFramePaths_List[itemIndex[i]].Path);
                    SongFramePaths_List.RemoveAt(itemIndex[i]);
                }
                else
                    Powerpoint.Song.refreshPresentation(SongFramePaths_List[itemIndex[i]].Path);
            }
            if (nonFrame.Length != 0)
                System.Windows.MessageBox.Show("해당 틀 파일이 존재하지 않습니다!" + nonFrame.ToString(), "틀 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error); ;
        }

        public void deleteSongFrame(int[] itemIndex)
        {
            module.ProgramOption.deleteSongFrameFiles(itemIndex);
        }
    }
}
