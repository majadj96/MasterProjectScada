using System;

namespace FrontEndProcessorService
{
    class Program
    {
        static void Main(string[] args)
        {
            FrontEndProcessorService fcs = new FrontEndProcessorService();

            try
            {
                fcs.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.ReadKey();
        }
    }
}
