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
    public class BuildSettlementMove : Move
    {
        private readonly Settlement settlement;

        public BuildSettlementMove(Settlement s)
        {
            settlement = s;
        }

        public void MakeMove(ref Board target)
        {
            if (CanMakeMove(target))
            {
                var originalSettlement = target.settlementLocations.Find(s => Equals(s, settlement));
                originalSettlement.buildSettlement(target.currentPlayer, true, true);
                Board.TheBank.takePayment(target.currentPlayer, Bank.SETTLEMENT_COST);
                target.addEventText(UserMessages.PlayerPlacedASettlement(target.currentPlayer));
            }
            else
            {
                throw new BuildError("Cannot build settlement!");
            }
        }

        public void MakeMove(ref BoardState target)
        {
            if (CanMakeMove(target))
            {
                var originalSettlement = target.settlements.ToList().Find(s => Equals(s, settlement));
                originalSettlement.buildSettlement(target.player, true, true);
                target.bank.takePayment(target.player, Bank.SETTLEMENT_COST);
            }
            else
            {
                throw new BuildError("Cannot build settlement!");
            }
        }

        private bool _canMakeMove(Settlement s, Player p)
        {
            return s.getOwningPlayer() == null && s.checkForOtherSettlement() &&
                   Bank.hasPayment(p, Bank.SETTLEMENT_COST);
        }

        public bool CanMakeMove(Board target)
        {
            var originalSettlement = target.settlementLocations.Find(s => Equals(s, settlement));
            return _canMakeMove(originalSettlement, target.currentPlayer);
        }

        public bool CanMakeMove(BoardState target)
        {
            var originalSettlement = target.settlements.ToList().Find(s => Equals(s, settlement));
            return _canMakeMove(originalSettlement, target.player);
        }
    }
}