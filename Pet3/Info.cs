using Newtonsoft.Json;

namespace Hamster_Kombat
{
    public class Info
    {

        [JsonProperty("balanceCoins")]
        public double BalanceCoins { get; set; }

    }
}
