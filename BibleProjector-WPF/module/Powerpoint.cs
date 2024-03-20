using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ppt
using Microsoft.Office.Interop.PowerPoint;



namespace BibleProjector_WPF
{
    partial class Powerpoint
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        static extern int ShowWindow(int hwnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        const int HWND_NOTOPMOST = -2;
        const int HWND_TOPMOST = -1;
        const int HWND_TOP = 0;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOACTIVATE = 0x0010;

        enum PptSlideState
        {
            NotRunning,
            WindowShow,
            WindowHide
        }

        enum PptTextShow
        {
            Show,
            Hide
        }

        static Application app;

        // ============================================ 프로그램 시작 / 종료 세팅 ========================================================

        public const string FRAME_TEMP_DIRECTORY = ".\\programData\\FrameTemp\\";
        public const string EXTERN_TEMP_DIRECTORY = ".\\programData\\ExternPPT\\";
        public const string EXTERN_THUMBNAIL_DIRECTORY = ".\\programData\\Thumbnails\\";

        static public string Initialize()
        {
            app = new Application();

            if (System.IO.Directory.Exists(FRAME_TEMP_DIRECTORY))
                System.IO.Directory.Delete(FRAME_TEMP_DIRECTORY, true);
            System.IO.Directory.CreateDirectory(FRAME_TEMP_DIRECTORY);

            StringBuilder pptFrameError = new StringBuilder(10);

            if (module.ProgramOption.BibleFramePath == null)
                pptFrameError.Append("성경 ppt틀\r\n");
            else
                Powerpoint.Bible.setPresentation(module.ProgramOption.BibleFramePath);

            if (module.ProgramOption.ReadingFramePath == null)
                pptFrameError.Append("교독문 ppt틀\r\n");
            else
                Powerpoint.Reading.setPresentation(module.ProgramOption.ReadingFramePath);

            if (module.ProgramOption.SongFrameFiles.Count == 0)
                pptFrameError.Append("찬양 ppt틀\r\n");
            else
                foreach (module.SongFrameFile f in module.ProgramOption.SongFrameFiles)
                    Powerpoint.Song.setPresentation(f.Path);

            return pptFrameError.ToString();
        }

        static public void FinallProcess()
        {
            Bible.close();
            Reading.close();
            Song.closeAll();
            ExternPPTs.closeAll();
        }

        // ========================================== 제공하는 기능 =======================================

        static public void setPageData(module.Data.ShowData Data, int PageIndex)
        {
            if (Data.getDataType() == ShowContentType.Bible)
            {
                module.Data.BibleData data = (module.Data.BibleData)Data;
                Bible.Change_BibleChapter(data.getBibleTitle(), data.chapter.ToString());
                Bible.Change_VerseContent(data.verse.ToString(), (string)data.getContents()[PageIndex].Content, PageIndex == 0);
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.SetPageData(((module.Data.SongData)Data).pptFrameFullPath, (string[][])Data.getContents()[PageIndex].Content);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                ExternPPTs.goToSlide(((module.Data.ExternPPTData)Data).fileFullPath, PageIndex);
            }
        }

        static public void SlideShowRun(module.Data.ShowData Data)
        {
            if (Data.getDataType() == ShowContentType.Bible)
            {
                Bible.SlideShowRun();
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.SlideShowRun(((module.Data.SongData)Data).pptFrameFullPath);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                ExternPPTs.SlideShowRun(((module.Data.ExternPPTData)Data).fileFullPath);
            }
        }

        static public void SlideShowHide(module.Data.ShowData Data)
        {
            if (Data.getDataType() == ShowContentType.Bible)
            {
                Bible.SlideShowHide();
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.SlideShowHide(((module.Data.SongData)Data).pptFrameFullPath);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                ExternPPTs.SlideShowHide(((module.Data.ExternPPTData)Data).fileFullPath);
            }
        }

        static public void ShowText(module.Data.ShowData Data)
        {
            if (Data.getDataType() == ShowContentType.Bible)
            {
                Bible.ShowText();
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.ShowText(((module.Data.SongData)Data).pptFrameFullPath);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                return;
            }
        }

        static public void HideText(module.Data.ShowData Data)
        {
            if (Data.getDataType() == ShowContentType.Bible)
            {
                Bible.HideText();
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.HideText(((module.Data.SongData)Data).pptFrameFullPath);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                return;
            }
        }
        
        static public void TopMost(module.Data.ShowData Data)
        {
            if (Data.getDataType() == ShowContentType.Bible)
            {
                Bible.TopMost();
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.TopMost(((module.Data.SongData)Data).pptFrameFullPath);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                ExternPPTs.TopMost(((module.Data.ExternPPTData)Data).fileFullPath);
            }
        }

        static public int getSlideCountFromFile(string path)
        {
            Presentation ppt =  app.Presentations.Open(path, WithWindow:Microsoft.Office.Core.MsoTriState.msoFalse);
            int slideCount = ppt.Slides.Count;
            ppt.Close();

            return slideCount;
        }

        static public void justOpen(string path)
        {
            app.Presentations.Open(path);
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

            static Presentation ppt = null;
            static SlideShowWindow SlideWindow = null;
            static List<string> Format;
            static List<Shape> TextShapes;

            static string currentBible = "";
            static string currentChapter = "";
            static string currentVerse = "";
            static bool isFirstPage = true;
            static string currentContent = "";

            static PptSlideState pptState = PptSlideState.NotRunning;
            static PptTextShow pptTextState = PptTextShow.Show;

            // ============================================ 세팅 및 종료 ========================================================

            static public void setPresentation(string path)
            {
                string tempPath = FRAME_TEMP_DIRECTORY + System.IO.Path.GetFileName(path);
                System.IO.File.Copy(path, tempPath, true);
                path = System.IO.Path.GetFullPath(tempPath);

                Format = new List<string>(3);
                TextShapes = new List<Shape>(3);

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                checkValidPPT();
                getCommand();
            }

            static void checkAndClose(Presentation ppt)
            {
                foreach(Presentation p in app.Presentations)
                    if (p == ppt) {
                        ppt.Close();
                        return;
                    }
            }

            static public void refreshPresentation(string path)
            {
                Presentation lastppt = ppt;

                if (pptState == PptSlideState.NotRunning)
                {
                    lastppt.Close();
                    setPresentation(path);
                }
                else if (pptState == PptSlideState.WindowHide)
                {
                    if (SlideWindow != null)
                    {
                        SlideWindow.View.Exit();
                        SlideWindow = null;
                    }
                    checkAndClose(lastppt);

                    setPresentation(path);
                    Change();
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    SlideShowWindow lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    checkValidPPT();
                    getCommand();
                    Change();
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);

                    lastppt = ppt;
                    lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    setPresentation(path);
                    Change();
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);
                }
            }

