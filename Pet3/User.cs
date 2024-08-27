using Newtonsoft.Json;
using System.Text;

namespace Hamster_Kombat
{
    public class User
    {
        public string Token { get; set; }
        public long SaveBalance { get; set; }

        public User(string token, long saveBalance)
        {
            Token = token;
            SaveBalance = saveBalance;

        }

        public User(string token)
        {
            Token = token;

        }


        // синзронизация
        public async Task<CatalogInfo> SynchronizationMethod()
        {
            string url = "https://api.hamsterkombatgame.io/clicker/sync";
            using HttpClient client = new HttpClient();


            client.DefaultRequestHeaders.Add("authorization", "Bearer " + Token);

            HttpResponseMessage response = await client.PostAsync(url, new StringContent(""));

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CatalogInfo>(responseBody);
        }

        // получить каталог улучшений
        public async Task<CatalogResponse> GetCatalog()
        {
            string url = "https://api.hamsterkombatgame.io/clicker/upgrades-for-buy";
            using HttpClient client = new HttpClient();

            
            client.DefaultRequestHeaders.Add("authorization", "Bearer " + Token);

            HttpResponseMessage response = await client.PostAsync(url, new StringContent(""));

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CatalogResponse>(responseBody);
        }

        // купить улучшение
        public async Task PurchasingUpgrade(Upgrade upgrade)
        {
            string url = "https://api.hamsterkombatgame.io/clicker/buy-upgrade";
            using HttpClient client = new HttpClient();


            client.DefaultRequestHeaders.Add("authorization", "Bearer " + Token);

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var requestBody = new {upgradeId = upgrade.Id, timestamp = timestamp};

            

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Куплено улучшение {upgrade.Name}");
            }
        }



        // сделать таб
        public async Task<CatalogInfo> MakeTabd()
        {
            string url = "https://api.hamsterkombatgame.io/clicker/tap";
            using HttpClient client = new HttpClient();


            client.DefaultRequestHeaders.Add("authorization", "Bearer " + Token);

            HttpResponseMessage response = await client.PostAsync(url, new StringContent(""));

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CatalogInfo>(responseBody);
        }


        // мониторить
        public async Task<long?> Monitor(Upgrade upgrade)
        {
            string url = "https://api.hamsterkombatgame.io/clicker/buy-upgrade";
            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("authorization", "Bearer " + Token);

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var requestBody = new { upgradeId = upgrade.Id, timestamp = timestamp };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var waitingResponse = JsonConvert.DeserializeObject<Waiting>(responseContent);

                if (waitingResponse != null)
                {
                    return waitingResponse.CooldownSeconds;
                }
            }

            return null;
        }




    }
}
