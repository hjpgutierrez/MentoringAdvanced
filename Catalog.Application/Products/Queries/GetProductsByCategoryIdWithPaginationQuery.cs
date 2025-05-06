using Catalog.Application.Common.Interfaces;
using Catalog.Application.Models;
using Catalog.Domain.Common;

namespace Catalog.Application.Products.Queries
{
    public class GetProductsByCategoryIdWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public int CategoryId { get; set; }
    }

    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetProductsByCategoryIdWithPaginationQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
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
}
