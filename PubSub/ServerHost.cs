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
            _subscribeServiceHost = new ServiceHost(typeof(SubNMS));
            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);

            _subscribeServiceHost.AddServiceEndpoint(typeof(ISubNMS), tcpBinding,
                                "net.tcp://localhost:7002/SubNMS");
            _subscribeServiceHost.Open();
        }

        private void HostPublishService()
        {
            _publishServiceHost = new ServiceHost(typeof(PubNMS));
            NetTcpBinding tcpBindingpublish = new NetTcpBinding();

            _publishServiceHost.AddServiceEndpoint(typeof(IPubNMS), tcpBindingpublish,
                                    "net.tcp://localhost:7001/PubNMS");
            _publishServiceHost.Open();
        }
        #endregion



    }
}
