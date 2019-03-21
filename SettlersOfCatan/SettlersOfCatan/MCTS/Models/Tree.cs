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

        public Node ExtendChildrenNode(BoardState state, Node node )
        {
            
            node.Children = new List<Node>();

            var roads = state.canBuildRoad.ToList();
            var settlements = state.canBuildNewSettlements.ToList();
            var cites = state.canUpgradeSettlement.ToList();
            foreach (var item in roads)
            {
                var copy = state;
                var road = new BuildRoadMove(item);
                node.Children.Add(new Node()
                {
                    BoardState = copy.MakeMove(road)
                });
            }
            foreach (var item in settlements)
            {
                var copy = state;
                node.Children.Add(new Node()
                {
                    BoardState = copy.MakeMove(new BuildSettlementMove(item))
                });
            }
            foreach (var item in cites)
            {
                var copy = state;
                node.Children.Add(new Node()
                {
                    BoardState = state.MakeMove(new BuildCityMove(item))
                });
            }
            return node;
        }

    }
}
