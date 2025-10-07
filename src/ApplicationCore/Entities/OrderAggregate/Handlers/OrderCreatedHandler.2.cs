#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Events;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Handlers
{
    public class OrderCreatedHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedHandler> _logger;
        private readonly IEmailSender _emailSender;

        public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order #{orderId} placed: ", domainEvent.Order.Id);

            await _emailSender.SendEmailAsync("to@test.com",
                                             "Order Created",
                                             "Order with id " + domainEvent.Order.Id + " was created.");
        }
    }
}

#endif
