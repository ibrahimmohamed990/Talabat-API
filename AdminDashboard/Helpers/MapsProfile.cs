using AdminDashboard.Models;
using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.Identity_Entities;
namespace AdminDashboard.Helpers
{
    public class MapsProfile : Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductDetailsDto>()
               .ForMember(dest => dest.BrandName, options => options.MapFrom(src => src.Brand.Name))
               .ForMember(dest => dest.TypeName, options => options.MapFrom(src => src.Type.Name))
               .ForMember(dest => dest.PictureURL, options => options.MapFrom<ProductUrlResolver>())
               .ReverseMap();

			CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.ProductBrandId, options => options.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.ProductTypeId, options => options.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.PictureURL))
                .ReverseMap();

            CreateMap<AppUser, UserViewModel>().ReverseMap();
		}
    }
}
