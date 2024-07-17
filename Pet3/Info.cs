using Newtonsoft.Json;

namespace Pet2
{
    public class Info
    {

        [JsonProperty("balanceCoins")]
        public double BalanceCoins { get; set; }

    }
}
