using System;
using System.Net;
using System.Net.Sockets;

namespace ServerLib
{
    public class Server
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);

        private IPAddress _ipAddress;
        private int _port;
        private int _bufferSize = 2000;

        private TcpListener _tcpListener;
        private NetworkStream _networkStream;

        public Server(IPAddress IP, int port)
        {
            _ipAddress = IP;
            _port = port;
        }

        private void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                _networkStream = tcpClient.GetStream();

                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                transmissionDelegate.BeginInvoke(_networkStream, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            client.Close();
        }

        private void BeginDataTransmission(NetworkStream stream)
        {
            LoginSystem.loginSystem(stream);

            while (true)
            {
            }
        }

        public void Start()
        {
            Console.WriteLine("Starting server");

            _tcpListener = new TcpListener(_ipAddress, _port);
            _tcpListener.Start();

            AcceptClient();
        }
    }
}