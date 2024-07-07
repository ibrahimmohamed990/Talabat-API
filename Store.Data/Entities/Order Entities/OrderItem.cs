namespace Store.Data.Entities.Order_Entities
{
    public class OrderItem : BaseEntity<Guid>
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOredered ItemOredered { get; set; }
        public Guid OrderId { get; set; }
    }
}