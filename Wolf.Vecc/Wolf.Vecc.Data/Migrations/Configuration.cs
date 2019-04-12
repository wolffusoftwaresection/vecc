namespace Wolf.Vecc.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Wolf.Vecc.Data.DataContext.VeccContext>
    {
        public Configuration()
        {
            //Add-Migration Initial
            //Update-Database -Verbose
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Wolf.Vecc.Data.DataContext.VeccContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
