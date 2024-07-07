using Store.Data.Entities;

namespace Store.Repository.Specification.ProductSpecfications
{
    public class ProductWithCountAndFilterSpecifications : BaseSpecification<Product>
    {
        public ProductWithCountAndFilterSpecifications(ProductSpecfication specs)
            : base(
                  product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId) &&
                             (!specs.TypeId.HasValue || product.TypeId == specs.TypeId) &&
                             (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search))
                  )
        {
        }

    }
}
