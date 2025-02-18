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
        const int SWP_NOZORDER = 0x0004;

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern Int32 SetWindowLong(int hWnd, Int32 nIndex, Int32 dwNewLong);

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern Int32 GetWindowLong(int hWnd, Int32 nIndex);
        const Int32 GWL_STYLE = -16;
        const Int32 WS_VISIBLE = 0x10000000;
        const Int32 WS_EX_TOOLWINDOW = 0x00000080;
        const Int32 WS_EX_APPWINDOW = 0x00040000;
        const Int32 WS_EX_NOACTIVATE = 0x08000000;

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int GetWindow(int hWnd, uint uCmd);
        const uint GW_HWNDNEXT = 2;
        const uint GW_HWNDPREV = 3;

        /// <summary>
        /// 기본 자막화면 표시 정책에 따라 슬라이드 쇼 Window를 재설정 및 표시합니다.
        /// <br/> 정책 :
        /// <br/>- 작업표시줄에 Windows를 표시하지 않습니다.
        /// <br/>- 기존 위치를 유지하거나, <paramref name="frontWindowHWND"/>의 바로 뒤로 위치시킬 수 있습니다.
        /// </summary>
        /// <param name="windowHWND"></param>
        /// <param name="frontWindowHWND"></param>
        protected static void ReshowSlideWindow(int windowHWND, int? frontWindowHWND = null)
        {
            Int32 style = GetWindowLong(windowHWND, GWL_STYLE);
            
            style |= WS_VISIBLE;
            style &= ~(WS_EX_APPWINDOW);
            style |= WS_EX_TOOLWINDOW;
            style |= WS_EX_NOACTIVATE;

            ShowWindow(windowHWND, SW_HIDE);
            if (frontWindowHWND == null)
                SetWindowPos(windowHWND, 0, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_NOZORDER);
            else
                SetWindowPos(windowHWND, (int)frontWindowHWND, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            SetWindowLong(windowHWND, GWL_STYLE, style);
        }

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
        public const string EXTERN_THUMBNAIL_DIRECTORY = ".\\programData\\Thumbnails\\";

        static public void Initialize()
        {
            app = new Application();

            closePPTFiles(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(FRAME_TEMP_DIRECTORY))); // Notice : if last has \\, use GetDirectoryName!!
            if (System.IO.Directory.Exists(FRAME_TEMP_DIRECTORY))
                System.IO.Directory.Delete(FRAME_TEMP_DIRECTORY, true);
            System.IO.Directory.CreateDirectory(FRAME_TEMP_DIRECTORY);

            if (System.IO.Directory.Exists(EXTERN_THUMBNAIL_DIRECTORY))
                System.IO.Directory.Delete(EXTERN_THUMBNAIL_DIRECTORY, true);
            System.IO.Directory.CreateDirectory(EXTERN_THUMBNAIL_DIRECTORY);
        }

        static private void closePPTFiles(string dirFullPath)
        {
            List<Presentation> ppts = new List<Presentation>();
            foreach (Presentation p in app.Presentations)
                if (System.IO.Path.GetDirectoryName(p.FullName).CompareTo(dirFullPath) == 0)
                    ppts.Add(p);

            foreach (Presentation p in ppts)
                p.Close();
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
                Bible.ChangeContent(
                    data.getBibleTitle(),
                    data.chapter.ToString(),
                    data.verse.ToString(),
                    (string)data.getContents()[PageIndex].Content,
                    PageIndex == 0);
            }
            else if (Data.getDataType() == ShowContentType.Song)
            {
                Song.SetPageData(((module.Data.SongData)Data).pptFrameFullPath, (string[][])Data.getContents()[PageIndex].Content);
            }
            else if (Data.getDataType() == ShowContentType.PPT)
            {
                ExternPPTs.goToSlide(((module.Data.ExternPPTData)Data).fileFullPath, PageIndex + 1);
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
                int currentSlide = -1;

                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow != null)
                    currentSlide = SlideWindow.View.CurrentShowPosition;
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (pptTextState == PptTextShow.Hide)
                    {
                        ppt.Slides[1].MoveTo(2);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[2].MoveTo(1);
                        currentSlide = 2;
                    }
                    else
                    {
                        SlideWindow = ppt.SlideShowSettings.Run();
                        currentSlide = 1;
                    }
                }
                ReshowSlideWindow(SlideWindow.HWND);

                // Taskbar에서 powerpoint 앱을 숨기는 window api를 사용하면
                // powerpoint show 화면이 꺼졌다 켜질 때 종종 검정화면으로 돌아가는 문제가 있어
                // 아래 코드가 필요합니다.
                SlideWindow.View.GotoSlide(currentSlide);

                pptState = PptSlideState.WindowShow;
                TopMost();
            }

            static public void SlideShowHide()
            {
                if (SlideWindow != null)
                {
                    ShowWindow(SlideWindow.HWND, SW_HIDE);
                    pptState = PptSlideState.WindowHide;
                }
            }

            static public void ChangeContent(string bible, string chapter, string verse, string content,bool FirstPage)
            {
                currentBible = bible;
                currentChapter = chapter;
                currentVerse = verse;
                currentContent = content;
                isFirstPage = FirstPage;

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
                int currentSlide = -1;

                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow != null)
                    currentSlide = SlideWindow.View.CurrentShowPosition;
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (pptTextState == PptTextShow.Hide)
                    {
                        ppt.Slides[1].MoveTo(2);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[2].MoveTo(1);
                        currentSlide = 2;
                    }
                    else
                    {
                        SlideWindow = ppt.SlideShowSettings.Run();
                        currentSlide = 1;
                    }
                }
                ReshowSlideWindow(SlideWindow.HWND);

                // Taskbar에서 powerpoint 앱을 숨기는 window api를 사용하면
                // powerpoint show 화면이 꺼졌다 켜질 때 종종 검정화면으로 돌아가는 문제가 있어
                // 아래 코드가 필요합니다.
                SlideWindow.View.GotoSlide(currentSlide);

                pptState = PptSlideState.WindowShow;
                TopMost();
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
                    if (ppt[i].FrameOriginFullPath.CompareTo(fullPath) == 0)
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
            public string FrameOriginFullPath;

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
                string newPath = FRAME_TEMP_DIRECTORY + System.IO.Path.GetFileName(path);
                System.IO.File.Copy(path, newPath, true);
                newPath = System.IO.Path.GetFullPath(newPath);

                this.FramePPTName = System.IO.Path.GetFileName(newPath);
                this.FrameFullPath = newPath;
                this.FrameOriginFullPath = path;
                textShapes = new List<TextShape>(5);

                ppt = app.Presentations.Open(newPath, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
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
                    SlideWindow = null;
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
                int currentSlide = -1;

                // 슬라이드쇼 점검하는부분 좀 더 개선
                // 슬라이드쇼 끄면 ppt도 꺼지기 때문
                if (SlideWindow != null)
                    currentSlide = SlideWindow.View.CurrentShowPosition;
                if (SlideWindow == null)
                {
                    ppt.SlideShowSettings.ShowType = PpSlideShowType.ppShowTypeKiosk;
                    if (pptTextState == PptTextShow.Hide)
                    {
                        ppt.Slides[1].MoveTo(2);
                        SlideWindow = ppt.SlideShowSettings.Run();
                        ppt.Slides[2].MoveTo(1);
                        currentSlide = 2;
                    }
                    else
                    {
                        SlideWindow = ppt.SlideShowSettings.Run();
                        currentSlide = 1;
                    }
                }
                ReshowSlideWindow(SlideWindow.HWND);

                // Taskbar에서 powerpoint 앱을 숨기는 window api를 사용하면
                // powerpoint show 화면이 꺼졌다 켜질 때 종종 검정화면으로 돌아가는 문제가 있어
                // 아래 코드가 필요합니다.
                SlideWindow.View.GotoSlide(currentSlide);

                pptState = PptSlideState.WindowShow;
                TopMost();
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

            static public void setPresentation(string path)
            {
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

            /// <summary>
            /// 기존 파일이 변경되었는지를 검사해, 자동으로 새로고침합니다.
            /// </summary>
            static public void fetchUpdateFile(string fullPath)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    ppt[index].fetchUpdateFile();
                }
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

            static public void goToSlide(string fullPath, int slideIndex)
            {
                int index;
                if ((index = pptFinder_fullPath(fullPath)) != -1)
                {
                    ppt[index].goToSlide(slideIndex);
                }
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

            // ============ File Checker ============

            private static System.Security.Cryptography.MD5CryptoServiceProvider fileHasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            private byte[] fileHash = null;

            private void assignFileHash()
            {
                fileHash = getFileHash();
            }

            private byte[] getFileHash()
            {
                return fileHasher.ComputeHash(new System.IO.FileStream(OriginalFullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read));
            }

            /// <summary>
            /// 파일을 해싱해 변경되었는지를 검사합니다.
            /// <br/> 파일이 없을 경우 예외가 발생합니다.
            /// </summary>
            /// <returns></returns>
            private bool hasFileChanged()
            {
                byte[] newHash = getFileHash();

                if (newHash.Length == fileHash.Length)
                {
                    int i = 0;
                    while ((i < newHash.Length) && (newHash[i] == fileHash[i]))
                    {
                        i += 1;
                    }
                    if (i == newHash.Length)
                    {
                        // equal hash
                        return false;
                    }
                }
                // not equal hash
                return true;
            }

            // ======================================

            public void setPresentation(string path)
            {
                this.OriginalFullPath = path;
                this.PPTName = System.IO.Path.GetFileName(path);

                ppt = app.Presentations.Open(path, Untitled: Microsoft.Office.Core.MsoTriState.msoTrue, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                checkValidPPT();
                assignFileHash();

                SetThumbNailImages();
            }

            void SetThumbNailImages()
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
                    bi.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                    bi.UriSource = new Uri(f.FullName, UriKind.Absolute);
                    bi.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    bi.Freeze();

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

            // 추후 file refresh와 display 기능을 역할 분리해야 할 것임.
            // file refresh에서 slide show까지 함께 처리하는 것은 복잡성을 높임.
            public void refreshPresentation(string path, bool setTopMost = false, bool setFirstSlide = false)
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

                    setPresentation(path);
                    goToSlide((setFirstSlide ? 1 : currentSlideNum));
                    SlideShowRun((setTopMost ? null : lastShowWindow));

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
                    SlideWindow = null;
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
                ppt.SaveAs(System.IO.Path.GetFullPath(EXTERN_THUMBNAIL_DIRECTORY + this.PPTName), PpSaveAsFileType.ppSaveAsJPG);
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
                    + System.IO.Path.GetFileNameWithoutExtension(this.PPTName)
                    );
            }

            public System.Windows.Media.Imaging.BitmapImage[] getThumbnailImages()
            {
                return thumbnails;
            }

            /// <summary>
            /// 기존 파일이 변경되었는지를 검사해, 자동으로 새로고침합니다.
            /// </summary>
            public void fetchUpdateFile()
            {
                if (new System.IO.FileInfo(OriginalFullPath).Exists
                    && hasFileChanged())
                {
                    refreshPresentation(OriginalFullPath);
                }
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

            /// <summary>
            /// 슬라이드 쇼를 표시합니다.
            /// <br/>- <paramref name="front"/>가 제시되면 <paramref name="front"/> 창의 바로 앞에 표시합니다.
            /// <br/>- <paramref name="front"/>가 제시되지 않으면 가장 앞 화면에 표시합니다.
            /// </summary>
            /// <param name="front"></param>
            public void SlideShowRun(SlideShowWindow front = null)
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
                        if (front == null)
                            ReshowSlideWindow(SlideWindow.HWND, null);
                        else
                            ReshowSlideWindow(SlideWindow.HWND, GetWindow(front.HWND, GW_HWNDPREV));
                        ppt.Slides[1].MoveTo(currentSlideNum);
                    }
                    else
                    {
                        SlideWindow = ppt.SlideShowSettings.Run();
                        if (front == null)
                            ReshowSlideWindow(SlideWindow.HWND, null);
                        else
                            ReshowSlideWindow(SlideWindow.HWND, GetWindow(front.HWND, GW_HWNDPREV));
                    }
                }
                else if (pptState != PptSlideState.WindowShow)
                    ReshowSlideWindow(SlideWindow.HWND);

                // Taskbar에서 powerpoint 앱을 숨기는 window api를 사용하면
                // powerpoint show 화면이 꺼졌다 켜질 때 종종 검정화면으로 돌아가는 문제가 있어
                // 아래 코드가 필요합니다.
                SlideWindow.View.GotoSlide(currentSlideNum);

                pptState = PptSlideState.WindowShow;
                if (front == null)
                    TopMost();
            }

            public void goToSlide(int slideIndex)
            {
                if (slideIndex < 1)
                    currentSlideNum = 1;
                else if (slideIndex > ppt.Slides.Count)
                    currentSlideNum = ppt.Slides.Count;
                else
                    currentSlideNum = slideIndex;

                requestMovingSlide(currentSlideNum);
            }

            // ========================== Threaded Slide Moving ==========================

            // 애니메이션이 포함된 슬라이드 이동시 UI가 잠시 멈추게 되는데 (UI는 동기(sync) 상태)
            // 이때, 빠르게 UI를 조작하면 이상한 번호로 슬라이드가 이동되는 문제가 발생함.
            // 따라서 비동기 로직으로 구성하기 위해, 실제 슬라이드의 이동은 스레딩을 통해 처리함.

            private static System.Threading.Mutex slideControlMutex = new System.Threading.Mutex();
            private static (bool isRequested, ExternPPT requester, int slideIndex) slideMoveRequest = NULL_REQUEST;
            private static System.Threading.Thread slideControlThread = null;

            private static readonly (bool, ExternPPT requester, int) NULL_REQUEST = (false, null, -1);

            private static void slideMoveThreadFunc()
            {
                (bool isRequested, ExternPPT requester, int slideIndex) readData;
                while (true)
                {
                    readData = NULL_REQUEST;
                    slideControlMutex.WaitOne();

                    if (slideMoveRequest.isRequested)
                    {
                        if (slideMoveRequest.requester.SlideWindow != null)
                            readData = slideMoveRequest;
                        slideMoveRequest = NULL_REQUEST;
                    }

                    slideControlMutex.ReleaseMutex();

                    if (readData.isRequested) {
                        try
                        {
                            readData.requester.SlideWindow.View.GotoSlide(readData.slideIndex);
                        }
                        catch (Exception e)
                        {
                            slideControlMutex.WaitOne();

                            if (!slideMoveRequest.isRequested)
                                slideMoveRequest = readData;

                            slideControlMutex.ReleaseMutex();
                        }
                        readData = NULL_REQUEST;
                    }

                    System.Threading.Thread.Sleep(10);
                }
            }

            private void requestMovingSlide(int slideIdx)
            {
                if (slideControlThread == null)
                {
                    slideControlThread = new System.Threading.Thread(slideMoveThreadFunc) { IsBackground = true };
                    slideControlThread.Start();
                }

                slideControlMutex.WaitOne();
                slideMoveRequest = (true, this, slideIdx);
                slideControlMutex.ReleaseMutex();
            }

            // ===========================================================================

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
