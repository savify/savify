using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Rules;

public class ConfirmationCodeMustMatchRule : IBusinessRule
{
    private readonly ConfirmationCode _providedConfirmationCode;

    private readonly ConfirmationCode _expectedConfirmationCode;

    public ConfirmationCodeMustMatchRule(ConfirmationCode providedConfirmationCode, ConfirmationCode expectedConfirmationCode)
    {
        _providedConfirmationCode = providedConfirmationCode;
        _expectedConfirmationCode = expectedConfirmationCode;
    }

    public bool IsBroken() => _providedConfirmationCode != _expectedConfirmationCode;

    public string MessageTemplate => "Provided confirmation code '{0}' is invalid";

    public object[] MessageArguments => new object[] { _providedConfirmationCode.Value };
}
