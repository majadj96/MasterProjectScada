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
            base.Seed(context);
        }
    }
}
