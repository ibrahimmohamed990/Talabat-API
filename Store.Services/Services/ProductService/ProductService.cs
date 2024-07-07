using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.ProductSpecfications;
using Store.Services.Helper;
using Store.Services.Services.ProductService.Dtos;

namespace Store.Services.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<IEnumerable<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            var mappedBrands = mapper.Map<IEnumerable<BrandTypeDetailsDto>>(brands);
            return mappedBrands;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecfication input)
        {
            var specs = new ProductWithSpecifications(input);
            
            var products = await unitOfWork.Repository<Product, int>().GetAllAsyncWithSpecification(specs);
            var countSpecs = new ProductWithCountAndFilterSpecifications(input);
            var count = await unitOfWork.Repository<Product, int>().CountSpecificationAsync(countSpecs);
            
            var mappedProducts = mapper.Map<IEnumerable<ProductDetailsDto>>(products);

            return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PigeSize,count, mappedProducts);
        }

        public async Task<IEnumerable<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.Repository<ProductType, int>().GetAllAsync();
            var mappedTypes = mapper.Map<IEnumerable<BrandTypeDetailsDto>>(types);
            return mappedTypes;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            //if (id is null) throw new Exception("Id is Null!");

            var specs = new ProductWithSpecifications(id);

            var product = await unitOfWork.Repository<Product, int>().GetByIdAsyncWithSpecification(specs);

            //if (product is null) throw new Exception("Product Not Found!!");

            var mappedProduct = mapper.Map<ProductDetailsDto>(product);
            return mappedProduct;
        }
    }
}
