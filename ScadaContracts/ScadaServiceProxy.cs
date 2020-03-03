using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaContracts
{
    public class ScadaServiceProxy : ClientBase<IScadaService>, IScadaService
    {
        public ScadaServiceProxy(string endpointName)
            : base(endpointName)
        {
        }

        public void CollectData(object data)
        {
            throw new NotImplementedException();
        }
    }
}
