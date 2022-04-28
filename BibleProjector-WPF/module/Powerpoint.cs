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

        static Presentation ppt_Bible;
        static SlideShowWindow ppt_Bible_Window = null;
        static List<string> ppt_Bible_Format;
        static List<Shape> ppt_Bible_ValueShape;

        static string Bible_currentBible = "";
        static string Bible_currentChapter = "";
        static string Bible_currentVerse = "";
        static string Bible_currentContent = "";

        // ============================================ 프로그램 시작 / 종료 세팅 ========================================================

        static public void Initialize()
        {
            app = new Application();

            setBiblePresentation(@"C:\Users\Sun\Desktop\test.pptx");
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
                    ppt_Bible_ValueShape.Add(s);
                    ppt_Bible_Format.Add(s.TextFrame.TextRange.Text);
                }
        }

        static public void FinallProcess()
        {
            ppt_Bible.Close();
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
    }
}
