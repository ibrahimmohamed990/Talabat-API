using Store.Data.Entities.Order_Entities;
using System.Linq.Expressions;

namespace Store.Repository.Specification.Order_Specifications
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string? paymentIntentId) 
            : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
