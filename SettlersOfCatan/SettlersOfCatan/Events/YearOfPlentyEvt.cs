using SettlersOfCatan.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Events
{
    class YearOfPlentyEvt : Event
    {
        Board theBoard;
        TradeWindow tradeWindow;
        EvtOwnr owner;

        public override void beginExecution(Board board, EvtOwnr evt)
        {
            theBoard = board;
            enableEventObjects();
            owner = evt;
        }

        public override void executeUpdate(Object sender, EventArgs e)
        {
            //Trade has finished.
            endExecution();
        }

        public override void endExecution()
        {
            disableEventObjects();
            if (!(owner is Board))
                owner.subeventEnded();
            else
                PlayerSemaphore.unlockGame();
        }

        public override void enableEventObjects()
        {
            tradeWindow = new TradeWindow();
            tradeWindow.loadYearOfPlenty(theBoard.currentPlayer);
            tradeWindow.Show();
            tradeWindow.Closing += executeUpdate;
        }

        public override void disableEventObjects()
        {
            tradeWindow = null;
        }
    }
}
