using Newtonsoft.Json;

namespace MediPortaApi.Models
{
    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
