﻿using ScadaCommon.Database;
using System.Data.Entity;

namespace AlarmEventServiceDatabase.Server
{
    public class AccessDB : DbContext
    {
        public AccessDB() : base("AlarmEventDB") { }

        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}