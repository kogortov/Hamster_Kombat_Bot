namespace Hamster_Kombat
{
    
    public class UpgradeAnalyzer
    {
        // метод для получения списка улушений, где каждое улучшение доступно и срок действия не истек.
        public static List<Upgrade> GetMostProfitableUpgrades(CatalogResponse catalog)
        {
            List<Upgrade> upgrades = new List<Upgrade>();

            foreach (var upgrade in catalog.UpgradesForBuy)
            {
                if(upgrade.IsAvailable && !upgrade.IsExpired)
                {
                    
                    upgrades.Add(upgrade);

                }
            }

            upgrades.Sort(CompareUpgrades);
            return upgrades;



        }


        // метод для определения, какое из двух улучшений прибыльней за свою стоиомть
        public static int CompareUpgrades(Upgrade x, Upgrade y)
        {
            double profitRatioX = (double)x.ProfitPerHourDelta / x.Price;
            double profitRatioY = (double)y.ProfitPerHourDelta / y.Price;
            return profitRatioY.CompareTo(profitRatioX);
        }


        // метод для получения только одного лучшего улучшения
        public static Upgrade GetOnlyOneMostProfitableUpgrade(CatalogResponse catalog)
        {
            List<Upgrade> upgrades = new List<Upgrade>();

            foreach (var upgrade in catalog.UpgradesForBuy)
            {
                if (upgrade.IsAvailable && !upgrade.IsExpired)
                {

                    upgrades.Add(upgrade);

                }
            }

            upgrades.Sort(CompareUpgrades);
            Upgrade bestUpgrade = upgrades[0];

            return bestUpgrade;



        }


    }
}
