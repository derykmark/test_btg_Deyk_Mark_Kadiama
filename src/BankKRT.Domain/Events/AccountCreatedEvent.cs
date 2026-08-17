using MediatR;

namespace BankKRT.Domain.Events;

public sealed record AccountCreatedEvent(
    int AccountId,
    string HolderName,
    string Cpf,
    DateTime CreatedAt
) : INotification;
