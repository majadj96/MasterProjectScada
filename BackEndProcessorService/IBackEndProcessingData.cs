using ScadaCommon.BackEnd_FrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndProcessorService
{
    public interface IBackEndProcessingData
    {
        void Process(IProcessingObject processingObject);
    }
}
