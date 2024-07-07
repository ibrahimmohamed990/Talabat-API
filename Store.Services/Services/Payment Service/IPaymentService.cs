using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.Order_Service.Dtos;

namespace Store.Services.Services.Payment_Service
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForOrder(CustomerBasketDto basket);
        //Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentSucceeded(string PaymentIntentId);
        Task<OrderResultDto> UpdateOrderPaymentFailed(string PaymentIntentId);
    }
}
