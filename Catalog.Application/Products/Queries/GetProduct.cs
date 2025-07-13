using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;

namespace Catalog.Application.Products.Queries
{
    [Authorize(Permission = "read:catalog")]
    public record GetProductQuery(int Id) : IRequest<ProductDto>;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.Include(p => p.Category)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);
            return entity;
        }
    }
}
