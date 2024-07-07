using System.Linq.Expressions;

namespace Store.Repository.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes {  get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderByAsc { get; private set; }

        public Expression<Func<T, object>> OrderByDesc { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
            => Includes.Add(includeExpression);
        protected void AddOrderByAsc(Expression<Func<T, object>> orderByAscExpression)
            => OrderByAsc = orderByAscExpression;
        protected void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression) 
            => OrderByDesc = orderByDescExpression;
        protected void ApplyPagination(int skip, int take)
        {
            Take = take;
            Skip = skip;
            IsPaginated = true;
        }
        
    }
}
