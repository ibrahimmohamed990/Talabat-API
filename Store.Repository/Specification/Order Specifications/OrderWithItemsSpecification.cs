
using Order = Store.Data.Entities.Order_Entities.Order;

namespace Store.Repository.Specification.Order_Specifications
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string BuyerEmail) 
            : base(order => order.BuyerEmail == BuyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDesc(order => order.OrderDate);

        }
        public OrderWithItemsSpecification(Guid id, string BuyerEmail)
            : base(order => order.BuyerEmail == BuyerEmail && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);

        }
    }
}
