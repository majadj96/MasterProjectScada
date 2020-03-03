using System;

namespace AlarmEventService
{
    class Program
    {
        static void Main(string[] args)
        {
            AlarmEventServiceHost alarmEventServiceHost = new AlarmEventServiceHost();

            try
            {
                alarmEventServiceHost.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();

            /*AlarmEventServices alarmEventServices = new AlarmEventServices();

            Alarm a = new Alarm()
            {
                AlarmReportedBy = AlarmEventType.CE,
                Message = "Bla",
                GiD = 12354654,
                PointName = "Name",
                Username = "somebody"
            };

            alarmEventServices.AddAlarm(a);

            Event e = new Event() { GiD = 1561321535, EventReportedBy = ScadaCommon.AlarmEventType.CE, Message = "some", PointName = "name" };

            alarmEventServices.AddEvent(e);


            Alarm a = alarmEventServices.GetAllAlarms().Last();
            a.AlarmAcknowledged = DateTime.Now;
            a.Username = "no one";
            alarmEventServices.AcknowledgeAlarm(a);*/
        }
    }
}
