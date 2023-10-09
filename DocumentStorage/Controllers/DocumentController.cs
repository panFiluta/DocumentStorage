using DocumentStorage.Factories;
using DocumentStorage.Models;
using DocumentStorage.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DocumentStorage.Controllers
{
    [ApiController]
    [Route("documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentStorageFactory _storageFactory;
        private readonly IDocumentStorage _storage;
        private readonly ILogger _logger;  


        public DocumentController(IDocumentStorageFactory storageFactory, ILogger<DocumentController> logger)
        {
            _storageFactory = storageFactory;
            _storage = _storageFactory.CreateDocumentStorage();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Document document)
        {
            try
            {
                _logger.LogInformation("Received POST request with Document: {Document}", JsonConvert.SerializeObject(document));

                var storedDocument = await _storage.StoreDocumentAsync(document);
                return CreatedAtAction("Get", new { id = storedDocument.Id }, storedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing POST request: {ErrorMessage}", ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromHeader(Name = "Accept")] string acceptHeader)
        {
            var document = await _storage.GetDocumentAsync(id);

            if (document == null)
                return NotFound();

            // Content negotiation based on the "acceptHeader" (XML, MessagePack, etc.)
            if (acceptHeader.Contains("application/json"))
            {
                // Return JSON
                return Ok(document);
            }
            else if (acceptHeader.Contains("application/xml"))
            {
                // Return XML
                var xmlContent = SerializeToXml(document);
                return Content(xmlContent, "application/xml");
            }
            else if (acceptHeader.Contains("application/msgpack"))
            {
                // Return MessagePack (you would need a MessagePack serializer)
                var msgpackContent = SerializeToMessagePack(document);
                return File(msgpackContent, "application/msgpack");
            }
            else
            {
                // Default to JSON if the requested format is not supported
                return Ok(document);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Document document)
        {
            var updatedDocument = await _storage.UpdateDocumentAsync(id, document);
            return Ok(updatedDocument);
        }

        private string SerializeToXml(Document document)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Document));
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, document);
                return writer.ToString();
            }
        }

        private byte[] SerializeToMessagePack(Document document)
        {
            // MessagePack serialization
            // Example using MessagePack-CSharp: https://github.com/neuecc/MessagePack-CSharp
            var bytes = MessagePack.MessagePackSerializer.Serialize(document);
            return bytes;
        }
    }
}
