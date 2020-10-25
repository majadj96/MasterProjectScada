using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.GDA;
using DataModel;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TransactionManagerContracts;

namespace NetworkModelService
{
	/// <summary>
	/// An instance of this class is created for each service replica by the Service Fabric runtime.
	/// </summary>
	internal sealed class NetworkModelService : StatefulService, IModelUpdateContract
	{
        private NetworkModel nm = null;
		public NetworkModelService(StatefulServiceContext context)
			: base(context)
		{
            nm = new NetworkModel(this.StateManager);
        }

        public UpdateResult UpdateModel(Delta delta)
        {
            return nm.UpdateModel(delta);
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
                    string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/ModelUpdateNMS", "net.tcp", host, port);

                    var listener = new WcfCommunicationListener<IModelUpdateContract>(
                        wcfServiceObject: this,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(SecurityMode.None),
                        address: new EndpointAddress(uri)
                    );
                    return listener;
                }, name: "ModelUpdateNMS"),
                new ServiceReplicaListener((context) =>
                {
                    string host = host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpointConfig = context.CodePackageActivationContext.GetEndpoint("GetResourceDescsEndpoint");
                    int port = endpointConfig.Port;
                    string scheme = endpointConfig.Protocol.ToString();
                    string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/GetResourceDescs", "net.tcp", host, port);

                    var listener = new WcfCommunicationListener<IGetResourceDescriptions>(
                        wcfServiceObject: this.nm,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(SecurityMode.None),
                        address: new EndpointAddress(uri)
                    );
                    return listener;
                }, name : "GetResourceDescs")
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

			foreach (var item in this.GetAddresses())
			{
				ServiceEventSource.Current.Message("Service opened: " + item.Key + item.Value);
			}

			var m = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModel");
			var m2 = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModelCopy");
			var m3 = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModelOld");

			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();

                if(nm.commitFinished)
                {
                    m = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModel");
                    m2 = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModelCopy");
                    m3 = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModelOld");

                    using (var tx = this.StateManager.CreateTransaction())
                    {
                        var enumerable = await m.CreateEnumerableAsync(tx);
                        var asyncEnumerator = enumerable.GetAsyncEnumerator();

                        while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                        {
                            KeyValuePair<short, Container> item = asyncEnumerator.Current;
                        }
                    }
                }

				await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
			}
		}
	}
}
