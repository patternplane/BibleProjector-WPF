using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace BibleProjector_WPF.module
{
    class ProgramOption
    {

        // ======================================= 옵션값 =======================================

        static public int Bible_CharPerLine { get; set; } = 20;
        static public int Bible_LinePerSlide { get; set; } = 5;


        static public string BibleFramePath { get; set; } = null;
        static public string ReadingFramePath { get; set; } = null;
        static public List<string> SongFramePath { get; set; } = new List<string>(2);


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
                BibleFramePath = items[2];
            if (items[3].CompareTo("") != 0)
                ReadingFramePath = items[3];
            for (int i = 4; i < items.Length; i++)
                SongFramePath.Add(items[i]);
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
            foreach (string path in SongFramePath)
            {
                str.Append(SEPARATOR);
                str.Append(path);
            }

            return str.ToString();
        }
    }
}
