using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScada1
{
    class Program
    {
        static void Main(string[] args)
        {
            //PubSCADA pub = new PubSCADA();
            while (true)
            {
                //Kada se u konzolu unese slovo a - imitira se Publish metoda (PublishMeasure)
                String message = Console.ReadLine();
                if (message.Contains("a"))
                    pub.SendEvent(message, null);
            }
        }
    }
}
