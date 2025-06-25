using System.Data;
using System.Reflection;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;

namespace Catalog.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IUser _user;

    public AuthorizationBehaviour(IUser user)
    {
        _user = user;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            EnsureUserIsAuthenticated();

            if (HasAuthorization(authorizeAttributes, a => !string.IsNullOrWhiteSpace(a.Roles))
                && !IsUserAuthorizedByRoles(authorizeAttributes))
            {
                throw new ForbiddenAccessException();
            }

            if (HasAuthorization(authorizeAttributes, a => !string.IsNullOrWhiteSpace(a.Permission))
                && !IsUserAuthorizedByPermissions(authorizeAttributes))
            {
                throw new ForbiddenAccessException();
            }
        }

        // User is authorized / authorization not required
        return await next();
    }

    private void EnsureUserIsAuthenticated()
    {
        if (_user.Id == null)
        {
            throw new UnauthorizedAccessException();
        }
    }

    private bool HasAuthorization(IEnumerable<AuthorizeAttribute> authorizeAttributes, Func<AuthorizeAttribute, bool> predicate)
        => authorizeAttributes.Any(predicate);

    private bool IsUserAuthorizedByRoles(IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        var roles = authorizeAttributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
            .SelectMany(a => a.Roles.Split(','));

        return roles.Any(role => _user!.Roles!.Contains(role));
    }


    private bool IsUserAuthorizedByPermissions(IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        var permissions = authorizeAttributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Permission))
            .SelectMany(a => a.Permission.Split(','));

        return permissions.Any(permission => _user!.Permissions!.Contains(permission));
    }
}
