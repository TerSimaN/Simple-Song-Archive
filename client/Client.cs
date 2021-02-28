using System;
using Buffer = System.Buffer;
using NetCoreServer;
using client.protocol;

namespace client
{
    class Client
    {
        private Serializer serializer;
        private SslClient sslClient;

        public Client(SslContext context, string address, int port)
        {
            this.sslClient = new SslClient(context, address, port);
            this.serializer = new Serializer();
            this.sslClient.Connect();
        }

        public void ReConnect()
        {
            this.sslClient.Reconnect();
        }

        public Response<DataType> SendRequest<DataType>(Request request)
        {
            if (!this.sslClient.IsConnected)
            {
                throw new Exception("Client must be connected first!");
            }

            byte[] serializedRequest = serializer.SerializeRequest(request);
            sslClient.Send(serializedRequest);

            byte[] buffer = new byte[2048];
            byte[] response = new byte[16 * 1024];
            int bytes = (int)sslClient.Receive(buffer);

            if (bytes > 0)
            {
                Buffer.BlockCopy(buffer, 0, response, 0, bytes);
            }
            else
            {
                throw new Exception("Error! Invalid response!");
            }

            return this.serializer.UnserializeResponse<DataType>(response);
        }
    }
}