using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.PubSubEngine
{
    class Filter
    {
        static Dictionary<string, List<IPubNMS>> _subscribersList = new Dictionary<string, List<IPubNMS>>();
        static public Dictionary<string, List<IPubNMS>> SubscribersList
        {
            get
            {
                lock (typeof(Filter))
                {
                    return _subscribersList;
                }
            }

        }

        static public List<IPubNMS> GetSubscribers(String topicName)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    return SubscribersList[topicName];
                }
                else
                    return null;
            }
        }

        static public void AddSubscriber(String topicName, IPubNMS subscriberCallbackReference)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    if (!SubscribersList[topicName].Contains(subscriberCallbackReference))
                    {
                        SubscribersList[topicName].Add(subscriberCallbackReference);
                    }
                }
                else
                {
                    List<IPubNMS> newSubscribersList = new List<IPubNMS>();
                    newSubscribersList.Add(subscriberCallbackReference);
                    SubscribersList.Add(topicName, newSubscribersList);
                }
            }

        }

        static public void RemoveSubscriber(String topicName, IPubNMS subscriberCallbackReference)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    if (SubscribersList[topicName].Contains(subscriberCallbackReference))
                    {
                        SubscribersList[topicName].Remove(subscriberCallbackReference);
                    }
                }
            }
        }
    }
}