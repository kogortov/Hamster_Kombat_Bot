using Newtonsoft.Json;

namespace Pet2
{
    public class CatalogInfo
    {
        [JsonProperty("clickerUser")]
        public Info ClickerUser { get; set; }
    }
}
