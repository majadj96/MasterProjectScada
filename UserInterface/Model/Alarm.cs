using System;

namespace UserInterface.Model
{
    public class Alarm
    {
        #region Variables
        private int iD;
        private long giD;
        private DateTime alarmReported;
        private string alarmReportedBy;
        private string message;
        private string pointName;
        private DateTime alarmAcknowledged;
        private string username;
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
            get { return alarmReported; }
            set { alarmReported = value; }
        }
        public string AlarmReportedBy
        {
            get { return alarmReportedBy; }
            set { alarmReportedBy = value; }
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
        public DateTime AlarmAcknowledged
        {
            get { return alarmAcknowledged; }
            set { alarmAcknowledged = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        #endregion

        public Alarm() { }
    }
}
