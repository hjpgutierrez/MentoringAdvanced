using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;

namespace Catalog.Application.Products.Commands
{
    [Authorize(Roles = "Manager")]
    public record DeleteProductCommand(int Id) : IRequest;

    public class DeleteProductsCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FindAsync(new object[] { request.Id }, cancellationToken);
            Guard.Against.NotFound(request.Id, entity);

            _context.Products.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
