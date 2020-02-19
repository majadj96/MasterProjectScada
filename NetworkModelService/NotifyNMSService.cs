using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace NetworkModelService
{
    public class NotifyNMSService : INotifyNMS
    {
        protected static NetworkModel _nm;
        private PubNMS _pub;

        public static NetworkModel NetworkModel
        {
            set
            {
                _nm = value;
            }
        }

        public NotifyNMSService()
        {
            _pub = new PubNMS();
        }

        public void UpdateUIModel(bool commitSucceed = true)
        {
            try
            {
                _pub.SendEvent(new PubSubCommon.NMSModel() { ResourceDescs = _nm.GetResourceDescriptions() }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
    }
}
