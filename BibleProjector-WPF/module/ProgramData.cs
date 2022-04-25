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

        // =========================================== 파일 경로 =========================================== 

        // 파일 경로들
        const string PROGRAM_DATA_PATH = ".\\programData";

        const string BIBLE_RESERVE_DATA = PROGRAM_DATA_PATH + "\\BibleReserve";
        const string LYRIC_DATA = PROGRAM_DATA_PATH + "\\Lyrics";
        

        // =========================================== 프로그램 종료시 ===========================================

        static public void saveProgramData()
        {
            saveBibleReserveData();
            saveLyricData();
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
            file.Write(VM_LyricViewModel.getSaveData());
            file.Close();
            
            /*
            catch (Exception e)
            {
                MessageBox.Show("가사 저장 실패!\n오류 : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            */
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
    }
}
