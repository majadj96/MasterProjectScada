﻿using ScadaCommon.BackEnd_FrontEnd;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IBackEndProessingData
    {
        [OperationContract]
        void Process(IProcessingObject processingObject);
    }
}
