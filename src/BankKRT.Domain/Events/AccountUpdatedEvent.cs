using MediatR;
using BankKRT.Domain.Enums;

namespace BankKRT.Domain.Events;

public sealed record AccountUpdatedEvent(
    int AccountId,
    string HolderName,
    AccountStatus OldStatus,
    AccountStatus NewStatus,
    DateTime UpdatedAt
) : INotification;
