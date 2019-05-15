using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndProcessorService
{
    public class FrontEndProcessorService : IDisposable
    {
        private List<ServiceHost> hosts = null;
        private FieldCommunicationService fieldCommunicationService;
        private IFEPConfigService nDSConfigurationService;
        

        public FrontEndProcessorService()
        {
            fieldCommunicationService = new FieldCommunicationService();
            nDSConfigurationService = new NDSConfigurationService(StartFCS);
            InitializeHosts();
        }

        public void Start()
        {
            StartHosts();
        }

        private void StartFCS(Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> model)
        {
            fieldCommunicationService.StartService(model);
        }
        private void StartHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("Field Communication Services can not be opend because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Open();
            }
        }


        private void InitializeHosts()
        {
            hosts = new List<ServiceHost>();
            hosts.Add(new ServiceHost(fieldCommunicationService));
            hosts.Add(new ServiceHost(nDSConfigurationService));
        }

        public void CloseHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("Network Dynamic Services can not be closed because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }

        public void Dispose()
        {
            CloseHosts();
        }
    }
}
