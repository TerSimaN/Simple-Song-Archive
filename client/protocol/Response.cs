using Newtonsoft.Json;

namespace client.protocol
{
    class Response<DataType>
    {
        [JsonRequired()]
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error_code")]
        public uint ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("data")]
        public DataType Data { get; set; }
    }
}