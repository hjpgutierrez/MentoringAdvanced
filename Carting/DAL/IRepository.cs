using Carting.BLL.Models;

namespace Carting.DAL
{
    public interface IRepository<T> where T : EntityBase
    {
        bool InsertDocument(T item);

        bool UpdateDocument(T item);

        bool DeleteDocument(string id);

        T GetDocumentById(string id);
    }
}
