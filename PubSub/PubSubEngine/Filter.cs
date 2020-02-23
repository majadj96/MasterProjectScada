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
        static Dictionary<string, List<IPub>> _subscribersList = new Dictionary<string, List<IPub>>();
        static public Dictionary<string, List<IPub>> SubscribersList
        {
            get
            {
                lock (typeof(Filter))
                {
                    return _subscribersList;
                }
            }

        }

        static public List<IPub> GetSubscribers(String topicName)
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

        static public void AddSubscriber(String topicName, IPub subscriberCallbackReference)
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
                    List<IPub> newSubscribersList = new List<IPub>();
                    newSubscribersList.Add(subscriberCallbackReference);
                    SubscribersList.Add(topicName, newSubscribersList);
                }
            }

        }

        static public void RemoveSubscriber(String topicName, IPub subscriberCallbackReference)
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