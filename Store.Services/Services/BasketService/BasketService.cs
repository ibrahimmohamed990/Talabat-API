using AutoMapper;
using Store.Repository.BasketRepository;
using Store.Repository.BasketRepository.Models;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketService(IBasketRepository _basketRepository, IMapper _mapper)
        {
            basketRepository = _basketRepository;
            mapper = _mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
            => await basketRepository.DeleteBasketAsync(basketId);

        public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            if(basket is null)
                return null;
            var mappedBasket = mapper.Map<CustomerBasketDto>(basket);

            return mappedBasket;
        }
        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto customerBasket)
        {
            var _customerBasket = mapper.Map<CustomerBasket>(customerBasket);
            var updatedBasket = await basketRepository.UpdateBasketAsync(_customerBasket);

            var mappedCustomerBasket = mapper.Map<CustomerBasketDto>(updatedBasket);

            return mappedCustomerBasket;
        }
    }
}
