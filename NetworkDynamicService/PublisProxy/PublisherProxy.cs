using Common.AlarmEvent;
using PubSubCommon;
using ScadaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
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
