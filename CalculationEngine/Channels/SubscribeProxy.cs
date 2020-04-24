using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
	public class SubscribeProxy
	{
		private readonly string _endpoint = "net.tcp://localhost:7002/Sub";
		private ISub _proxy;

		public SubscribeProxy(IPub callback)
		{
			NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
			EndpointAddress endpointAddress = new EndpointAddress(_endpoint);
			InstanceContext context = new InstanceContext(callback);
			DuplexChannelFactory<ISub> channelFactory = new DuplexChannelFactory<ISub>(context, netTcpbinding, endpointAddress);
			_proxy = channelFactory.CreateChannel();
		}

		public void Subscribe(string topicName)
		{
			try
			{
				_proxy.Subscribe(topicName);
			}
			catch
			{
				Console.WriteLine("Subscription to '{0}' failed.", topicName);
			}
		}

		public void UnSubscribe(string topicName)
		{
			try
			{
				_proxy.UnSubscribe(topicName);
			}
			catch
			{
				Console.WriteLine("Unsubscribe from '{0}' failed.", topicName);
			}
		}
	}
}
