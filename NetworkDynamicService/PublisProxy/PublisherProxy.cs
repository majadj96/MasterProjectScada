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

        public void Publish(NMSModel model, string topicName)
        {
            throw new NotImplementedException();
        }

        public void PublishAlarm(AlarmDescription alarmDesc, string topicName)
        {
            throw new NotImplementedException();
        }

        public void PublishMeasure(ScadaUIExchangeModel []measurement, string topicName)
        {
            Channel.PublishMeasure(measurement, topicName);
        }

		public void PublishConnectionState(ConnectionState connectionState, string topicName)
		{
			Channel.PublishConnectionState(connectionState, topicName);
		}

        public void PublishEvent(Event eventObject, string topicName)
        {
            throw new NotImplementedException();
        }
    }
}
