using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace TransactionManager
{
    //proveri ove parametre (per session, reentrant...)
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                 ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class EnlistManager : IEnlistManager
    {
        private IReliableStateManager StateManager;

        //razmotri da vratis ovo u TMData pa nek bude pristupanje kao ranije (pristupa se odavde i iz TransationSteps)
        //private IReliableConcurrentQueue<ITransactionSteps> CurrentlyEnlistedServices;
        //private List<ITransactionSteps> CompleteEnlistedServices;
        
        public EnlistManager(IReliableStateManager stateManager)
        {
            StateManager = stateManager;
            //CompleteEnlistedServices = new List<ITransactionSteps>(5);

            //mislim da je dovoljno samo u konstruktoru preuzeti Queue
            //besides, queue already gets created in Service RunAsync method, so this shouldn't take long to fetch it
            TMData.CurrentlyEnlistedServices = Task.Run(async() => await StateManager.GetOrAddAsync<IReliableConcurrentQueue<ITransactionSteps>>("CurrentlyEnlistedServices")).Result;
        }

        public void EndEnlist(bool isSuccessful)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                ConditionalValue<ITransactionSteps> ret;
                do
                {
                    ret = Task.Run(async () => await TMData.CurrentlyEnlistedServices.TryDequeueAsync(tx)).Result;

                    if (isSuccessful && ret.HasValue)
                    {
                        TMData.CompleteEnlistedServices.Add(ret.Value);
                    }

                } while (TMData.CurrentlyEnlistedServices.Count > 0 && ret.HasValue);

                tx.CommitAsync();
            }

            if (!isSuccessful)
            {
                return;
            }
                
            TransactionSteps.BeginTransaction();
        }

        public void Enlist()
        {
            OperationContext context = OperationContext.Current;
            context.Channel.Closing += Channel_Closing;

            var callbackObj = context.GetCallbackChannel<ITransactionSteps>();

            using (var tx = StateManager.CreateTransaction())
            {
                TMData.CurrentlyEnlistedServices.EnqueueAsync(tx, callbackObj);
                
                tx.CommitAsync();
            }  
        }

        private void Channel_Closing(object sender, EventArgs e)
        {
            var service = sender as ITransactionSteps;

            if (service != null)
                TMData.CompleteEnlistedServices.Remove(service);
        }
    }
}
