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
            this.sslClient.ConnectAsync();
        }

        public Response<DataType> SendRequest<DataType>(Request request)
        {
            if (!this.sslClient.IsConnected)
            {
                throw new Exception("Client must be connected first!");
            }

            byte[] serializedRequest = serializer.SerializeRequest(request);
            sslClient.Send(serializedRequest);

            byte[] buffer = new byte[64*1024];
            int responseSize = (int)sslClient.Receive(buffer);
            if (responseSize == 0)
            {
                throw new Exception("Error! Invalid response!");
            }

            byte[] serializedResponse = new byte[responseSize];
            Buffer.BlockCopy(buffer, 0, serializedResponse, 0, responseSize);

            return serializer.UnserializeResponse<DataType>(serializedResponse);
        }
    }
}