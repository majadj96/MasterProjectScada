using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementRepositoryService
{
    public class MeasurementRepository : IMeasurementRepository
    {
        public void Add(Measurement measurement)
        {
            throw new NotImplementedException();
        }

        public void AddMeasurements(Measurement[] measurements)
        {
            throw new NotImplementedException();
        }

        public Measurement[] GetAllMeasurementsByGid(long gid)
        {
            throw new NotImplementedException();
        }

        public Measurement[] GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid)
        {
            throw new NotImplementedException();
        }
    }
}
