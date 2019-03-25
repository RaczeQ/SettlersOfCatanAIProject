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
            foreach (var item in state.canBuildRoad.ToList())
            {
                var copy = state;
                var move = new BuildRoadMove(item);
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = copy.MakeMove(move),
 
                 });
               
            }
            foreach (var item in state.canBuildNewSettlements.ToList())
            {
                var copy = state;
                var move = new BuildSettlementMove(item);
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = copy.MakeMove(move)
                });

            }
            foreach (var item in state.canUpgradeSettlement.ToList())
            {
                var copy = state;
                var move = new BuildCityMove(item);
                node.Children.Add(new Node()
                {
                    Move = move,
                    BoardState = copy.MakeMove(move)
                });

            }
            foreach (var toBuy in state.resourcesAvailableToBuy.ToList().Where(x=> x.Value))
            {
                var copy = state;
                foreach(var toSell in state.resourcesAvailableToSell.ToList().Where(x=> x.Value))
                {
                    var copy2 = copy;
                    var move= new BankTradeMove(toBuy.Key, toSell.Key, 1);
                    node.Children.Add(new Node()
                    {
                        Move = move,
                        BoardState = copy2.MakeMove(move)
                    });
                }
            }
            //node.Children.Add(new Node()
            //{
            //    BoardState = node.BoardState,
            //    Move = new EndMove()
            //});
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

