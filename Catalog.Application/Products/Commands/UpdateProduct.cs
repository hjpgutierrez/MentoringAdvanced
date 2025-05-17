using Catalog.Application.Categories.Queries;
using Catalog.Application.Common.Interfaces;

namespace Catalog.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public int Amount { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            Guard.Against.NotFound(request.Id, entity);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Image = request.Image;
            entity.CategoryId = request.CategoryId;
            entity.Price = request.Price;
            entity.Amount = request.Amount;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(ICategoryValidator _categoryValidator)
        {
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
            RuleFor(v => v.CategoryId)
                .GreaterThan(0);
            RuleFor(v => v.Price)
                .GreaterThan(0);
            RuleFor(v => v.Amount)
                .GreaterThan(0);
            RuleFor(v => v.CategoryId).MustAsync(_categoryValidator.BeValidCategoryId).WithMessage("CategoryId not found.");
        }
    }
}
