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

        public void beginExecution(Board board, EvtOwnr evt)
        {
            theBoard = board;
            enableEventObjects();
            owner = evt;
        }

        public void executeUpdate(Object sender, EventArgs e)
        {
            //Trade has finished.
            endExecution();
        }

        public void endExecution()
        {
            disableEventObjects();
            owner.subeventEnded();
        }

        public void enableEventObjects()
        {
            tradeWindow = new TradeWindow();
            tradeWindow.loadYearOfPlenty(theBoard.currentPlayer);
            tradeWindow.Show();
            tradeWindow.Closing += executeUpdate;
        }

        public void disableEventObjects()
        {
            tradeWindow = null;
        }
    }
}
