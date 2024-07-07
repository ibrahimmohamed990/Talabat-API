using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;

namespace Store.Repository.Specification.ProductSpecfications
{
    public class SpecificationEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specs)
        {
            var query = inputQuery;

            if(specs.Criteria is not null)
                query = query.Where(specs.Criteria);

            if(specs.OrderByAsc is not null)
                query = query.OrderBy(specs.OrderByAsc);
            
            if(specs.OrderByDesc is not null)
                query = query.OrderByDescending(specs.OrderByDesc);
            
            if(specs.IsPaginated)
                query = query.Skip(specs.Skip).Take(specs.Take);

            query = specs.Includes.Aggregate(query, (current, IncludeExpression) => current.Include(IncludeExpression));
            
            return query;
        }


    }
}
