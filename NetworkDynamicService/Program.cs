using ScadaCommon.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkDynamicServiceHost nds = new NetworkDynamicServiceHost();

            AlarmServiceRef.AlarmServiceOperationsClient alarmServiceClient = new AlarmServiceRef.AlarmServiceOperationsClient();
            EventServiceRef.EventServiceOperationsClient eventServiceClient = new EventServiceRef.EventServiceOperationsClient();

            try
            {
                nds.Start();
                /*Alarm a = new Alarm() { AlarmReportedBy = ScadaCommon.AlarmEventType.CE, Message = "Bla", GiD = 12354654,
                                        PointName = "Name", Username = "somebody"};
                Alarm a1 = new Alarm()
                {
                    AlarmReportedBy = ScadaCommon.AlarmEventType.SCADA,
                    GiD = 65451063,
                    Message = "message",
                    PointName = "pointName",
                    Username = "somebody nobody"
                };
                alarmServiceClient.AddAlarm(a);
                alarmServiceClient.AddAlarm(a1);

                List<Alarm> list = alarmServiceClient.GetAllAlarms().ToList<Alarm>();
                a1 = list[1];
                a1.AlarmAcknowledged = DateTime.Now;
                a1.Username = "no one";
                alarmServiceClient.AcknowledgeAlarm(a1);
                alarmServiceClient.DeleteAlarm(a1.ID);*/

                /*Event e = new Event() { GiD = 1561321535, EventReportedBy = ScadaCommon.AlarmEventType.CE, Message = "some", PointName = "name" };
                eventServiceClient.AddEvent(e);*/

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.ReadKey();
        }
    }
}
