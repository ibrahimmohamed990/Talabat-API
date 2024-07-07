using AutoMapper;
using Store.Data.Entities;

namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.BrandName, options => options.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.TypeName, options => options.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.PictureURL, options => options.MapFrom<ProductUrlResolver>())
                .ReverseMap();
            
            CreateMap<BrandTypeDetailsDto, ProductBrand>().ReverseMap();
            
            CreateMap<BrandTypeDetailsDto, ProductType>().ReverseMap();
        }
    }
}
