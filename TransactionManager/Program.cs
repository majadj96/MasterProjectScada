﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManager
{
    class Program
    {
       static void Main(string[] args)
        {
            EnlistManagerServiceHost host = new EnlistManagerServiceHost();
            TMData.CreateNMSProxy();

            Console.ReadLine();
        }
    }
}
