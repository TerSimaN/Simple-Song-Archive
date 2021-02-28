using System;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using server.protocol;

namespace server
{
    class Session : SslSession
    {
        private Serializer serializer = new Serializer();
        private Request request;
        private bool requestIsSuccessful = false;
        private Random random = new Random();

        public Session(SslServer sslServer) : base(sslServer) { }

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
            try
            {
                request = serializer.UnserializeRequest(buffer);
                requestIsSuccessful = true;
                Console.WriteLine("Incoming: {0}", request.Data);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Request Error: {0}", exception.Message);
            }

            Response<int> response = new Response<int>();
            if (requestIsSuccessful)
            {
                response.Success = true;
                response.Data = random.Next(0, 1024);
            }
            else
            {
                response.ErrorCode = 404;
                response.ErrorMessage = "No message was received!";
            }

            try
            {
                this.Send(serializer.SerializeResponse(response));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Response Error: {0}", exception.Message);
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"SSL session caught an error with code {error}");
        }
    }
}