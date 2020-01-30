using System.Collections.Generic;
using System.ServiceModel;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BackEndPocessingModule : IBackEndProessingData
    {
        private IPointUpdateService pointUpdateProxy;
        public List<IProcessingData> ProcessingModules { get; set; }
        public BackEndPocessingModule(IPointUpdateService pointUpdateProxy)
        {
            InitializeProcessingModules();
            this.pointUpdateProxy = pointUpdateProxy;
        }

        public void Process(IProcessingObject[] inputObj)
        {
            for (int index = 0; index < inputObj.Length; index++)
            {
                foreach (var item in ProcessingModules)
                {
                    item.Process(inputObj[index]);
                }

                //this.pointUpdateProxy.UpdatePoint(inputObj[index]);
            }
        }

        private void InitializeProcessingModules()
        {
            this.ProcessingModules = new List<IProcessingData>();
            this.ProcessingModules.Add(new EGUModule());
            this.ProcessingModules.Add(new AlarmingModule());
        }
    }
}
