using AutoMapper;
using Store.Data.Entities.Identity_Entities;
using Store.Data.Entities.Order_Entities;

namespace Store.Services.Services.Order_Service.Dtos
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.DeliveryMethodName, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, options => options.MapFrom(src => src.DeliveryMethod.Price)).ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductItemId, options => options.MapFrom(src => src.ItemOredered.ProductItemId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.ItemOredered.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.ItemOredered.PictureUrl))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemUrlResolver>()).ReverseMap();

        }
    }
}
