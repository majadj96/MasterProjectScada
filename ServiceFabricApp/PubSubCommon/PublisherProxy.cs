using System.ServiceModel;

namespace PubSubCommon
{
    public class PublisherProxy : ClientBase<IPub>, IPub
    {
		public PublisherProxy(string endpoint) : base(endpoint)
        {

        }
		public void Publish(object data, string topicName)
		{
			Channel.Publish(data, topicName);
		}
	}
}
