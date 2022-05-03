﻿using System;
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

        [System.Runtime.InteropServices.DllImport("user32")]
        static extern int ShowWindow(int hwnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

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
                foreach (module.ProgramOption.SongFrameFile f in module.ProgramOption.SongFrameFiles)
                    Powerpoint.Song.setPresentation(f.Path);
            

            return pptFrameError.ToString();
        }

        static public void FinallProcess()
        {
            Bible.close();
            Reading.close();
            Song.closeAll();
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
                getCommand();
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
                    int currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;
                    lastppt.Close();

                    setPresentation(path);
                    Change();
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    int currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    getCommand();
                    Change();
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                    SlideShowRun();

                    lastppt.Close();

                    lastppt = ppt;
                    currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;

                    setPresentation(path);
                    Change();
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                    SlideShowRun();

                    lastppt.Close();
                }
            }

            static void getCommand()
            {
                foreach (Shape s in ppt.Slides[1].Shapes)
                    if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{b}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{ch}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{v}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{va}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{c}", module.StringKMP.DefaultStringCompaerFunc))
                        {
                            TextShapes.Add(s);
                            Format.Add(s.TextFrame.TextRange.Text);
                        }
                    }
            }

            static public void close()
            {
                if (ppt != null)
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
                    if (isFirstPage)
                        TextShapes[i].TextFrame.TextRange.Text =
                        Format[i]
                            .Replace("{b}", currentBible)
                            .Replace("{ch}", currentChapter)
                            .Replace("{va}", currentVerse)
                            .Replace("{c}", currentContent)
                            .Replace("{v}", currentVerse);
                    else
                        TextShapes[i].TextFrame.TextRange.Text =
                            Format[i]
                            .Replace("{b}", currentBible)
                            .Replace("{ch}", currentChapter)
                            .Replace("{va}", currentVerse)
                            .Replace("{c}", currentContent)
                            .Replace("{v}", "");
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
                getCommand();
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
                    int currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;
                    lastppt.Close();

                    setPresentation(path);
                    Change();
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    int currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    getCommand();
                    Change();
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                    SlideShowRun();

                    lastppt.Close();

                    lastppt = ppt;
                    currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;

                    setPresentation(path);
                    Change();
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                    SlideShowRun();

                    lastppt.Close();
                }
            }

            static void getCommand()
            {
                foreach (Shape s in ppt.Slides[1].Shapes)
                    if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        if (module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{t}", module.StringKMP.DefaultStringCompaerFunc)
                            || module.StringKMP.HasPattern(s.TextFrame.TextRange.Text, "{c}", module.StringKMP.DefaultStringCompaerFunc))
                        {
                            TextShapes.Add(s);
                            Format.Add(s.TextFrame.TextRange.Text);
                        }
                    }
            }

            static public void close()
            {
                if (ppt != null)
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

            static public void SetSongData(string FramePPTName, string[][][] songData)
            {
                int index;
                if ((index = pptFinder(FramePPTName)) != -1)
                {
                    ppt[index].SetSongData( songData);
                }
            }

            static public void SlideShowRun(string FramePPTName)
            {
                int index;
                if ((index = pptFinder(FramePPTName)) != -1)
                {
                    ppt[index].SlideShowRun();
                    for (int i = 0; i < ppt.Count; i++)
                        if (i != index)
                            ppt[i].SlideShowHide();
                }
            }

            static public void SlideShowHide(string FramePPTName)
            {
                int index;
                if ((index = pptFinder(FramePPTName)) != -1)
                {
                    ppt[index].SlideShowHide();
                }
            }

            static public void ChangePage(string FramePPTName, int Page)
            {
                int index;
                if ((index = pptFinder(FramePPTName)) != -1)
                {
                    ppt[index].ChangeApply(Page);
                }
            }

            static public void HideText(string FramePPTName)
            {
                int index;
                if ((index = pptFinder(FramePPTName)) != -1)
                {
                    ppt[index].HideText();
                }
            }

            static public void ShowText(string FramePPTName)
            {
                int index;
                if ((index = pptFinder(FramePPTName)) != -1)
                {
                    ppt[index].ShowText();
                }
            }
        }

        public class SongFormatPPT
        {
            // ============================================ 필요 변수 ============================================ 

            public string FramePPTName;

            Presentation ppt;
            class TextShape
            {
                public List<string> Format;
                public Shape shape;
            }
            List<TextShape> textShapes;
            string[][][] SongData;
            int currentPage = -1;
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
                textShapes = new List<TextShape>(5);

                ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                getCommand();
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
                    int currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;
                    lastppt.Close();

                    setPresentation(path);
                    ChangeApply(currentPage);
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                }
                else if (pptState == PptSlideState.WindowShow)
                {
                    int currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;

                    ppt = app.Presentations.Open(path, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                    getCommand();
                    ChangeApply(currentPage);
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                    SlideShowRun();

                    lastppt.Close();

                    lastppt = ppt;
                    currentPosition = SlideWindow.View.CurrentShowPosition;
                    SlideWindow = null;

                    setPresentation(path);
                    ChangeApply(currentPage);
                    if (currentPosition == 1)
                        ShowText();
                    else if (currentPosition == 2)
                        HideText();
                    SlideShowRun();

                    lastppt.Close();
                }
            }

            public void deletePresentation()
            {
                ppt.Close();

                System.IO.File.Delete(System.IO.Path.GetFullPath(FRAME_TEMP_DIRECTORY + FramePPTName));
            }

            void getCommand()
            {
                string shapeText;
                TextShape ts;
                foreach (Shape s in ppt.Slides[1].Shapes)
                    if (s.HasTextFrame != Microsoft.Office.Core.MsoTriState.msoFalse)
                    {
                        int[] index = module.StringKMP.FindPattern(s.TextFrame.TextRange.Text, "{", module.StringKMP.DefaultStringCompaerFunc);
                        if (index.Length != 0)
                        {
                            shapeText = s.TextFrame.TextRange.Text;
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
                                if(endIndex == shapeText.Length)
                                    break;
                                endIndex++;

                                format.Add(shapeText.Substring(startIndex, endIndex - startIndex));
                                startIndex = endIndex;
                            }
                            if (i != index.Length)
                                break;
                            else if (startIndex != shapeText.Length)
                                format.Add(shapeText.Substring(startIndex, shapeText.Length - startIndex));

                            ts = new TextShape();
                            ts.shape = s;
                            ts.Format = format;
                            textShapes.Add(ts);
                        }
                    }
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

            public void ChangeApply(int Page)
            {
                currentPage = Page;
                int index;
                StringBuilder str = new StringBuilder(50);
                for (int i = 0; i < textShapes.Count; i++)
                {
                    str.Clear();
                    foreach (string s in textShapes[i].Format)
                    {
                        if (s[0] == '{')
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
}
