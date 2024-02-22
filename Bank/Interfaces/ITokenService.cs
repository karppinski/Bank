using System.Security.Claims;

namespace Bank.Interfaces
{
    public interface ITokenService
    {
        string GetUserIdFromClaims (ClaimsPrincipal user);
        Task<bool> UserCanAccessAccount(ClaimsPrincipal user, int id);
        bool UserCanAccessUser(ClaimsPrincipal user, string userIdFromRoute);

    }
}
