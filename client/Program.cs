using System;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using NetCoreServer;
using client.protocol;

namespace client
{
    class Program
    {
        static private Client client;
        static private void CreateNewSong()
        {
            Song song = new Song()
            {
                Name = ReadFieldValue("Name"),
                Genre = ReadFieldValue("Genre"),
                Artist = ReadFieldValue("Artist"),
                Lyricist = ReadFieldValue("Lyricist"),
                Year = ushort.Parse(ReadFieldValue("Year")),
                Album = ReadFieldValue("Album"),
                Length = ReadFieldValue("Length"),
                Label = ReadFieldValue("Label"),
                SubmittedBy = ReadFieldValue("Submitted by"),
                SubmissionDate = ReadFieldValue("Submission date"),
            };
            Request songRequest = new Request("create", song);

            try
            {
                Response<int> songResponse = client.SendRequest<int>(songRequest);

                if (songResponse.Success)
                {
                    Console.WriteLine("Created song with ID: {0}", songResponse.Data);
                }
                else
                {
                    Console.WriteLine("Error: {0}", songResponse.ErrorMessage);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        static private string ReadFieldValue(string name)
        {
            while (true)
            {
                Console.Write("{0}: ", name);
                string line = Console.ReadLine();

                if (line.Length > 0)
                {
                    return line;
                }
            }
        }

        static void Main(string[] args)
        {
            SslContext context = new SslContext(SslProtocols.Tls12, new X509Certificate2("../ssl_certs/client.pfx"), (sender, certificate, chain, sslPolicyErrors) => true);
            client = new Client(context, IPAddress.Loopback, 1111);

            Console.WriteLine("Enter \"create\" to create new song or \"exit\" to exit.");

            while (true)
            {
                Console.Write("Command> ");
                string command = Console.ReadLine();
                bool shouldExit = false;

                switch (command)
                {
                    case "create":
                        CreateNewSong();
                        client.ReConnect();
                        break;

                    case "exit":
                        shouldExit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid command!");
                        break;
                }

                if (shouldExit)
                {
                    break;
                }
            }
        }
    }
}
