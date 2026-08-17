using BankKRT.Domain.Entities;
using BankKRT.Domain.Enums;
using BankKRT.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BankKRT.UnitTests.Domain;

public class AccountTests
{
    private readonly CPF _validCpf = CPF.Create("529.982.247-25");

    [Fact]
    public void Should_Create_Account_With_Active_Status()
    {
        // Arrange & Act
        var account = Account.Create("John Doe", _validCpf);

        // Assert
        account.Should().NotBeNull();
        account.Status.Should().Be(AccountStatus.Active);
    }

    [Fact]
    public void Should_Create_Account_With_CreatedAt()
    {
        // Arrange & Act
        var before = DateTime.UtcNow;
        var account = Account.Create("John Doe", _validCpf);
        var after = DateTime.UtcNow;

        // Assert
        account.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void Should_Update_HolderName()
    {
        // Arrange
        var account = Account.Create("John Doe", _validCpf);
        var newName = "Jane Doe";

        // Act
        account.Update(newName);

        // Assert
        account.HolderName.Should().Be(newName);
        account.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Deactivate_Account()
    {
        // Arrange
        var account = Account.Create("John Doe", _validCpf);

        // Act
        account.Deactivate();

        // Assert
        account.Status.Should().Be(AccountStatus.Inactive);
        account.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Activate_Account()
    {
        // Arrange
        var account = Account.Create("John Doe", _validCpf);
        account.Deactivate();

        // Act
        account.Activate();

        // Assert
        account.Status.Should().Be(AccountStatus.Active);
    }

    [Fact]
    public void Should_Throw_When_Creating_With_Empty_Name()
    {
        // Arrange & Act
        var action = () => Account.Create(string.Empty, _validCpf);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Throw_When_Creating_With_Null_CPF()
    {
        // Arrange & Act
        var action = () => Account.Create("John Doe", null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
