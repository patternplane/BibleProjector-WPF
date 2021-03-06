using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;

namespace BibleProjector_WPF.module
{
    class ProgramOption
    {

        // ======================================= 옵션값 =======================================

        static public int Bible_CharPerLine { get; set; } = 20;
        static public int Bible_LinePerSlide { get; set; } = 5;


        static public string BibleFramePath { get; set; } = null;
        static public string ReadingFramePath { get; set; } = null;
        public class SongFrameFile
        {
            public string Path { get; set; }
            public string FileName { get; set; }
        }
        static public BindingList<SongFrameFile> SongFrameFiles { get; set; } = new BindingList<SongFrameFile>();

        static public void test(SongFrameFile f)
        {
            SongFrameFiles.Add(f);
        }

        // ======================================= 세팅 및 종료 =======================================

        const string SEPARATOR = "∂";

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
            for (int i = 4; i < items.Length; i++)
            {
                System.IO.FileInfo file = new FileInfo(items[i]);
                if (file.Exists)
                    SongFrameFiles.Add(new SongFrameFile() { Path = items[i], FileName = System.IO.Path.GetFileName(items[i]) });
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
                str.Append(f.Path);
            }

            return str.ToString();
        }
    }
}
