using BankKRT.Application.DTOs;
using BankKRT.Application.Services;
using BankKRT.Domain.Entities;
using BankKRT.Domain.Events;
using BankKRT.Domain.Interfaces;
using BankKRT.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace BankKRT.UnitTests.Application;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IMemoryCache _memoryCache;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _repositoryMock = new Mock<IAccountRepository>();
        _mediatorMock = new Mock<IMediator>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _service = new AccountService(_repositoryMock.Object, _mediatorMock.Object, _memoryCache);
    }

    [Fact]
    public async Task CreateAsync_Should_Return_AccountResponse()
    {
        // Arrange
        var request = new CreateAccountRequest("John Doe", "529.982.247-25");
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync((Account?)null);
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) => a);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.HolderName.Should().Be(request.HolderName);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Account>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_CPF_Already_Exists()
    {
        // Arrange
        var request = new CreateAccountRequest("John Doe", "529.982.247-25");
        var existingAccount = Account.Create("Jane Doe", CPF.Create(request.Cpf));
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync(existingAccount);

        // Act
        var action = () => _service.CreateAsync(request);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateAsync_Should_Publish_AccountCreatedEvent()
    {
        // Arrange
        var request = new CreateAccountRequest("John Doe", "529.982.247-25");
        _repositoryMock.Setup(x => x.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync((Account?)null);
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) => a);

        // Act
        await _service.CreateAsync(request);

        // Assert
        _mediatorMock.Verify(x => x.Publish(It.IsAny<AccountCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_From_Cache_When_Available()
    {
        // Arrange
        var response = new AccountResponse(1, "John", "52998224725", "Active", DateTime.UtcNow, null);
        _memoryCache.Set("account:1", response);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        _repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Query_DB_And_Cache_When_Not_In_Cache()
    {
        // Arrange
        var account = Account.Create("John Doe", CPF.Create("529.982.247-25"));
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(account);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        _repositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        _memoryCache.TryGetValue("account:1", out var _).Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Account?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Account_Not_Found()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Account?)null);
        var request = new UpdateAccountRequest("New Name", null);

        // Act
        var action = () => _service.UpdateAsync(1, request);

        // Assert
        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_Should_Publish_AccountUpdatedEvent()
    {
        // Arrange
        var account = Account.Create("John Doe", CPF.Create("529.982.247-25"));
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(account);
        var request = new UpdateAccountRequest("New Name", "Inactive");

        // Act
        await _service.UpdateAsync(1, request);

        // Assert
        _mediatorMock.Verify(x => x.Publish(It.IsAny<AccountUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        account.HolderName.Should().Be("New Name");
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Account_Not_Found()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Account?)null);

        // Act
        var action = () => _service.DeleteAsync(1);

        // Assert
        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_Should_Publish_AccountDeletedEvent()
    {
        // Arrange
        var account = Account.Create("John Doe", CPF.Create("529.982.247-25"));
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(account);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(x => x.DeleteAsync(account), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<AccountDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
