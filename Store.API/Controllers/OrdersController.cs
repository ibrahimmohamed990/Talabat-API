using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Services.HandleResponses;
using Store.Services.Services.Order_Service;
using Store.Services.Services.Order_Service.Dtos;
using System.Security.Claims;

namespace Store.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService _orderService)
        {
            orderService = _orderService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync([FromQuery]OrderDto input)
        {
            var order = await orderService.CreateOrderAsync(input);
            if (order is null)
                return BadRequest(new Response(400, "Error While creating your Order."));
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResultDto>>> GetAllOrdersForUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await orderService.GetAllOrdersForUserAsync(email);
            
            return Ok(orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync([FromQuery]Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrderByIdAsync(id, email);

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> GetAllDeliveryMethodsAsync()
            => Ok(await orderService.GetAllDeliveryMethods());

    }
}
