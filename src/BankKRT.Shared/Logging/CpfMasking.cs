using System;
using System.Linq;

namespace BankKRT.Shared.Logging;

public static class CpfMasking
{
    public static string Mask(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return string.Empty;
        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length < 2) return "**";
        var last = digits.Substring(digits.Length - 2);
        return new string('*', Math.Max(0, digits.Length - 2)) + last;
    }
}
