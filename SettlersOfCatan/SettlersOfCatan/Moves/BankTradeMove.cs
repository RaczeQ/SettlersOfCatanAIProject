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
    public class BankTradeMove : Move
    {
        private readonly Board.ResourceType boughtResource;
        private readonly Board.ResourceType selledResource;
        private readonly int boughtResourceAmount;

        public BankTradeMove(Board.ResourceType bR, Board.ResourceType sR, int rA)
        {
            boughtResource = bR;
            selledResource = sR;
            boughtResourceAmount = rA;
        }

        public void MakeMove(ref Board target)
        {
            if (CanMakeMove(target))
            {
                var costs = BoardFunctions.GetPlayerBankCosts(target.currentPlayer, target.harbors);
                var proposition = new TradeProposition
                {
                    boughtResource = boughtResource,
                    selledResource = selledResource,
                    boughtResourceAmount = boughtResourceAmount
                };
                Board.TheBank.tradeWithBank(target.currentPlayer, proposition, costs);
                target.addEventText($"{target.currentPlayer.getName()} traded {selledResource} for {boughtResource}");
            }
            else
            {
                throw new BankError("Cannot trade with bank!");
            }
        }

        public void MakeMove(ref BoardState target)
        {
            if (CanMakeMove(target))
            {
                var costs = BoardFunctions.GetPlayerBankCosts(target.player, target.harbors);
                var proposition = new TradeProposition
                {
                    boughtResource = boughtResource,
                    selledResource = selledResource,
                    boughtResourceAmount = boughtResourceAmount
                };
                target.bank.tradeWithBank(target.player, proposition, costs);
            }
            else
            {
                throw new BankError("Cannot trade with bank!");
            }
        }

        private bool _canMakeMove(Bank b, Player p, IEnumerable<Harbor> h)
        {
            var costs = BoardFunctions.GetPlayerBankCosts(p, h);
            var selledResourceAmount = boughtResourceAmount * costs[selledResource];
            return (b.canGiveOutResource(boughtResource, boughtResourceAmount)
                    && (p.getResourceCount(selledResource) >= selledResourceAmount));
        }

        public bool CanMakeMove(Board target)
        {
            return _canMakeMove(Board.TheBank, target.currentPlayer, target.harbors);
        }

        public bool CanMakeMove(BoardState target)
        {
            return _canMakeMove(target.bank, target.player, target.harbors);
        }
    }
}