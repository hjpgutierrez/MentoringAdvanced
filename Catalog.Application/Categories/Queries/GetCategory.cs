using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Security;

namespace Catalog.Application.Categories.Queries
{
    [Authorize(Permission = "read:catalog")]
    public record GetCategoryQuery(int Id): IRequest<CategoryDto>;

    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.AsNoTracking()
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);
            return entity;
        }
    }
}
