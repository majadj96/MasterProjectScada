using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCore.Interfaces
{
    public interface IMeasurementRepository
    {
        void Add(Measurement measurement);
        void AddMeasurements(Measurement []measurements);
        void GetAllMeasurementsByGid(long gid);
        void GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid);
    }
}
