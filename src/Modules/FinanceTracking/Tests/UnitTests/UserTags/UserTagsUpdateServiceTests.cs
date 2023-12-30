using App.Modules.FinanceTracking.Domain.Users.Tags;
using App.Modules.FinanceTracking.Domain.Users.Tags.Events;

namespace App.Modules.FinanceTracking.UnitTests.UserTags;

[TestFixture]
public class UserTagsUpdateServiceTests : UnitTestBase
{
    private readonly UserTagsUpdateService _service;
    private readonly IUserTagsRepository _repository;

    public UserTagsUpdateServiceTests()
    {
        _repository = Substitute.For<IUserTagsRepository>();
        _service = new UserTagsUpdateService(_repository);
    }

    [Test]
    public async Task Update_UpdatesUserTags()
    {
        // Arrange
        var userId = new Domain.Users.UserId(Guid.NewGuid());
        var userTags = Domain.Users.Tags.UserTags.Create(userId);
        _repository.GetByUserIdOrDefaultAsync(userId).Returns(userTags);

        string[] tags = ["tag1", "tag2"];

        // Act
        await _service.UpdateAsync(userId, tags);

        // Assert
        var userTagsUpdatedDomainEvent = AssertPublishedDomainEvent<UserTagsUpdatedDomainEvent>(userTags);
        Assert.That(userTagsUpdatedDomainEvent, Is.Not.Null);
        Assert.That(userTagsUpdatedDomainEvent.NewTags, Is.EquivalentTo(tags));
    }

    [Test]
    public async Task Update_WhenUserTagsDoesNotExist_CreatesNewOne()
    {
        // Arrange
        Domain.Users.Tags.UserTags? notExistingUserTags = null;

        var userId = new Domain.Users.UserId(Guid.NewGuid());
        _repository.GetByUserIdOrDefaultAsync(userId).Returns(notExistingUserTags);

        string[] tags = ["tag"];

        // Act
        await _service.UpdateAsync(userId, tags);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Is<Domain.Users.Tags.UserTags>(u => u.UserId == userId));
    }
}
