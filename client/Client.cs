using NetCoreServer;

namespace client
{
    class Client : SslClient
    {
        private Serializer serializer;

        public Client(SslContext context, string address, int port) : base(context, address, port)
        {
            this.serializer = new Serializer();
        } 
    }
}