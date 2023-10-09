using DocumentStorage.Models;

namespace DocumentStorage.Services
{
    public interface IDocumentStorage
    {
        Task<Document> GetDocumentAsync(string id);
        Task<Document> StoreDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(string id, Document document);
    }
}
