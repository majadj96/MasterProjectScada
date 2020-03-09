using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EntityFrameworkMeasurementInfrastructure
{
    public class MeasurementInitializeDB : DropCreateDatabaseIfModelChanges<MeasurementContext>
    {
        protected override void Seed(MeasurementContext context)
        {
            context.Measurements.Add(new RepositoryCore.Measurement() { Gid = 0, ChangedTime = DateTime.Now, Value = 2 });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
