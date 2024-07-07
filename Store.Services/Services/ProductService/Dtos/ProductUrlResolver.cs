using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;

namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDetailsDto, string>
    {
        private readonly IConfiguration configuration;
        public ProductUrlResolver(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public string Resolve(Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if (source.PictureURL != null)
                return  $"{configuration["BaseUrl"]}{source.PictureURL}";
            return null;
        }
    }
}
