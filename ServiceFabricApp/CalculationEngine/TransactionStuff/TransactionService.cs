using CalculationEngine.Channels;
using CalculationEngine.Model;
using Common.AlarmEvent;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class TransactionService : ITransactionSteps
    {
        private ProcessingData _processingData;
        private ConcreteModel Model;
        private IPub publisher;

        public TransactionService(ProcessingData processingData, ConcreteModel model, IPub pub)
        {
            _processingData = processingData;
            Model = model;
            this.publisher = pub;
        }

        public bool Prepare()
        {
            Console.WriteLine("CE Prepare");
            ServiceEventSource.Current.Message("CE Prepare");
            return true;
        }

        public bool Commit()
        {
            Console.WriteLine("CE Commit");
            ServiceEventSource.Current.Message("CE Commit");

            Model.BackupModel = new Dictionary<long, IdObject>(Model.CurrentModel);
            Model.CurrentModel = new Dictionary<long, IdObject>(Model.CurrentModel_Copy);
            Model.CurrentModel_Copy.Clear();

            SubscribeProxy sub = new SubscribeProxy(publisher);
            sub.Subscribe("scada");
            //_processingData.UpdateAsyncMachines();
            _processingData.IsModelChanged = true;

            CalcEngine.aTimer.Start();

            Task.Run(() =>
            {
                Event newEvent = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.CE, Message = "Integrity update complete" };
                EventProxy proxy = new EventProxy("EventServiceEndpoint");
                proxy.AddEvent(newEvent);
            });

            return true;
        }

        public void Rollback()
        {
            Console.WriteLine("CE Rollback");
            ServiceEventSource.Current.Message("CE Rollback");

            Model.CurrentModel = new Dictionary<long, IdObject>(Model.BackupModel);
            Model.CurrentModel_Copy.Clear();
        }
    }
}
