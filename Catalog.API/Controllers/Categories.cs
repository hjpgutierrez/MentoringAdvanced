using Catalog.API.Infrastructure;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Categories.Queries;
using Catalog.Application.Models;
using MediatR;

namespace Catalog.API.Controllers
{
    public class Categories : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .MapGet(GetCategoriesWithPagination)
                .MapPost(CreateCategory)
                .MapPut(UpdateCategory, "{id}")
                .MapDelete(DeleteCategory, "{id}");
        }

        public Task<PaginatedList<CategoryDto>> GetCategoriesWithPagination(ISender sender, [AsParameters] GetCategoriesWithPaginationQuery query)
        {
            return sender.Send(query);
        }

        public Task<int> CreateCategory(ISender sender, CreateCategoryCommand command)
        {
            return sender.Send(command);
        }

        public async Task<IResult> UpdateCategory(ISender sender, int id, UpdateCategoryCommand command)
        {
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.NoContent();
        }

        public async Task<IResult> DeleteCategory(ISender sender, int id)
        {
            await sender.Send(new DeleteCategoryCommand(id));
            return Results.NoContent();
        }
    }
}
