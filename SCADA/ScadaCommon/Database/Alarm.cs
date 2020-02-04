using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System;

namespace ScadaCommon.Database
{
    [DataContract]
    public class Alarm : IAlarm
    {
        #region Variables
        private int iD;
        private long giD;
        private DateTime alarmReported;
        private AlarmEventType alarmReportedBy;
        private string message;
        private string pointName;
        private DateTime? alarmAcknowledged;
        private string username;
        #endregion

        #region Props
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        [DataMember]
        public long GiD
        {
            get { return giD; }
            set { giD = value; }
        }
        [DataMember]
        public DateTime AlarmReported
        {
            get { return alarmReported; }
            set { alarmReported = value; }
        }
        [DataMember]
        public AlarmEventType AlarmReportedBy
        {
            get { return alarmReportedBy; }
            set { alarmReportedBy = value; }
        }
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember]
        public string PointName
        {
            get { return pointName; }
            set { pointName = value; }
        }
        [DataMember]
        public DateTime? AlarmAcknowledged
        {
            get { return alarmAcknowledged; }
            set
            {
                if(value != null)
                    alarmAcknowledged = (DateTime)value;
            }
        }
        [DataMember]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        #endregion

        public Alarm()
        {
            AlarmReported = DateTime.Now;
        }
    }
}
