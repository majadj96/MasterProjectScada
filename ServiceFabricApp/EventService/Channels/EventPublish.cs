using Common.AlarmEvent;
using PubSubCommon;
using System;
using System.ServiceModel;

namespace EventService.Channels
{
    public class EventPublish
    {
        private IPub CreateProxy()
        {
            string endpointAddressString = "net.tcp://localhost:7001/Pub";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            return ChannelFactory<IPub>.CreateChannel(netTcpBinding, endpointAddress);
        }

        public void SendEvent(Event newEvent, EventArgs e)
        {
            IPub proxy = CreateProxy();

            proxy.Publish(newEvent, "event");
        }
    }
}
