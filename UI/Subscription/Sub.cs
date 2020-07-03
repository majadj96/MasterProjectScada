using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
using ScadaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Subscription
{
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
            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress = new EndpointAddress(EndpoindAddress);
            InstanceContext context = new InstanceContext(callbackinstance);
            DuplexChannelFactory<ISub> channelFactory = new DuplexChannelFactory<ISub>(context, netTcpbinding, endpointAddress);
            _proxy = channelFactory.CreateChannel();
        }

        public void OnSubscribe(string topic)
        {
            try
            {
                _proxy.Subscribe(topic);
            }
            catch
            {

            }
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
