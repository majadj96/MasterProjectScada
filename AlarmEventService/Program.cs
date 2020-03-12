using System;

namespace AlarmEventService
{
    class Program
    {
        static void Main(string[] args)
        {
            AlarmEventServiceHost aesh = new AlarmEventServiceHost();

            try
            {
                aesh.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
