using Newtonsoft.Json;

namespace Pet2
{
    public class Upgrade
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("profitPerHourDelta")]
        public int ProfitPerHourDelta { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }

    }
}
