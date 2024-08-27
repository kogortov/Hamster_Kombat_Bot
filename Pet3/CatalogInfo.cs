using Newtonsoft.Json;

namespace Hamster_Kombat
{
    public class CatalogInfo
    {
        [JsonProperty("clickerUser")]
        public Info ClickerUser { get; set; }
    }
}
