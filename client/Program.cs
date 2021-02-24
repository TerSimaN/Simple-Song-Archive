using System;
using Newtonsoft.Json;
using client.protocol;
using NetCoreServer;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace client
{
    class Program
    {
        static private Client client;
        static private void CreateNewSong()
        {
            Song song = new Song();
            song.Name = ReadFieldValue("Name");
            song.Genre = ReadFieldValue("Genre");
            song.Artist = ReadFieldValue("Artist");
            song.Lyricist = ReadFieldValue("Lyricist");
            song.Year = UInt16.Parse(ReadFieldValue("Year"));
            song.Album = ReadFieldValue("Album");
            song.Length = ReadFieldValue("Length");
            song.Label = ReadFieldValue("Label");
            song.SubmittedBy = ReadFieldValue("Submitted by");
            song.SubmissionDate = ReadFieldValue("Submission date");

            Request songRequest = new Request("create", song);
            Response<int> songResponse = client.SendRequest<int>(songRequest);

            if (songResponse.Success)
            {
                Console.Write("ID: {0}", songResponse.Data);
            }
            else
            {
                Console.Write(songResponse.ErrorMessage);
            }
        }

        static private string ReadFieldValue(string name)
        {
            while (true)
            {
                Console.Write("{0}: ", name);
                string line = Console.ReadLine();

                if (line.Length > 0) {
                    return line;
                }
            }
        }

        static void Main(string[] args)
        {
            string address = "127.0.0.1";
            int port = 1111;
            SslContext context = new SslContext(SslProtocols.Tls12, new X509Certificate2("../../SSL_Certs_Out/client.pfx"));
            client = new Client(context, address, port);

            while (true) {
                Console.Write("Command> ");
                string command = Console.ReadLine();
                bool shouldExit = false;

                switch (command)
                {
                    case "create":
                        CreateNewSong();
                        break;

                    case "get":
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
