using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;

namespace Hamster_Kombat
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("F.A.Q:");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("1. Как запустить несколько пользователей?");
            Console.WriteLine("1. Запустите несколько раз файл.exe, каждое окно будет для отдельного пользователя.");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("2. Как получить токен пользователя?");
            Console.WriteLine("2. Вам необходимо сделать самостоятельное исследование данного вопроса в Яндексе, в результатах поиска много инструкций, как узнать свой токен.");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("3. Могу ли я воспользоваться только одной функциональной возможностью программы?");
            Console.WriteLine("3. Да, из 3 предложенных функциональных возможностей, вы можете выбрать и запустить, как все сразу, так и две или одну функциональную возможность.");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("4. Как начать заново после запуска программы?");
            Console.WriteLine("4. Вам необходимо закрыть программу, затем снова её запустить.");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("5. Это безопасно?");
            Console.WriteLine("5. Программа общается только с серверами игры Hamster Kombat, и не с какими другими серверами она не взаимодействует.");
            Console.WriteLine("5. Использование автоматизации может трактоваться авторами игры Hamster Kombat, как не честная игра, и может повлечь негативные последствия для вашего аккаунта в игре.");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Автор: Кирилл Когортов");
            Console.WriteLine("Версия: v.2");
            Console.WriteLine("Дата обновления: 27.08.2024");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("");
            // получаем токен пользователя
            Console.WriteLine("Введите токен пользователя:");
            string token = Console.ReadLine();

            User user = new User(token);
            Console.WriteLine("Пользователь создан!");

            var tasks = new List<Task>();

            // забирать деньги каждый час
            Console.WriteLine("Хотите ли вы забирать каждый час монеты?");
            Console.WriteLine("1 —  это Да");
            Console.WriteLine("2 —  это Нет");
            Console.WriteLine("Например: 1");
            int decisionTakeCoins = Convert.ToInt32(Console.ReadLine());
            if (decisionTakeCoins == 1)
            {
                tasks.Add(TakeCoinsEveryHour(user));
            }

            // покупка топ улучшений
            Console.WriteLine("Хотите ли вы каждый час покупать ТОП улучшений?");
            Console.WriteLine("1 —  это Да");
            Console.WriteLine("2 —  это Нет");
            Console.WriteLine("Например: 1");
            int decisionHourlyPurchasesTopImprovements = Convert.ToInt32(Console.ReadLine());
            long saveBalance = 0;

            if (decisionHourlyPurchasesTopImprovements == 1)
            {
                Console.WriteLine("Ниже какой суммы баланса нельзя тратить на покупку улучшений:");
                saveBalance = Convert.ToInt64(Console.ReadLine());
                tasks.Add(PurchaseTopUpgradesEveryHour(user, saveBalance));
            }

            // мониторинг одного улучшения
            Console.WriteLine("Хотите ли вы мониторить только одно самое лучшие улучшение и покупать его, как оно станет доступным?");
            Console.WriteLine("1 —  это Да");
            Console.WriteLine("2 —  это Нет");
            Console.WriteLine("Например: 1");
            int decisionMonitoring = Convert.ToInt32(Console.ReadLine());

            if (decisionMonitoring == 1)
            {
                tasks.Add(MonitorAndPurchaseUpgrade(user));
            }

            // Ожидание завершения всех задач
            await Task.WhenAll(tasks);
        }

        static async Task TakeCoinsEveryHour(User user)
        {
            while (true)
            {
                await user.SynchronizationMethod();
                await Task.Delay(TimeSpan.FromHours(1));
            }
        }

        static async Task PurchaseTopUpgradesEveryHour(User user, long saveBalance)
        {
            while (true)
            {
                var lastSynchronization = await user.SynchronizationMethod();
                if (lastSynchronization.ClickerUser.BalanceCoins > (double)saveBalance)
                {
                    var catalog = await user.GetCatalog();
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

        static async Task MonitorAndPurchaseUpgrade(User user)
        {
            var listUpgrades = await user.GetCatalog();
            var oneUpgrade = UpgradeAnalyzer.GetOnlyOneMostProfitableUpgrade(listUpgrades);

            while (true)
            {
                await user.PurchasingUpgrade(oneUpgrade);
                var time = await user.Monitor(oneUpgrade);

                if (time.HasValue)
                {
                    Console.WriteLine($"Ждём {time.Value} секунд");
                    await Task.Delay(TimeSpan.FromSeconds(time.Value));
                }
                else
                {
                    await Task.Delay(TimeSpan.FromHours(1));
                }
            }
        }

    }
}

