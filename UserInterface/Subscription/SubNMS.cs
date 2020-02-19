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
    public class SubNMS : IPubNMS
    {
        ISubNMS _proxy;
        static int _eventCount;
        string _endpoint = string.Empty;

        public SubNMS()
        {
            _endpoint = "net.tcp://localhost:7002/SubNMS";
            MakeProxy(_endpoint, this);
            _eventCount = 0;
        }

        public void MakeProxy(string EndpoindAddress, object callbackinstance)
        {
            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress = new EndpointAddress(EndpoindAddress);
            InstanceContext context = new InstanceContext(callbackinstance);
            DuplexChannelFactory<ISubNMS> channelFactory = new DuplexChannelFactory<ISubNMS>(new InstanceContext(this), netTcpbinding, endpointAddress);
            _proxy = channelFactory.CreateChannel();
        }

        public void Publish(NMSModel model, string topicName)
        {
            if (model != null)
            {
                _eventCount += 1;
                NotificationMessage n = new NotificationMessage(null, model, "model");
                Messenger.Default.Send<NotificationMessage>(n);
            }
        }

        public void OnSubscribe()
        {
            try
            {
                _proxy.Subscribe("nms");
            }
            catch
            {

            }
        }

        void OnUnSubscribe(object sender, EventArgs e)
        {
            _proxy.UnSubscribe("nms");
        }

    }
}
