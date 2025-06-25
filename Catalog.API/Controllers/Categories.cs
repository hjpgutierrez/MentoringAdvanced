using Catalog.API.Helpers;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Categories.Queries;
using Catalog.Application.Models;

namespace Catalog.API.Controllers
{
    public class Categories : EndpointGroupBase
    {
        public override void Map(WebApplication app) => app.MapGroup(this).WithOpenApi()
                .MapGet(GetCategoriesWithPagination)
                .MapGet(GetCategory, "{id}")
                .MapPost(CreateCategory)
                .MapPut(UpdateCategory, "{id}")
                .MapDelete(DeleteCategory, "{id}");

        public async Task<PaginatedList<CategoryDto>> GetCategoriesWithPagination(ISender sender, [AsParameters] GetCategoriesWithPaginationQuery query, HttpContext httpContext)
        {
            var response = await sender.Send(query);
            response.Items.ForEach(product =>
            {
                product.Href = UrlHelper.GetUrl(httpContext, product.Id.ToString());
            });

            return response;
        }

        public Task<CategoryDto> GetCategory(ISender sender, [AsParameters] GetCategoryQuery query) => sender.Send(query);

        public Task<int> CreateCategory(ISender sender, CreateCategoryCommand command) => sender.Send(command);

        public async Task<IResult> UpdateCategory(ISender sender, int id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

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
