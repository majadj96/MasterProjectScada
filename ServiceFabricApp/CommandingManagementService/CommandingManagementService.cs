using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using BackEndProcessorService;
using BackEndProcessorService.Proxy;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using PubSubCommon;
using RepositoryCore.Interfaces;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;
using ScadaCommon.ServiceProxies;

namespace CommandingManagementService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CommandingManagementService : StatelessService
    {
        private ICommandingServiceContract commandService;
        private IFEPCommandingServiceContract fepCmdProxy;
        private IRealTimeCacheService realTimeCacheProxy;
        private IBackendProcessor backEndProcessor;

        private IAlarmService alarmServiceProxy;
        private IPub publisherProxy;
        private IMeasurementRepository measurementRepository;

        public CommandingManagementService(StatelessServiceContext context)
            : base(context)
        {
            alarmServiceProxy = new AlarmServiceProxy("AlarmServiceEndPoint");
            measurementRepository = new MeasurementRepositoryProxy("MeasurementServiceProxy");
            publisherProxy = new PublisherProxy("PublisherEndPoint");
            fepCmdProxy = new FepCommandingServiceProxy("FEPCommandingService");

            //PointUpdate = null ---> ne koristi se nigde, ali ga ne brisemo da ne bi bilo gresaka u drugom(izvornom) solutionu...
            backEndProcessor = new BackEndPocessingModule(null, alarmServiceProxy, publisherProxy, measurementRepository);
            realTimeCacheProxy = new RealTimeCacheServiceProxy("RealTimeCacheProxy");

            commandService = new CommandingService(fepCmdProxy, backEndProcessor, realTimeCacheProxy);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener((context) =>
                {
                    string host = host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpointConfig = context.CodePackageActivationContext.GetEndpoint("CommandingService");
                    int port = endpointConfig.Port;
                    string scheme = endpointConfig.Protocol.ToString();
                    string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/ICommandingServiceContract", "net.tcp", host, port);

                    var listener = new WcfCommunicationListener<ICommandingServiceContract>(
                        wcfServiceObject: this.commandService,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                    );
                    return listener;
                })
            };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
			foreach (var item in this.GetAddresses())
			{
				ServiceEventSource.Current.Message("Service opened: " + item.Key + item.Value);
			}

			while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
