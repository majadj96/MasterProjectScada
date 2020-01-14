using System;

namespace FrontEndProcessorService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FrontEndProcessorService fcs = new FrontEndProcessorService())
            {
                fcs.Start();
                Console.ReadKey();
            }
        }
    }
}
