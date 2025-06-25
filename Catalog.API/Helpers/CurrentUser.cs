using System.Security.Claims;
using Catalog.Application.Common.Interfaces;


namespace Catalog.API.Helpers;

public class CurrentUser : IUser
{
    private const string RolesClaim = "MentoringAdvanced/roles";
    private const string PermissionsClaim = "permissions";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public IEnumerable<string>? Roles => _httpContextAccessor.HttpContext?.User?.Claims
                                                .Where(c => c.Type == RolesClaim)
                                                .Select(c => c.Value);

    public IEnumerable<string>? Permissions => _httpContextAccessor.HttpContext?.User?.Claims
                                                .Where(c => c.Type == PermissionsClaim)
                                                .Select(c => c.Value);

}
