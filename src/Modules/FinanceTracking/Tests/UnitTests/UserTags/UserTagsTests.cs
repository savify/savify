using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.UnitTests.UserTags;

[TestFixture]
public class UserTagsTests : UnitTestBase
{
    [Test]
    public void CreatingNewUserTags_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());

        var userTags = Domain.Users.Tags.UserTags.Create(userId);

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags.UserId, Is.EqualTo(userId));
    }

    [Test]
    public void UpdatingUserTags_AddsTagsToEntity()
    {
        var userId = new UserId(Guid.NewGuid());
        var userTags = Domain.Users.Tags.UserTags.Create(userId);

        string[] tags = ["tag1", "tag2"];

        userTags.Update(tags);

        Assert.That(userTags.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void UpdatingUserTags_WhenAlreadyContainsTags_DoesNotAddDuplicatedTags()
    {
        // Arrange
        var userId = new UserId(Guid.NewGuid());
        var userTags = Domain.Users.Tags.UserTags.Create(userId);

        string[] tags = ["tag1", "tag2"];

        userTags.Update(tags);
        userTags.ClearDomainEvents();

        string[] newTags = [.. tags, "tag3"];

        // Act
        userTags.Update(newTags);

        // Assert
        Assert.That(userTags.Tags, Has.Exactly(1).EqualTo("tag3"));
    }
}
