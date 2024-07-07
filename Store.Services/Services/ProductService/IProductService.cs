using Store.Repository.Specification.ProductSpecfications;
using Store.Services.Helper;
using Store.Services.Services.ProductService.Dtos;

namespace Store.Services.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? id);
        Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecfication input);
        Task<IEnumerable<BrandTypeDetailsDto>> GetAllBrandsAsync();
        Task<IEnumerable<BrandTypeDetailsDto>> GetAllTypesAsync();

    }
}
