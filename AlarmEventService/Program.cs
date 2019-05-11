using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
