using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService.DeltaDB.Interfaces
{
    [ServiceContract]
    public interface IDeltaRepository
    {
        [OperationContract]
        void Add(DeltaDBModel delta);

        [OperationContract]
        List<DeltaDBModel> GetAllDeltas();

        [OperationContract]
        int GetNumberOfDeltas();
    }
}
