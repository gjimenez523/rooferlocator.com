using System.Data.Common;
using Abp.Zero.EntityFramework;
using rooferlocator.com.Authorization.Roles;
using rooferlocator.com.MultiTenancy;
using rooferlocator.com.Users;
using System.Data.Entity;
using rooferlocator.com.Common;
using rooferlocator.com.Common.News;
using rooferlocator.com.Common.Types;
using rooferlocator.Sales;

namespace rooferlocator.com.EntityFramework
{
    public class comDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        public virtual IDbSet<Company> Company { get; set; }
        public virtual IDbSet<Member> Member { get; set; }
        public virtual IDbSet<MembersRoofType> MembersRoofType { get; set; }
        public virtual IDbSet<MembersServiceType> MembersServiceType { get; set; }
        public virtual IDbSet<News> News { get; set; }
        public virtual IDbSet<NewsCategories> NewsCategories { get; set; }
        public virtual IDbSet<Tips> Tips { get; set; }
        public virtual IDbSet<TipsCategories> TipsCategories { get; set; }
        public virtual IDbSet<Testimonial> Testimonial { get; set; }
        public virtual IDbSet<LocationType> LocationType { get; set; }
        public virtual IDbSet<QuoteType> QuoteType { get; set; }
        public virtual IDbSet<RoofType> RoofType { get; set; }
        public virtual IDbSet<ServiceType> ServiceType { get; set; }
        public virtual IDbSet<Lead> Lead { get; set; }
        public virtual IDbSet<Payment> Payment { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public comDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in comDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of comDbContext since ABP automatically handles it.
         */
        public comDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public comDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}
