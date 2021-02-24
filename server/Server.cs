using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace server
{
    class Session : SslSession
    {
        public Session(SslServer sslServer) : base(sslServer) {}

        protected override void OnConnected()
        {
            Console.WriteLine($"SSL session with Id {Id} connected!");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"SSL session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming: " + message);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"SSL session caught an error with code {error}");
        }
    }

    class Server : SslServer
    {
        public Server(SslContext sslContext, IPAddress address, int port) : base(sslContext, address, port) {}

        protected override SslSession CreateSession() { return new Session(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"SSL server caught an error with code {error}");
        }
    }
}