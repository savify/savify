using App.Modules.FinanceTracking.Domain.Users.Tags;

namespace App.Modules.FinanceTracking.UnitTests.Users.Tags;

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
        var userTags = UserTags.Create(userId);
        _repository.GetByUserIdOrDefaultAsync(userId).Returns(userTags);

        string[] tags = ["tag1", "tag2"];

        // Act
        await _service.UpdateAsync(userId, tags);

        // Assert
        Assert.That(userTags.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public async Task Update_WhenUserTagsDoesNotExist_CreatesNewOne()
    {
        // Arrange
        UserTags? notExistingUserTags = null;

        var userId = new Domain.Users.UserId(Guid.NewGuid());
        _repository.GetByUserIdOrDefaultAsync(userId).Returns(notExistingUserTags);

        string[] tags = ["tag"];

        // Act
        await _service.UpdateAsync(userId, tags);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Is<UserTags>(u => u.UserId == userId));
    }
}
