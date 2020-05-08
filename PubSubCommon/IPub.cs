using Common.AlarmEvent;
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
        void Publish(NMSModel model, string topicName);
        [OperationContract(IsOneWay = true)]
        void PublishMeasure(ScadaUIExchangeModel []measurement, string topicName);
        [OperationContract(IsOneWay = true)]
        void PublishAlarm(AlarmDescription alarmDesc, string topicName);
    }
}
