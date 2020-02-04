using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel;

namespace ScadaCommon.Database
{
    [ServiceContract]
    public interface IEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int ID { get; set; }
        long GiD { get; set; }
        DateTime EventReported { get; set; }
        AlarmEventType EventReportedBy { get; set; }
        string Message { get; set; }
        string PointName { get; set; }
    }
}
