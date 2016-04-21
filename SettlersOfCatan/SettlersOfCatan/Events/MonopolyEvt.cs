using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SettlersOfCatan.Events
{
    class MonopolyEvt : Event
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
            
            //We know what resource was selected by asking the trade window.
            Board.ResourceType selectedResource = tradeWindow.selectedResource;
            foreach (Player player in theBoard.playerOrder)
            {
                if (player != theBoard.currentPlayer)
                {
                    int count = player.getResourceCount(selectedResource);
                    for (int i = 0; i < count; i ++)
                    {
                        theBoard.currentPlayer.giveResource(player.takeResource(selectedResource));
                    }
                }
            }
            MessageBox.Show(theBoard.currentPlayer.getName() + " has taken " + "x" + " from all players.");
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
            tradeWindow.loadMonopoly(theBoard.currentPlayer);
            tradeWindow.Show();
            tradeWindow.Closing += executeUpdate;
        }

        public void disableEventObjects()
        {
            tradeWindow = null;
        }
    }
}
