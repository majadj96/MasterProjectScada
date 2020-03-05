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
        }
    }
}
