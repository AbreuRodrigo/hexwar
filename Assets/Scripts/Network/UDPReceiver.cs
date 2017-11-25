using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Runtime.Serialization;

public class UDPReceiver
{
    private const int RECEIVING_PORT = 11777;

    private Thread receiveThread;
    private IPEndPoint remoteEndPoint;
    private UdpClient udpClient;
    private bool running = true;


    public UDPReceiver(IPEndPoint remoteEndPoint)
    {
        this.remoteEndPoint = remoteEndPoint;
        Init();
    }

    public void StopReceiving()
    {
        running = false;
        receiveThread.Abort();
        udpClient.Close();
    }

    private void Init()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, RECEIVING_PORT);
        udpClient = new UdpClient(anyIP);
        byte[] data = null;
        Response response = null;

        while (running)
        {
            try
            {
                data = udpClient.Receive(ref anyIP);
                response = ObjectFromByteArray(data);

                NetworkManager.Instance.ProcessResponse(response);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }

        Debug.Log("Receiver ended successfully...");
    }

    private Response ObjectFromByteArray(byte[] data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Response));
        object raw = serializer.Deserialize(new MemoryStream(data));

        return (Response)raw;
    }
}