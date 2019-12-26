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
            using (NetworkDynamicService nds = new NetworkDynamicService())
            {
                nds.Start();

                Console.WriteLine("Kao krenulo!");
                Console.ReadLine();
            }

            Console.ReadKey();
        }
    }
}
