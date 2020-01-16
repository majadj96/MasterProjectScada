using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.PubSubEngine
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class SubNMS : ISubNMS
    {
        #region ISubscription Members

        public void Subscribe(string topicName)
        {
            Console.WriteLine("Sab se lepo");
            IPubNMS subscriber = OperationContext.Current.GetCallbackChannel<IPubNMS>();
            Filter.AddSubscriber(topicName, subscriber);
        }

        public void UnSubscribe(string topicName)
        {
            IPubNMS subscriber = OperationContext.Current.GetCallbackChannel<IPubNMS>();
            Filter.RemoveSubscriber(topicName, subscriber);
        }

        #endregion
    }
}