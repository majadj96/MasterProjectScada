using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS
{
    class Program
    {
        static void Main(string[] args)
        {
            PubNMS pub = new PubNMS();
            while(true)
            {
                String message = Console.ReadLine();
                if (message.Contains("a"))
                {
                    pub.SendEvent(message, null);
                }
            }

        }
    }
}
