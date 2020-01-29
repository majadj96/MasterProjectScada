using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldCommunicationService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FieldCommunicationService fcs = new FieldCommunicationService())
            {
                fcs.Start();
            }
            Console.ReadKey();
        }
    }
}
