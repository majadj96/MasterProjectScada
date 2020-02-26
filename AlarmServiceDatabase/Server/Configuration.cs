using System.Data.Entity.Migrations;

namespace AlarmEventServiceDatabase.Server
{
    public class Configuration : DbMigrationsConfiguration<AccessDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AlarmEventDB";
        }
    }
}