using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManagerContracts
{
    public class TMProxy
    {
        IEnlistManager _proxy;
        public TMProxy(ITransactionSteps callbackObject)
        {
            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:20000/TM");
            InstanceContext context = new InstanceContext(callbackObject);
            DuplexChannelFactory<IEnlistManager> channelFactory = new DuplexChannelFactory<IEnlistManager>(context, netTcpbinding, endpointAddress);
            _proxy = channelFactory.CreateChannel();
        }

        public void EndEnlist(bool v)
        {
            _proxy.EndEnlist(v);
        }

        public void Enlist()
        {
            _proxy.Enlist();
        }
    }
}
