using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Networking
{
    public class MeasurementProxy: ClientBase<IMeasurementRepository>, IMeasurementRepository
    {
        public MeasurementProxy(string endpointName) : base(endpointName)
        {

        }

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
            return Channel.GetAllMeasurementsByTime(startTime, endTime, gid);
        }
    }
}
