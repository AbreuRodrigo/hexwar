using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Hexwar
{
    public class TcpConnector : MonoBehaviour
    {
        public string m_IPAdress = "192.168.0.11";
        public const int kPort = 7777;
        public int bytes;
        private Socket m_Socket;
        public byte[] bytesReceived;
        public string receivedData = "";

        void Awake()
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(m_IPAdress);
            System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, kPort);
            m_Socket.Connect(remoteEndPoint);
        }

        void Update()
        {
            bytes = m_Socket.Receive(bytesReceived, bytesReceived.Length, 0);
            while (bytes != 0)
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                string receivedData = encoding.GetString(bytesReceived);
            }
        }

        void OnApplicationQuit()
        {
            m_Socket.Close();
            m_Socket = null;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 40, 1000, 400), receivedData);
        }
    }
}