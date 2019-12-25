using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NMS
{
    public class PubNMS
    {
        IPubNMS _proxy;
        int _eventCounter;

        public PubNMS()
        {
            CreateProxy();
            _eventCounter = 0;
        }
        private void CreateProxy()
        {
            string endpointAddressInString = "net.tcp://localhost:7001/PubNMS";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            _proxy = ChannelFactory<IPubNMS>.CreateChannel(netTcpBinding, endpointAddress);
        }

        public void SendEvent(String message, EventArgs e)
        {
            try
            {
                NMSModel model = new NMSModel();
                model.EventData = message;
                model.TopicName = "nms";
                _proxy.Publish(model, "nms");
                _eventCounter += 1;
            }
            catch { }
        }

        private NMSModel PrepareEvent()
        {
            NMSModel e = new NMSModel();
            e.TopicName = "nms";
            e.EventData = "p1";
            return e;
        }

        void OnResetCounter(object sender, EventArgs e)
        {
            _eventCounter = 0;
        }
    }
}