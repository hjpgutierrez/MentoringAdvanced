﻿using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;

namespace Catalog.Application.Categories.Commands
{
    [Authorize(Roles = "Manager")]
    public record DeleteCategoryCommand(int Id) : IRequest;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .FindAsync(new object[] { request.Id }, cancellationToken);
            Guard.Against.NotFound(request.Id, entity);

            _context.Categories.Remove(entity);
            var productToDelete = _context.Products.Where(p => p.CategoryId == request.Id);
            _context.Products.RemoveRange(productToDelete);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
