using Newtonsoft.Json;

namespace Pet2
{
    public class CatalogResponse
    {
        [JsonProperty("upgradesForBuy")]
        public List<Upgrade> UpgradesForBuy { get; set; }
    }
}
