﻿using System;
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
using ScadaCommon.ServiceContract;
using ScadaCommon.ServiceProxies;

namespace DynamicDataProccessorService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class DynamicDataProccessorService : StatelessService
    {
        private IProcessingServiceContract processingService;
        private IAlarmService alarmServiceProxy;
        private IPub publisherProxy;
        private IMeasurementRepository measurementRepository;
        private IBackendProcessor backEndProcessor;
        public DynamicDataProccessorService(StatelessServiceContext context)
            : base(context)
        {
            alarmServiceProxy = new AlarmServiceProxy("AlarmServiceEndPoint");
            measurementRepository = new MeasurementRepositoryProxy("MeasurementServiceProxy");
            publisherProxy = new PublisherProxy("PublisherEndPoint");

            //PointUpdate = null ---> ne koristi se nigde, ali ga ne brisemo da ne bi bilo gresaka u drugom(izvornom) solutionu...
            backEndProcessor = new BackEndPocessingModule(null, alarmServiceProxy, publisherProxy, measurementRepository);

            processingService = new ProccessingService(backEndProcessor);
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

                    EndpointResourceDescription endpointConfig = context.CodePackageActivationContext.GetEndpoint("ProcessingService");
                    int port = endpointConfig.Port;
                    string scheme = endpointConfig.Protocol.ToString();
                    string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/IProcessingServiceContract", "net.tcp", host, port);

                    var listener = new WcfCommunicationListener<IProcessingServiceContract>(
                        wcfServiceObject: this.processingService,
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
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
