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
                typeof((string text, (int startIdx, int lastIdx)[] highlightPos)),
                typeof(TextHighlighter),
                new FrameworkPropertyMetadata(propertyChangedCallback: TextInfoChangedCallback)
                );

        public static (string text, (int startIdx, int lastIdx)[] highlightPos) GetTextInfo(UIElement target) =>
            ((string text, (int startIdx, int lastIdx)[] highlightPos))target.GetValue(TextInfoProperty);

        public static void SetTextInfo(UIElement target, (string text, (int startIdx, int lastIdx)[] highlightPos) value) =>
            target.SetValue(TextInfoProperty, value);

        private static void TextInfoChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock obj)
            {
                (string text, (int startIdx, int lastIdx)[] highlightPos) textInfo = ((string text, (int startIdx, int lastIdx)[] highlightPos))e.NewValue;
                if (textInfo.highlightPos == null || textInfo.highlightPos.Length == 0)
                {
                    obj.Text = textInfo.text;
                    return;
                }

                obj.Inlines.Clear();
                int i = 0;
                foreach ((int startIdx, int lastIdx) pos in textInfo.highlightPos)
                {
                    if (i < pos.startIdx)
                        obj.Inlines.Add(new Run(textInfo.text.Substring(i, pos.startIdx - i)));
                    obj.Inlines.Add(new Run(textInfo.text.Substring(pos.startIdx, pos.lastIdx - pos.startIdx + 1))
                        {
                            Foreground = new SolidColorBrush(Color.FromRgb(173, 79, 15)),
                            FontWeight = FontWeights.Bold,
                            TextDecorations = TextDecorations.Underline
                        });
                    i = pos.lastIdx + 1;
                }
                if (i < textInfo.text.Length)
                    obj.Inlines.Add(new Run(textInfo.text.Substring(i, textInfo.text.Length - i)));
            }
        }
    }
}
