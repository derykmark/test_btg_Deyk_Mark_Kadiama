using System.Net;
using System.Net.Http.Json;
using BankKRT.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BankKRT.IntegrationTests;

public class AccountsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccountsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GET_Accounts_Returns_200()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/accounts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task POST_Account_With_Invalid_Data_Returns_400()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateAccountRequest("", "invalid-cpf");

        // Act
        var response = await client.PostAsJsonAsync("/api/accounts", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
