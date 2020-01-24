using NMS;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;
using UserInterface.Subscription;

namespace PubSub
{
    class Program
    { 
        static void Main(string[] args)
        {
            ServerHost serverHost = new ServerHost();
            Console.WriteLine("Hosts are opened");

            //PubSub ovde poziva CE i simulira poziv koji treba da dodje od strane NMS-a (TEST, OBRISATI)

            //ModelUpdateProxy proxy = new ModelUpdateProxy("net.tcp://localhost:50000/CE");
            //proxy.UpdateModel(new Common.GDA.Delta());
            //NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            //EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:10000/CE");
            //InstanceContext context = new InstanceContext(callbackinstance);
            //ChannelFactory<IModelUpdateContract> channelFactory = new ChannelFactory<IModelUpdateContract>(netTcpbinding, endpointAddress);
            //var _proxy = channelFactory.CreateChannel();
            //_proxy.UpdateModel(new Common.GDA.Delta());

            Console.ReadLine();
        }
    }
}
