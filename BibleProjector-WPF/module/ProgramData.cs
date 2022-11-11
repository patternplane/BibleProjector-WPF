using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace BibleProjector_WPF.module
{
    class ProgramData
    {
        // 데이터를 가진 객체에 접근하기 위한 참조변수들
        static ViewModel.BibleReserveData VM_BibleReserve;
        static ViewModel.LyricViewModel VM_LyricViewModel;
        static ViewModel.ExternPPTViewModel VM_ExternPPT;

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■ 공사부근 : 여러 곳의 예약을 하나로 통합중  ■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        static ViewModel.ReserveManagerViewModel VM_ReserveManager;

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        // =========================================== 파일 경로 =========================================== 

        // 파일 경로들
        const string PROGRAM_DATA_PATH = ".\\programData";

        const string BIBLE_RESERVE_DATA = PROGRAM_DATA_PATH + "\\BibleReserve";
        const string LYRIC_DATA = PROGRAM_DATA_PATH + "\\Lyrics";
        const string LYRIC_RESERVE_DATA = PROGRAM_DATA_PATH + "\\LyricReserve";
        const string HYMN_DATA = PROGRAM_DATA_PATH + "\\Hymns";
        const string OPTION_DATA = PROGRAM_DATA_PATH + "\\Option";
        const string LAYOUT_DATA = PROGRAM_DATA_PATH + "\\LayoutData";
        const string EXTERN_PPT_DATA = PROGRAM_DATA_PATH + "\\ExternPPTpaths";

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■ 공사부근 : 여러 곳의 예약을 하나로 통합중  ■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        const string RESERVE_DATA = PROGRAM_DATA_PATH + "\\ReserveData";

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        // =========================================== 프로그램 종료시 ===========================================

        static public void saveProgramData()
        {
            saveBibleReserveData();
            saveLyricData();
            saveOptionData();
            saveLayoutData();
            saveExternPPTData();

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // ■■■■■■■■■■■■■ 공사부근 : 여러 곳의 예약을 하나로 통합중  ■■■■■■■■■■■■■■
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            saveReserveData();

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        }

        static void saveBibleReserveData()
        {
            StreamWriter file = new StreamWriter(BIBLE_RESERVE_DATA, false);
            file.Write(VM_BibleReserve.getSaveData());
            file.Close();
        }

        static void saveLyricData()
        {
            StreamWriter file = new StreamWriter(LYRIC_DATA, false);
            file.Write(VM_LyricViewModel.getSaveData_Lyric());
            file.Close();

            file = new StreamWriter(LYRIC_RESERVE_DATA, false);
            file.Write(VM_LyricViewModel.getSaveData_Reserve());
            file.Close();

            file = new StreamWriter(HYMN_DATA, false);
            file.Write(VM_LyricViewModel.getSaveData_Hymn());
            file.Close();

            /*
            catch (Exception e)
            {
                MessageBox.Show("가사 저장 실패!\n오류 : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            */
        }

        static void saveOptionData()
        {
            StreamWriter file = new StreamWriter(OPTION_DATA, false);
            file.Write(module.ProgramOption.getSaveData());
            file.Close();
        }

        static void saveLayoutData()
        {
            StreamWriter file = new StreamWriter(LAYOUT_DATA, false);
            file.Write(module.LayoutInfo.getSaveData());
            file.Close();
        }

        static void saveExternPPTData()
        {
            StreamWriter file = new StreamWriter(EXTERN_PPT_DATA, false);
            file.Write(VM_ExternPPT.getSaveData());
            file.Close();
        }

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■ 공사부근 : 여러 곳의 예약을 하나로 통합중  ■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        static void saveReserveData()
        {
            StreamWriter file = new StreamWriter(RESERVE_DATA, false);
            file.Write(VM_ReserveManager.getSaveData());
            file.Close();
        }

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        // =========================================== 별도 메소드 =========================================== 

        /// <summary>
        /// 특정 파일의 정보를 문자열로 읽어옵니다.
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>파일의 내용</returns>
        static string getDataFromFile(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filePath));
            if (!di.Exists)
                di.Create();

            StringBuilder fileContent = new StringBuilder(50);
            fileContent.Clear();

            if ((new FileInfo(filePath)).Exists)
            {
                StreamReader file = new StreamReader(filePath);
                fileContent.Append(file.ReadToEnd());
                file.Close();
            }

            return fileContent.ToString();
        }

        public static string getBibleReserveData(ViewModel.BibleReserveData bibleReserveData)
        {
            VM_BibleReserve = bibleReserveData;
            return getDataFromFile(BIBLE_RESERVE_DATA);
        }

        public static string getLyricData(ViewModel.LyricViewModel lyricViewModel)
        {
            VM_LyricViewModel = lyricViewModel;
            return getDataFromFile(LYRIC_DATA);
        }

        public static string getLyricReserveData()
        {
            return getDataFromFile(LYRIC_RESERVE_DATA);
        }

        public static string getHymnData()
        {
            return getDataFromFile(HYMN_DATA);
        }

        public static string getOptionData()
        {
            return getDataFromFile(OPTION_DATA);
        }

        public static string getLayoutData()
        {
            return getDataFromFile(LAYOUT_DATA);
        }

        public static string getExternPPTData(ViewModel.ExternPPTViewModel ExternPPTViewModel)
        {
            VM_ExternPPT = ExternPPTViewModel;
            return getDataFromFile(EXTERN_PPT_DATA);
        }

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■ 공사부근 : 여러 곳의 예약을 하나로 통합중  ■■■■■■■■■■■■■■
        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        public static string getReserveData(ViewModel.ReserveManagerViewModel ReserveManagerViewModel)
        {
            VM_ReserveManager = ReserveManagerViewModel;
            return getDataFromFile(RESERVE_DATA);
        }

        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    }
}
