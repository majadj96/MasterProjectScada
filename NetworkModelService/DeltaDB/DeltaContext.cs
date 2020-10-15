using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService.DeltaDB
{
    public class DeltaContext : DbContext
    {
        private DeltaInitializeDB deltaDB = new DeltaInitializeDB();
        public DeltaContext() : base("DeltaHistoryDB")
        {
            Database.SetInitializer<DeltaContext>(deltaDB);
        }

        public DbSet<DeltaDBModel> Deltas { get; set; }
    }
}
