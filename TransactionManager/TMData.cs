using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace TransactionManager
{
    public static class TMData
    {
        //thread-safe list, jer vise servisa poziva enlist() u razlicito vreme i pristupa ovom objektu
        public static SynchronizedCollection<ITransactionSteps> CurrentlyEnlistedServices = new SynchronizedCollection<ITransactionSteps>();

        public static List<ITransactionSteps> CompleteEnlistedServices = new List<ITransactionSteps>();

    }
}
