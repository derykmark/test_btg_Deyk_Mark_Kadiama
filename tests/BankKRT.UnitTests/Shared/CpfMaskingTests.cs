using FluentAssertions;
using BankKRT.Shared.Logging;
using Xunit;

namespace BankKRT.UnitTests.Shared;

public class CpfMaskingTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("1", "**")] // less than 2 digits
    [InlineData("12", "12")] // exactly 2 digits -> returns them
    [InlineData("529.982.247-25", "*********25")]
    [InlineData("000.000.000-00", "*********00")]
    [InlineData("abc12def", "12")]
    public void Mask_Should_Mask_Cpf_Correctly(string input, string expected)
    {
        var result = CpfMasking.Mask(input);
        result.Should().Be(expected);
    }
}
