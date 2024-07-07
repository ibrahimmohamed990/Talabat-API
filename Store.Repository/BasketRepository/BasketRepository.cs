using StackExchange.Redis;
using Store.Repository.BasketRepository.Models;
using System.Text.Json;

namespace Store.Repository.BasketRepository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
            => await _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            if (data.IsNullOrEmpty)
                return null;
            return JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var isCreated = await _database.StringSetAsync(customerBasket.BasketId, JsonSerializer.Serialize(customerBasket), TimeSpan.FromDays(30));
            if (!isCreated)
                return null;
            return await GetBasketAsync(customerBasket.BasketId);
        }
    }
}
