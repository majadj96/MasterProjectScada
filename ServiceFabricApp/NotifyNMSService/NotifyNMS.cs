using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace NotifyNMSService
{
    public class NotifyNMS : INotifyNMS
    {
        private Pub _pub;

        public NotifyNMS()
        {
            _pub = new Pub();
        }

        public void UpdateUIModel(bool commitSucceed = true)
        {
            try
            {
                IGetResourceDescriptions proxy = new ChannelFactory<IGetResourceDescriptions>(new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://localhost:10009/GetResourceDescs")).CreateChannel();

                _pub.SendEvent(new PubSubCommon.NMSModel() { ResourceDescs = proxy.GetResourceDescriptions() }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
