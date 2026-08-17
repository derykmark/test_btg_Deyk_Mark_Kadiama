using BankKRT.Domain.Enums;
using BankKRT.Domain.ValueObjects;

namespace BankKRT.Domain.Entities;

public sealed class Account
{
    public int Id { get; private set; }
    public string HolderName { get; private set; } = string.Empty;
    public CPF Cpf { get; private set; } = null!;
    public AccountStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Required by EF Core
    private Account() { }

    private Account(string holderName, CPF cpf)
    {
        HolderName = holderName;
        Cpf = cpf;
        Status = AccountStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public static Account Create(string holderName, CPF cpf)
    {
        if (string.IsNullOrWhiteSpace(holderName))
            throw new ArgumentException("Holder name cannot be empty.", nameof(holderName));

        if (holderName.Length > 100)
            throw new ArgumentException("Holder name cannot exceed 100 characters.", nameof(holderName));

        ArgumentNullException.ThrowIfNull(cpf);

        return new Account(holderName, cpf);
    }

    public void Update(string holderName)
    {
        if (string.IsNullOrWhiteSpace(holderName))
            throw new ArgumentException("Holder name cannot be empty.", nameof(holderName));

        if (holderName.Length > 100)
            throw new ArgumentException("Holder name cannot exceed 100 characters.", nameof(holderName));

        HolderName = holderName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == AccountStatus.Inactive)
            throw new InvalidOperationException("Account is already inactive.");

        Status = AccountStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == AccountStatus.Active)
            throw new InvalidOperationException("Account is already active.");

        Status = AccountStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
}
