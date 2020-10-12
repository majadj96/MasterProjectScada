﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using CalculationEngine.Model;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TransactionManagerContracts;

namespace CalculationEngine
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class CalculationEngine : StatefulService
    {
        private ModelUpdateContract modelUpdateService;
        public CalculationEngine(StatefulServiceContext context)
            : base(context)
        {
            ConcreteModel model = new ConcreteModel();

            int config = LoadConfiguration();
            ProcessingData processingData = new ProcessingData(model, config);
            PublishMeasurements pub = new PublishMeasurements(processingData);

            TransactionService transactionService = new TransactionService(processingData, model, pub);
            this.modelUpdateService = new ModelUpdateContract(model, transactionService);

            
            //SubscribeProxy sub = new SubscribeProxy(pub);
            //sub.Subscribe("scada");

            CalcEngine engine = new CalcEngine(processingData);
        }

        private int LoadConfiguration()
        {
            string configString = ConfigurationManager.AppSettings.Get("MaxDifference");
            return int.Parse(configString);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[] { new ServiceReplicaListener((context) =>
                {
                    string host = host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpointConfig = context.CodePackageActivationContext.GetEndpoint("ModelUpdateEndpoint");
                    int port = endpointConfig.Port;
                    string scheme = endpointConfig.Protocol.ToString();
                    string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/CE", "net.tcp", host, port);

                    var listener = new WcfCommunicationListener<IModelUpdateContract>(
                        wcfServiceObject: this.modelUpdateService,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(SecurityMode.None),
                        address: new EndpointAddress(uri)
                    );
                    return listener;
                })
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
