using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
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
            DuplexChannelFactory<ISub> channelFactory = new DuplexChannelFactory<ISub>(new InstanceContext(this), netTcpbinding, endpointAddress);
            _proxy = channelFactory.CreateChannel();
        }

        public void Publish(NMSModel model, string topicName)
        {
            if (model != null)
            {
                _eventCount += 1;
                NotificationMessage n = new NotificationMessage(null, model, topicName);
                Messenger.Default.Send<NotificationMessage>(n);
            }
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

        public void PublishMeasure(ScadaUIExchangeModel []measurement, string topicName)
        {
            Console.WriteLine("Stigao model....");
            //_eventCount += 1;
            //NotificationMessage n = new NotificationMessage(null, test, topicName);
            //Messenger.Default.Send<NotificationMessage>(n);

            //Ovde stizu merenje sa skade, izmenjen je contract odnosno interfejs kod ove metode....
        }
    }
}