            static void getCommand()
            {
                foreach (Shape s in ppt.Slides[1].Shapes)
                {
                    if (s.Type == Microsoft.Office.Core.MsoShapeType.msoGroup)
                    {
                        foreach (Shape s2 in s.GroupItems)
                            if (s2.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                            {
                                getCommand_sub(s2);
                            }
                    }
                    else if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        getCommand_sub(s);
                    }
                }
            }

            static void getCommand_sub(Shape s)
            {
                if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{b}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{ch}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{v}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{va}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{c}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{cm}", module.StringKMP.DefaultStringCompaerFunc))
                {
                    TextShapes.Add(s);
                    Format.Add(s.TextFrame.TextRange.Text);
                }
            }

            static void checkValidPPT()
            {
                if (ppt.Slides.Count == 0)
                    ppt.Slides.AddSlide(1, ppt.SlideMaster.CustomLayouts[1]);
                if (ppt.Slides.Count == 1)
                    ppt.Slides.AddSlide(2,ppt.Slides[1].CustomLayout);
            }

            static public void close()
            {
                try
                {
                    SlideWindow.View.Exit();
                }
                catch { }
                try
                {
                    ppt.Close();
                }
                catch { }
            }

            // ============================================ 메소드 ============================================ 

            static public void TopMost()
            {
                if (SlideWindow != null && pptState == PptSlideState.WindowShow)
                {
                    SetWindowPos(SlideWindow.HWND, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    SetWindowPos(SlideWindow.HWND, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    //SetWindowPos(SlideWindow.HWND, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                }
            }

            static public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (pptTextState == PptTextShow.Hide)
                    {
                        ppt.Slides[1].MoveTo(2);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[2].MoveTo(1);
                    }
                    else
                        SlideWindow = ppt.SlideShowSettings.Run();
                }
                else
                    ShowWindow(SlideWindow.HWND, SW_SHOW);

                pptState = PptSlideState.WindowShow;
            }

            static public void SlideShowHide()
            {
                if (SlideWindow != null)
                {
                    ShowWindow(SlideWindow.HWND, SW_HIDE);
                    pptState = PptSlideState.WindowHide;
                }
            }

            static public void Change_VerseContent(string verse, string content,bool FirstPage)
            {
                currentVerse = verse;
                currentContent = content;
                isFirstPage = FirstPage;

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
                {
                    TextShapes[i].TextFrame.TextRange.Text =
                       Format[i]
                            .Replace("{b}", currentBible)
                            .Replace("{ch}", currentChapter)
                            .Replace("{va}", currentVerse)
                            .Replace("{c}", currentContent)
                            .Replace("{v}", (isFirstPage ? currentVerse :""))
                            .Replace("{cm}", ((currentBible.CompareTo("시편")==0)?"편":"장")); // 굳이 시편 코드를 직접 기입해야 하나. Bible 관리 객체 어디갔냐.
                }
            }

            static public void HideText()
            {
                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(2);

                pptTextState = PptTextShow.Hide;
            }

            static public void ShowText()
            {
                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(1);
                
                pptTextState = PptTextShow.Show;
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

            static Presentation ppt = null;
            static SlideShowWindow SlideWindow = null;
            static List<string> Format;
            static List<Shape> TextShapes;

            static string currentTitle = "";
            static string currentContent = "";

            static PptSlideState pptState = PptSlideState.NotRunning;
            static PptTextShow pptTextState = PptTextShow.Show;

            // ============================================ 세팅 및 종료 ============================================ 

            static public void setPresentation(string path)
            {
                string tempPath = FRAME_TEMP_DIRECTORY + System.IO.Path.GetFileName(path);
                System.IO.File.Copy(path, tempPath, true);
                path = System.IO.Path.GetFullPath(tempPath);

                Format = new List<string>(3);
                TextShapes = new List<Shape>(3);

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                checkValidPPT();
                getCommand();
            }

            static void checkAndClose(Presentation ppt)
            {
                foreach (Presentation p in app.Presentations)
                    if (p == ppt)
                    {
                        ppt.Close();
                        return;
                    }
            }

            static public void refreshPresentation(string path)
            {
                Presentation lastppt = ppt;

                if (pptState == PptSlideState.NotRunning)
                {
                    lastppt.Close();
                    setPresentation(path);
                }
                else if (pptState == PptSlideState.WindowHide)
                {
                    if (SlideWindow != null)
                    {
                        SlideWindow.View.Exit();
                        SlideWindow = null;
                    }
                    checkAndClose(lastppt);

                    setPresentation(path);
                    Change();
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    SlideShowWindow lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    checkValidPPT();
                    getCommand();
                    Change();
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);

                    lastppt = ppt;
                    lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    setPresentation(path);
                    Change(); 
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);
                }
            }

