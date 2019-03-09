using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.SimplifiedModels
{
    class SimplifiedRoad
    {
        public int Id { get; set; }
        public Player OwningPlayer { get; set; }
        public List<SimplifiedSettlement> ConnectedSettlements { get; set;}
    }
}
