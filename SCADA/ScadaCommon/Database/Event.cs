using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System;

namespace ScadaCommon.Database
{
    [DataContract]
    [KnownTypeAttribute(typeof(AlarmEventType))]
    public class Event
    {
        #region Variables
        private int iD;
        private long giD;
        private DateTime eventReported;
        private AlarmEventType eventReportedBy;
        private string message;
        private string pointName;
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
        public DateTime EventReported
        {
            get { return eventReported; }
            set { eventReported = value; }
        }
        [DataMember]
        public AlarmEventType EventReportedBy
        {
            get { return eventReportedBy; }
            set { eventReportedBy = value; }
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
        #endregion

        public Event()
        {
            EventReported = DateTime.Now;
        }
    }
}
