using SettlersOfCatan.AI.AssesmetFunctions;
using SettlersOfCatan.SimplifiedModels;
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
            if (state.canBuildNewSettlements.Count() > 0)
            {
                var index = settlementBuildAssesmentFunction.getNewSettlementIndex(state);
                return state.canBuildNewSettlements.ElementAt(state.canBuildNewSettlements.ToList().IndexOf(state.canBuildNewSettlements.Where(x=> x.id==index).FirstOrDefault()));
            }
            else if (state.canUpgradeSettlement.Count() > 0)
            {
                //TODO
                return state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count()));
            }
           
            else if (state.canBuildRoad.Count() > 0)
            {
                var index = settlementBuildAssesmentFunction.getNewRoadIndex(state);
                return state.canBuildRoad.ElementAt(state.canBuildRoad.ToList().IndexOf(state.canBuildRoad.Where(x=> x.id==index).FirstOrDefault()));
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
            List<SimplifiedSettlement> selected = null;
            var currentSettlements = state.player.settlements
                .Select(x => new SimplifiedSettlement
                {
                    Id = x.id,
                    Weight = x.adjacentTiles.Sum(y => getTileWeightBasedOnCreatingNewSettlementsStrategy(y.tileType)),
                    TitleWeight = x.adjacentTiles.Select(y => y.tileType).ToList()
                }).ToList();

            var possible_settlements = state.availableSettlements.
                Select(x => new SimplifiedSettlement
                {
                    Id = x.id,
                    Weight = x.adjacentTiles.Sum(y => getTileWeightBasedOnCreatingNewSettlementsStrategy(y.tileType)),
                    TitleWeight = x.adjacentTiles.Select(y=> y.tileType).ToList()
                }).ToList();

            if (currentSettlements.Count()>0)
            {
                selected = possible_settlements.OrderByDescending(x => x.TitleWeight.Distinct().Count()).ThenByDescending(z => z.Weight).ToList();
                var result = selected.Where(x => !currentSettlements.Any(y => y.TitleWeight.OrderBy(z => z.ToString()) == x.TitleWeight.OrderBy(z => z.ToString()))).FirstOrDefault();
                return state.availableSettlements.ElementAt(state.availableSettlements.ToList().IndexOf(state.availableSettlements.Where(x => x.id == result.Id).FirstOrDefault()));
            }
            else
            {
                selected = possible_settlements.OrderByDescending(x => x.TitleWeight.Distinct().Count()).ThenByDescending(z => z.Weight).ToList();
                var result = selected.FirstOrDefault();
                return state.availableSettlements.ElementAt(state.availableSettlements.ToList().IndexOf(state.availableSettlements.Where(x => x.id == result.Id).FirstOrDefault()));
            }
        }

        private int getTileWeightBasedOnCreatingNewSettlementsStrategy(Board.ResourceType type)
        {
            if (type == Board.ResourceType.Brick || type == Board.ResourceType.Wood)
                return 2;
            else if (type == Board.ResourceType.Sheep || type == Board.ResourceType.Ore)
                return 1;
            else return 0;
        }
        
    }  
}
