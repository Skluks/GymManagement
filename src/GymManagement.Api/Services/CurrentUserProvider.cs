using System.Security.Claims;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        Claim? idClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");

        var permissionClaims = _httpContextAccessor.HttpContext.User.Claims
            .Where(x => x.Type == "permissions")
            .Select(x => x.Value)
            .ToList();

        return new CurrentUser(Guid.Parse(idClaim!.Value), permissionClaims);
    }
}
