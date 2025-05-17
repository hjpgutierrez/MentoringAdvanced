using Carting.BLL.Models;

namespace Carting.BLL.DTOs
{
    public record CartReponseDtoV1(string code, IList<Item> items);
}
