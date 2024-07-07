using Store.Data.Context;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using System.Collections;

namespace Store.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext context;
        private Hashtable? repositories;

        public UnitOfWork(StoreDbContext _context)
        {
            context = _context;
            //repositories = new Hashtable();
        }
        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            repositories = repositories ?? new Hashtable();
            var EntityKey = typeof(TEntity);
            if(!repositories.ContainsKey(EntityKey.Name))
            {
                var repositoryType = typeof(GenericRepository<,>).MakeGenericType(typeof(TEntity), typeof(TKey));
                var repositoryInstance = Activator.CreateInstance(repositoryType, context);
                repositories.Add(EntityKey.Name, repositoryInstance);
            }
            return repositories[EntityKey.Name] as IGenericRepository<TEntity, TKey>;
        }
        public async Task<int> CompleteAsync()
            => await context.SaveChangesAsync();
    }
}

