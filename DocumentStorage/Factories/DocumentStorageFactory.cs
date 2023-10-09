using DocumentStorage.Configuration;
using DocumentStorage.Services;
using Microsoft.Extensions.Options;

namespace DocumentStorage.Factories
{
    public class DocumentStorageFactory : IDocumentStorageFactory
    {
        private readonly StorageSettings _storageSettings;
        private readonly IServiceProvider _serviceProvider;

        public DocumentStorageFactory(IOptions<StorageSettings> storageSettings, IServiceProvider serviceProvider)
        {
            _storageSettings = storageSettings.Value;
            _serviceProvider = serviceProvider;
        }

        public IDocumentStorage CreateDocumentStorage()
        {
            if (_storageSettings.UseCloudStorage)
            {
                return _serviceProvider.GetService<ICloudDocumentStorage>(); // Use a cloud storage implementation
            }
            else
            {
                return _serviceProvider.GetService<IHddDocumentStorage>(); // Use an HDD storage implementation
            }
        }
    }

}
