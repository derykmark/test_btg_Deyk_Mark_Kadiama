using BankKRT.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankKRT.Infrastructure.Events.Handlers;

public class CardDepartmentHandler : 
    INotificationHandler<AccountCreatedEvent>,
    INotificationHandler<AccountDeletedEvent>
{
    private readonly ILogger<CardDepartmentHandler> _logger;

    public CardDepartmentHandler(ILogger<CardDepartmentHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Card Department Notified: Account Created - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }

    public Task Handle(AccountDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Card Department Notified: Account Deleted - ID {AccountId}", notification.AccountId);
        return Task.CompletedTask;
    }
}
