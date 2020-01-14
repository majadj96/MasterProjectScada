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
            FrontEndProcessorServiceProxy fepsProxy = new FrontEndProcessorServiceProxy("FieldCommunicationServiceEndPoint");

            using (NetworkDynamicService nds = new NetworkDynamicService())
            {
                nds.Start();
                Console.WriteLine("Kao krenulo!");

                Console.ReadKey();
                fepsProxy.Open();
                fepsProxy.ReadAnalogInput(0);
                
                Console.ReadLine();
            }

            Console.ReadKey();
        }
    }
}
