using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.SimplifiedModels
{
    class SimplifiedSettlement
    {
        public int Id { get; set; }
        public Player OwningPlayer { get; set; }
        public List<SimplifiedRoad> ConnectedRoads { get; set; }
        public int OccupiedRoads { get; set; }
        public List<SimplifiedTitle> AdjustedTiles { get; set; }
        public int Weight { get; set; }
        public List<Board.ResourceType> TitleWeight { get; set; } = new List<Board.ResourceType>();
    }
}
