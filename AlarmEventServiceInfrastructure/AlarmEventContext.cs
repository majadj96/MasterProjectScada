using RepositoryCore;
using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventServiceInfrastructure
{
    public class AlarmEventContext : DbContext
    {
        private AlarmEventInitializeDB alarmEventDB = new AlarmEventInitializeDB();
        public AlarmEventContext() : base("AlarmEventDB")
        {
            Database.SetInitializer<AlarmEventContext>(alarmEventDB);
        }

        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
