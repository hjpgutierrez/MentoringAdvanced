using Catalog.Application.Categories.Queries;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;

namespace Catalog.Application.Categories.Commands
{
    [Authorize(Roles = "Manager")]
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; init; }

        public required string Name { get; set; }

        public string? Image { get; set; }

        public int? ParentCategoryId { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
            Guard.Against.NotFound(request.Id, entity);

            entity.Name = request.Name;
            entity.Image = request.Image;
            entity.ParentCategoryId = request.ParentCategoryId;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator(ICategoryValidator _categoryValidator)
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.ParentCategoryId)
            .MustAsync(_categoryValidator.BeValidParentCategoryId).WithMessage("ParentCategoryId not found.");
        }
    }
}
