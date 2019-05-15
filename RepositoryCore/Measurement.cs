using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCore
{
    public class Measurement
    {
        public int Id { get; set; }
        public long Gid { get; set; }
        public int Value { get; set; }
        public DateTime? ChangedTime { get; set; }
    }
}
