using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace TransactionManager
{
   public class EnlistManagerServiceHost 
    {
        ServiceHost _subscribeServiceHost = null;
        public EnlistManagerServiceHost()
        {
            _subscribeServiceHost = new ServiceHost(typeof(EnlistManager));
            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);

            _subscribeServiceHost.AddServiceEndpoint(typeof(IEnlistManager), tcpBinding,
                                "net.tcp://localhost:20000/TM");
            _subscribeServiceHost.Open();
        }
    }
}
