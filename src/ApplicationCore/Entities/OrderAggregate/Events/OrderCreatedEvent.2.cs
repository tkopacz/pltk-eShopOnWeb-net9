#if NETCOREAPP2_0

using Ardalis.SharedKernel;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Events
{
    public class OrderCreatedEvent : DomainEventBase
    {
        public OrderCreatedEvent(Order order)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }
}

#endif
