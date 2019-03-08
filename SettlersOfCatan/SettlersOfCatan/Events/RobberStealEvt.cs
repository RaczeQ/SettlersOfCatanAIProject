using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SettlersOfCatan.Events
{
    /*
     * This represents when the robber is activated by the roll of "7" and steals the player's resources.
     */
    class RobberStealEvt : Event, EvtOwnr
    {

        Board theBoard;
        EvtOwnr owner;
        private List<Player> playersToGiveUpCards;
        private int state = 0;

        private Player activePlayer;

        public override void beginExecution(Board board, EvtOwnr evt)
        {
            activePlayer = board.currentPlayer;
            enableEventObjects();
            theBoard = board;
            owner = evt;
            enableEventObjects();
            playersToGiveUpCards = new List<Player>();
            //Figure out the players that need to give up resources.
            foreach (Player pl in theBoard.playerOrder)
            {
                if (pl.getTotalResourceCount() > 7)
                {
                    playersToGiveUpCards.Add(pl);
                }
            }
            executeUpdate(this, new EventArgs());
        }

        public override void executeUpdate(Object sender, EventArgs e)
        {
            switch (state)
            {
                case 0:

                    if (playersToGiveUpCards.Count() > 0)
                    {
                        theBoard.currentPlayer = playersToGiveUpCards[0];
                        MessageBox.Show(playersToGiveUpCards[0].getName() + " is holding more than 7 cards. Please select ones you wish to give up.");
                        //make the player at index 0 give up their cards
                        disableEventObjects();
                        TradeWindow tradeWindow = new TradeWindow();
                        tradeWindow.loadPlayerResourceLoss(playersToGiveUpCards[0]);
                        tradeWindow.Closing += finishedStealing;
                        tradeWindow.Show();
                        playersToGiveUpCards.Remove(playersToGiveUpCards[0]);
                    } else
                    {
                        theBoard.currentPlayer = activePlayer;
                        state++;
                        executeUpdate(sender, e);
                    }
                    break;
                case 1:
                    //Run the regular robber event.
                    ThiefEvt evt = new ThiefEvt();
                    disableEventObjects();
                    evt.beginExecution(theBoard, this);
                    state++;
                    break;
                case 2:
                    endExecution();
                    break;
            }
        }

        public void finishedStealing(Object sender, EventArgs e)
        {
            executeUpdate(sender, e);
        }

        public override void endExecution()
        {
            disableEventObjects();
            if (!(owner is Board))
                owner.subeventEnded();
        }

        public bool subeventEnded()
        {
            executeUpdate(this, new EventArgs());
            return true;
        }

        public override void enableEventObjects()
        {
            //Does not require anything on its own.
        }

        public override void disableEventObjects()
        {

        }
    }
}
