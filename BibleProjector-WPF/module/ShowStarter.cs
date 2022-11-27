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

        public void SongShowStart(string[][][] songData, string path, bool isHymn)
        {
            // 곡별 사용할 틀에 대한 설계가 없어 수정되지 않음 -> ??
            ControlWindowManager cwm = new ControlWindowManager();
            cwm.showSongControl().showSong(songData, System.IO.Path.GetFileName(path), isHymn);
        }

        public void ExternPPTShowStart(string fileName, int StartSlide)
        {
            ControlWindowManager cwm = new ControlWindowManager();
            cwm.showExternPPTControl().ShowExternPPT(fileName, StartSlide);
        }
    }
}
