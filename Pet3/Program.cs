using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Pet2
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Введите токен пользователя:");
            string token = Console.ReadLine();

            Console.WriteLine("Ниже какой суммы баланса нельзя тратить на покупку улучшений:");
            long saveBalance = Convert.ToInt64(Console.ReadLine());


            User user = new User(token, saveBalance);
            Console.WriteLine("Пользователь создан!");

            while (true)
            {
                var lastSynchronization = await user.SynchronizationMethod();


                if (lastSynchronization.ClickerUser.BalanceCoins > (double)saveBalance)
                {

                    // помещаем каталог улучшений
                    var catalog = await user.GetCatalog();

                    // получаем выгодные улучшения 
                    var profitableUpgrades = UpgradeAnalyzer.GetMostProfitableUpgrades(catalog);

                    foreach (var upgrade in profitableUpgrades)
                    {
                        lastSynchronization = await user.SynchronizationMethod();

                        if (lastSynchronization.ClickerUser.BalanceCoins > (double)saveBalance)
                        {
                            await user.PurchasingUpgrade(upgrade);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine("Ждем один час до следующей проверки...");
                await Task.Delay(TimeSpan.FromHours(1));
            }
        }
    }
}
