using System;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IStateUpdateService
    {
        [OperationContract]
        void UpdateState(short connectionState);

        [OperationContract]
        void UpdateDateAndTime(DateTime dateTime);
    }
}
