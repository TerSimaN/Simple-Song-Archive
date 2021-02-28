using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.IO.Compression;

namespace server.protocol
{
    class Serializer
    {
        public byte[] SerializeResponse<T>(Response<T> response)
        {
            byte[] payload;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                string serializedResponse = JsonConvert.SerializeObject(response);
                byte[] encodedResponse = unicodeEncoding.GetBytes(serializedResponse);

                GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
                gZipStream.Write(encodedResponse, 0, encodedResponse.Length);
                gZipStream.Close();
                memoryStream.Close();

                payload = memoryStream.ToArray();
            }

            byte[] message = new byte[5 + payload.Length];
            byte[] payloadLength = BitConverter.GetBytes((UInt32)payload.Length);

            Buffer.SetByte(message, 0, 0x0B);
            Buffer.BlockCopy(payloadLength, 0, message, 1, 4);
            Buffer.BlockCopy(payload, 0, message, 5, payload.Length);

            return message;
        }

        public Request UnserializeRequest(byte[] message)
        {
            if (message.Length > 5 && message[0] != 0x0A)
            {
                throw new Exception("Server Error! Invalid request!");
            }

            UInt32 payloadLength = BitConverter.ToUInt32(message, 1);
            byte[] payload = new byte[payloadLength];
            Buffer.BlockCopy(message, 5, payload, 0, (int)payloadLength);

            Request deserializedRequest;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] decompressedPayload = new byte[4096];
                memoryStream.Write(payload, 0, (int)payloadLength);
                memoryStream.Seek(0, SeekOrigin.Begin);

                GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                int bytesRead = -1;

                do {
                    bytesRead = gZipStream.Read(decompressedPayload);
                } while (bytesRead != 0);

                gZipStream.Close();
                memoryStream.Close();

                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();

                string serializedRequest = unicodeEncoding.GetString(decompressedPayload);
                deserializedRequest = JsonConvert.DeserializeObject<Request>(serializedRequest);
            }

            return deserializedRequest;
        }
    }
}