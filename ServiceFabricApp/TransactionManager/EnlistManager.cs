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
    public class EnlistManager : IEnlistManager
    {
        private IReliableStateManager StateManager;
        
        public EnlistManager(IReliableStateManager stateManager)
        {
            StateManager = stateManager;
        }

        public async void EndEnlist(bool isSuccessful)
        {
            try
            {
                var CurrentlyEnlistedServices = await StateManager.GetOrAddAsync<IReliableConcurrentQueue<ITransactionSteps>>("CurrentlyEnlistedServices");

                using (var tx = StateManager.CreateTransaction())
                {
                    ConditionalValue<ITransactionSteps> ret;
                    do
                    {
                        ret = await CurrentlyEnlistedServices.TryDequeueAsync(tx);

                        if (isSuccessful && ret.HasValue)
                        {
                            TMData.CompleteEnlistedServices.Add(ret.Value);
                        }

                    } while (CurrentlyEnlistedServices.Count > 0 && ret.HasValue);

                    await tx.CommitAsync();
                }

                if (!isSuccessful)
                {
                    return;
                }

                TransactionSteps.BeginTransaction();
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message($"Exception raised in EndEnlist method: {ex.Message}");
            }
            
        }

        public async void Enlist()
        {
            try
            {
                OperationContext context = OperationContext.Current;
                context.Channel.Closing += Channel_Closing;

                var callbackObj = context.GetCallbackChannel<ITransactionSteps>();
                var CurrentlyEnlistedServices = await StateManager.GetOrAddAsync<IReliableConcurrentQueue<ITransactionSteps>>("CurrentlyEnlistedServices");

                using (var tx = StateManager.CreateTransaction())
                {
                    await CurrentlyEnlistedServices.EnqueueAsync(tx, callbackObj);

                    await tx.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message($"Exception raised in Enlist method: {ex.Message}");
            }
            
        }

        private void Channel_Closing(object sender, EventArgs e)
        {
            try
            {
                var service = sender as ITransactionSteps;

                if (service != null)
                    TMData.CompleteEnlistedServices.Remove(service);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message($"Exception raised in Channel_Closing method: {ex.Message}");
            }
            
        }
    }
}
