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
        public void EndEnlist(bool isSuccessful)
        {
            if (!isSuccessful)
            {                
                TMData.CurrentlyEnlistedServices = new SynchronizedCollection<ITransactionSteps>();
                return;
            }
            TMData.CompleteEnlistedServices = new List<ITransactionSteps>(TMData.CurrentlyEnlistedServices);
            TMData.CurrentlyEnlistedServices = new SynchronizedCollection<ITransactionSteps>();

            TransactionSteps.BeginTransaction();
        }

        public void Enlist()
        {
            OperationContext context = OperationContext.Current;            

            var service = context.GetCallbackChannel<ITransactionSteps>();
            TMData.CurrentlyEnlistedServices.Add(service);

            //AKO try-catch radi posao u slucaju diskonektovanja pozivajuceg servisa, onda zanemari ovaj deo sa izbacivanjem iz liste

            context.Channel.Closing += Channel_Closing;
        }

        private void Channel_Closing(object sender, EventArgs e)
        {
            var service = sender as ITransactionSteps;

            if (service != null)
                TMData.CompleteEnlistedServices.Remove(service);
        }
    }
}
