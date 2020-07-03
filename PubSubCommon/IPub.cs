using Common.AlarmEvent;
using ScadaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubCommon
{
    [ServiceContract]
    public interface IPub
    {
		[OperationContract(IsOneWay = true)]
		[ServiceKnownType("GetKnownTypes", typeof(ObjectTypeHelper))]
		void Publish(object data, string topicName);
    }
}
