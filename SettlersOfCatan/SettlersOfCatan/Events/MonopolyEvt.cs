using SettlersOfCatan.GameObjects;
using SettlersOfCatan.Utils;
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

        public override void beginExecution(Board board, EvtOwnr evt)
        {
            theBoard = board;
            enableEventObjects();
            owner = evt;
        }

        public override void executeUpdate(Object sender, EventArgs e)
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
            MessageBox.Show(theBoard.currentPlayer.getName() + " has taken " + Board.RESOURCE_NAMES[(int)selectedResource] + " from all players.");
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
            tradeWindow.loadMonopoly(theBoard.currentPlayer);
            tradeWindow.Show();
            tradeWindow.Closing += executeUpdate;
        }

        public override void disableEventObjects()
        {
            tradeWindow = null;
        }
    }
}
