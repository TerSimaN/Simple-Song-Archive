using Newtonsoft.Json;

namespace server.protocol
{
    class Response<DataType>
    {
        [JsonRequired()]
        [JsonProperty("success")]
        public bool Success;

        [JsonProperty("error_code")]
        public uint Error_code;

        [JsonProperty("error_message")]
        public string Error_Message;

        [JsonProperty("data")]
        public DataType Data;
    }
}