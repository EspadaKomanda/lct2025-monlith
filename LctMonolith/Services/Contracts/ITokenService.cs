using LctMonolith.Models.Database;
using LctMonolith.Services.Models;

namespace LctMonolith.Services.Contracts;

public interface ITokenService
{
    Task<TokenPair> IssueAsync(AppUser user, CancellationToken ct = default);
    Task<TokenPair> RefreshAsync(string refreshToken, CancellationToken ct = default);
    Task RevokeAsync(string refreshToken, CancellationToken ct = default);
}
