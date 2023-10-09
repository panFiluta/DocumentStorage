using DocumentStorage.Services;

namespace DocumentStorage.Factories
{
    public interface IDocumentStorageFactory
    {
        IDocumentStorage CreateDocumentStorage();
    }
}
