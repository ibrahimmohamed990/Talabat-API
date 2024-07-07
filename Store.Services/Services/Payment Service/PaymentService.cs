using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Data.Entities.Order_Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order_Specifications;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.Order_Service.Dtos;
using Store.Services.Services.Payment_Service.PaymentDto;
using Stripe;
using Product = Store.Data.Entities.Product;

namespace Store.Services.Services.Payment_Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketService basketService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(
            IConfiguration _configuration, 
            IBasketService _basketService, 
            IUnitOfWork _unitOfWork,
            IMapper mapper
            )
        {
            configuration = _configuration;
            basketService = _basketService;
            unitOfWork = _unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForOrder(CustomerBasketDto basket)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            //var basket = await basketService.GetBasketAsync(basketId);

            if (basket is null)
                throw new Exception("Basket is Null!");

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);

            var shippingPrice = deliveryMethod.Price;
            foreach (var item in basket.BasketItems)
            {
                var product = await unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);

                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice * 100,
                };

                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await basketService.UpdateBasketAsync(basket);
            return basket;
        }

        //public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
        //{
        //    StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            
        //    var basket = await basketService.GetBasketAsync(basketId);
        //    if (basket is null)
        //        return null;

        //    var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);

        //    var shippingPrice = deliveryMethod.Price;

        //    foreach (var item in basket.BasketItems)
        //    {
        //        var product = await unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);

        //        if (item.Price != product.Price)
        //            item.Price = product.Price;
        //    }

        //    var service = new PaymentIntentService();

        //    PaymentIntent paymentIntent;

        //    if (string.IsNullOrEmpty(basket.PaymentIntentId))
        //    {
        //        var options = new PaymentIntentCreateOptions
        //        {
        //            Amount = (long)basket.BasketItems.Sum(item => item.Quantity + (item.Price * 100)) + (long)shippingPrice * 100,
        //            Currency = "usd",
        //            PaymentMethodTypes = new List<string> { "card" }
        //        };

        //        paymentIntent = await service.CreateAsync(options);
        //        basket.PaymentIntentId = paymentIntent.Id;
        //        basket.ClientSecret = paymentIntent.ClientSecret;
        //    }
        //    else
        //    {
        //        var options = new PaymentIntentUpdateOptions
        //        {
        //            Amount = (long)basket.BasketItems.Sum(item => item.Quantity + (item.Price * 100)) + (long)shippingPrice * 100,
        //        };

        //        paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
        //        basket.PaymentIntentId = paymentIntent.Id;
        //        basket.ClientSecret = paymentIntent.ClientSecret;
        //    }
        //    await basketService.UpdateBasketAsync(basket);
        //    return basket;
        //}

        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string PaymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecification(PaymentIntentId);

            var order = await unitOfWork.Repository<Order, Guid>().GetByIdAsyncWithSpecification(specs);

            if (order is null)
                throw new Exception("Order Does not Exist.");

            order.OrderPaymentStatus = OrderPaymentStatus.Failed;

            unitOfWork.Repository<Order, Guid>().Update(order);
            await unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentSucceeded(string PaymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecification(PaymentIntentId);

            var order = await unitOfWork.Repository<Order, Guid>().GetByIdAsyncWithSpecification(specs);

            if (order is null)
                throw new Exception("Order Does not Exist.");

            order.OrderPaymentStatus = OrderPaymentStatus.Received;

            unitOfWork.Repository<Order, Guid>().Update(order);
            await unitOfWork.CompleteAsync();
            await basketService.DeleteBasketAsync(order.BasketId);

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;


        }
    }
}
