using BankKRT.Application.DTOs;
using BankKRT.Application.Validators;
using FluentAssertions;
using Xunit;

namespace BankKRT.UnitTests.Validators;

public class CreateAccountValidatorTests
{
    private readonly CreateAccountValidator _validator;

    public CreateAccountValidatorTests()
    {
        _validator = new CreateAccountValidator();
    }

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        var request = new CreateAccountRequest("John Doe", "529.982.247-25");
        var result = _validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_HolderName_Is_Empty()
    {
        var request = new CreateAccountRequest("", "529.982.247-25");
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "HolderName");
    }

    [Fact]
    public void Should_Fail_When_HolderName_Too_Short()
    {
        var request = new CreateAccountRequest("Jo", "529.982.247-25");
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "HolderName");
    }

    [Fact]
    public void Should_Fail_When_Cpf_Is_Empty()
    {
        var request = new CreateAccountRequest("John Doe", "");
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Cpf");
    }

    [Fact]
    public void Should_Fail_When_Cpf_Is_Invalid()
    {
        var request = new CreateAccountRequest("John Doe", "12345678901");
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Cpf");
    }
}
