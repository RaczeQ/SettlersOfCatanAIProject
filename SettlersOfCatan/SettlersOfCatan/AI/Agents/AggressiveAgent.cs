using SettlersOfCatan.AI.AssesmetFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.Agents
{
    public class AggressiveAgent : IAgent
    {
        SimplifiedSettlementBuildAssesmentFunction settlementBuildAssesmentFunction = new SimplifiedSettlementBuildAssesmentFunction();
        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public object makeMove(BoardState state)
        {
            if (state.canUpgradeSettlement.Count() > 0)
            {
                //TODO
                return state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count()));
            }
            else if (state.canBuildNewSettlements.Count() > 0)
            {
                var index = settlementBuildAssesmentFunction.getNewSettlementIndex(state);
                return state.canBuildNewSettlements.ElementAt(index);
            }
            else if (state.canBuildRoad.Count() > 0)
            {
                //TODO
                return state.canBuildRoad.ElementAt(_r.Next(0, state.canBuildRoad.Count()));
            }
            else if (state.resourcesAvailableToBuy.Values.Any(x => x))
            {
                //TODO
                // Buy resource with lowest amount for resource with highest amount left after buy
                var amountLeft = state.playerResourcesAmounts.ToDictionary(k => k.Key, v => v.Value - state.bankTradePrices[v.Key]);
                var boughtResource = state.playerResourcesAmounts.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                TradeProposition proposition = new TradeProposition()
                {
                    boughtResource = boughtResource,
                    selledResource = selledResource,
                    boughtResourceAmount = 1
                };
                return proposition;
            }
            else
            {
                return state.endTurnButton;
            }
        }

        public Road placeFreeRoad(BoardState state)
        {
            //TODO
            return state.availableRoads.ElementAt(_r.Next(0, state.availableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            //TODO
            return state.availableSettlements.ElementAt(_r.Next(0, state.availableSettlements.Count()));
        }
    }
}
