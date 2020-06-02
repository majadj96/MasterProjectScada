using Common.AlarmEvent;
using PubSubCommon;
using ScadaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class PublishMeasurements : IPub
    {
        private static ProcessingData _processingData;
        private SubscribeProxy _proxy;

        public PublishMeasurements()
        {
            
        }

        public PublishMeasurements(ProcessingData processingData)
        {
            _processingData = processingData;
            _proxy = new SubscribeProxy(this);
        }

		public void Publish(object data, string topicName)
		{
			_processingData.ProccessData(data);
		}

		public void SubscribeTo(string topic)
        {
            _proxy.Subscribe(topic);
        }

        public void UnsubscribeFrom(string topic)
        {
            _proxy.UnSubscribe(topic);
        }
    }
}
