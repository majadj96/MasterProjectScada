using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementRepositoryService
{
    public class MeasurementTableRepository : IMeasurementRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public MeasurementTableRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["DataConnectionString"].ConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("MeasurementsTable");
            _table.CreateIfNotExists();
        }
        public IQueryable<Measurement> RetrieveAllMeasurements()
        {
            var results = from g in _table.CreateQuery<Measurement>()
                          where g.PartitionKey == "Measurement"
                          select g;
            return results;
        }
        public void AddMeasurement(Measurement newMeasurement)
        {
            //TableOperation insertOperation = TableOperation.Insert(newMeasurement);
            //_table.Execute(insertOperation);
        }

        public void Add(Measurement measurement)
        {
            AddMeasurement(measurement);
        }

        public void AddMeasurements(Measurement[] measurements)
        {
            for (int i = 0; i < measurements.Length; i++)
            {
                AddMeasurement(measurements[i]);
            }
        }

        public Measurement[] GetAllMeasurementsByGid(long gid)
        {
            return RetrieveAllMeasurements().Where(x => x.Gid == gid).ToArray();
        }

        public Measurement[] GetAllMeasurementsByTime(DateTime startTime, DateTime endTime, long gid)
        {
            return RetrieveAllMeasurements().Where(x => x.Gid == gid & x.ChangedTime > startTime & x.ChangedTime < endTime).ToArray();
        }
    }
}
