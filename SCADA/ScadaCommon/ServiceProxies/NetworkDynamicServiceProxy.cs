﻿using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;
using System.ServiceModel;

namespace ScadaCommon.ServiceProxies
{
    public class NetworkDynamicServiceProxy : ClientBase<IBackEndProessingData>, IBackEndProessingData
    {
        public NetworkDynamicServiceProxy(string endpointName) : base(endpointName)
        {

        }

        public void Process(IProcessingObject[] inputObj)
        {
            Channel.Process(inputObj);
        }
    }
}
