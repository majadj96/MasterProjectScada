using System.ServiceModel;

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
