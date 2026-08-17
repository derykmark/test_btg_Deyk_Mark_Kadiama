using BankKRT.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankKRT.Infrastructure.Events.Handlers;

public class ComplianceHandler : 
    INotificationHandler<AccountCreatedEvent>,
    INotificationHandler<AccountUpdatedEvent>,
    INotificationHandler<AccountDeletedEvent>
{
    private readonly ILogger<ComplianceHandler> _logger;

    public ComplianceHandler(ILogger<ComplianceHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Compliance Notified: Account Created - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }

    public Task Handle(AccountUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Compliance Notified: Account Updated - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }

    public Task Handle(AccountDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Compliance Notified: Account Deleted - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }
}
