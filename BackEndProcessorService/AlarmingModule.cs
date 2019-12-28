using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaCommon.BackEnd_FrontEnd;

namespace BackEndProcessorService
{
    public class AlarmingModule : IProcessingData
    {
        public void Process(IProcessingObject processingObject)
        {

            Console.WriteLine("Alarmi rade!");
        }
    }
}
