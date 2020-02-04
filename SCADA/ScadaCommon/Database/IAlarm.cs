using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel;

namespace ScadaCommon.Database
{
    [ServiceContract]
    public interface IAlarm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int ID { get; set; }
        long GiD { get; set; }
        DateTime AlarmReported { get; set; }
        AlarmEventType AlarmReportedBy { get; set; }
        string Message { get; set; }
        string PointName { get; set; }
        DateTime? AlarmAcknowledged { get; set; }
        string Username { get; set; }
    }
}
