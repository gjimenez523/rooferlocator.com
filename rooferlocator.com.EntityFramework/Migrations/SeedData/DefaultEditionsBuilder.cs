using System.Linq;
using Abp.Application.Editions;
using rooferlocator.com.Editions;
using rooferlocator.com.EntityFramework;

namespace rooferlocator.com.Migrations.SeedData
{
    public class DefaultEditionsBuilder
    {
        private readonly comDbContext _context;

        public DefaultEditionsBuilder(comDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                //TODO: Add desired features to the standard edition, if wanted!
            }   
        }
    }
}