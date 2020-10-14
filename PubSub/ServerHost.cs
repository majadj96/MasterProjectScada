using PubSub.PubSubEngine;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSub
{
    public class ServerHost
    {
        ServiceHost _publishServiceHost = null;
        ServiceHost _subscribeServiceHost = null;
        public ServerHost()
        {
            HostPublishService();
            HostSubscriptionService();
        }

        #region Service Hosting


        public void HostSubscriptionService()
        {
            _subscribeServiceHost = new ServiceHost(typeof(Sub));
            NetTcpBinding tcpBinding = new NetTcpBinding();

            _subscribeServiceHost.AddServiceEndpoint(typeof(ISub), tcpBinding,
                                "net.tcp://localhost:7002/Sub");
            _subscribeServiceHost.Open();
        }

        private void HostPublishService()
        {
            _publishServiceHost = new ServiceHost(typeof(Pub));
            NetTcpBinding tcpBindingpublish = new NetTcpBinding();

            _publishServiceHost.AddServiceEndpoint(typeof(IPub), tcpBindingpublish,
                                    "net.tcp://localhost:7001/Pub");
            _publishServiceHost.Open();
        }
        #endregion



    }
}
