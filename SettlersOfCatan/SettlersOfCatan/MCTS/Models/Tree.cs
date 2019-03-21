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
            DefineRootChildren(state);
            return Root;
        }

        public void DefineRootChildren(BoardState state)
        {
            Root.Children = new List<Node>();
           
            foreach(var item in state.canBuildRoad.ToList())
            {
                var copy = state;
                var road = new BuildRoadMove(item);
                Root.Children.Add(new Node()
                { 
                    BoardState = copy.MakeMove(road)
                });
            }
            foreach(var item in state.canBuildNewSettlements.ToList())
            {
                var copy = state;
                Root.Children.Add(new Node()
                {
                    BoardState = copy.MakeMove(new BuildSettlementMove(item))
                });
            }
            foreach(var item in state.canUpgradeSettlement.ToList())
            {
                var copy = state;
                Root.Children.Add(new Node()
                {
                    BoardState = state.MakeMove(new BuildCityMove(item))
                });
            }
        }

    }
}
