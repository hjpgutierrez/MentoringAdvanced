using Catalog.Application.Categories.Queries;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;
using Catalog.Application.Models;
using Catalog.Domain.Common;

namespace Catalog.Application.Products.Queries
{
    [Authorize(Permission = "read:catalog")]
    public class GetProductsByCategoryIdWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public int CategoryId { get; set; }
    }

    public class GetProductsByCategoryIdWithPaginationQueryHandler : IRequestHandler<GetProductsByCategoryIdWithPaginationQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsByCategoryIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProductDto>> Handle(GetProductsByCategoryIdWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.Include(p => p.Category)
                .Where(p => p.CategoryId == request.CategoryId)
                .OrderBy(x => x.Name)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }

    public class GetProductsByCategoryIdWithPaginationQueryValidator : AbstractValidator<GetProductsByCategoryIdWithPaginationQuery>
    {
        public GetProductsByCategoryIdWithPaginationQueryValidator(ICategoryValidator _categoryValidator)
        {
            RuleFor(v => v.PageSize).GreaterThan(0);
            RuleFor(v => v.PageNumber).GreaterThan(0);
            RuleFor(v => v.CategoryId)
            .MustAsync(_categoryValidator.BeValidCategoryId).WithMessage("CategoryId not found.");
        }
    }
}
