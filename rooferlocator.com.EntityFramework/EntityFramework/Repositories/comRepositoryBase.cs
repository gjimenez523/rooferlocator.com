using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace rooferlocator.com.EntityFramework.Repositories
{
    public abstract class comRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<comDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected comRepositoryBase(IDbContextProvider<comDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class comRepositoryBase<TEntity> : comRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected comRepositoryBase(IDbContextProvider<comDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
