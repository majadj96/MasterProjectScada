using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NotifyNMSService
{
    public class Pub
    {
        //IPub _proxy;

        private IPub CreateProxy()
        {
            string endpointAddressString = "net.tcp://localhost:7001/Pub";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            return ChannelFactory<IPub>.CreateChannel(netTcpBinding, endpointAddress);
        }

        public void SendEvent(NMSModel model, EventArgs e)
        {
            IPub proxy = CreateProxy();

            proxy.Publish(model, "nms");

            //try
            //{
            //    model.EventData = "a";
            //    model.TopicName = "nms";
            //    _proxy.Publish(model, "nms");
            //    _eventCounter += 1;
            //}
            //catch(Exception ee) {
            //    Console.WriteLine(ee.Message);
            //}
        }
    }
}
