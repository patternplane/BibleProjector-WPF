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
        // 성경 예약
        static public BindingList<ViewModel.BibleReserveData.BibleReserveContent> BibleReserveList;

        // =========================================== 프로그램 초기 세팅 =========================================== 

        static public void getProgramData()
        {
            getDataFromFile();
        }

        // 파일 경로들
        const string PROGRAM_DATA_PATH = ".\\programData";
        const string BIBLE_RESERVE_DATA = PROGRAM_DATA_PATH + "\\BibleReserve";

        static void getDataFromFile()
        {
            DirectoryInfo di = new DirectoryInfo(PROGRAM_DATA_PATH);
            if (!di.Exists)
                di.Create();

            FileInfo fi = new FileInfo(BIBLE_RESERVE_DATA);
            StringBuilder fileContent = new StringBuilder(50);
            StreamReader file = new StreamReader(BIBLE_RESERVE_DATA);
            fileContent.Clear();
            if (fi.Exists)
                fileContent.Append(file.ReadToEnd());
            file.Close();
            getBibleReserveData(fileContent.ToString());
        }

        static void getBibleReserveData(string fileContent)
        {
            BibleReserveList = new BindingList<ViewModel.BibleReserveData.BibleReserveContent>();
            string[] data;
            foreach (string token in fileContent.Split(new string[] {"\r\n" },StringSplitOptions.RemoveEmptyEntries))
            {
                data = token.Split(new char[] { ' '},StringSplitOptions.RemoveEmptyEntries);
                BibleReserveList.Add(new ViewModel.BibleReserveData.BibleReserveContent(data[0],data[1],data[2]));
            }
        }

        // =========================================== 프로그램 종료 ===========================================

        static public void saveProgramData()
        {
            saveBibleReserveData();
        }

        static void saveBibleReserveData()
        {
            StringBuilder str = new StringBuilder(50).Clear();
            foreach (ViewModel.BibleReserveData.BibleReserveContent item in BibleReserveList)
            {
                str.Append(item.Book);
                str.Append(" ");
                str.Append(item.Chapter);
                str.Append(" ");
                str.Append(item.Verse);
                str.Append("\r\n");
            }

            StreamWriter file = new StreamWriter(BIBLE_RESERVE_DATA);
            file.Write(str.ToString());
            file.Close();
        }
    }
}
