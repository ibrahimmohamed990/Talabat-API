

namespace Store.Services.Services.BasketService.Dtos
{
    public class CustomerBasketDto
    {
        public string BasketId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
        public List<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();
        public string? PaymentIntentId { get; set; } = string.Empty;
        public string? ClientSecret { get; set; } = string.Empty;

    }
}
