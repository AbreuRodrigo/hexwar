using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class UDPSender
{
    private IPEndPoint remoteEndPoint;
    private IPAddress ip;
    private int port;
    private UdpClient udpClient;
    
    public UDPSender(IPEndPoint remoteEndPoint)
    {
        this.remoteEndPoint = remoteEndPoint;
        this.udpClient = new UdpClient();
        this.ip = remoteEndPoint.Address;
        this.port = remoteEndPoint.Port;
    }

    public void SendPayload(Payload payload)
    {
        try
        {
            string xmlString = ObjectToXml(payload, typeof(Payload));

            byte[] data = Encoding.UTF8.GetBytes(xmlString);
            udpClient.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
    
    private string ObjectToXml(object obj, Type t)
    {
        XmlSerializer serializer = new XmlSerializer(t);
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = new UnicodeEncoding(false, false);
        settings.Indent = false;
        settings.OmitXmlDeclaration = true;

        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        using (StringWriter textWriter = new StringWriter())
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                serializer.Serialize(xmlWriter, obj, ns);
            }

            return textWriter.ToString();
        }
    }
}