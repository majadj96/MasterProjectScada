﻿using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService
{
    public class Pub
    {
        IPub _proxy;
        int _eventCounter;

        public Pub()
        {
            try
            {
                CreateProxy();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            _eventCounter = 0;
        }
        private void CreateProxy()
        {
            string endpointAddressString = "net.tcp://localhost:7001/Pub";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            _proxy = ChannelFactory<IPub>.CreateChannel(netTcpBinding, endpointAddress);
        }

        public void SendEvent(NMSModel model, EventArgs e)
        {
            try
            {
                model.EventData = "a";
                model.TopicName = "nms";
                _proxy.Publish(model, "nms");
                _eventCounter += 1;
            }
            catch(Exception ee) {
                Console.WriteLine(ee.Message);
            }
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
