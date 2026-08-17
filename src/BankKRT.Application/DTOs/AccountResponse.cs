using System;

namespace BankKRT.Application.DTOs;

public record AccountResponse(
    int Id,
    string HolderName,
    string Cpf,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
