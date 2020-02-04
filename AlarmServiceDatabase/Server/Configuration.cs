using System.Data.Entity.Migrations;

namespace AlarmEventService.Server
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