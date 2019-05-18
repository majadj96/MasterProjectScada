﻿using System;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IPointUpdateService
    {
        [OperationContract]
        void UpdateState(ConnectionState connectionState);

        [OperationContract]
        void UpdateDateAndTime(DateTime dateTime);
    }
}
