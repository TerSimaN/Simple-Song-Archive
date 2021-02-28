using Newtonsoft.Json;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace client.protocol
{
    class Serializer
    {
        public byte[] SerializeRequest(Request request)
        {
            byte[] payload;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                string serializedRequest = JsonConvert.SerializeObject(request);
                byte[] encodedRequest = unicodeEncoding.GetBytes(serializedRequest);

                GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
                gZipStream.Write(encodedRequest, 0, encodedRequest.Length);
                gZipStream.Close();
                memoryStream.Close();

                payload = memoryStream.ToArray();
            }

            byte[] message = new byte[5 + payload.Length];
            byte[] payloadLength = BitConverter.GetBytes((UInt32)payload.Length);

            Buffer.SetByte(message, 0, 0x0A);
            Buffer.BlockCopy(payloadLength, 0, message, 1, 4);
            Buffer.BlockCopy(payload, 0, message, 5, payload.Length);

            return message;
        }

        public Response<T> UnserializeResponse<T>(byte[] message)
        {
            if (message.Length > 5 && message[0] != 0x0B)
            {
                throw new Exception("Client Error! Invalid response!");
            }

            UInt32 payloadLength = BitConverter.ToUInt32(message, 1);
            byte[] payload = new byte[payloadLength];
            Buffer.BlockCopy(message, 5, payload, 0, (int)payloadLength);

            Response<T> deserializedResponse;

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

                string serializedResponse = unicodeEncoding.GetString(decompressedPayload);
                deserializedResponse = JsonConvert.DeserializeObject<Response<T>>(serializedResponse);
            }

            return deserializedResponse;
        }
    }
}