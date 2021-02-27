using Newtonsoft.Json;

namespace client.protocol
{
    class Request
    {
        [JsonProperty("data")]
        public readonly object Data;

        [JsonProperty("method")]
        public readonly string Method;

        public Request(string method, object data)
        {
            this.Data = data;
            this.Method = method;
        }
    }
}