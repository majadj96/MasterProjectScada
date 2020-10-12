using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace PubSubService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class Sub : ISub
    {
        #region ISubscription Members

        public void Subscribe(string topicName)
        {
            IPub subscriber = OperationContext.Current.GetCallbackChannel<IPub>();
            Filter.AddSubscriber(topicName, subscriber);

            OperationContext.Current.Channel.Closing += Channel_Closing;

            MessageProperties mp = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty messageProperty = (RemoteEndpointMessageProperty)mp[RemoteEndpointMessageProperty.Name];

            string address = messageProperty.Address;
            int port = messageProperty.Port;


            Console.WriteLine("Endpoint address '{0}:{1}' subscribed to '{2}'", address, port, topicName);
            ServiceEventSource.Current.Message("Endpoint address '{0}:{1}' subscribed to '{2}'", address, port, topicName);

            //Notify NMS to send current model to UI
            if (topicName == "nms")
            {
                Task.Run(() =>
                {
                    INotifyNMS proxy = CreateNMSProxy();
                    proxy.UpdateUIModel();
                });
            }

            return;
        }

        private void Channel_Closing(object sender, EventArgs e)
        {
            IPub subscriber = sender as IPub;

            Filter.RemoveSubscriberAllTopics(subscriber);
        }

        private INotifyNMS CreateNMSProxy()
        {
            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:10010/NetworkModelService/NotifyNMS");
            ChannelFactory<INotifyNMS> channelFactory = new ChannelFactory<INotifyNMS>(netTcpbinding, endpointAddress);
            INotifyNMS proxy = channelFactory.CreateChannel();

            return proxy;
        }

        public void UnSubscribe(string topicName)
        {
            IPub subscriber = OperationContext.Current.GetCallbackChannel<IPub>();
            Filter.RemoveSubscriber(topicName, subscriber);

            Console.WriteLine($"Unsubscription for topic name: {topicName}");
        }

        #endregion
    }
}