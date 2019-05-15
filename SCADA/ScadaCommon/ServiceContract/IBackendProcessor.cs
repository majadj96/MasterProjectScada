using ScadaCommon.BackEnd_FrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceContract
{
    public interface IBackendProcessor
    {
        void Process(ProcessingObject[] inputObj);
        void CommandingProcess(ProcessingObject[] inputObj);
    }
}
