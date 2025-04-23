using Ardalis.GuardClauses;
using Catalog.Application.Common.Interfaces;

namespace Catalog.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public string Image { get; set; }

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
        public UpdateCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
