using System;
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

            //For testing purposes only:
            /* song.Name = "Name";
            song.Genre = "Genre";
            song.Artist = "Artist";
            song.Lyricist = "Lyricist";
            song.Year = 1024;
            song.Album = "Album";
            song.Length = "Length";
            song.Label = "Label";
            song.SubmittedBy = "Submitted by";
            song.SubmissionDate = "Submission date"; */

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
            string address = "127.0.0.1";
            int port = 1111;
            SslContext context = new SslContext(SslProtocols.Tls12, new X509Certificate2("../ssl_certs/client.pfx"), (sender, certificate, chain, sslPolicyErrors) => true);
            client = new Client(context, address, port);

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

                    case "get":
                        Console.WriteLine("Not yet implemented!");
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
