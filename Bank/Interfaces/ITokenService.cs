using System.Security.Claims;

namespace Bank.Interfaces
{
    public interface ITokenService
    {
        string GetUserIdFromClaims (ClaimsPrincipal user);
        Task<bool> UserCanAccess(ClaimsPrincipal user, int id);

    }
}
