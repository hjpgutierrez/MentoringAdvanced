using Catalog.Application.Common.Interfaces;
using Catalog.Application.Models;
using Catalog.Domain.Common;

namespace Catalog.Application.Categories.Queries
{
    public class GetCategoriesWithPaginationQuery : IRequest<PaginatedList<CategoryDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetCategoriesWithPaginationQuery, PaginatedList<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CategoryDto>> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .OrderBy(x => x.Name)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }

    public class GetCategoriesWithPaginationQueryValidator : AbstractValidator<GetCategoriesWithPaginationQuery>
    {
        public GetCategoriesWithPaginationQueryValidator()
        {
            RuleFor(v => v.PageSize).GreaterThan(0);
            RuleFor(v => v.PageNumber).GreaterThan(0);
        }
    }
}
