using System;
using System.Net;
using NetCoreServer;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace server
{
    class Program
    {
        static private Server server;
        static void Main(string[] args)
        {
            int port = 1111;
            SslContext sslContext = new SslContext(SslProtocols.Tls12, new X509Certificate2("../ssl_certs/server.pfx"));
            
            server = new Server(sslContext, IPAddress.Any, port);
            server.Start();

            while (true)
            {
                Console.Write("Command> ");
                string command = Console.ReadLine();
                bool shouldExit = false;

                switch (command)
                {
                    case "get":
                        Console.WriteLine("Nothing to get!");
                        break;
                        
                    case "exit":
                        shouldExit = true;
                        break;
                    
                    default:
                        Console.WriteLine("Unknown command!");
                        break;
                }

                if (shouldExit) {
                    break;
                }
            }
        }
    }
}
