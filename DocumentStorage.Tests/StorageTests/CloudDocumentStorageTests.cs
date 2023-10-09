using DocumentStorage.Models;
using DocumentStorage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;

namespace DocumentStorage.Tests.StorageTests
{
    public class CloudDocumentStorageTests
    {
        private readonly CloudDocumentStorage _storage;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public CloudDocumentStorageTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["CloudStorageConnection"]).Returns("your-cloud-connection-string");
            _storage = new CloudDocumentStorage(_mockConfiguration.Object, Mock.Of<ILogger<CloudDocumentStorage>>());
        }

        [Fact]
        public async Task GetDocumentAsync_ExistingDocument_ReturnsDocument()
        {
            // Arrange
            var documentId = "existing-doc";
      
            // Act
            var result = await _storage.GetDocumentAsync(documentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(documentId, result.Id);
        }

        [Fact]
        public async Task StoreDocumentAsync_AddsDocumentToCloud()
        {
            // Arrange
            var documentId = "new-doc";
            var document = new Document 
            { 
                Id = documentId, 
                Tags = new List<string>(), 
                Data = new JObject() 
            };
         
            // Act
            var result = await _storage.StoreDocumentAsync(document);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(documentId, result.Id);
        }

        [Fact]
        public async Task UpdateDocumentAsync_ExistingDocument_UpdatesDocumentInCloud()
        {
            // Arrange
            var documentId = "existing-doc";
            var document = new Document 
            { 
                Id = documentId, 
                Tags = new List<string>(), 
                Data = new JObject() 
            };
       
            var updatedDocument = new Document 
            { 
                Id = documentId, 
                Tags = new List<string> { "updated-tag" }, 
                Data = new JObject { { "key", "value" } } 
            };

            // Act
            var result = await _storage.UpdateDocumentAsync(documentId, updatedDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(documentId, result.Id);
            Assert.Equal(updatedDocument.Tags, result.Tags);
            Assert.Equal(updatedDocument.Data, result.Data);
        }
    }
}
