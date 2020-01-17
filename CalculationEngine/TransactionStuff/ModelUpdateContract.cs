using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.GDA;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class ModelUpdateContract : IModelUpdateContract
    {
        public UpdateResult UpdateModel(Delta delta)
        {
            Console.WriteLine("Update model invoked");

            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:20000/TM");
            InstanceContext context = new InstanceContext(new TransactionService());
            DuplexChannelFactory<IEnlistManager> channelFactory = new DuplexChannelFactory<IEnlistManager>(context, netTcpbinding, endpointAddress);
            var _proxy = channelFactory.CreateChannel();
            _proxy.Enlist();

            _proxy.EndEnlist(true);

            return new UpdateResult();
        }
    }
}
