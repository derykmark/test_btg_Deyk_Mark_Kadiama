using BankKRT.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankKRT.Infrastructure.Events.Handlers;

public class FraudPreventionHandler : 
    INotificationHandler<AccountCreatedEvent>,
    INotificationHandler<AccountDeletedEvent>
{
    private readonly ILogger<FraudPreventionHandler> _logger;

    public FraudPreventionHandler(ILogger<FraudPreventionHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fraud Prevention Notified: Account Created - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }

    public Task Handle(AccountDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fraud Prevention Notified: Account Deleted - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }
}
