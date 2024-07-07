using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.Order_Service.Dtos;
using Store.Services.Services.Payment_Service;
using Stripe;

namespace Store.API.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> logger;

        private const string endpointSecret = "whsec_971667d2c2a821640f0c3bb54a6588ccfcf04c918b67f6553fd482d4252b10cd";

        public PaymentController(
            IPaymentService _paymentService,
            ILogger<PaymentController> _logger)
        {
            paymentService = _paymentService;
            logger = _logger;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForOrder([FromQuery] CustomerBasketDto basket)
            => Ok(await paymentService.CreateOrUpdatePaymentIntentForOrder(basket));
        
        //[HttpPost("{basketId}")]
        //public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder([FromQuery] CustomerBasketDto basket)
        //    => Ok(await paymentService.CreateOrUpdatePaymentIntentForOrder(basket));

        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, 
                    Request.Headers["Stripe-Signature"], endpointSecret);
                PaymentIntent paymentIntent;
                OrderResultDto order; 

                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    logger.LogInformation("Order Updated to payment Failed.", order.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                    order = await paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    logger.LogInformation("Order Updated to payment Succeeded.", order.Id);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }


    }
}
