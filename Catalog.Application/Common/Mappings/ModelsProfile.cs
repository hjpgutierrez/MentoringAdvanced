using Catalog.Application.Categories.Queries;
using Catalog.Application.Products.Queries;
using Catalog.Domain.Entities;

namespace Catalog.Application.Common.Mappings
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
                CreateMap<Category, CategoryDto>()
                                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));
        }
    }

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                            ;
        }
    }
}
