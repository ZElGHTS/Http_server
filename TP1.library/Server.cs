using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TP1.library
{
    public class Server
    {
        private readonly int _port;
        private static bool _running;
        private static TcpListener _listener;

        public Server(int port)
        {
            _port = port;
            InitHost();
            _running = true;
        }
        
        public void Start()
        {
            while (_running)
            {
                var socket = _listener.AcceptSocket();
                
                var thread = new Thread(() => HandleClientConnection(socket));
                thread.Start();
            }
        }

        private void HandleClientConnection(Socket socket)
        {
            Thread.Sleep(50);
            var request = new Request(socket);
            var response = new Response(socket);
            var headers = new Header();
            
            request.ReceiveRequest(headers);
            response.StartResponse(headers);          
            
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void InitHost()
        {
            var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            _listener = new TcpListener(ipAddress, _port);
            _listener.Start();
        }
    }
}
