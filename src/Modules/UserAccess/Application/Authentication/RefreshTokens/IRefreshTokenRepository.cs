namespace App.Modules.UserAccess.Application.Authentication;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenDto?> GetByUserIdAsync(Guid userId);
    
    public Task UpdateAsync(Guid userId, string token, DateTime expiresAt);
}
