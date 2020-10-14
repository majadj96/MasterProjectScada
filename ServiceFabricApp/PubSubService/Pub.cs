using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Pub : IPub
    {
		#region IPublishing Members

		public void Publish(object data, string topicName)
		{
            var subscribers = Filter.GetSubscribers(topicName);
			if (subscribers == null) return;

			Type type = typeof(IPub);
			MethodInfo publishMethodInfo = type.GetMethod("Publish");

			foreach (IPub subscriber in subscribers)
			{
				try
				{
                    //publishMethodInfo.Invoke(subscriber, new object[] { data, topicName });
                    subscriber.Publish(data, topicName);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Could not invoke TryToPublish method. {0}", ex.Message);
                    ServiceEventSource.Current.Message("Could not invoke TryToPublish method. {0}", ex.Message);
				}
			}
		}
		
        #endregion
    }
}

