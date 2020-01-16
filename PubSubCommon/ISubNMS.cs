using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubCommon
{
    [ServiceContract(CallbackContract = typeof(IPubNMS))]
    public interface ISubNMS
    {
        [OperationContract]
        void Subscribe(string topicName);

        [OperationContract]
        void UnSubscribe(string topicName);
    }
}
