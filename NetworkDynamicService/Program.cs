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
            Console.Title = "NDS";

            NetworkDynamicServiceHost nds = new NetworkDynamicServiceHost();
            
            try
            {
                nds.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.ReadKey();
        }
    }
}
