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

namespace Hexwar
{
    public class UDPReceiver
    {
        private Thread listenThread = null;
        private UdpClient udpClient;

        private IPEndPoint localEP = null;
        private IPEndPoint remoteEP = null;
        private IAsyncResult currentAsyncResult = null;

        public bool Initialized { get; set; }

        private readonly object clientLock = new object();

        public IPEndPoint LocalEndPoint { get { return localEP; } }

        private class UdpState
        {
            public IPEndPoint e;
            public UdpClient c;

            public UdpState(IPEndPoint e, UdpClient c)
            {
                this.e = e;
                this.c = c;
            }
        }

        public UDPReceiver(IPEndPoint localEP, IPEndPoint remoteEP)
        {
            this.localEP = localEP;
            this.remoteEP = remoteEP;
            this.Initialized = false;

            InitUdpClient();
            Init();
        }

        public void StopReceiving()
        {
            listenThread.Abort();
            udpClient.Close();
        }

        public void InitUdpClient()
        {
            if (udpClient != null)
            {
                currentAsyncResult = null;
                udpClient.Close();
            }
            udpClient = new UdpClient();
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(localEP);

            localEP = (IPEndPoint)udpClient.Client.LocalEndPoint;
        }

        public void Init()
        {
            if (!Initialized)
            {
                Initialized = true;
                listenThread = new Thread(StartListening);
                listenThread.Start();
            }
        }

        private void StartListening()
        {
            lock (clientLock)
            {
                UdpState s = new UdpState(localEP, udpClient);
                currentAsyncResult = udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (ar == currentAsyncResult)
            {
                UdpClient c = (UdpClient)((UdpState)(ar.AsyncState)).c;
                IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

                Response response = null;
                Byte[] data = c.EndReceive(ar, ref e);

                if (data.Length > 0)
                {
                    response = ObjectFromByteArray(data);
                    NetworkManager.Instance.ProcessResponse(response);
                }

                UdpState s = new UdpState(e, c);
                currentAsyncResult = udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);

                Debug.Log("Receiver ended successfully...");
            }
        }

        private Response ObjectFromByteArray(byte[] data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Response));
            object raw = serializer.Deserialize(new MemoryStream(data));

            return (Response)raw;
        }
    }
}