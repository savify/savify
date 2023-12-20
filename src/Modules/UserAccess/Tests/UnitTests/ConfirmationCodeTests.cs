using App.Modules.UserAccess.Domain;

namespace App.Modules.UserAccess.UnitTests;

[TestFixture]
public class ConfirmationCodeTests : UnitTestBase
{
    [Test]
    public void ConfirmationCode_Generate_ShouldGenerateUniqueCode()
    {
        var code1 = ConfirmationCode.Generate();
        var code2 = ConfirmationCode.Generate();

        Assert.That(code1, Is.Not.EqualTo(code2));
    }
}
