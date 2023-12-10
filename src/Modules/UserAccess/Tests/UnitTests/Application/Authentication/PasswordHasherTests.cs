using App.Modules.UserAccess.Application.Authentication;

namespace App.Modules.UserAccess.UnitTests.Application.Authentication;

[TestFixture]
public class PasswordHasherTests : UnitTestBase
{
    [Test]
    public void IsPasswordValid_ReturnsTrue_ForValidPassword()
    {
        var plainPassword = "password";

        var hashedPassword = PasswordHasher.HashPassword(plainPassword);

        Assert.That(PasswordHasher.IsPasswordValid(hashedPassword, plainPassword), Is.True);
    }

    [Test]
    public void IsPasswordValid_ReturnsFalse_ForInvalidPassword()
    {
        var plainPassword = "password";

        var hashedPassword = PasswordHasher.HashPassword(plainPassword);

        Assert.That(PasswordHasher.IsPasswordValid(hashedPassword, "invalid-password"), Is.False);
    }
}
