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
    public class PubNMS : IPubNMS
    {
        #region IPublishing Members
        public void Publish(NMSModel e, string topicName)
        {
            List<IPubNMS> subscribers = Filter.GetSubscribers(topicName);
            if (subscribers == null) return;

            Type type = typeof(IPubNMS);
            MethodInfo publishMethodInfo = type.GetMethod("Publish");

            foreach (IPubNMS subscriber in subscribers)
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
        #endregion
    }
}

