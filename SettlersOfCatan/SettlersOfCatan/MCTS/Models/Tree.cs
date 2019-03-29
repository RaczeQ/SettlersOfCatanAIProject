using SettlersOfCatan.AI;
using SettlersOfCatan.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Models
{
    public class Tree
    {
        public Node Root { get; set; }
        private Random _r = new Random();

        public Node CreateRoot(BoardState state)
        {
            Root = new Node()
            {
                BoardState = state,
                Children = new List<Node>()
            };
            return ExtendChildrenNode(state, Root);
        }

        public Node ExtendChildrenNode(BoardState state, Node node)
        {
            node.Children = new List<Node>();

            if (state.canBuildNewSettlements.Any())
            {
                var sortedSettlements = state.canBuildNewSettlements
                .OrderByDescending(
                    s => s.adjacentTiles
                        .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                            ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                            : 0)
                );
                var move = new BuildSettlementMove(sortedSettlements.First());
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                });
            }

            if (state.canUpgradeSettlement.Any())
            {
                var sortedCities = state.canUpgradeSettlement
                .OrderByDescending(
                    s => s.adjacentTiles
                        .Sum(t => t.getResourceType() != Board.ResourceType.Desert
                            ? BoardState.CHIP_MULTIPLIERS[t.numberChip.numberValue]
                            : 0)
                );
                var move = new BuildCityMove(sortedCities.First());
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                });
            }

            if (state.canBuildRoad.Any())
            {
                var move = new BuildRoadMove(state.canBuildRoad.ElementAt(_r.Next(0, state.canBuildRoad.Count())));
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = state.MakeMove(move),
                });
            }

            if (state.resourcesAvailableToSell.Values.Any(x => x) &&
                state.resourcesAvailableToBuy.Values.Any(x => x))
            {
                var copy = state;
                var amountLeft = state.playerResourcesAmounts
                    .Where(x => state.resourcesAvailableToSell[x.Key])
                    .ToDictionary(k => k.Key, v => v.Value - state.bankTradePrices[v.Key]);
                var boughtResource = state.playerResourcesAcquiredPerResource
                    .Where(x => state.resourcesAvailableToBuy[x.Key])
                    .OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList()[0];
                var selledResource = amountLeft.OrderBy(kv => -kv.Value).Select(kv => kv.Key).ToList()[0];
                if (boughtResource != selledResource)
                {
                    var move = new BankTradeMove(boughtResource, selledResource, 1);
                    if (move.CanMakeMove(copy))
                    {
                        node.Children.Add(new Node()
                        {
                            Move = move,
                            BoardState = copy.MakeMove(move)
                        });
                    }
                }
            }

            var copyState = state;
            node.Children.Add(new Node()
            {  
                Move = new EndMove(),
                Children = new List<Node>(),
                BoardState = copyState.MakeMove(new EndMove())
            });
            return node;
        }


        public Node CreateRootToPlaceFreeRoad(BoardState state)
        {
            Root = new Node()
            {
                BoardState = state,
                Children = new List<Node>()
            };

            return ExtendRoadChildrenNode(state, Root);
        }
        public Node ExtendRoadChildrenNode(BoardState state, Node node)
        {

            node.Children = new List<Node>();
            var roads = state.availableRoads.ToList();        
            foreach (var item in roads)
            {
                var copy = state;
                var roadMove = new BuildRoadMove(item);
                node.Children.Add(new Node()
                {
                    BoardState = copy.MakeMove(roadMove)
                });
            }
            return node;
        }


        public Node CreateRootToPlaceFreeSettlement(BoardState state)
        {
            Root = new Node()
            {
                BoardState = state,
                Children = new List<Node>()
            };

            return ExtendSettlementChildrenNode(state, Root);
        }
        public Node ExtendSettlementChildrenNode(BoardState state, Node node)
        {

            node.Children = new List<Node>();
            var settlements = state.availableSettlements.ToList();
            foreach (var item in settlements)
            {
                var copy = state;
                var settlementMove = new BuildSettlementMove(item);
                node.Children.Add(new Node()
                {
                    BoardState = copy.MakeMove(settlementMove)
                });
            }
            return node;
        }
    }
}

