using BankKRT.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BankKRT.UnitTests.Domain;

public class CPFTests
{
    [Fact]
    public void Should_Create_Valid_CPF()
    {
        // Arrange & Act
        var cpf = CPF.Create("529.982.247-25");

        // Assert
        cpf.Should().NotBeNull();
        cpf.Value.Should().Be("52998224725");
    }

    [Fact]
    public void Should_Create_CPF_Without_Formatting()
    {
        // Arrange & Act
        var cpf = CPF.Create("52998224725");

        // Assert
        cpf.Should().NotBeNull();
        cpf.Value.Should().Be("52998224725");
    }

    [Fact]
    public void Should_Throw_For_Empty_CPF()
    {
        // Arrange & Act
        var action = () => CPF.Create(string.Empty);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Throw_For_Invalid_Length()
    {
        // Arrange & Act
        var action = () => CPF.Create("1234567890"); // 10 digits

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Throw_For_All_Same_Digits()
    {
        // Arrange & Act
        var action = () => CPF.Create("11111111111");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Throw_For_Invalid_Check_Digits()
    {
        // Arrange & Act
        var action = () => CPF.Create("52998224726"); // invalid check digit

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Remove_Formatting()
    {
        // Arrange & Act
        var cpf = CPF.Create("529.982.247-25");

        // Assert
        cpf.Value.Should().NotContain(".");
        cpf.Value.Should().NotContain("-");
        cpf.Value.Should().Be("52998224725");
    }

    [Fact]
    public void Two_CPFs_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var cpf1 = CPF.Create("529.982.247-25");
        var cpf2 = CPF.Create("52998224725");

        // Act & Assert
        cpf1.Should().Be(cpf2);
    }
}
