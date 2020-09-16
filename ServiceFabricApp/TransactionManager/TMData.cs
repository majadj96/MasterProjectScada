using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace TransactionManager
{
    public static class TMData
    {

        ////thread-safe list, jer vise servisa poziva enlist() u razlicito vreme i pristupa ovom objektu
        //public static SynchronizedCollection<ITransactionSteps> CurrentlyEnlistedServices = new SynchronizedCollection<ITransactionSteps>();

        public static IReliableConcurrentQueue<ITransactionSteps> CurrentlyEnlistedServices;
        public static List<ITransactionSteps> CompleteEnlistedServices = new List<ITransactionSteps>(5);

        public static INotifyNMS NotifyNMSProxy = null;

        public static void CreateNMSProxy()
        {
            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress = new EndpointAddress("net.tcp://localhost:10010/NetworkModelService/NotifyNMS");
            ChannelFactory<INotifyNMS> channelFactory = new ChannelFactory<INotifyNMS>(netTcpbinding, endpointAddress);
            NotifyNMSProxy = channelFactory.CreateChannel();
        }
    }
}
