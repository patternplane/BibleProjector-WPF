using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using BibleProjector_WPF.module.Infrastructure;

namespace BibleProjector_WPF.module
{
    public class Manual
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    internal class ManualData
    {
		private Manual[] manuals = null;

		public ManualData()
		{
			string rawString = ProgramData.getManualData();
			try
			{
				manuals = JsonSerializer.Deserialize<Manual[]>(rawString);
			}
			catch (Exception e)
            {
				if (e is JsonException)
				{
					ProgramData.writeErrorLog("메뉴얼 데이터 에러 : Json 형태가 아님", e);
				}
				else
				{
					System.Windows.MessageBox.Show("메뉴얼 로딩 에러", "메뉴얼 읽기 과정에서 내부 충돌이 발생하여, 메뉴얼 생성은 무시됩니다.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
					ProgramData.writeErrorLog("메뉴얼 처리 중 Json 로직 에러 - (데이터 문제가 아님) 로직 에러가 발생했습니다!", e);
				}
            }
		}

		public Manual[] GetManuals()
		{
			return manuals;
		}
    }
}
