using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService.DeltaDB
{
    public class DeltaDBModel
    {
        public long Id { get; set; }
        public byte[] Data { get; set; }
    }
}
