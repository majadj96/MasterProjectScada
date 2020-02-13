using System.Collections.Generic;
using System.ServiceModel;
using BackEndProcessorService.Proxy;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BackEndPocessingModule : IBackEndProessingData
    {
        private IPointUpdateService pointUpdateProxy;
        private AlarmEventServiceProxy alarmEventServiceProxy;
        public List<IProcessingData> ProcessingModules { get; set; }
        public BackEndPocessingModule(IPointUpdateService pointUpdateProxy, AlarmEventServiceProxy alarmEventServiceProxy)
        {
            this.alarmEventServiceProxy = alarmEventServiceProxy;
            InitializeProcessingModules();
            this.pointUpdateProxy = pointUpdateProxy;
        }

        public void Process(ProcessingObject[] inputObj)
        {
            for (int index = 0; index < inputObj.Length; index++)
            {
                foreach (var item in ProcessingModules)
                {
                    item.Process(inputObj);
                }
            }

            this.pointUpdateProxy.UpdatePoint(inputObj);
        }

        private void InitializeProcessingModules()
        {
            this.ProcessingModules = new List<IProcessingData>();
            this.ProcessingModules.Add(new EGUModule());
            this.ProcessingModules.Add(new AlarmingModule(this.alarmEventServiceProxy));
        }
    }
}
