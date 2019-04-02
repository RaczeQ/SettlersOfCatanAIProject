using SettlersOfCatan.AI;
using SettlersOfCatan.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.MCTS.Models
{
    public class Node
    {
        public int RolloutScore { get; set; } = 0;
        public int WinsNum { get; set; } = 0;
        public int VisitsNum { get; set; } = 0;
        public double TotalScore => VisitsNum != 0 ? ((double)WinsNum / VisitsNum) : 0;

        public Move Move { get; set; }
        public BoardState BoardState { get; set; }
        public List<Node> Children { get; set; }
        public int Depth { get; set; } = 0;

        public List<Node> NodesAfterRandomRollDice { get; set; } = new List<Node>();
        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return BoardState.CurrentStateHashCode == other.BoardState.CurrentStateHashCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Node)obj);
        }

        public void PrintPretty(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            var moveName = 
                Move == null ? 
                    $"Root[{BoardState.RollDiceToCurentState}][{BoardState.CurrentStateHashCode}]" : 
                    (VisitsNum == 0 ? $"({Move.GetType().Name})" : $"{Move.GetType().Name}");
            Console.WriteLine($"[{GameObjects.Player.playerColorNames[BoardState.player.playerNumber]}] {moveName} {Depth} ({WinsNum}/{VisitsNum})");

            if (Children != null)
            {
                for (int i = 0; i < Children.Count; i++)
                    Children[i].PrintPretty(indent, i == Children.Count - 1);
            }
            if (NodesAfterRandomRollDice != null)
            {
                for (int i = 0; i < NodesAfterRandomRollDice.Count; i++)
                    NodesAfterRandomRollDice[i].PrintPretty(indent, i == NodesAfterRandomRollDice.Count - 1);
            }
        }
    }
}
