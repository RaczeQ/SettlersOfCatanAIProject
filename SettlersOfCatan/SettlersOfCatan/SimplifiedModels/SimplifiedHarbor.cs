using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.SimplifiedModels
{
    public class SimplifiedHarbor
    {
        public int TradeRequiredResourceCount { get; set; }
        //If a harbor has resource type of desert, the player can choose their desired resource to trade for.
        public Board.ResourceType TradeOutputResource { get; set; } = Board.ResourceType.Desert;

        public List<SimplifiedSettlement> ValidTradeLocations { get; set; } = new List<SimplifiedSettlement>();
    }
}
