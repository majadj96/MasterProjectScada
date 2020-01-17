using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class UpdateModelServiceHost
    {
        ServiceHost _subscribeServiceHost = null;
        public UpdateModelServiceHost()
        {
            _subscribeServiceHost = new ServiceHost(typeof(ModelUpdateContract));
            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);

            _subscribeServiceHost.AddServiceEndpoint(typeof(IModelUpdateContract), tcpBinding,
                                "net.tcp://localhost:10000/CE");
            _subscribeServiceHost.Open();
        }
    }
}
