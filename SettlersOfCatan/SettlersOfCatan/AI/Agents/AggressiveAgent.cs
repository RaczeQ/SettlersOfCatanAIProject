﻿using SettlersOfCatan.AI.AssesmetFunctions;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.SimplifiedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.Moves;

namespace SettlersOfCatan.AI.Agents
{
    public class AggressiveAgent : IAgent
    {
        SimplifiedSettlementBuildAssesmentFunction settlementBuildAssesmentFunction =
            new SimplifiedSettlementBuildAssesmentFunction();

        private Random _r = new Random();
        private const int minResourceAmount = 4;

        public Move makeMove(BoardState state)
        {
            if (state.CanBuildNewSettlements.Any())
            {
                var index = settlementBuildAssesmentFunction.getNewSettlementIndex(state);
                var settlement = state.CanBuildNewSettlements.ElementAt(index == null
                    ? _r.Next(0, state.CanBuildNewSettlements.Count())
                    : state.CanBuildNewSettlements.ToList()
                        .IndexOf(state.CanBuildNewSettlements.FirstOrDefault(x => x.id == index)));
                return new BuildSettlementMove(settlement);
            }
            else if (state.CanUpgradeSettlement.Any())
            {
                //TODO
                return new BuildCityMove(
                    state.CanUpgradeSettlement.ElementAt(_r.Next(0, state.CanUpgradeSettlement.Count())));
            }

            else if (state.CanBuildRoad.Any())
            {
                var index = settlementBuildAssesmentFunction.getNewRoadIndex(state);
                var road = state.CanBuildRoad.ElementAt(index == null
                    ? _r.Next(0, state.CanBuildRoad.Count())
                    : state.CanBuildRoad.ToList().IndexOf(state.CanBuildRoad.FirstOrDefault(x => x.id == index)));
                return new BuildRoadMove(road);
            }
            else if (state.ResourcesAvailableToSell.Values.Any(x => x) &&
                     state.ResourcesAvailableToBuy.Values.Any(x => x))
            {
                var amountLeft = state.PlayerResourcesAmounts
                    .Where(x => state.ResourcesAvailableToSell[x.Key])
                    .ToDictionary(k => k.Key, v => v.Value - state.BankTradePrices[v.Key]);
                var boughtResource = state.PlayerResourcesAcquiredPerResource
                    .Where(x => state.ResourcesAvailableToBuy[x.Key])
                    .OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    var move = new BankTradeMove(boughtResource, selledResource, 1);
                    if (move.CanMakeMove(state))
                    {
                        return new BankTradeMove(boughtResource, selledResource, 1);
                    }
                }
            }

            return new EndMove();
        }


        public Road placeFreeRoad(BoardState state)
        {
            //TODO
            return state.AvailableRoads.ElementAt(_r.Next(0, state.AvailableRoads.Count()));
        }

        public Settlement placeFreeSettlement(BoardState state)
        {
            var sortedSettlements = state.AvailableSettlements
                .OrderByDescending(
                    s => s.adjacentTiles
                        .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                            ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                            : 0)
                );
            return sortedSettlements.First();

            // List<SimplifiedSettlement> selected = null;
            // var currentSettlements = state.player.settlements
            //     .Select(x => new SimplifiedSettlement
            //     {
            //         Id = x.id,
            //         Weight = x.adjacentTiles.Sum(y => getTileWeightBasedOnCreatingNewSettlementsStrategy(y.tileType)),
            //         TileWeights = x.adjacentTiles.Select(y => y.tileType).ToList()
            //     }).ToList();

            // var possible_settlements = state.availableSettlements.
            //     Select(x => new SimplifiedSettlement
            //     {
            //         Id = x.id,
            //         Weight = x.adjacentTiles.Sum(y => getTileWeightBasedOnCreatingNewSettlementsStrategy(y.tileType)),
            //         TileWeights = x.adjacentTiles.Select(y=> y.tileType).ToList()
            //     }).ToList();

            // if (currentSettlements.Count()>0)
            // {
            //     selected = possible_settlements.OrderByDescending(x => x.TileWeights.Distinct().Count()).ThenByDescending(z => z.Weight).ToList();
            //     var result = selected.Where(x => !currentSettlements.Any(y => y.TileWeights.OrderBy(z => z.ToString()) == x.TileWeights.OrderBy(z => z.ToString()))).FirstOrDefault();
            //     return state.availableSettlements.ElementAt(state.availableSettlements.ToList().IndexOf(state.availableSettlements.Where(x => x.id == result.Id).FirstOrDefault()));
            // }
            // else
            // {
            //     selected = possible_settlements.OrderByDescending(x => x.TileWeights.Distinct().Count()).ThenByDescending(z => z.Weight).ToList();
            //     var result = selected.FirstOrDefault();
            //     return state.availableSettlements.ElementAt(state.availableSettlements.ToList().IndexOf(state.availableSettlements.Where(x => x.id == result.Id).FirstOrDefault()));
            // }
        }

        private int getTileWeightBasedOnCreatingNewSettlementsStrategy(Board.ResourceType type)
        {
            if (type == Board.ResourceType.Brick || type == Board.ResourceType.Wood)
                return 2;
            else if (type == Board.ResourceType.Sheep || type == Board.ResourceType.Ore)
                return 1;
            else return 0;
        }


        private bool agent_can_build_road()
        {
            return false;
        }
    }
}