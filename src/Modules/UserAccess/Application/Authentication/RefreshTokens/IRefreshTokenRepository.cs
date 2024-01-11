namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenDto?> GetByUserIdAsync(Guid userId);

    public Task UpdateAsync(Guid userId, string token, DateTime expiresAt);

    public Task InvalidateAsync(Guid userId);
}
