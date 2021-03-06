﻿using RepositoryCore;
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

        public void AddMeasurements(Measurement[] measurements)
        {
            context.Measurements.AddRange(measurements);
            context.SaveChanges();
        }

        public Measurement[] GetAllMeasurementsByGid(long gid)
        {
            return (context.Measurements.Where(x => x.Gid == gid)).ToArray();
        }

        public Measurement[] GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid)
        {
            return (context.Measurements.Where(x => x.Gid == gid & x.ChangedTime > startTime & x.ChangedTime < endTime)).ToArray();
        }
    }
}
