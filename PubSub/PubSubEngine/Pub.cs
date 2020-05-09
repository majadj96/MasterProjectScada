using Common.AlarmEvent;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.PubSubEngine
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Pub : IPub
    {
        #region IPublishing Members
        public void Publish(NMSModel e, string topicName)
        {
            List<IPub> subscribers = Filter.GetSubscribers(topicName);
            if (subscribers == null) return;

            Type type = typeof(IPub);
            MethodInfo publishMethodInfo = type.GetMethod("Publish");

            foreach (IPub subscriber in subscribers)
            {
                try
                {
                    publishMethodInfo.Invoke(subscriber, new object[] { e, topicName });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not invoke Publish method. {0}", ex.Message);
                }
            }
        }

        public void PublishAlarm(AlarmDescription alarmDesc, string topicName)
        {
            List<IPub> subscribers = Filter.GetSubscribers(topicName);
            if (subscribers == null) return;

            Type type = typeof(IPub);
            MethodInfo publishMethodInfo = type.GetMethod("PublishAlarm");

            foreach (IPub subscriber in subscribers)
            {
                try
                {
                    publishMethodInfo.Invoke(subscriber, new object[] { alarmDesc, topicName });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not invoke PublishAlarm method. {0}", ex.Message);
                }
            }
        }

        public void PublishMeasure(ScadaUIExchangeModel []measurement, string topicName)
        {
            List<IPub> subscribers = Filter.GetSubscribers(topicName);
            if (subscribers == null) return;

            Type type = typeof(IPub);
            MethodInfo publishMethodInfo = type.GetMethod("PublishMeasure");

            foreach (IPub subscriber in subscribers)
            {
                try
                {
                    publishMethodInfo.Invoke(subscriber, new object[] { measurement, topicName });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not invoke PublishMeasure method. {0}", ex.Message);
                }
            }
        }
        #endregion
    }
}

