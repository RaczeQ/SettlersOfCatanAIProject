using SettlersOfCatan.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.SimplifiedModels
{
    public class SimplifiedSettlement
    {
        public int Id { get; set; }
        public Player OwningPlayer { get; set; }
        public List<SimplifiedRoad> ConnectedRoads { get; set; }
        public int OccupiedRoads { get; set; }
        public List<SimplifiedTile> AdjacentTiles { get; set; }
        public int Weight { get; set; }
        public List<Board.ResourceType> TileWeights { get; set; } = new List<Board.ResourceType>();
    }
}
