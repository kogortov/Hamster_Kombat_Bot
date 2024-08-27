using Newtonsoft.Json;

namespace Hamster_Kombat
{
    public class CatalogResponse
    {
        [JsonProperty("upgradesForBuy")]
        public List<Upgrade> UpgradesForBuy { get; set; }
    }
}
