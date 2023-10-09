using DocumentStorage.Models;
using DocumentStorage.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;

namespace DocumentStorage.Tests.StorageTests
{
    public class InMemoryDocumentStorageTests
    {
        private readonly InMemoryDocumentStorage _storage;

        public InMemoryDocumentStorageTests()
        {
            _storage = new InMemoryDocumentStorage(Mock.Of<ILogger<InMemoryDocumentStorage>>());
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
            await _storage.StoreDocumentAsync(document);

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

        [Fact]
        public async Task StoreDocumentAsync_AddsDocumentToStore()
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
        public async Task UpdateDocumentAsync_ExistingDocument_UpdatesDocument()
        {
            // Arrange
            var documentId = "existing-doc";
            var document = new Document 
            { 
                Id = documentId, 
                Tags = new List<string>(), 
                Data = new JObject() 
            };
            await _storage.StoreDocumentAsync(document);

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

        [Fact]
        public async Task UpdateDocumentAsync_NonExistentDocument_ReturnsNull()
        {
            // Arrange
            var documentId = "nonexistent-doc";
            var updatedDocument = new Document 
            { 
                Id = documentId, 
                Tags = new List<string> { "updated-tag" }, 
                Data = new JObject { { "key", "value" } } 
            };

            // Act
            var result = await _storage.UpdateDocumentAsync(documentId, updatedDocument);

            // Assert
            Assert.Null(result);
        }
    }
}
