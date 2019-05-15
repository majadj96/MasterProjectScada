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
            Console.Title = "PubSub";

            ServerHost serverHost = new ServerHost();
            Console.WriteLine("Hosts are opened");
            Console.ReadLine();
        }
    }
}
