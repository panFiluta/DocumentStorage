using DocumentStorage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DocumentStorage.Models;
using ConfigurationConstants;

namespace DocumentStorage.Tests.StorageTests
{
    public class HddDocumentStorageTests
    {
        private readonly HddDocumentStorage _storage;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly string _storagePath; 

        public HddDocumentStorageTests()
        {
            _storagePath = "test-storage-path";
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c[ConfigurationKeys.HddDocumentStoragePath]).Returns(_storagePath);
            _storage = new HddDocumentStorage(_mockConfiguration.Object, Mock.Of<ILogger<HddDocumentStorage>>());
        }

        [Fact]
        public async Task GetDocumentAsync_ExistingDocument_ReturnsDocument()
        {
            // Arrange
            var documentId = "existing-doc";
            var document = new Document 
            { 
                Id = documentId, 
                Tags = new List<string>(), 
                Data = new JObject() 
            };
            
            var storagePath = Path.Combine(_storagePath, $"{documentId}.json");
            File.WriteAllText(storagePath, JsonConvert.SerializeObject(document));

            // Act
            var result = await _storage.GetDocumentAsync(documentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(documentId, result.Id);
        }

        [Fact]
        public async Task GetDocumentAsync_NonExistentDocument_ReturnsNull()
        {
            // Arrange
            var documentId = "nonexistent-doc";

            // Act
            var result = await _storage.GetDocumentAsync(documentId);

            // Assert
            Assert.Null(result);
        }
        // Add more test cases for StoreDocumentAsync and UpdateDocumentAsync
    }
}
