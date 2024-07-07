using Store.Data.Entities;
using Store.Repository.Specification;

namespace Store.Repository.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsyncWithSpecification(ISpecification<TEntity> specification);
        Task<IEnumerable<TEntity>> GetAllAsyncWithSpecification(ISpecification<TEntity> specification);
        Task<TEntity> GetByIdAsync(TKey id);
        Task<int> CountSpecificationAsync(ISpecification<TEntity> specification);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
