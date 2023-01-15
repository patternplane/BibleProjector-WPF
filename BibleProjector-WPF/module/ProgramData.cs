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
        static ViewModel.LyricViewModel VM_LyricViewModel;
        static ViewModel.ReserveManagerViewModel VM_ReserveManager;

        // =========================================== 파일 경로 =========================================== 

        // 파일 경로들
        const string PROGRAM_DATA_PATH = ".\\programData";

        const string LYRIC_DATA = PROGRAM_DATA_PATH + "\\Lyrics";
        const string HYMN_DATA = PROGRAM_DATA_PATH + "\\Hymns";
        const string OPTION_DATA = PROGRAM_DATA_PATH + "\\Option";
        const string LAYOUT_DATA = PROGRAM_DATA_PATH + "\\LayoutData";
        const string RESERVE_DATA = PROGRAM_DATA_PATH + "\\ReserveData";

        const string MANUAL_DATA = PROGRAM_DATA_PATH + "\\RawManual";

        // 이전 버전의 프로그램 저장값을 지원하기 위한 장치
        // 더 이상 추가 지원하지 않을 기능들
        const string LYRIC_RESERVE_DATA = PROGRAM_DATA_PATH + "\\LyricReserve";
        const string BIBLE_RESERVE_DATA = PROGRAM_DATA_PATH + "\\BibleReserve";
        const string EXTERN_PPT_DATA = PROGRAM_DATA_PATH + "\\ExternPPTpaths";

        // =========================================== 프로그램 종료시 ===========================================

        static public void saveProgramData()
        {
            saveLyricData();
            saveOptionData();
            saveLayoutData();
            saveReserveData();
        }

        static void saveLyricData()
        {
            StreamWriter file = new StreamWriter(LYRIC_DATA, false);
            file.Write(VM_LyricViewModel.getSaveData_Lyric());
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

        static void saveReserveData()
        {
            StreamWriter file = new StreamWriter(RESERVE_DATA, false);
            file.Write(VM_ReserveManager.getSaveData());
            file.Close();
        }

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

        public static string getLyricData(ViewModel.LyricViewModel lyricViewModel)
        {
            VM_LyricViewModel = lyricViewModel;
            return getDataFromFile(LYRIC_DATA);
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

        public static string getReserveData(ViewModel.ReserveManagerViewModel ReserveManagerViewModel)
        {
            VM_ReserveManager = ReserveManagerViewModel;
            return getDataFromFile(RESERVE_DATA);
        }

        public static string getManualData()
        {
            return getDataFromFile(MANUAL_DATA);
        }

        // 더 이상 추가 지원하지 않을 기능들

        public static string getLyricReserveData()
        {
            string data = getDataFromFile(LYRIC_RESERVE_DATA);
            File.Delete(LYRIC_RESERVE_DATA);
            return data;
        }

        public static string getBibleReserveData()
        {
            string data = getDataFromFile(BIBLE_RESERVE_DATA);
            File.Delete(BIBLE_RESERVE_DATA);
            return data;
        }

        public static string getExternPPTData()
        {
            string data = getDataFromFile(EXTERN_PPT_DATA);
            File.Delete(EXTERN_PPT_DATA);
            return data;
        }
    }
}
