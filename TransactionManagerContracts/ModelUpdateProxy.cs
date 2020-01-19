using Common.GDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManagerContracts
{
    public class ModelUpdateProxy : ClientBase<IModelUpdateContract>, IModelUpdateContract
    {
        public ModelUpdateProxy(string endpointName)
            : base(endpointName)
        {
        }

        public UpdateResult UpdateModel(Delta delta)
        {
            return Channel.UpdateModel(delta);
        }
    }
}
