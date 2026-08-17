using BankKRT.Application.DTOs;
using BankKRT.Domain.ValueObjects;
using FluentValidation;

namespace BankKRT.Application.Validators;

public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.HolderName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .Must(BeAValidCpf).WithMessage("Invalid CPF.");
    }

    private bool BeAValidCpf(string cpf)
    {
        try
        {
            CPF.Create(cpf);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
