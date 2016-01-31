using System.Data.Entity.Migrations;
using rooferlocator.com.Migrations.SeedData;

namespace rooferlocator.com.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<com.EntityFramework.comDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "com";
        }

        protected override void Seed(com.EntityFramework.comDbContext context)
        {
            new InitialDataBuilder(context).Build();
        }
    }
}
