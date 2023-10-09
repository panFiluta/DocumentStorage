using DocumentStorage.Controllers;
using DocumentStorage.Factories;
using DocumentStorage.Models;
using DocumentStorage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;

namespace DocumentStorage.Tests.ControllerTests
{
    public class DocumentControllerTests
    {
        private readonly DocumentController _controller;
        private readonly Mock<IDocumentStorage> _mockStorage;
        private readonly Mock<IDocumentStorageFactory> _mockStorageFactory;
        private readonly Mock<ILogger<DocumentController>> _mockLogger;

        public DocumentControllerTests()
        {
            _mockStorageFactory = new Mock<IDocumentStorageFactory>();
            _mockStorage = new Mock<IDocumentStorage>();
            _mockLogger = new Mock<ILogger<DocumentController>>();

            _mockStorageFactory.Setup(f => f.CreateDocumentStorage()).Returns(_mockStorage.Object);

            _controller = new DocumentController(_mockStorageFactory.Object, _mockLogger.Object);       
        }

        [Fact]
        public async Task Get_ValidDocumentId_ReturnsOk()
        {
            // Arrange
            var documentId = "valid-id";
            var mockDocument = new Document
            {
                Id = documentId,
                Tags = new List<string> { "tag1", "tag2" },
                Data = new JObject { { "key", "value" } }
            };
            _mockStorage.Setup(s => s.GetDocumentAsync(documentId)).ReturnsAsync(mockDocument);

            // Act
            var result = await _controller.Get(documentId, acceptHeader: "application/json");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDocument = Assert.IsType<Document>(okResult.Value);
            Assert.Equal(documentId, returnedDocument.Id);
        }

        [Fact]
        public async Task Get_NonExistentDocumentId_ReturnsNotFound()
        {
            // Arrange
            var documentId = "nonexistent-id";
            _mockStorage.Setup(s => s.GetDocumentAsync(documentId)).ReturnsAsync(null as Document);

            // Act
            var result = await _controller.Get(documentId, acceptHeader: "application/json");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Add more test cases for different scenarios and content types (XML, MessagePack, etc.)
    }

}
