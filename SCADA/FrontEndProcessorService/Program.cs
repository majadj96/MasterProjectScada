using System;

namespace FrontEndProcessorService
{
    class Program
    {
        static void Main(string[] args)
        {
            FieldCommunicationService fcs = new FieldCommunicationService();

            try
            {
                fcs.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
