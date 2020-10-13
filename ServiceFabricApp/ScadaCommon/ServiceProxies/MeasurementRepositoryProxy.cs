using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceProxies
{
    public class MeasurementRepositoryProxy : ClientBase<IMeasurementRepository>, IMeasurementRepository
    {
        public MeasurementRepositoryProxy(string endpointName) : base(endpointName) { }
        public void Add(Measurement measurement)
        {
            Channel.Add(measurement);
        }

        public void AddMeasurements(Measurement[] measurements)
        {
            Channel.AddMeasurements(measurements);
        }

        public Measurement[] GetAllMeasurementsByGid(long gid)
        {
            return Channel.GetAllMeasurementsByGid(gid);
        }

        public Measurement[] GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid)
        {
            return GetAllMeasurementsByTime(startTime, endTime, gid);
        }
    }
}
