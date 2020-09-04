using Common.GDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    [ServiceContract]
    public interface IGetResourceDescriptions
    {
        [OperationContract]
        List<ResourceDescription> GetResourceDescriptions();
    }
}
