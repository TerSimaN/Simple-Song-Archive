using Newtonsoft.Json;

namespace server.protocol
{
    class Request
    {
        [JsonProperty("data")]
        public readonly object Data;

        [JsonProperty("method")]
        public readonly string Method;

        public Request(object Data, string Method)
        {
            this.Data = Data;
            this.Method = Method;
        }
    }
}