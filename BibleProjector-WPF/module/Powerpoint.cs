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
    class Powerpoint
    {
        static Application app;

        // ============================================ 성경 ============================================ 

        static Presentation ppt_Bible;
        static SlideShowWindow ppt_Bible_Window = null;
        static List<string> ppt_Bible_Format;
        static List<Shape> ppt_Bible_ValueShape;

        static string Bible_currentBible = "";
        static string Bible_currentChapter = "";
        static string Bible_currentVerse = "";
        static string Bible_currentContent = "";

        // ============================================ 교독문 ============================================ 

        static Presentation ppt_Reading;
        static SlideShowWindow ppt_Reading_Window = null;
        static List<string> ppt_Reading_Format;
        static List<Shape> ppt_Reading_ValueShape;

        static string Reading_currentTitle = "";
        static string Reading_currentContent = "";

        // ============================================ 프로그램 시작 / 종료 세팅 ========================================================

        static public void Initialize()
        {
            app = new Application();

            setBiblePresentation(@"C:\Users\Sun\Desktop\test.pptx");
            setReadingPresentation(@"C:\Users\Sun\Desktop\test.pptx");
        }

        static private void setBiblePresentation(string path)
        {
            // temp폴더에 ppt파일을 복사하여 그걸로 쓰기
            
            ppt_Bible_Format = new List<string>(3);
            ppt_Bible_ValueShape = new List<Shape>(3);

            ppt_Bible = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
            foreach (Shape s in ppt_Bible.Slides[1].Shapes)
                if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                {
                    if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/권", module.StringKMP.DefaultStringCompaerFunc)
                        || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/장", module.StringKMP.DefaultStringCompaerFunc)
                        || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/절", module.StringKMP.DefaultStringCompaerFunc)
                        || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/본문", module.StringKMP.DefaultStringCompaerFunc))
                    {
                        ppt_Bible_ValueShape.Add(s);
                        ppt_Bible_Format.Add(s.TextFrame.TextRange.Text);
                    }
                }
        }

        static private void setReadingPresentation(string path)
        {
            // temp폴더에 ppt파일을 복사하여 그걸로 쓰기

            ppt_Reading_Format = new List<string>(3);
            ppt_Reading_ValueShape = new List<Shape>(3);

            ppt_Reading = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
            foreach (Shape s in ppt_Reading.Slides[1].Shapes)
                if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                {
                    if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/제목", module.StringKMP.DefaultStringCompaerFunc)
                        || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "/본문", module.StringKMP.DefaultStringCompaerFunc))
                    {
                        ppt_Reading_ValueShape.Add(s);
                        ppt_Reading_Format.Add(s.TextFrame.TextRange.Text);
                    }
                }
        }

        static public void FinallProcess()
        {
            ppt_Bible.Close();
            ppt_Reading.Close();
        }

        // ============================================ 메소드 : 성경 ============================================ 

        static public void Bible_SlideShowRun()
        {
            // 슬라이드쇼 점검하는부분 좀 더 개선
            // 슬라이드쇼 끄면 ppt도 꺼지기 때문
            if (ppt_Bible_Window == null)
            {
                ppt_Bible.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                ppt_Bible_Window = ppt_Bible.SlideShowSettings.Run();
            }
        }

        static public void Bible_Change_VerseContent(string verse, string content)
        {
            Bible_currentVerse = verse;
            Bible_currentContent = content;

            Bible_Change();
        }

        static public void Bible_Change_BibleChapter(string bible, string chapter)
        {
            Bible_currentBible = bible;
            Bible_currentChapter = chapter;

            Bible_Change();
        }

        static private void Bible_Change()
        {
            for (int i = 0; i < ppt_Bible_ValueShape.Count; i++)
                ppt_Bible_ValueShape[i].TextFrame.TextRange.Text = 
                    ppt_Bible_Format[i]
                    .Replace("/권",Bible_currentBible)
                    .Replace("/장", Bible_currentChapter)
                    .Replace("/절", Bible_currentVerse)
                    .Replace("/본문", Bible_currentContent);
        }

        static public void Bible_HideText()
        {
            ppt_Bible_Window.View.GotoSlide(2);
        }

        static public void Bible_ShowText()
        {
            ppt_Bible_Window.View.GotoSlide(1);
        }

        static public void Bible_OffDisplay()
        {
            ppt_Bible_Window.View.State = PpSlideShowState.ppSlideShowBlackScreen;
        }

        static public void Bible_OnDisplay()
        {
            ppt_Bible_Window.View.State = PpSlideShowState.ppSlideShowRunning;
        }

        // ============================================ 메소드 : 교독문 ============================================ 

        static public void Reading_SlideShowRun()
        {
            // 슬라이드쇼 점검하는부분 좀 더 개선
            // 슬라이드쇼 끄면 ppt도 꺼지기 때문
            if (ppt_Reading_Window == null)
            {
                ppt_Reading.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                ppt_Reading_Window = ppt_Reading.SlideShowSettings.Run();
            }
        }

        static public void Reading_Change_Content(string content)
        {
            Reading_currentContent = content;

            Reading_Change();
        }

        static public void Reading_Change_Title(string title)
        {
            Reading_currentTitle = title;

            Reading_Change();
        }

        static private void Reading_Change()
        {
            for (int i = 0; i < ppt_Reading_ValueShape.Count; i++)
                ppt_Reading_ValueShape[i].TextFrame.TextRange.Text =
                    ppt_Reading_Format[i]
                    .Replace("/제목", Reading_currentTitle)
                    .Replace("/본문", Reading_currentContent);
        }

        static public void Reading_HideText()
        {
            ppt_Reading_Window.View.GotoSlide(2);
        }

        static public void Reading_ShowText()
        {
            ppt_Reading_Window.View.GotoSlide(1);
        }

        static public void Reading_OffDisplay()
        {
            ppt_Reading_Window.View.State = PpSlideShowState.ppSlideShowBlackScreen;
        }

        static public void Reading_OnDisplay()
        {
            ppt_Reading_Window.View.State = PpSlideShowState.ppSlideShowRunning;
        }
    }
}
