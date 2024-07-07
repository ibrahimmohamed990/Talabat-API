using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetBasketAsync(string basketId);
        Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto customerBasket);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
