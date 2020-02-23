using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class Event
    {
        #region Variables
        private int iD;
        private long giD;
        private DateTime eventReported;
        private string eventReportedBy;
        private string message;
        private string pointName;
        #endregion

        #region Props
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public long GiD
        {
            get { return giD; }
            set { giD = value; }
        }
        public DateTime AlarmReported
        {
            get { return eventReported; }
            set { eventReported = value; }
        }
        public string AlarmReportedBy
        {
            get { return eventReportedBy; }
            set { eventReportedBy = value; }
        }
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public string PointName
        {
            get { return pointName; }
            set { pointName = value; }
        }
        #endregion

        public Event() { }
    }
}
