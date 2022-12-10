using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class Bible : IReserveOptionViewModel
    {
        module.BibleReserveDataUnit selection;

        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            selection = (module.BibleReserveDataUnit)data[0].reserveData;
        }

        public void ShowContent()
        {
            if (module.ProgramOption.BibleFramePath == null)
                    System.Windows.MessageBox.Show("성경 출력 틀ppt를 등록해주세요!", "ppt틀 등록되지 않음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
                new module.ShowStarter().BibleShowStart(
                    selection.Book
                    + selection.Chapter
                    + selection.Verse);
        }
    }
}
