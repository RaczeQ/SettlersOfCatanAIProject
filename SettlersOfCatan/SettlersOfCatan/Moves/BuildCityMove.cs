using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.AI;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.Utils;

namespace SettlersOfCatan.Moves
{
    public class BuildCityMove : Move
    {
        private readonly Settlement settlement;

        public BuildCityMove(Settlement s)
        {
            settlement = s;
        }

        public void MakeMove(ref Board target)
        {
            if (CanMakeMove(target))
            {
                var originalSettlement = target.settlementLocations.Find(s => Equals(s, settlement));
                originalSettlement.buildSettlement(target.currentPlayer, true, true);
                Board.TheBank.takePayment(target.currentPlayer, Bank.CITY_COST);
                target.addEventText(UserMessages.PlayerBuiltACity(target.currentPlayer));
            }
            else
            {
                throw new BuildError("Cannot build city!");
            }
        }

        public void MakeMove(ref BoardState target)
        {
            if (CanMakeMove(target))
            {
                var originalSettlement = target.Settlements.ToList().Find(s => Equals(s, settlement));
                originalSettlement.buildSettlement(target.player, true, true);
                target.bank.takePayment(target.player, Bank.CITY_COST);
            }
            else
            {
                throw new BuildError("Cannot build city!");
            }
        }

        private bool _canMakeMove(Settlement s, Player p)
        {
            return Equals(s.getOwningPlayer(), p) && Bank.hasPayment(p, Bank.CITY_COST);
        }

        public bool CanMakeMove(Board target)
        {
            var originalSettlement = target.settlementLocations.Find(s => Equals(s, settlement));
            return _canMakeMove(originalSettlement, target.currentPlayer);
        }

        public bool CanMakeMove(BoardState target)
        {
            var originalSettlement = target.Settlements.ToList().Find(s => Equals(s, settlement));
            return _canMakeMove(originalSettlement, target.player);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            BuildCityMove move = (BuildCityMove)obj;
            return this.settlement == move.settlement;
        }
    }
}