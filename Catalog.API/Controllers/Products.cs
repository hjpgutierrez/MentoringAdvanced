using Catalog.API.Helpers;
using Catalog.Application.Models;
using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;

namespace Catalog.API.Controllers
{
    public class Products : EndpointGroupBase
    {
        public override void Map(WebApplication app) => app.MapGroup(this).WithOpenApi()
                .MapGet(GetProductsByCategoryIdWithPagination)
                .MapGet(GetProduct, "{id}")
                .MapPost(CreateProduct)
                .MapPut(UpdateProduct, "{id}")
                .MapDelete(DeleteProduct, "{id}")
                ;

        public async Task<PaginatedList<ProductDto>> GetProductsByCategoryIdWithPagination(ISender sender, [AsParameters] GetProductsByCategoryIdWithPaginationQuery query, HttpContext httpContext)
        {
            var response = await sender.Send(query);
            response.Items.ForEach(product =>
            {
                product.Href = UrlHelper.GetUrl(httpContext, product.Id.ToString());
            });
            return response;
        }

        public Task<ProductDto> GetProduct(ISender sender, [AsParameters] GetProductQuery query) => sender.Send(query);

        public Task<int> CreateProduct(ISender sender, CreateProductCommand command) => sender.Send(command);

        public async Task<IResult> UpdateProduct(ISender sender, int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return Results.BadRequest();
            }

            await sender.Send(command);
            return Results.NoContent();
        }

        public async Task<IResult> DeleteProduct(ISender sender, int id)
        {
            await sender.Send(new DeleteProductCommand(id));
            return Results.NoContent();
        }
    }
}
