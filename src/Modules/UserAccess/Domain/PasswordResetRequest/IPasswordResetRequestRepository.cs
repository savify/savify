using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest;

public interface IPasswordResetRequestRepository
{
    public Task AddAsync(PasswordResetRequest passwordResetRequest);

    public Task<PasswordResetRequest> GetByIdAsync(PasswordResetRequestId id);

    public PasswordResetRequest? GetActiveByUserIdOrNullAsync(UserId userId);
}
