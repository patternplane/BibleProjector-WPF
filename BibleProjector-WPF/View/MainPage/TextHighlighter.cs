using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BibleProjector_WPF.View.MainPage
{
    public class TextHighlighter
    {
        public static readonly DependencyProperty TextInfoProperty =
            DependencyProperty.RegisterAttached(
                "TextInfo",
                typeof(HighlightedText),
                typeof(TextHighlighter),
                new FrameworkPropertyMetadata(propertyChangedCallback: TextInfoChangedCallback)
                );

        public static HighlightedText GetTextInfo(UIElement target) =>
            (HighlightedText)target.GetValue(TextInfoProperty);

        public static void SetTextInfo(UIElement target, HighlightedText value) =>
            target.SetValue(TextInfoProperty, value);

        private static void TextInfoChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock obj
                && e.NewValue is HighlightedText textInfo)
            {
                if (textInfo.positions == null || textInfo.positions.Length == 0)
                {
                    obj.Text = textInfo.text;
                    return;
                }

                obj.Inlines.Clear();
                int i = 0;
                foreach ((int startIdx, int lastIdx, HighlightType type) pos in textInfo.positions)
                {
                    if (i < pos.startIdx)
                        obj.Inlines.Add(new Run(textInfo.text.Substring(i, pos.startIdx - i)));
                    Run highlightedRun = new Run(textInfo.text.Substring(pos.startIdx, pos.lastIdx - pos.startIdx + 1));
                    if (pos.type == HighlightType.SEARCH_RESULT)
                    {
                        highlightedRun.Foreground = new SolidColorBrush(Color.FromRgb(173, 79, 15));
                        highlightedRun.FontWeight = FontWeights.Bold;
                        highlightedRun.TextDecorations = TextDecorations.Underline;
                    }
                    obj.Inlines.Add(highlightedRun);
                    i = pos.lastIdx + 1;
                }
                if (i < textInfo.text.Length)
                    obj.Inlines.Add(new Run(textInfo.text.Substring(i, textInfo.text.Length - i)));
            }
        }
    }
}