            static void getCommand()
            {
                foreach (Shape s in ppt.Slides[1].Shapes)
                {
                    if (s.Type == Microsoft.Office.Core.MsoShapeType.msoGroup)
                    {
                        foreach (Shape s2 in s.GroupItems)
                            if (s2.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                            {
                                getCommand_sub(s2);
                            }
                    }
                    else if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        getCommand_sub(s);
                    }
                }
            }

            static void getCommand_sub(Shape s)
            {
                if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{t}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{c}", module.StringKMP.DefaultStringCompaerFunc))
                {
                    TextShapes.Add(s);
                    Format.Add(s.TextFrame.TextRange.Text);
                }
            }

            static void checkValidPPT()
            {
                if (ppt.Slides.Count == 0)
                    ppt.Slides.AddSlide(1, ppt.SlideMaster.CustomLayouts[1]);
                if (ppt.Slides.Count == 1)
                    ppt.Slides.AddSlide(2, ppt.Slides[1].CustomLayout);
            }

            static public void close()
            {
                try
                {
                    SlideWindow.View.Exit();
                }
                catch { }
                try
                {
                    ppt.Close();
                }
                catch { }
            }

            // ============================================ 메소드 ============================================ 

            static public void TopMost()
            {
                if (SlideWindow != null && pptState == PptSlideState.WindowShow)
                {
                    SetWindowPos(SlideWindow.HWND, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    SetWindowPos(SlideWindow.HWND, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    //SetWindowPos(SlideWindow.HWND, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                }
            }

            static public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (pptTextState == PptTextShow.Hide)
                    {
                        ppt.Slides[1].MoveTo(2);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[2].MoveTo(1);
                    }
                    else
                        SlideWindow = ppt.SlideShowSettings.Run();
                }
                else
                    ShowWindow(SlideWindow.HWND, SW_SHOW);

                pptState = PptSlideState.WindowShow;
            }

            static public void SlideShowHide()
            {
                if (SlideWindow != null)
                {
                    ShowWindow(SlideWindow.HWND, SW_HIDE);
                    pptState = PptSlideState.WindowHide;
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
                        .Replace("{t}", currentTitle)
                        .Replace("{c}", currentContent);
            }

            static public void HideText()
            {
                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(2);

                pptTextState = PptTextShow.Hide;    
            }

            static public void ShowText()
            {
                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(1);

                pptTextState = PptTextShow.Show;
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

            static List<SongFormatPPT> ppt = new List<SongFormatPPT>();

            // ============================================ 세팅 및 종료 ============================================ 

            static public void setPresentation(string path)
            {
                ppt.Add(new SongFormatPPT(path));
            }

            static public void refreshPresentation(string path)
            {
                SongFormatPPT findedSong = ppt.Find(x => x.FramePPTName.CompareTo(System.IO.Path.GetFileName(path)) == 0);
                if (findedSong != null)
                    findedSong.refreshPresentation(path);
            }

            static public void closeSingle(string path)
            {
                SongFormatPPT findedSong = ppt.Find(x => x.FramePPTName.CompareTo(System.IO.Path.GetFileName(path)) == 0);
                if (findedSong != null)
                    findedSong.deletePresentation();
                ppt.Remove(findedSong);
            }

            static public void closeAll()
            {
                foreach (SongFormatPPT sf in ppt)
                    sf.deletePresentation();
                ppt.Clear();
            }

            // ============================================ 메소드 ============================================ 

            static private int pptFinder(string FramePPTName)
            {
                for (int i = 0; i < ppt.Count; i++)
                    if (ppt[i].FramePPTName.CompareTo(FramePPTName) == 0)
                        return i;

                return -1;
            }

            static private int pptFinder_fullPath(string fullPath)
            {
                for (int i = 0; i < ppt.Count; i++)
                    if (ppt[i].FrameFullPath.CompareTo(fullPath) == 0)
                        return i;

                return -1;
            }

            static public void TopMost(string FrameFileFullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(FrameFileFullPath)) != -1)
                {
                    ppt[index].TopMost();
                }
            }

            static public void SlideShowRun(string FrameFileFullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(FrameFileFullPath)) != -1)
                {
                    ppt[index].SlideShowRun();
                    for (int i = 0; i < ppt.Count; i++)
                        if (i != index)
                            ppt[i].SlideShowHide();
                }
            }

            static public void SlideShowHide(string FrameFileFullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(FrameFileFullPath)) != -1)
                {
                    ppt[index].SlideShowHide();
                }
            }

            static public void SetPageData(string FrameFileFullPath, string[][] PageData)
            {
                int index;
                if ((index = pptFinder_fullPath(FrameFileFullPath)) != -1)
                {
                    ppt[index].ChangeApply(PageData);
                }
            }

            static public void HideText(string FrameFileFullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(FrameFileFullPath)) != -1)
                {
                    ppt[index].HideText();
                }
            }

            static public void ShowText(string FrameFileFullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(FrameFileFullPath)) != -1)
                {
                    ppt[index].ShowText();
                }
            }
        }

        public class SongFormatPPT
        {
            // ============================================ 필요 변수 ============================================ 

            public string FramePPTName;
            public string FrameFullPath;

            Presentation ppt;
            class TextShape
            {
                public List<string> Format;
                public Shape shape;
            }
            List<TextShape> textShapes;
            string[][] currentData;
            SlideShowWindow SlideWindow = null;

            PptSlideState pptState = PptSlideState.NotRunning;
            PptTextShow pptTextState = PptTextShow.Show;

            // ============================================ 세팅 및 종료 ============================================ 

            public SongFormatPPT(string path)
            {
                setPresentation(path);
            }

            public void setPresentation(string path)
            {
                string tempPath = FRAME_TEMP_DIRECTORY + System.IO.Path.GetFileName(path);
                System.IO.File.Copy(path, tempPath, true);
                path = System.IO.Path.GetFullPath(tempPath);

                this.FramePPTName = System.IO.Path.GetFileName(path);
                this.FrameFullPath = path;
                textShapes = new List<TextShape>(5);

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                checkValidPPT();
                getCommand();
            }

            void checkAndClose(Presentation ppt)
            {
                foreach (Presentation p in app.Presentations)
                    if (p == ppt)
                    {
                        ppt.Close();
                        return;
                    }
            }

            public void refreshPresentation(string path)
            {
                Presentation lastppt = ppt;

                if (pptState == PptSlideState.NotRunning)
                {
                    lastppt.Close();
                    setPresentation(path);
                }
                else if (pptState == PptSlideState.WindowHide)
                {
                    if (SlideWindow != null)
                    {
                        SlideWindow.View.Exit();
                        SlideWindow = null;
                    }
                    checkAndClose(lastppt);

                    setPresentation(path);
                    ChangeApply(currentData);
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    SlideShowWindow lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    checkValidPPT();
                    getCommand();
                    ChangeApply(currentData);
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);

                    lastppt = ppt;
                    lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    setPresentation(path);
                    ChangeApply(currentData);
                    if (pptTextState == PptTextShow.Show)
                        ShowText();
                    else if (pptTextState == PptTextShow.Hide)
                        HideText();
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);
                }
            }

            public void deletePresentation()
            {
                try
                {
                    SlideWindow.View.Exit();
                }
                catch { }
                try
                {
                    ppt.Close();
                }
                catch { }

                System.IO.File.Delete(System.IO.Path.GetFullPath(FRAME_TEMP_DIRECTORY + FramePPTName));
            }

            void getCommand()
            {
                foreach (Shape s in ppt.Slides[1].Shapes)
                {
                    if (s.Type == Microsoft.Office.Core.MsoShapeType.msoGroup)
                    {
                        foreach (Shape s2 in s.GroupItems)
                            if (s2.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                            {
                                getCommand_sub(s2);
                            }
                    }
                    else if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        getCommand_sub(s);
                    }
                }
            }

            void getCommand_sub(Shape s)
            {
                int[] index = module.StringKMP.FindPattern(s.TextFrame.TextRange.Text, "{", module.StringKMP.DefaultStringCompaerFunc);
                if (index.Length != 0)
                {
                    string shapeText = s.TextFrame.TextRange.Text;
                    List<string> format = new List<string>(5);

                    int startIndex = 0;
                    int endIndex = 0;
                    int i = 0;
                    for (; i < index.Length; i++)
                    {
                        endIndex = index[i];
                        if (startIndex != endIndex)
                            format.Add(shapeText.Substring(startIndex, endIndex - startIndex));

                        for (startIndex = endIndex; endIndex < shapeText.Length && shapeText[endIndex] != '}'; endIndex++) ;
                        if (endIndex == shapeText.Length)
                            break;
                        endIndex++;

                        format.Add(shapeText.Substring(startIndex, endIndex - startIndex));
                        startIndex = endIndex;
                    }
                    if (i != index.Length)
                        return;
                    else if (startIndex != shapeText.Length)
                        format.Add(shapeText.Substring(startIndex, shapeText.Length - startIndex));

                    TextShape ts = new TextShape();
                    ts.shape = s;
                    ts.Format = format;
                    textShapes.Add(ts);
                }
            }

            void checkValidPPT()
            {
                if (ppt.Slides.Count == 0)
                    ppt.Slides.AddSlide(1, ppt.SlideMaster.CustomLayouts[1]);
                if (ppt.Slides.Count == 1)
                    ppt.Slides.AddSlide(2,ppt.Slides[1].CustomLayout);
            }

            // ============================================ 메소드 ============================================

            public void TopMost()
            {
                if (SlideWindow != null && pptState == PptSlideState.WindowShow)
                {
                    SetWindowPos(SlideWindow.HWND, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    SetWindowPos(SlideWindow.HWND, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    //SetWindowPos(SlideWindow.HWND, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                }
            }

            private int findChangeTableIndex(string command, string[][] ChangeTable)
            {
                for (int i = 0; i < ChangeTable.Length; i++)
                    if (ChangeTable[i][0].CompareTo(command) == 0)
                        return i;

                return -1;
            }

            public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (pptTextState == PptTextShow.Hide)
                    {
                        ppt.Slides[1].MoveTo(2);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[2].MoveTo(1);
                    }
                    else
                        SlideWindow = ppt.SlideShowSettings.Run();
                }
                else
                    ShowWindow(SlideWindow.HWND, SW_SHOW);

                pptState = PptSlideState.WindowShow;
            }

            public void SlideShowHide()
            {
                if (SlideWindow != null)
                {
                    ShowWindow(SlideWindow.HWND, SW_HIDE);
                    pptState = PptSlideState.WindowHide;
                }
            }

            public void ChangeApply(string[][] SongData)
            {
                int index;
                StringBuilder str = new StringBuilder(50);
                for (int i = 0; i < textShapes.Count; i++)
                {
                    str.Clear();
                    foreach (string s in textShapes[i].Format)
                    {
                        if (s[0] == '{')
                        {
                            if ((index = findChangeTableIndex(s, SongData)) == -1)
                                str.Append("");
                            else
                                str.Append(SongData[index][1]);
                        }
                        else
                            str.Append(s);
                    }
                    textShapes[i].shape.TextFrame.TextRange.Text = str.ToString();
                }
            }

            public void HideText()
            {
                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(2);

                pptTextState = PptTextShow.Hide;
            }

            public void ShowText()
            {
                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(1);

                pptTextState = PptTextShow.Show;
            }
        }
    }

    /// <summary>
    /// 외부 ppt
    /// </summary>
    partial class Powerpoint
    {
        public class ExternPPTs
        {

            // ============================================ 필요 변수 ============================================ 

            static List<ExternPPT> ppt = new List<ExternPPT>();

            // ============================================ 세팅 및 종료 ============================================ 

            static bool isSetup = false;
            static void Initialize()
            {
                if (isSetup)
                    return;
                else
                    isSetup = true;

                if (System.IO.Directory.Exists(EXTERN_TEMP_DIRECTORY))
                    System.IO.Directory.Delete(EXTERN_TEMP_DIRECTORY, true);
                System.IO.Directory.CreateDirectory(EXTERN_TEMP_DIRECTORY);

                if (System.IO.Directory.Exists(EXTERN_THUMBNAIL_DIRECTORY))
                    System.IO.Directory.Delete(EXTERN_THUMBNAIL_DIRECTORY, true);
                System.IO.Directory.CreateDirectory(EXTERN_THUMBNAIL_DIRECTORY);
            }

            static public void setPresentation(string path)
            {
                Initialize();
                ppt.Add(new ExternPPT(path));
            }

            static public void refreshPresentation(string path)
            {
                ExternPPT findedExternPPT = ppt.Find(x => x.PPTName.CompareTo(System.IO.Path.GetFileName(path)) == 0);
                if (findedExternPPT != null)
                    findedExternPPT.refreshPresentation(path);
            }

            static public void closeSingle(string path)
            {
                ExternPPT findedExternPPT = ppt.Find(x => x.PPTName.CompareTo(System.IO.Path.GetFileName(path)) == 0);
                if (findedExternPPT != null)
                    findedExternPPT.deletePresentation();
                ppt.Remove(findedExternPPT);
            }

            static public void closeAll()
            {
                foreach (ExternPPT ep in ppt)
                    ep.deletePresentation();
                ppt.Clear();
            }

            // ============================================ 메소드 ============================================ 

            static private int pptFinder(string PPTName)
            {
                for (int i = 0; i < ppt.Count; i++)
                    if (ppt[i].PPTName.CompareTo(PPTName) == 0)
                        return i;

                return -1;
            }

            static private int pptFinder_fullPath(string fullPath)
            {
                for (int i = 0; i < ppt.Count; i++)
                    if (ppt[i].OriginalFullPath.CompareTo(fullPath) == 0)
                        return i;

                return -1;
            }

            static public System.Windows.Media.Imaging.BitmapImage[] getThumbnailImages(string fullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    return ppt[index].getThumbnailImages();
                }

                return null;
            }

            static public void TopMost(string fullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    ppt[index].TopMost();
                }
            }

            static public void SlideShowRun(string fullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    ppt[index].SlideShowRun();
                    for (int i = 0; i < ppt.Count; i++)
                        if (i != index)
                            ppt[i].SlideShowHide();
                }
            }

            static public int goToSlide(string fullPath, int slideIndex)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    return ppt[index].goToSlide(slideIndex);
                }

                return -1;
            }

            static public int goNextSlide(string PPTName)
            {
                int index;
                if ((index = pptFinder(PPTName)) != -1)
                {
                    return ppt[index].goNextSlide();
                }

                return -1;
            }

            static public int goPreviousSlide(string PPTName)
            {
                int index;
                if ((index = pptFinder(PPTName)) != -1)
                {
                    return ppt[index].goPreviousSlide();
                }

                return -1;
            }

            static public void SlideShowHide(string fullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    ppt[index].SlideShowHide();
                }
            }
        }

        public class ExternPPT
        {
            // ============================================ 필요 변수 ============================================ 

            public string FullPath;
            public string OriginalFullPath;
            public string PPTName;

            Presentation ppt;
            SlideShowWindow SlideWindow = null;
            System.Windows.Media.Imaging.BitmapImage[] thumbnails;
            int currentSlideNum = 1;

            bool isNeedMakeThumbnail = true;

            PptSlideState pptState = PptSlideState.NotRunning;

            // ============================================ 세팅 및 종료 ============================================ 

            public ExternPPT(string path)
            {
                setPresentation(path);
            }

            public void setPresentation(string path)
            {
                string tempPath = EXTERN_TEMP_DIRECTORY + System.IO.Path.GetFileName(path);
                string newPath = System.IO.Path.GetFullPath(tempPath);
                string originalPath = path;
                System.IO.File.Copy(originalPath, newPath, true);

                this.FullPath = System.IO.Path.GetFullPath(newPath);
                this.OriginalFullPath = originalPath;
                this.PPTName = System.IO.Path.GetFileName(newPath);

                ppt = app.Presentations.Open(newPath, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                checkValidPPT();

                SetThumbNailImages(newPath);
            }

            void SetThumbNailImages(string fullPath)
            {
                this.ThumbnailGenerator();

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(this.getThumbnailPath());
                System.IO.FileInfo[] imageFiles = di.GetFiles();
                System.Windows.Media.Imaging.BitmapImage[] imageData = new System.Windows.Media.Imaging.BitmapImage[imageFiles.Length];

                System.Windows.Media.Imaging.BitmapImage bi;
                int idx;
                foreach (System.IO.FileInfo f in di.GetFiles())
                {
                    bi = new System.Windows.Media.Imaging.BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(f.FullName, UriKind.Absolute);
                    bi.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bi.EndInit();

                    idx = int.Parse(module.StringModifier.makeOnlyNum(f.Name)) - 1;
                    imageData[idx] = bi;
                }

                this.thumbnails = imageData;
            }

            void checkAndClose(Presentation ppt)
            {
                foreach (Presentation p in app.Presentations)
                    if (p == ppt)
                    {
                        ppt.Close();
                        return;
                    }
            }

            public void refreshPresentation(string path)
            {
                isNeedMakeThumbnail = true;
                Presentation lastppt = ppt;

                if (pptState == PptSlideState.NotRunning)
                {
                    lastppt.Close();
                    setPresentation(path);
                }
                else if (pptState == PptSlideState.WindowHide)
                {
                    if (SlideWindow != null)
                    {
                        SlideWindow.View.Exit();
                        SlideWindow = null;
                    }
                    checkAndClose(lastppt);

                    setPresentation(path);
                    goToSlide(currentSlideNum);
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    SlideShowWindow lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    checkValidPPT();
                    goToSlide(currentSlideNum);
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);

                    lastppt = ppt;
                    lastShowWindow = SlideWindow;
                    SlideWindow = null;

                    setPresentation(path);
                    goToSlide(currentSlideNum);
                    SlideShowRun();

                    lastShowWindow.View.Exit();
                    checkAndClose(lastppt);
                }
            }

            public void deletePresentation()
            {
                deleteThumbNail();

                try
                {
                    SlideWindow.View.Exit();
                }
                catch { }
                try
                {
                    ppt.Close();
                }
                catch { }
            }

            void checkValidPPT()
            {
                if (ppt.Slides.Count == 0)
                    ppt.Slides.AddSlide(1, ppt.SlideMaster.CustomLayouts[1]);
            }

            // ============================================ 썸네일 작업 ==========================================

            void deleteThumbNail()
            {
                if (System.IO.Directory.Exists(getThumbnailPath()))
                    System.IO.Directory.Delete(getThumbnailPath(), true);
            }

            void makeThumbNail()
            {
                deleteThumbNail();
                ppt.SaveAs(System.IO.Path.GetFullPath(EXTERN_THUMBNAIL_DIRECTORY+ppt.Name), PpSaveAsFileType.ppSaveAsJPG);
            }

            // ============================================ 메소드 ============================================

            public void ThumbnailGenerator()
            {
                if (isNeedMakeThumbnail)
                {
                    makeThumbNail();
                    isNeedMakeThumbnail = false;
                }
            }

            public string getThumbnailPath()
            {
                return System.IO.Path.GetFullPath(
                    EXTERN_THUMBNAIL_DIRECTORY 
                    + System.IO.Path.GetFileNameWithoutExtension(ppt.Name)
                    );
            }

            public System.Windows.Media.Imaging.BitmapImage[] getThumbnailImages()
            {
                return thumbnails;
            }

            public void TopMost()
            {
                if (SlideWindow != null && pptState == PptSlideState.WindowShow)
                {
                    SetWindowPos(SlideWindow.HWND, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    SetWindowPos(SlideWindow.HWND, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    //SetWindowPos(SlideWindow.HWND, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                }
            }

            public void SlideShowRun()
            {
                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (currentSlideNum != 1)
                    {
                        ppt.Slides[currentSlideNum].MoveTo(1);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[1].MoveTo(currentSlideNum);
                    }
                    else
                        SlideWindow = ppt.SlideShowSettings.Run();
                }
                else
                    ShowWindow(SlideWindow.HWND, SW_SHOW);

                pptState = PptSlideState.WindowShow;
            }

            /// <summary>
            /// 이동한 슬라이드 위치를 반환
            /// </summary>
            /// <param name="slideIndex"></param>
            /// <returns></returns>
            public int goToSlide(int slideIndex)
            {
                if (slideIndex < 1)
                    currentSlideNum = 1;
                else if (slideIndex > ppt.Slides.Count)
                    currentSlideNum = ppt.Slides.Count;
                else
                    currentSlideNum = slideIndex;

                if (SlideWindow != null)
                    SlideWindow.View.GotoSlide(currentSlideNum);

                return currentSlideNum;
            }

            public int goNextSlide()
            {
                return goToSlide(currentSlideNum + 1);
            }

            public int goPreviousSlide()
            {
                return goToSlide(currentSlideNum - 1);
            }

            public void SlideShowHide()
            {
                if (SlideWindow != null)
                {
                    ShowWindow(SlideWindow.HWND, SW_HIDE);
                    pptState = PptSlideState.WindowHide;
                }
            }
        }
    }
}
