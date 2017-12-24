using System.Xml.Serialization;

namespace Hexwar
{
    [XmlRoot("response")]
    public class Response
    {
        [XmlElement("code")]
        public short code { get; set; }

        [XmlElement("message")]
        public string message { get; set; }
    }
}