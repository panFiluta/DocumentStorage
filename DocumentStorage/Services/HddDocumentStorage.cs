using ConfigurationConstants;
using DocumentStorage.Models;
using Newtonsoft.Json;

namespace DocumentStorage.Services
{
    public class HddDocumentStorage : IHddDocumentStorage
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HddDocumentStorage> _logger;
        private readonly string _storagePath;

        public HddDocumentStorage(IConfiguration configuration, ILogger<HddDocumentStorage> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _storagePath = _configuration[ConfigurationKeys.HddDocumentStoragePath];
            Directory.CreateDirectory(_storagePath); // Create storage directory if it doesn't exist
        }

        public async Task<Document> GetDocumentAsync(string id)
        {
            var filePath = Path.Combine(_storagePath, $"{id}.json");

            if (!File.Exists(filePath))
            {
                _logger.LogInformation($"Document {id} not found on HDD.");
                return null;
            }

            // Implement code to read document from HDD
            _logger.LogInformation($"Getting document {id} from HDD.");
            var jsonContent = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to a Document object
            return JsonConvert.DeserializeObject<Document>(jsonContent);
        }

        public async Task<Document> StoreDocumentAsync(Document document)
        {
            var filePath = Path.Combine(_storagePath, $"{document.Id}.json");

            // Serialize the document to JSON
            var jsonContent = JsonConvert.SerializeObject(document);

            // Implement code to store document on HDD
            _logger.LogInformation($"Storing document {document.Id} on HDD.");
            await File.WriteAllTextAsync(filePath, jsonContent);

            return document;
        }

        public async Task<Document> UpdateDocumentAsync(string id, Document document)
        {
            var existingDocument = await GetDocumentAsync(id);

            if (existingDocument == null)
            {
                _logger.LogInformation($"Document {id} not found on HDD. Cannot update.");
                return null;
            }

            // Update the existing document
            existingDocument.Tags = document.Tags;
            existingDocument.Data = document.Data;

            // Store the updated document
            await StoreDocumentAsync(existingDocument);

            return existingDocument;
        }
    }

}
