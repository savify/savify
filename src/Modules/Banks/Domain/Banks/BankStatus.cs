namespace App.Modules.Banks.Domain.Banks;

public record BankStatus(string Value)
{
    public static BankStatus Enabled = new(nameof(Enabled));

    public static BankStatus Disabled = new(nameof(Disabled));

    public static BankStatus Beta = new(nameof(Beta));
}
