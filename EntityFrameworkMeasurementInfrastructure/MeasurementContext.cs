using Microsoft.EntityFrameworkCore;
using RepositoryCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMeasurementInfrastructure
{
    public class MeasurementContext : DbContext
    {
        public MeasurementContext() : base("name=connectionString")
        {

        }

        public DbSet<Measurement> Measurements { get; set; }
    }
}
