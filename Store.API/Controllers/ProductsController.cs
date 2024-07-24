using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.ProductSpecfications;
using Store.Services.HandleResponses;
using Store.Services.Helper;
using Store.Services.Services.ProductService;
using Store.Services.Services.ProductService.Dtos;

namespace Store.API.Controllers
{

    [Cache(10000)]
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        public ProductsController(IProductService _productService)
        {
            productService = _productService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandTypeDetailsDto>>> GetAllBrands()
            => Ok(await productService.GetAllBrandsAsync());
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandTypeDetailsDto>>> GetAllTypes()
            => Ok(await productService.GetAllTypesAsync());
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecfication input)
            => Ok(await productService.GetAllProductsAsync(input));
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int? id)
        {
            if (id is null)
                return BadRequest(new CustomException(400, "Id is Null!"));
            var product = await productService.GetProductByIdAsync(id);
            if (product is null)
                return NotFound(new CustomException(404));

            return Ok(product);
        }


    }
}
