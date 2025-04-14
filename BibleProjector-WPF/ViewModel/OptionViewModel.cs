using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using BibleProjector_WPF.module.Infrastructure;
using BibleProjector_WPF.module.ProgramOption;

namespace BibleProjector_WPF.ViewModel
{
    public class OptionViewModel : ViewModel
    {

        // ========================================== View 연결 속성들 ========================================== 

        public string LinePerSlide_Text {
            get
            {
                return ProgramOptionManager.Bible_LinePerSlide.ToString();
            }
            set
            {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    ProgramOptionManager.Bible_LinePerSlide = 0;
                else
                    ProgramOptionManager.Bible_LinePerSlide = int.Parse(res);
                OnPropertyChanged();
            }
        }
        public string CharPerLine_Text
        {
            get
            {
                return ProgramOptionManager.Bible_CharPerLine.ToString();
            }
            set
            {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    ProgramOptionManager.Bible_CharPerLine = 0;
                else
                    ProgramOptionManager.Bible_CharPerLine = int.Parse(res);
                OnPropertyChanged();
            }
        }

        public string SongLinePerSlide_Text
        {
            get
            {
                return ProgramOptionManager.Song_LinePerSlide.ToString();
            }
            set
            {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    ProgramOptionManager.Song_LinePerSlide = 0;
                else
                    ProgramOptionManager.Song_LinePerSlide = int.Parse(res);
                OnPropertyChanged();
            }
        }

        public string BibleFramePath_Display
        {
            get
            {
                if (ProgramOptionManager.BibleFramePath == null)
                    return null;
                else
                    return System.IO.Path.GetFileName(ProgramOptionManager.BibleFramePath) + " [" + ProgramOptionManager.BibleFramePath + "]";
            }
        }
        public string ReadingFramePath_Display
        {
            get
            {
                if (ProgramOptionManager.ReadingFramePath == null)
                    return null;
                else
                    return System.IO.Path.GetFileName(ProgramOptionManager.ReadingFramePath) + " [" + ProgramOptionManager.ReadingFramePath + "]";
            }
        }
        public BindingList<SongFrameFile> SongFramePaths_List { get { return ProgramOptionManager.SongFrameFiles; } set{}
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
            while (!ProgramOptionManager.isValidFrameFile(FD_BibleFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.","중복된 파일 등록",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);
                if (FD_BibleFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_BibleFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_BibleFrame.FileName) + "\\";

            ProgramOptionManager.setBibleFrameFile(FD_BibleFrame.FileName);
            OnPropertyChanged("BibleFramePath_Display");
        }

        public void refreshBibleFrame()
        {
            if (ProgramOptionManager.BibleFramePath == null)
                return;

            if (!new System.IO.FileInfo(ProgramOptionManager.BibleFramePath).Exists)
            {
                System.Windows.MessageBox.Show("해당 틀 파일이 존재하지 않습니다!\r\n" + ProgramOptionManager.BibleFramePath, "틀 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                ProgramOptionManager.BibleFramePath = null;
            }
            else
                Powerpoint.Bible.refreshPresentation(ProgramOptionManager.BibleFramePath);
        }

        public void setReadingFrame()
        {
            if (FD_ReadingFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            while (!ProgramOptionManager.isValidFrameFile(FD_ReadingFrame.FileName))
            {
                System.Windows.MessageBox.Show("이미 사용중인 ppt 틀입니다.", "중복된 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                if (FD_ReadingFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            FD_ReadingFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_ReadingFrame.FileName) + "\\";

            ProgramOptionManager.setReadingFrameFile(FD_ReadingFrame.FileName);
            OnPropertyChanged("ReadingFramePath_Display");
        }

        public void refreshReadingFrame()
        {
            if (ProgramOptionManager.ReadingFramePath == null)
                return;

            if (!new System.IO.FileInfo(ProgramOptionManager.ReadingFramePath).Exists)
            {
                System.Windows.MessageBox.Show("해당 틀 파일이 존재하지 않습니다!\r\n" + ProgramOptionManager.ReadingFramePath, "틀 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                ProgramOptionManager.ReadingFramePath = null;
            }
            else
                Powerpoint.Reading.refreshPresentation(ProgramOptionManager.ReadingFramePath);
        }

        public void setSongFrame()
        {
            if (FD_SongFrame.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            FD_SongFrame.InitialDirectory = System.IO.Path.GetDirectoryName(FD_SongFrame.FileName) + "\\";

            StringBuilder overlappedFile = new StringBuilder();
            foreach (string newFilePath in FD_SongFrame.FileNames)
            {
                if (ProgramOptionManager.isValidFrameFile(newFilePath))
                    ProgramOptionManager.setSongFrameFile(newFilePath);
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
            ProgramOptionManager.deleteSongFrameFiles(itemIndex);
        }
    }
}
