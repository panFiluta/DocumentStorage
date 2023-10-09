using MessagePack;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;

namespace DocumentStorage.Models
{
    [MessagePackObject]
    public class Document
    {
        [Key(0)]
        public string Id { get; set; }
        [Key(1)]
        public List<string> Tags { get; set; }

        [Key(2)]
        [XmlIgnore] // Ignore this property for XML serialization
        public JObject Data { get; set; }

        [Key(3)]
        // Create a property for XML serialization
        [XmlElement("Data")]
        public string DataXml
        {
            get => Data.ToString();
            set => Data = JObject.Parse(value);
        }
    }
}
