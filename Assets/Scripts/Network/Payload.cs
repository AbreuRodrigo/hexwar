using System.Xml.Serialization;

[XmlRoot("payload")]
public class Payload
{
    [XmlElement("code")]
    public short code { get; set; }

    [XmlElement("message")]
    public string message { get; set; }

    [XmlElement("clientID")]
    public string clientID { get; set; }
}