﻿using ScadaCommon;
using ScadaCommon.ServiceContract;
using System;
using System.ServiceModel;

namespace NetworkDynamicService
{
    // Proxy for communication with UI
    public class StateUpdateServiceProxy : ClientBase<IStateUpdateService>, IStateUpdateService
    {
        public StateUpdateServiceProxy(string endpointName) : base(endpointName)
        {

        }

        public void UpdateState(ConnectionState connectionState)
        {
            Channel.UpdateState(connectionState);
        }
    }
}
