using MediatR;

namespace BankKRT.Domain.Events;

public sealed record AccountDeletedEvent(
    int AccountId,
    string HolderName,
    string Cpf,
    DateTime DeletedAt
) : INotification;
