using NetworkModelService.DeltaDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService.DeltaDB
{
    public class DeltaRepository : IDeltaRepository
    {
        private DeltaContext context = new DeltaContext();

        public void Add(DeltaDBModel delta)
        {
            context.Deltas.Add(delta);
            context.SaveChanges();
        }

        public List<DeltaDBModel> GetAllDeltas()
        {
            return context.Deltas.ToList();
        }
    }
}
