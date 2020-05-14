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

        public void SubscribeTo(string topic)
        {
            _proxy.Subscribe(topic);
        }

        public void UnsubscribeFrom(string topic)
        {
            _proxy.UnSubscribe(topic);
        }

        public void Publish(NMSModel model, string topicName)
        {
            throw new ActionNotSupportedException("CE does not have implementation for this method.");
        }

        public void PublishMeasure(ScadaUIExchangeModel[] measurement, string topicName)
        {
            _processingData.ProccessData(measurement);
        }

        public void PublishAlarm(AlarmDescription alarmDesc, string topicName)
        {
            throw new ActionNotSupportedException("CE does not have implementation for this method.");
        }

		public void PublishConnectionState(ConnectionState connectionState, string topicName)
		{
			throw new ActionNotSupportedException("CE does not have implementation for this method.");
		}
	}
}
