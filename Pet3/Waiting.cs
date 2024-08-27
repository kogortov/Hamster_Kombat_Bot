using Newtonsoft.Json;

public class Waiting
{
        [JsonProperty("cooldownSeconds")]
        public long CooldownSeconds { get; set; }
}
