using Carting.BLL.Models;

namespace Carting.BLL.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        bool InsertDocument(T item);

        bool UpdateDocument(T item);

        bool DeleteDocument(string id);

        T GetDocumentById(string id);

        IList<Cart> GetDocumentsByItemId(string itemId);
    }
}