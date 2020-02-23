using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestScada1
{
    public class PubSCADA
    {
        IPub _proxy;
        int _eventCounter;

        public PubSCADA()
        {
            CreateProxy();
            _eventCounter = 0;
        }
        private void CreateProxy()
        {
            string endpointAddressInString = "net.tcp://localhost:7001/Pub";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressInString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            _proxy = ChannelFactory<IPub>.CreateChannel(netTcpBinding, endpointAddress);
        }

        public void SendEvent(string model, EventArgs e)
        {
            try
            {  
                _proxy.PublishMeasure(model, "scada");
                _eventCounter += 1;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        private NMSModel PrepareEvent()
        {
            NMSModel e = new NMSModel();
            e.TopicName = "scada";
            e.EventData = "p1";
            return e;
        }

        void OnResetCounter(object sender, EventArgs e)
        {
            _eventCounter = 0;
        }
    }
}
