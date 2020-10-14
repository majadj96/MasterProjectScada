using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCore.Interfaces
{
    [ServiceContract]
    public interface IMeasurementRepository
    {
        [OperationContract]
        void Add(Measurement measurement);
        [OperationContract]
        void AddMeasurements(Measurement []measurements);
        [OperationContract]
        Measurement[] GetAllMeasurementsByGid(long gid);
        [OperationContract]
        Measurement[] GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid);
    }
}
