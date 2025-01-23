using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public abstract class ShowData
    {
        abstract public string getTitle2();
        abstract public string getTitle1();
        abstract public ShowContentData[] getContents();
        abstract public ShowData getNextShowData();
        abstract public ShowData getPrevShowData();
        abstract public ShowContentType getDataType();
        public void deleteProcess()
        {
            OnItemDeleted();
        }
        abstract public bool isSameData(ShowData data);
        public virtual void preprocessBeforeShow() { }
        abstract public ShowExcuteErrorEnum canExcuteShow();
        abstract public bool isAvailData();

        // =================== Events ==================

        public event EventHandler ItemRefreshedEvent;
        protected void OnItemRefreshed()
        {
            ItemRefreshedEvent?.Invoke(this, new EventArgs());
        }

        public event EventHandler ItemDeletedEvent;
        protected void OnItemDeleted()
        {
            ItemDeletedEvent?.Invoke(this, new EventArgs());
        }
    }
}
