using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.ProgramOption
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
                ProgramOptionManager.setThisFrameToCCM(this, value);
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
                ProgramOptionManager.setThisFrameToHymn(this, value);
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
}
