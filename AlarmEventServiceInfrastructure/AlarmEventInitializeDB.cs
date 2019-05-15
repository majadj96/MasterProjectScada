using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventServiceInfrastructure
{
    public class AlarmEventInitializeDB : DropCreateDatabaseIfModelChanges<AlarmEventContext>
    {
        protected override void Seed(AlarmEventContext context)
        {
            base.Seed(context);
        }
    }
}
