using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification;
using Store.Repository.Specification.ProductSpecfications;

namespace Store.Repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext context;

        public GenericRepository(StoreDbContext _context)
        {
            context = _context;
        }
        public async Task AddAsync(TEntity entity)
            => await context.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
            => context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await context.Set<TEntity>().ToListAsync();

        public async Task<TEntity> GetByIdAsync(TKey id)
            => await context.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
            => context.Set<TEntity>().Update(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsyncWithSpecification(ISpecification<TEntity> specs)
            => await ApplySpecification(specs).ToListAsync();
        public async Task<TEntity> GetByIdAsyncWithSpecification(ISpecification<TEntity> specs)
            => await ApplySpecification(specs).FirstOrDefaultAsync();
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specs)
            => SpecificationEvaluator<TEntity, TKey>.GetQuery(context.Set<TEntity>(), specs);

        public async Task<int> CountSpecificationAsync(ISpecification<TEntity> specification)
        => await ApplySpecification(specification).CountAsync();
    }
}
