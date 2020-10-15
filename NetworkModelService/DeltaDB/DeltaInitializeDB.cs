using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService.DeltaDB
{
    public class DeltaInitializeDB : DropCreateDatabaseIfModelChanges<DeltaContext>
    {
        protected override void Seed(DeltaContext context)
        {
            base.Seed(context);
        }
    }
}
