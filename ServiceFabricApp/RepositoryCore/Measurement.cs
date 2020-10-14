using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCore
{
    public class Measurement : TableEntity
    {
        public Measurement(DateTime rowKey)
        {
            PartitionKey = "Measurement";
            RowKey = rowKey.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            this.Timestamp = DateTime.Now;
        }

        public Measurement()
        {
            PartitionKey = "Measurement";
            RowKey = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            this.Timestamp = DateTime.Now;
        }

        public int Id { get; set; }
        public long Gid { get; set; }
        public int Value { get; set; }
        public DateTime? ChangedTime { get; set; }
    }
}
