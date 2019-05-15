using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMeasurementInfrastructure
{
    public class MeasurementRepository : IMeasurementRepository
    {
        MeasurementContext context = new MeasurementContext();
             
        public void Add(Measurement measurement)
        {
            context.Measurements.Add(measurement);
            context.SaveChanges();
        }

        public void GetAllMeasurementsByGid(long gid)
        {
            throw new NotImplementedException();
        }

        public void GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid)
        {
            throw new NotImplementedException();
        }
    }
}
