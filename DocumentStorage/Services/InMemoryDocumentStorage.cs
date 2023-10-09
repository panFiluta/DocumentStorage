using System.Collections.Concurrent;
using DocumentStorage.Models;

namespace DocumentStorage.Services
{
    public class InMemoryDocumentStorage : IDocumentStorage
    {
        private readonly ILogger<InMemoryDocumentStorage> _logger;
        private readonly ConcurrentDictionary<string, Document> _documentStore = new ConcurrentDictionary<string, Document>();

        public InMemoryDocumentStorage(ILogger<InMemoryDocumentStorage> logger)
        {
            _logger = logger;
        }

        public async Task<Document> GetDocumentAsync(string id)
        {
            if (_documentStore.TryGetValue(id, out var document))
            {
                _logger.LogInformation($"Getting document {id} from in-memory storage.");
                return document;
            }

            _logger.LogInformation($"Document {id} not found in in-memory storage.");
            return null;
        }

        public async Task<Document> StoreDocumentAsync(Document document)
        {
            _documentStore[document.Id] = document;
            _logger.LogInformation($"Storing document {document.Id} in in-memory storage.");
            return document;
        }

        public async Task<Document> UpdateDocumentAsync(string id, Document document)
        {
            if (_documentStore.TryGetValue(id, out var existingDocument))
            {
                // Update the existing document
                existingDocument.Tags = document.Tags;
                existingDocument.Data = document.Data;
                _logger.LogInformation($"Updating document {id} in in-memory storage.");
                return existingDocument;
            }

            _logger.LogInformation($"Document {id} not found in in-memory storage. Cannot update.");
            return null;
        }
    }

}
