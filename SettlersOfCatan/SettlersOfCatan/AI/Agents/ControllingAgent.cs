using SettlersOfCatan.SimplifiedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI.Agents
{
   
    public class ControllingAgent : IAgent
    {
        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public object makeMove(BoardState state)
        {
            if (state.canBuildNewSettlements.Count() > 0)
            {
                return state.canBuildNewSettlements.ElementAt(_r.Next(0, state.canBuildNewSettlements.Count()));
            }
            else if (state.canUpgradeSettlement.Count() > 0)
            {
                return state.canUpgradeSettlement.ElementAt(_r.Next(0, state.canUpgradeSettlement.Count()));
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
            var possible_settlements = state.availableSettlements.
                Select(x => new SimplifiedSettlement
                {
                    Id = x.id,
                    TitleWeight = x.adjacentTiles.Select(y => y.tileType).ToList()
                }).ToList();

            var oponentResources = state.settlements
                .Where(x => x.owningPlayer != null
                && x.owningPlayer != state.player)
                .Select(y => new
                {
                    Title = y.adjacentTiles.Select(z => z.tileType).ToList()       ,
                    Wood = y.adjacentTiles.Where(z=> z.tileType == Board.ResourceType.Wood).Count(),
                    Ore = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Ore).Count(),
                    Brick = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Brick).Count(),
                    Sheep = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Sheep).Count(),
                    Desert = y.adjacentTiles.Where(z => z.tileType == Board.ResourceType.Desert).Count()
                }).ToList();

            if (oponentResources != null)
            {
                //var all_tiles = new List<Board.ResourceType>();
                var res = oponentResources.Sum(x => x.Wood);
                var t = oponentResources.Sum(x => x.Ore);
               

                var hhht = 9;


                //var result = possible_settlements.Where(x => !oponentResources.Any(u => u.TitleWeight.OrderBy(z => z.ToString()) == x.TitleWeight.OrderBy(z => z.ToString())))
              //  var ordered_oponent_resources = oponentResources
                
            }
            return state.availableSettlements.ElementAt(_r.Next(0, state.availableSettlements.Count()));
        }


    }
}
