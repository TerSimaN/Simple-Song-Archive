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
            int responseOffset = 0;
            int bytes = -1;

            do
            {
                bytes = (int)sslClient.Receive(buffer);

                if (bytes > 0)
                {
                    Buffer.BlockCopy(buffer, 0, response, responseOffset, bytes);
                    responseOffset += (bytes - 1);
                }
                else
                {
                    throw new Exception("Error! Nothing was received!");
                }
            } while (bytes != 0);

            return this.serializer.UnserializeResponse<DataType>(response);
        }
    }
}