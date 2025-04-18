using Catalog.Application.Categories.Queries;
using Catalog.Domain.Entities;

namespace Catalog.Application.Common.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
                CreateMap<Category, CategoryDto>()
                                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));
        }
    }
}
