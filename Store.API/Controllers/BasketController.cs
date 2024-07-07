using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;

namespace Store.API.Controllers
{

    public class BasketController : BaseController
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService _basketService)
        {
            basketService = _basketService;
        }
        [HttpGet("id")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketByIdAsync([FromQuery]string id)
            => Ok(await basketService.GetBasketAsync(id));
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basket)
            => Ok(await basketService.UpdateBasketAsync(basket));
        [HttpDelete]
        public async Task<ActionResult> DeleteBasketAsync([FromQuery]string id)
            => Ok(await basketService.DeleteBasketAsync(id) ? "Basket Deleted Successfully" : "There is no Basket to Delete!!");
    }
}
