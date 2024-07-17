namespace Pet2
{
    public class UpgradeAnalyzer
    {
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


        public static int CompareUpgrades(Upgrade x, Upgrade y)
        {
            double profitRatioX = (double)x.ProfitPerHourDelta / x.Price;
            double profitRatioY = (double)y.ProfitPerHourDelta / y.Price;
            return profitRatioY.CompareTo(profitRatioX);
        }
        
    }
}
