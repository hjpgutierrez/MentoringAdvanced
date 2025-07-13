namespace Catalog.Application.Common.Interfaces;

public interface IUser
{
    string? Id { get; }

    IEnumerable<string>? Roles { get; }

    IEnumerable<string>? Permissions { get; }
}
