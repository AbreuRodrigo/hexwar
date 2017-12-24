using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Hexwar
{
    public class Tester
    {
        private static string localHost = "127.0.0.1";
        private IPEndPoint localEP = null;
        private IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(localHost), 7777);
        private Thread listenThread = null;
        private UdpClient udpClient = null;
        private IAsyncResult currentAsyncResult = null;
        private int localPort = 11777;

        public void Send(string message)
        {
            PrepareClient(localPort);
            SendBytes(System.Text.Encoding.UTF8.GetBytes(message));
        }

        private void PrepareClient(int localPort)
        {
            if (localEP == null)
            {
                localEP = new IPEndPoint(IPAddress.Parse(localHost), localPort);
                listenThread = new Thread(StartListening);
                listenThread.Start();
            }
        }

        private readonly object _clientLock = new object();

        private void StartListening()
        {
            lock (_clientLock)
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

                var s = new UdpState(localEP, udpClient);
                currentAsyncResult = udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (ar == currentAsyncResult)
            {
                UdpClient c = (UdpClient)((UdpState)(ar.AsyncState)).c;
                IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

                Byte[] buffer = c.EndReceive(ar, ref e);
                if (buffer.Length > 0)
                    System.Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));

                UdpState s = new UdpState(e, c);
                currentAsyncResult = udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            }
        }

        private void SendBytes(byte[] messageData)
        {
            var sender = new UdpClient();
            sender.ExclusiveAddressUse = false;
            sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            sender.Client.Bind(localEP);
            sender.Send(messageData, messageData.Length, remoteEP);
            sender.Close();
        }

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
    }
}