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
    private IPEndPoint localEndPoint = null;
    private IPEndPoint remoteEndPoint = null;
    private IPAddress ip;
    private int port;
    private UdpClient udpClient;
    
    public UDPSender(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
    {
        this.localEndPoint = localEndPoint;
        this.remoteEndPoint = remoteEndPoint;
        this.udpClient = new UdpClient();
        this.ip = remoteEndPoint.Address;
        this.port = remoteEndPoint.Port;
    }

    public void SendPayload(Payload payload)
    {
        try
        {
            string data = MountPayloadString(payload);
            Send(data);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void Send(string message)
    {
        byte[] messageData = System.Text.Encoding.UTF8.GetBytes(message);

        UdpClient sender = new UdpClient();
        sender.ExclusiveAddressUse = false;
        sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        sender.Client.Bind(localEndPoint);
        sender.Send(messageData, messageData.Length, remoteEndPoint);
        sender.Close();
    }

    private string MountPayloadString(Payload payload)
    {
        string payloadStr = "<payload><code>{0}</code><message>{1}</message><clientID>{2}</clientID></payload>";

        payloadStr = string.Format(payloadStr, payload.code, payload.message, payload.clientID);

        return payloadStr;
    }

    private static string Serialize<T>(T toSerialize)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StringWriter textWriter = new StringWriter();

        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        xmlSerializer.Serialize(textWriter, toSerialize, ns);
        return textWriter.ToString();
    }
}