using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ppt
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office;



namespace BibleProjector_WPF
{
    partial class Powerpoint
    {
        static Application app;

        // ============================================ 프로그램 시작 / 종료 세팅 ========================================================

        static public void Initialize()
        {
            app = new Application();

            Bible.setPresentation(@"C:\Users\Sun\Desktop\test.pptx");
            Reading.setPresentation(@"C:\Users\Sun\Desktop\test.pptx");
            Song.setPresentation();
        }

        static public void FinallProcess()
        {
            Bible.close();
            Reading.close();
            Song.close();
        }
    }

    /// <summary>
    /// 성경
    /// </summary>
    partial class Powerpoint
    {
        public class Bible
        {

            // ============================================ 필요 변수 ============================================ 

            static Presentation ppt;
            static SlideShowWindow SlideWindow = null;
            static List<string> Format;
            static List<Shape> TextShapes;

            static string currentBible = "";
            static string currentChapter = "";
            static string currentVerse = "";
            static string currentContent = "";

            // ============================================ 세팅 및 종료 ========================================================

            static public void setPresentation(string path)
            {
                // temp폴더에 ppt파일을 복사하여 그걸로 쓰기

                Format = new List<string>(3);
                TextShapes = new List<Shape>(3);

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                foreach (Shape s in ppt.Slides[1].Shapes)
                    if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/권", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/장", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/절", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/본문", module.StringKMP.DefaultStringCompaerFunc))
                        {
                            TextShapes.Add(s);
                            Format.Add(s.TextFrame.TextRange.Text);
                        }
                    }
            }

            static public void close()
            {
                ppt.Close();
            }

            // ============================================ 메소드 ============================================ 

            static public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    SlideWindow = ppt.SlideShowSettings.Run();
                }
            }

            static public void Change_VerseContent(string verse, string content)
            {
                currentVerse = verse;
                currentContent = content;

                Change();
            }

            static public void Change_BibleChapter(string bible, string chapter)
            {
                currentBible = bible;
                currentChapter = chapter;

                Change();
            }

            static private void Change()
            {
                for (int i = 0; i < TextShapes.Count; i++)
                    TextShapes[i].TextFrame.TextRange.Text =
                        Format[i]
                        .Replace("/권", currentBible)
                        .Replace("/장", currentChapter)
                        .Replace("/절", currentVerse)
                        .Replace("/본문", currentContent);
            }

            static public void HideText()
            {
                SlideWindow.View.GotoSlide(2);
            }

            static public void ShowText()
            {
                SlideWindow.View.GotoSlide(1);
            }

            static public void OffDisplay()
            {
                SlideWindow.View.State = PpSlideShowState.ppSlideShowBlackScreen;
            }

            static public void OnDisplay()
            {
                SlideWindow.View.State = PpSlideShowState.ppSlideShowRunning;
            }
        }
    }

    /// <summary>
    /// 교독문
    /// </summary>
    partial class Powerpoint
    {
        public class Reading
        {

            // ============================================ 필요 변수 ============================================ 

            static Presentation ppt;
            static SlideShowWindow SlideWindow = null;
            static List<string> Format;
            static List<Shape> TextShapes;

            static string currentTitle = "";
            static string currentContent = "";

            // ============================================ 세팅 및 종료 ============================================ 

            static public void setPresentation(string path)
            {
                // temp폴더에 ppt파일을 복사하여 그걸로 쓰기

                Format = new List<string>(3);
                TextShapes = new List<Shape>(3);

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                foreach (Shape s in ppt.Slides[1].Shapes)
                    if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/제목", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/본문", module.StringKMP.DefaultStringCompaerFunc))
                        {
                            TextShapes.Add(s);
                            Format.Add(s.TextFrame.TextRange.Text);
                        }
                    }
            }

            static public void close()
            {
                ppt.Close();
            }

            // ============================================ 메소드 ============================================ 

            static public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    SlideWindow = ppt.SlideShowSettings.Run();
                }
            }

            static public void Change_Content(string content)
            {
                currentContent = content;

                Change();
            }

            static public void Change_Title(string title)
            {
                currentTitle = title;

                Change();
            }

            static private void Change()
            {
                for (int i = 0; i < TextShapes.Count; i++)
                    TextShapes[i].TextFrame.TextRange.Text =
                        Format[i]
                        .Replace("/제목", currentTitle)
                        .Replace("/본문", currentContent);
            }

            static public void HideText()
            {
                SlideWindow.View.GotoSlide(2);
            }

            static public void ShowText()
            {
                SlideWindow.View.GotoSlide(1);
            }

            static public void OffDisplay()
            {
                SlideWindow.View.State = PpSlideShowState.ppSlideShowBlackScreen;
            }

            static public void OnDisplay()
            {
                SlideWindow.View.State = PpSlideShowState.ppSlideShowRunning;
            }
        }
    }

    /// <summary>
    /// 찬양곡
    /// </summary>
    partial class Powerpoint
    {
        public class Song
        {

            // ============================================ 필요 변수 ============================================ 

            static List<SongFormatPPT> ppt;

            // ============================================ 세팅 및 종료 ============================================ 

            static public void setPresentation(string[] paths)
            {
                ppt = new List<SongFormatPPT>();
                // temp폴더에 ppt파일을 복사하여 그걸로 쓰기
                foreach (string path in paths)
                    ppt.Add(new SongFormatPPT(path));
            }

            static public void refreshPresentation(string path)
            {
                for(int i = 0; i < ppt.Count; i++)
                    if (ppt[i].path.CompareTo(path) == 0)
                        ppt[i].refreshPresentation(path);
            }

            static public void close()
            {
                foreach (SongFormatPPT sf in ppt)
                    sf.close();
            }

            // ============================================ 메소드 ============================================ 

            static private int pptFinder(string path)
            {
                for (int i = 0; i < ppt.Count; i++)
                    if (ppt[i].path.CompareTo(path) == 0)
                        return i;

                return -1;
            }

            static public void SetSongData(string path, string[][][] songData)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].SetSongData( songData);
                }
            }

            static public void SlideShowRun(string path)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].SlideShowRun();
                }
            }

            static public void ChangePage(string path, int Page)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].ChangeApply(Page);
                }
            }

            static public void HideText(string path)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].HideText();
                }
            }

            static public void ShowText(string path)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].ShowText();
                }
            }

            static public void OffDisplay(string path)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].OffDisplay();
                }
            }

            static public void OnDisplay(string path)
            {
                int index;
                if ((index = pptFinder(path)) != -1)
                {
                    ppt[index].OnDisplay();
                }
            }
        }

        public class SongFormatPPT
        {
            // ============================================ 필요 변수 ============================================ 

            public string path;

            Presentation ppt;
            class TextShape
            {
                public List<string> Format;
                public Shape shape;
            }
            List<TextShape> textShapes;
            string[][][] SongData;
            SlideShowWindow SlideWindow = null;

            // ============================================ 세팅 및 종료 ============================================ 

            public SongFormatPPT(string path)
            {
                setPresentation(path);
            }

            public void setPresentation(string path)
            {
                // temp폴더에 ppt파일을 복사하여 그걸로 쓰기
                this.path = path;

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                textShapes = new List<TextShape>(5);

                string shapeText;
                TextShape ts;
                foreach (Shape s in ppt.Slides[1].Shapes)
                    if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse 
                        && module.StringKMP.HasPattern(s.TextFrame.TextRange.Text,"/",module.StringKMP.DefaultStringCompaerFunc))
                    {
                        ts = new TextShape();
                        ts.shape = s;
                        ts.Format = new List<string>(5);

                        shapeText = s.TextFrame.TextRange.Text;
                        int startIndex;
                        for (int i = 0; i < shapeText.Length;)
                        {
                            startIndex = i;
                            if (shapeText[i] == '/')
                                while (i < shapeText.Length && !char.IsWhiteSpace(shapeText[i]))
                                    i++;
                            else
                                while (i < shapeText.Length && shapeText[i] != '/')
                                    i++;
                            ts.Format.Add(shapeText.Substring(startIndex,i-startIndex));
                        }

                        textShapes.Add(ts);
                    }
            }

            public void refreshPresentation(string path)
            {
                ppt.Close();
                setPresentation(path);
            }

            public void close()
            {
                ppt.Close();
            }

            // ============================================ 메소드 ============================================

            private int findChangeTableIndex(string command, string[][] ChangeTable)
            {
                for (int i = 0; i < ChangeTable.Length; i++)
                    if (ChangeTable[i][0].CompareTo(command) == 0)
                        return i;

                return -1;
            }

            public void SetSongData(string[][][] songData)
            {
                this.SongData = songData;
            }

            public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    SlideWindow = ppt.SlideShowSettings.Run();
                }
            }

            public void ChangeApply(int Page)
            {
                int index;
                StringBuilder str = new StringBuilder(50);
                for (int i = 0; i < textShapes.Count; i++)
                {
                    str.Clear();
                    foreach (string s in textShapes[i].Format)
                    {
                        if (s[0] == '/')
                        {
                            if ((index = findChangeTableIndex(s, SongData[Page])) == -1)
                                str.Append("");
                            else
                                str.Append(SongData[Page][index][1]);
                        }
                        else
                            str.Append(s);
                    }
                    textShapes[i].shape.TextFrame.TextRange.Text = str.ToString();
                }
            }

            public void HideText()
            {
                SlideWindow.View.GotoSlide(2);
            }

            public void ShowText()
            {
                SlideWindow.View.GotoSlide(1);
            }

            public void OffDisplay()
            {
                SlideWindow.View.State = PpSlideShowState.ppSlideShowBlackScreen;
            }

            public void OnDisplay()
            {
                SlideWindow.View.State = PpSlideShowState.ppSlideShowRunning;
            }
        }
    }
}
