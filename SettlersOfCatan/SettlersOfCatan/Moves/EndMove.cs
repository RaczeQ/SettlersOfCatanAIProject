using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.AI;

namespace SettlersOfCatan.Moves
{
    public class EndMove : Move
    {
        public EndMove() { }
        public void MakeMove(ref Board target)
        { }

        public void MakeMove(ref BoardState target)
        { }

        public bool CanMakeMove(Board target)
        {
            return true;
        }

        public bool CanMakeMove(BoardState target)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            return true;
        }
    }
}
