using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PubSubService
{
    public class Filter
    {
        static Dictionary<string, List<IPub>> _subscribersList = new Dictionary<string, List<IPub>>();
        static object lock_object = new object();
        static public Dictionary<string, List<IPub>> SubscribersList
        {
            get
            {
                //lock (lock_object)
                //{

                //}
                return _subscribersList;
            }

        }

        static public List<IPub> GetSubscribers(String topicName)
        {
            //lock (lock_object)
            //{

            //}
            if (SubscribersList.ContainsKey(topicName))
            {
                return SubscribersList[topicName];
            }
            else
                return null;
        }

        static public void AddSubscriber(String topicName, IPub subscriberCallbackReference)
        {
            //lock (lock_object)
            //{

            //}

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

        internal static void RemoveSubscriberAllTopics(IPub subscriber)
        {
            foreach (List<IPub> subs in SubscribersList.Values)
            {
                subs.Remove(subscriber);
            }
        }

        static public void RemoveSubscriber(String topicName, IPub subscriberCallbackReference)
        {
            //lock (lock_object)
            //{

            //}
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