using Catalog.Application.Common.Interfaces;

namespace Catalog.Application.Categories.Queries
{
    public class CategoryValidator : ICategoryValidator
    {
        private readonly IApplicationDbContext _context;

        public CategoryValidator(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BeValidParentCategoryId(int? parentCategoryId, CancellationToken cancellationToken)
        {
            if (!parentCategoryId.HasValue)
                return true;

            if (IsZeroOrNegative(parentCategoryId.Value)) 
                return false;

            return await BeValidCategoryId(parentCategoryId.Value, cancellationToken);
        }

        public async Task<bool> BeValidCategoryId(int categoryId, CancellationToken cancellationToken)
        {
            if (IsZeroOrNegative(categoryId))
                return false;

            return await _context.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
        }

        private bool IsZeroOrNegative(int categoryId)
        {
            if (categoryId <= 0)
                return true;

            return false;
        }
    }

    public interface ICategoryValidator
    {

        Task<bool> BeValidParentCategoryId(int? parentCategoryId, CancellationToken cancellationToken);

        Task<bool> BeValidCategoryId(int categoryId, CancellationToken cancellationToken);
    }
}
