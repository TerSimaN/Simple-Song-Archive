using Newtonsoft.Json;

namespace client.protocol
{
  class Response<DataType>
  {
    [JsonRequired()]
    [JsonProperty("success")]    
    public bool Success;
    [JsonProperty("error_code")]
    public uint ErrorCode;
    [JsonProperty("error_message")]
    public string ErrorMessage;
    [JsonProperty("data")]
    public DataType Data;
  }
}