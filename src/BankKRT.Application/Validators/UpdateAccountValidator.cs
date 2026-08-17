using BankKRT.Application.DTOs;
using FluentValidation;

namespace BankKRT.Application.Validators;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.HolderName)
            .MinimumLength(3)
            .MaximumLength(150)
            .When(x => !string.IsNullOrEmpty(x.HolderName));

        RuleFor(x => x.Status)
            .Must(status => status == "Active" || status == "Inactive")
            .WithMessage("Status must be 'Active' or 'Inactive'")
            .When(x => !string.IsNullOrEmpty(x.Status));
    }
}
