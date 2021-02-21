using System;
using Newtonsoft.Json;

namespace client
{
    class Program
    {
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
