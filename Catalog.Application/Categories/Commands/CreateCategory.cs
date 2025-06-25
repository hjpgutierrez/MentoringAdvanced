using Catalog.Application.Categories.Queries;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;
using Catalog.Domain.Entities;

namespace Catalog.Application.Categories.Commands
{
    [Authorize(Roles = "Manager")]
    public class CreateCategoryCommand : IRequest<int>
    {
        public required string Name { get; set; }

        public string? Image { get; set; }

        public int? ParentCategoryId { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Category
            {
                Image = request.Image,
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId
            };

            _context.Categories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(ICategoryValidator _categoryValidator)
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.ParentCategoryId)
            .MustAsync(_categoryValidator.BeValidParentCategoryId).WithMessage("ParentCategoryId not found.");
        }
    }
}
