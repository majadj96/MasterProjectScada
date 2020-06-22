using System;

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
