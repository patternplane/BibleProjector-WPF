using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class ProgramOption
    {
        static public int Bible_CharPerLine;
        static public int Bible_LinePerSlide;

        static public void Initialize()
        {
            Bible_CharPerLine = 15;
            Bible_LinePerSlide = 5;
        }
    }
}
