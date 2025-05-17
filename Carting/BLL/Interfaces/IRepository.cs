namespace Carting.BLL.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        bool InsertDocument(T item);

        bool UpdateDocument(T item);

        bool DeleteDocument(string id);

        T GetDocumentById(string id);

        IList<T> GetDocumentsByItemId(string itemId);
    }
}