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
        private ProcessingData ProcessingData;

        public PublishMeasurements(ProcessingData processingData)
        {
            ProcessingData = processingData;
        }

        public void Publish(object data, string topicName)
        {
            ProcessingData.ProccessData(data);
        }
    }
}
