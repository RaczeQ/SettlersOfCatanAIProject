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
        public List<Node> Children { get; set; } = new List<Node>();
        public int Depth { get; set; } = 0;
        public int Playouts { get; set; } = 0;

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

        public static Tuple<double, double, int> GetDepthMeasures(Node node)
        {
            var depths = GetDepthsList(node);
            double mean = depths.Average();
            int numberCount = depths.Count();
            int halfIndex = depths.Count() / 2;
            var sortedNumbers = depths.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = (double)(sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2;
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            var max = depths.Max();
            return new Tuple<double, double, int>(mean, median, max);
        }

//        public static int GetTheDeepestNodeValue(Node node, int max)
//        {
//            if (node.Children == null || node.Children.Count == 0)
//                return max;
//            foreach (var item in node.Children)
//            {
//                if (item.Depth > max)
//                    max = item.Depth;
//                max =  GetTheDeepestNodeValue(item, max);     
//            }
//            return max;
//        }

        public static IList<int> GetDepthsList(Node node)
        {
            var result = new List<int> { node.Depth };
            foreach (var c in node.Children.Concat(node.NodesAfterRandomRollDice))
            {
                result.AddRange(GetDepthsList(c));
            }

            return result;
        }

        private static Tuple<int, int> ChildrenAmount(Node node)
        {
            var children = node.Children?.Count ?? 0;
            children += node.NodesAfterRandomRollDice?.Count ?? 0;
            var visited = node.Children?.Where(c => c.VisitsNum > 0).Count() ?? 0;
            visited += node.NodesAfterRandomRollDice?.Where(c => c.VisitsNum > 0).Count() ?? 0;
            foreach (var c in node.Children.Concat(node.NodesAfterRandomRollDice))
            {
                var result = ChildrenAmount(c);
                children += result.Item1;
                visited += result.Item2;
            }
            return new Tuple<int, int>(children, visited);
        }

        public static double ChildrenExploredFactor(Node node)
        {
            var tuple = ChildrenAmount(node);
            return  (double)(tuple.Item2  + 1) / (tuple.Item1 + 1);
        }
    }
} 
