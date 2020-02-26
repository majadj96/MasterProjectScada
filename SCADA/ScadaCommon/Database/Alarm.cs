using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System;

namespace ScadaCommon.Database
{
    [DataContract]
    [KnownTypeAttribute(typeof(AlarmEventType))]
    public class Alarm
    {
        #region Variables
      /*  private int iD;
        private long giD;
        private DateTime alarmReported;
        private AlarmEventType alarmReportedBy;
        private string message;
        private string pointName;
        private DateTime? alarmAcknowledged;
        private string username;*/
        #endregion

        #region Props
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int ID
        {
            get;
            set;
        }
        [DataMember]
        public long GiD
        {
            get;
            set;
        }
        [DataMember]
        public DateTime AlarmReported
        {
            get;
            set;
        }
        [DataMember]
        public AlarmEventType AlarmReportedBy
        {
            get;
            set;
        }
        [DataMember]
        public string Message
        {
            get;
            set;
        }
        [DataMember]
        public string PointName
        {
            get;
            set;
        }
        [DataMember]
        public DateTime? AlarmAcknowledged
        {
            get;
            set;
        }
        [DataMember]
        public string Username
        {
            get;
            set;
        }
        #endregion

        public Alarm()
        {
            AlarmReported = DateTime.Now;
        }
    }
}
