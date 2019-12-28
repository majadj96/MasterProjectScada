using FrontEndProcessorService.ViewModel;
using ScadaCommon.ServiceContract;
using System;

namespace FrontEndProcessorService
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
