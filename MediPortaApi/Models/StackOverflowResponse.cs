using Newtonsoft.Json;

namespace MediPortaApi.Models
{
    public class StackOverflowResponse
    {
        [JsonProperty("items")]
        public required List<Item> Items { get; set; }
    }
}
