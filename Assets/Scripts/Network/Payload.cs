using System.Xml.Serialization;

namespace Hexwar
{
    [XmlRoot("payload")]
    public class Payload
    {
        [XmlElement(ElementName = "code")]
        public short code;

        [XmlElement(ElementName = "message")]
        public string message = null;

        [XmlElement(ElementName = "clientID")]
        public string clientID = null;
    }
}