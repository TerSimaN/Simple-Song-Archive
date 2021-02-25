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

            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");

            Console.WriteLine("Enter \"restart\" to restart the server or \"stop\" to stop the server.");

            while (true)
            {
                string command = Console.ReadLine();
                bool shouldExit = false;

                switch (command)
                {
                    case "restart":
                        Console.Write("Server restarting...");
                        server.Restart();
                        Console.WriteLine("Done!");
                        break;
                        
                    case "stop":
                        shouldExit = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid command!");
                        break;
                }

                if (shouldExit) {
                    break;
                }
            }

            Console.Write("Server stopping...");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}
