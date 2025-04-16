using Carting.BLL.Models;
using FluentValidation;

namespace Carting.BLL.Validations
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(item => item.Id).GreaterThan(0);
            RuleFor(item => item.Name).NotNull().NotEmpty();
            RuleFor(item => item.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(item => item.Quantity).NotNull().GreaterThanOrEqualTo(1);
        }
    }
}
