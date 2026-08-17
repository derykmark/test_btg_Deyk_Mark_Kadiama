namespace BankKRT.Domain.ValueObjects;

public sealed class CPF : IEquatable<CPF>
{
    public string Value { get; }

    private CPF(string value)
    {
        Value = value;
    }

    public static CPF Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CPF cannot be empty.", nameof(value));

        var cleanValue = new string(value.Where(char.IsDigit).ToArray());

        if (!IsValidCpf(cleanValue))
            throw new ArgumentException("Invalid CPF.", nameof(value));

        return new CPF(cleanValue);
    }

    private static bool IsValidCpf(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        string digit = remainder.ToString();
        tempCpf = tempCpf + digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        digit = digit + remainder.ToString();

        return cpf.EndsWith(digit);
    }

    public static implicit operator string(CPF cpf) => cpf.Value;
    public static explicit operator CPF(string value) => Create(value);

    public override bool Equals(object? obj) => obj is CPF cpf && Equals(cpf);
    public bool Equals(CPF? other) => other is not null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
