using Store.Data.Entities;
using Store.Services.Services.Order_Service.Dtos;

namespace Store.Services.Services.Order_Service
{
    public interface IOrderService
    {
        Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto);
        Task<IEnumerable<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<OrderResultDto> GetOrderByIdAsync(Guid Id, string BuyerEmail);
        Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethods();
    }
}
