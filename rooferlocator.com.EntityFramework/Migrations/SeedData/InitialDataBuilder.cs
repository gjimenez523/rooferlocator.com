using rooferlocator.com.EntityFramework;
using EntityFramework.DynamicFilters;

namespace rooferlocator.com.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly comDbContext _context;

        public InitialDataBuilder(comDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new DefaultEditionsBuilder(_context).Build();
            new DefaultTenantRoleAndUserBuilder(_context).Build();
            new DefaultLanguagesBuilder(_context).Build();
        }
    }
}
