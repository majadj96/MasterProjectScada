using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MeasurementProviderService : IMeasurementRepository
    {
        private IMeasurementRepository measurementRepository;
        public MeasurementProviderService(IMeasurementRepository measurementRepository)
        {
            this.measurementRepository = measurementRepository;
        }

        public void Add(Measurement measurement)
        {
            this.measurementRepository.Add(measurement);
        }

        public void AddMeasurements(Measurement[] measurements)
        {
            this.measurementRepository.AddMeasurements(measurements);
        }

        public Measurement[] GetAllMeasurementsByGid(long gid)
        {
             return this.measurementRepository.GetAllMeasurementsByGid(gid);
        }

        public Measurement[] GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid)
        {
            return this.GetAllMeasurementsByTime(startTime, endTime, gid);
        }
    }
}
