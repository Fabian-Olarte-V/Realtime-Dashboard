using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Common.Auth
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.Parse(sub!);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("ADMIN");
        }
    }
}
