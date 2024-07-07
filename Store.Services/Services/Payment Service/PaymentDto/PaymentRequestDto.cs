namespace Store.Services.Services.Payment_Service.PaymentDto
{
    public class PaymentRequestDto
    {
        public string BasketId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string? PaymentMethodId { get; set; }

    }
}
