using System;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IStateUpdateService
    {
        [OperationContract]
        void UpdateState(ConnectionState connectionState);
    }
}
