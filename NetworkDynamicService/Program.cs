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
            NetworkDynamicService nds = new NetworkDynamicService(); 
            try
            {
                nds.Start();
                //Console.WriteLine("Kao krenulo!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.ReadKey();
        }
    }
}
