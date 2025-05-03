using Catalog.Application.Categories.Queries;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public int Amount { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product
            {
                Image = request.Image,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Amount = request.Amount,
                CategoryId = request.CategoryId
            };

            _context.Products.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(ICategoryValidator _categoryValidator)
        {
            RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
            RuleFor(v => v.Price).GreaterThanOrEqualTo(0);
            RuleFor(v => v.Amount).GreaterThan(0);
            RuleFor(v => v.CategoryId).MustAsync(_categoryValidator.BeValidCategoryId).WithMessage("CategoryId not found.");
        }
    }
}