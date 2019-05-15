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
                catch
                {

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
                catch
                {

                }

            }
        }
        #endregion
    }
}

