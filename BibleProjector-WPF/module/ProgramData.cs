﻿using System;
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
        // =========================================== 파일 경로 =========================================== 

        // 파일 경로들
        const string PROGRAM_DATA_PATH = ".\\programData";

        const string LYRIC_DATA = PROGRAM_DATA_PATH + "\\Lyrics";
        const string HYMN_DATA = PROGRAM_DATA_PATH + "\\Hymns";
        const string HYMN_SUB_DATA = PROGRAM_DATA_PATH + "\\HymnSubData";
        const string OPTION_DATA = PROGRAM_DATA_PATH + "\\Option";
        const string RESERVE_DATA = PROGRAM_DATA_PATH + "\\ReserveData";
        const string EXTERN_PPT_DATA = PROGRAM_DATA_PATH + "\\ExternPPTpaths";

        // 이전 버전의 프로그램 저장값을 지원하기 위한 장치
        // 더 이상 추가 지원하지 않을 기능들
        const string LAYOUT_DATA = PROGRAM_DATA_PATH + "\\LayoutData";
        const string LYRIC_RESERVE_DATA = PROGRAM_DATA_PATH + "\\LyricReserve";
        const string BIBLE_RESERVE_DATA = PROGRAM_DATA_PATH + "\\BibleReserve";

        static public void Initialize()
        {
            string[] fileList = { HYMN_DATA, HYMN_SUB_DATA };

            StringBuilder warningPhrase = new StringBuilder("프로그램 실행에 필요한 다음의 파일들이 없습니다!\n");
            bool isFileMissing = false;
            foreach(string FilePath in fileList)
            {
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(FilePath));
                if (!di.Exists)
                    di.Create();

                if (!new FileInfo(FilePath).Exists)
                {
                    warningPhrase.Append(Path.GetFileName(FilePath)).Append('\n');
                    isFileMissing = true;
                }
            }

            if (isFileMissing)
                throw new Exception(warningPhrase.ToString());
        }

        // =========================================== 프로그램 종료시 ===========================================

        static public event Event.SaveDataEventHandler SaveDataEvent;

        static public void saveProgramData()
        {
            SaveDataEvent?.Invoke(null, new Event.SaveDataEventArgs(saveData));
        }

        static void saveData(SaveDataTypeEnum type, string data)
        {
            string savePath = null;

            if (type == SaveDataTypeEnum.LyricData)
                savePath = LYRIC_DATA;
            else if (type == SaveDataTypeEnum.HymnData)
                savePath = HYMN_DATA;
            else if (type == SaveDataTypeEnum.ExternPPTData)
                savePath = EXTERN_PPT_DATA;
            else if (type == SaveDataTypeEnum.ReserveData)
                savePath = RESERVE_DATA;
            else if (type == SaveDataTypeEnum.OptionData)
                savePath = OPTION_DATA;

            if (savePath == null)
                return;

            StreamWriter file = new StreamWriter(savePath, false);
            file.Write(data);
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

        public static string getLyricData()
        {
            return getDataFromFile(LYRIC_DATA);
        }

        public static string getHymnData()
        {
            return getDataFromFile(HYMN_DATA);
        }

        public static string getHymnSubData()
        {
            return getDataFromFile(HYMN_SUB_DATA);
        }

        public static string getOptionData()
        {
            return getDataFromFile(OPTION_DATA);
        }

        public static string getLayoutData()
        {
            return getDataFromFile(LAYOUT_DATA);
        }

        public static string getReserveData()
        {
            return getDataFromFile(RESERVE_DATA);
        }

        public static string getExternPPTData()
        {
            string data = getDataFromFile(EXTERN_PPT_DATA);
            File.Delete(EXTERN_PPT_DATA);
            return data;
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
    }
}
