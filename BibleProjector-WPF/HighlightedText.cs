using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF
{
    public class HighlightedText
    {
        public string text;
        public (int, int, HighlightType)[] positions;

        public HighlightedText(string text, (int, int)[] positions, HighlightType type)
        {
            this.text = text;
            this.positions = new (int, int, HighlightType)[positions.Length];
            for (int i = 0; i < positions.Length; i++)
                this.positions[i] = (positions[i].Item1, positions[i].Item2, type);
        }

        public HighlightedText(string text, (int, int, HighlightType)[] positions)
        {
            this.text = text;
            this.positions = positions;
        }
    }
}
