using Store.Data.Entities;

namespace Store.Repository.Specification.ProductSpecfications
{
    public class ProductWithSpecifications : BaseSpecification<Product>
    {
        public ProductWithSpecifications(ProductSpecfication specs) 
            : base(
                  product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId) &&
                             (!specs.TypeId.HasValue || product.TypeId == specs.TypeId) &&
                             (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search))
                  )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
            AddOrderByAsc(x => x.Name);

            ApplyPagination(specs.PigeSize * (specs.PageIndex - 1), specs.PigeSize);
            

            if (!string.IsNullOrEmpty(specs.SortBy))
            {
                switch (specs.SortBy)
                {
                    case "priceAsc":
                        AddOrderByAsc(x => x.Price);  break;
                    case "priceDesc":
                        AddOrderByDesc(x => x.Price); break;
                    default:
                        AddOrderByAsc(x => x.Name);   break;
                }
            }
        }
        public ProductWithSpecifications(int? id) : base(product => product.Id == id)
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
        }
        public ProductWithSpecifications() : base(product => product.Id == product.Id)
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
        }
    }
}
