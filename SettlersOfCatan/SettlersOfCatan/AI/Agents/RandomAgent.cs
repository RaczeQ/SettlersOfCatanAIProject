using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.Agents
{
    public class RandomAgent : IAgent
    {
        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public object makeMove(BoardState state)
        {
            if (state.canUpgradeSettlement.Count() > 0)
            {
                return state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count()));
            }
            else if (state.canBuildNewSettlements.Count() > 0)
            {
                return state.canBuildNewSettlements.ElementAt(_r.Next(0, state.canBuildNewSettlements.Count()));
            }
            else if (state.canBuildRoad.Count() > 0)
            {
                return state.canBuildRoad.ElementAt(_r.Next(0, state.canBuildRoad.Count()));
            }
            else if (state.resourcesAvailableToBuy.Values.Any(x => x))
            {
                // Buy resource with lowest amount for resource with highest amount left after buy
                var amountLeft = state.playerResourcesAmounts.ToDictionary(k => k.Key, v => v.Value - state.bankTradePrices[v.Key]);
                //var boughtResource = state.playerResourcesAmounts.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var boughtResource = state.playerResourcesAcquiredPerResource.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    TradeProposition proposition = new TradeProposition()
                    {
                        boughtResource = boughtResource,
                        selledResource = selledResource,
                        boughtResourceAmount = 1
                    };
                    return proposition;
                }
            }
            return state.endTurnButton;
        }

        public Road placeFreeRoad(BoardState state)
        {
            return state.availableRoads.ElementAt(_r.Next(0, state.availableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            return state.availableSettlements.ElementAt(_r.Next(0, state.availableSettlements.Count()));
        }
    }
}
