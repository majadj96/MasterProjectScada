using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using UserInterface.Helper;

namespace UserInterface.Subscription
{
    [Serializable]
    [DataContract]
    public class Sub : IPub
    {
        ISub _proxy;
        static int _eventCount;
        string _endpoint = string.Empty;

        public Sub()
        {
            _endpoint = "net.tcp://localhost:7002/Sub";
            MakeProxy(_endpoint, this);
            _eventCount = 0;
        }

        public void MakeProxy(string EndpoindAddress, object callbackinstance)
        {
            NetTcpBinding netTcpbinding = new NetTcpBinding();
            EndpointAddress endpointAddress = new EndpointAddress(EndpoindAddress);
            InstanceContext context = new InstanceContext(callbackinstance);
            DuplexChannelFactory<ISub> channelFactory = new DuplexChannelFactory<ISub>(context, netTcpbinding, endpointAddress);
            _proxy = channelFactory.CreateChannel();
        }

        public void OnSubscribe(string topic)
        {
            _proxy.Subscribe(topic);

            //=> RetryHelper.Retry(
            //        action: () =>
            //        {
            //            _proxy.Subscribe(topic);
            //        },
            //        retryCount: 30,
            //        delay: TimeSpan.FromSeconds(1))
            //    .GetAwaiter()
            //    .GetResult();
        }

        void OnUnSubscribe(object sender, EventArgs e, string topic)
        {
            _proxy.UnSubscribe(topic);
        }

		public void Publish(object data, string topicName)
		{
			if (data != null)
			{
				_eventCount += 1;
				NotificationMessage n = new NotificationMessage(null, data, topicName);
				Messenger.Default.Send<NotificationMessage>(n);
			}
		}
	}
}
