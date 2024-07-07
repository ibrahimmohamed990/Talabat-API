using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.Order_Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order_Specifications;
using Store.Services.Services.BasketService;
using Store.Services.Services.Order_Service.Dtos;
using Store.Services.Services.Payment_Service;
using Store.Services.Services.Payment_Service.PaymentDto;

namespace Store.Services.Services.Order_Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketService basketService;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;

        public OrderService(IUnitOfWork _unitOfWork,
            IBasketService _basketService,
            IMapper _mapper,
            IPaymentService _paymentService)
        {
            unitOfWork = _unitOfWork;
            basketService = _basketService;
            mapper = _mapper;
            paymentService = _paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            // Get Basket 
            var basket = await basketService.GetBasketAsync(input.BasketId);
            if (basket is null)
                throw new Exception("Basket Not Exist!!");

            // Fill Order Items from Basket Items
            var orderItems = new List<OrderItemDto>();
            foreach (var basketItem in basket.BasketItems)
            {
                var productItem = await unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);
                if (productItem is null)
                    throw new Exception($"Product {basketItem.ProductName} with Id {basketItem.ProductId} Not Exist!");

                var itemOrderd = new ProductItemOredered
                {
                    ProductName = productItem.Name,
                    ProductItemId = productItem.Id,
                    PictureUrl = productItem.PictureURL
                };
                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    ItemOredered = itemOrderd,
                    Quantity = basketItem.Quantity
                };
                var mappedOrderItem = mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem);
            }
            // Get Delivery Mehtod
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new Exception("Delivery Method Not Provided!");

            // Calculate SubTotal 
            foreach (var item in basket.BasketItems)
            {
                var product = await unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);

                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);
            basket.DeliveryMethodId = input.DeliveryMethodId;
            

            // To Do ==>  Check If Order Exist
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await unitOfWork.Repository<Order, Guid>().GetByIdAsyncWithSpecification(specs);
            
            if (existingOrder is null)
            {
                basket = await paymentService.CreateOrUpdatePaymentIntentForOrder(basket);
            }
            else
            {
                unitOfWork.Repository<Order, Guid>().Delete(existingOrder);
                basket = await paymentService.CreateOrUpdatePaymentIntentForOrder(basket);
            }

            // Create Order
            var mappedShippingaddress = mapper.Map<ShippingAddress>(input.ShippingAddress);

            var mappedOrderItems = mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = mappedShippingaddress,
                BuyerEmail = input.BuyerEmail,
                SubTotal = subTotal,
                OrderItems = mappedOrderItems,
                DeliveryMethod = deliveryMethod,
                PaymentIntentId = basket.PaymentIntentId,
                BasketId = basket.BasketId
            };
            await unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await unitOfWork.CompleteAsync();

            var mappedOrder = mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethods()
            => await unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
        
        public async Task<IEnumerable<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecification(BuyerEmail);

            var orders = await unitOfWork.Repository<Order, Guid>().GetAllAsyncWithSpecification(specs);

            if (orders.Count() <= 0)
                throw new Exception("You does not have any orders yet!!");
            
            var mappedOrders = mapper.Map<List<OrderResultDto>>(orders);
            
            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid Id, string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecification(Id, BuyerEmail);

            var order = await unitOfWork.Repository<Order, Guid>().GetByIdAsyncWithSpecification(specs);

            if (order is null)
                throw new Exception($"There is no order with id : {Id}");

            var mappedOrder = mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }
    }
}
