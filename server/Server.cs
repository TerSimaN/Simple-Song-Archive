using System;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace server
{
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