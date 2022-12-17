using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BibleProjector_WPF.module
{
    internal class ShowStarter
    {
        public void BibleShowStart(string KuenJangJeul)
        {
            ControlWindowManager cwm = new ControlWindowManager();
            cwm.showBibleControl().showBible(KuenJangJeul);
        }

        public void ReadingShowStart(int ReadingIndex)
        {
            ControlWindowManager cwm = new ControlWindowManager();
            cwm.showReadingControl().showReading(ReadingIndex);
        }

        public void SongShowStart(ViewModel.SingleLyric lyric, int linePerSlide, string FrameFilePath)
        {
            ControlWindowManager cwm = new ControlWindowManager();
            cwm.showSongControl().showSong(lyric, linePerSlide, FrameFilePath);
        }

        public void ExternPPTShowStart(string fileName, int StartSlide)
        {
            ControlWindowManager cwm = new ControlWindowManager();
            cwm.showExternPPTControl().ShowExternPPT(fileName, StartSlide);
        }
    }
}
