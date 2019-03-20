using SettlersOfCatan.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Utils
{
    public class BoardFunctions
    {
        public static Player GetPlayerWithBiggestArmy(IEnumerable<Player> players)
        {
            int _biggestArmy = 0;
            Player lapl = null;
            foreach (Player pl in players)
            {
                int army = pl.getArmySize();
                if (army > _biggestArmy)
                {
                    lapl = pl;
                    _biggestArmy = army;
                }
                else if (army == _biggestArmy)
                {
                    lapl = null;
                }
            }
            if (_biggestArmy < 4)
            {
                lapl = null;
            }
            return lapl;
        }

        public static Player GetPlayerWithLongestRoad(IEnumerable<Player> players)
        {
            int _longestRoad = 0;
            Player llpl = null;
            foreach (Player pl in players)
            {
                int road = pl.getLongestRoadCount();
                if (road > _longestRoad)
                {
                    llpl = pl;
                    _longestRoad = road;
                }
                else if (road == _longestRoad)
                {
                    llpl = null;
                }
            }
            if (_longestRoad < 5)
            {
                llpl = null;
            }
            return llpl;
        }

        public static Player GetWinner(IEnumerable<Player> players)
        {
            foreach (Player pl in players)
            {
                int vps = Player.calculateVictoryPoints(true, pl);
                if (vps >= 10)
                {
                    return pl;
                }
            }
            return null;
        }

        public static IDictionary<Board.ResourceType, int> GetPlayerBankCosts(Player p, IEnumerable<Harbor> harbors)
        {
            var result = new Dictionary<Board.ResourceType, int>();
            foreach (Board.ResourceType rt in Enum.GetValues(typeof(Board.ResourceType)))
            {
                result.Add(rt, 4);
            }
            foreach (Harbor hb in harbors.Where(h => h.playerHasValidSettlement(p)))
            {
                var harborResource = hb.getTradeOutputResource();
                if (harborResource == Board.ResourceType.Desert)
                {
                    foreach (Board.ResourceType rt in Enum.GetValues(typeof(Board.ResourceType)))
                    {
                        if (result[rt] > 3) result[rt] = 3;
                    }
                }
                else
                {
                    result[harborResource] = 2;
                }
            }
            return result;
        }
    }
}
