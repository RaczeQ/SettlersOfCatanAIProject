using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.AI
{
    public class TradeProposition
    {
        public Board.ResourceType boughtResource { get; set; }
        public Board.ResourceType selledResource { get; set; }
        public int boughtResourceAmount { get; set; }
    }
}
