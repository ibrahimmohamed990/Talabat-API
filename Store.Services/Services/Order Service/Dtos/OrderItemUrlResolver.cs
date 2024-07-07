using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities.Order_Entities;

namespace Store.Services.Services.Order_Service.Dtos
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;
        public OrderItemUrlResolver(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOredered.PictureUrl))
                return $"{configuration["BaseUrl"]}{source.ItemOredered.PictureUrl}";
            return null;
        }
    }
}