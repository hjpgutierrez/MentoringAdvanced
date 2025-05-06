using Catalog.Application.Products.Queries;
using Catalog.Application.Models;
using Catalog.Application.Products.Commands;

namespace Catalog.API.Controllers
{
    public class Products : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this).WithOpenApi()
                .MapGet(GetProductsByCategoryIdWithPagination)
                .MapGet(GetProduct, "{id}")
                .MapPost(CreateProduct)
                .MapPut(UpdateProduct, "{id}")
                .MapDelete(DeleteProduct, "{id}")
                ;
        }

        public Task<PaginatedList<ProductDto>> GetProductsByCategoryIdWithPagination(ISender sender, [AsParameters] GetProductsByCategoryIdWithPaginationQuery query)
        {
            return sender.Send(query);
        }

        public Task<ProductDto> GetProduct(ISender sender, [AsParameters] GetProductQuery query)
        {
            return sender.Send(query);
        }

        public Task<int> CreateProduct(ISender sender, CreateProductCommand command)
        {
            return sender.Send(command);
        }

        public async Task<IResult> UpdateProduct(ISender sender, int id, UpdateProductCommand command)
        {
            if (id != command.Id) return Results.BadRequest();
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
