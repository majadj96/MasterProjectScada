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
            PointOperateService pos = new PointOperateService();
            NetworkDynamicService nds = new NetworkDynamicService();
            
            try
            {
                nds.Start();
                pos.Start();
                Console.WriteLine("Kao krenulo!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.ReadKey();
        }
    }
}
