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
            Console.WriteLine("Incoming: {0}", message);

            try
            {
                request = serializer.UnserializeRequest(buffer);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Request Error: {0}", exception.Message);
            }

            Response<int> response = new Response<int>();
            response.Success = true;
            response.Data = 12;
            response.Error_code = 404;
            response.Error_Message = "Error Message!";
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