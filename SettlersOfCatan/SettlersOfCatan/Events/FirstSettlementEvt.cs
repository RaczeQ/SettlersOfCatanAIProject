using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan.Events
{
    /*

        The First settlement event deals with players getting their first two settlements and first two roads.

        The first player places 1 settlement and 1 road and in order each player does the same.
        The last player gets to place two settlements and two roads and in reverse order each player places an additional 1 settlement and road each.

        Then each player gets awared resources, which is not covered by this event!!!

     */
    
    class FirstSettlementEvt : Event
    {
        Board theBoard;
        EvtOwnr owner;
        public int playerNum;

        public List<int> playerTurnOrder;

        public void beginExecution(Board b, EvtOwnr evt)
        {
            theBoard = b;
            owner = evt;
            //We don't need any of these controls (we can safely assume no other controls will be available.
            playerTurnOrder = new List<int>();
            int num = -1;
            enableEventObjects();
            for (int i = 0; i < theBoard.playerOrder.Count()*2; i ++)
            {
                if (i < theBoard.playerOrder.Count())
                {
                    playerTurnOrder.Add(i);
                } else
                {
                    num += 2;
                    playerTurnOrder.Add(i - num);
                }
            }
            theBoard.addEventText("Player " + theBoard.playerOrder[0].getPlayerName() + " please place your first settlement and road.");
        }

        public void executeUpdate(Object sender, EventArgs e)
        {
            //Determines if this is only the first pass or the second pass.
            bool firstPass = !(playerNum+1>theBoard.playerPanels.Count());
            //Determine what player is placing the settlement || road.
            Player p = theBoard.playerOrder[playerTurnOrder[playerNum]];
            //Determine what the player is trying to do.
            if (sender is Settlement)
            {
                //Check if the player has any more settlements they are allowed to build.
                if ((firstPass && p.getSettlementCount() < 1) || (!firstPass && p.getSettlementCount() < 2))
                {
                    try
                    {
                        ((Settlement)sender).buildSettlement(p, false);
                        theBoard.addEventText(UserMessages.PlayerPlacedASettlement(p));
                        theBoard.checkForWinner();
                    } catch(BuildError be)
                    {
                        theBoard.addEventText(be.Message);
                    }
                } else
                {
                    theBoard.addEventText("You may not place any more settlements.");
                }
            }
            else if (sender is Road)
            {
                //Check if the player is allowed to build another road.
                if ((firstPass && p.getRoadCount() < 1) || (!firstPass && p.getRoadCount() < 2))
                {
                    try
                    {
                        ((Road)sender).buildRoad(p, false);
                        theBoard.addEventText(UserMessages.PlayerPlacedARoad(p));
                        theBoard.checkForWinner();
                    } catch (BuildError be)
                    {
                        theBoard.addEventText(be.Message);
                    }
                } else
                {
                    theBoard.addEventText("You may not place any more roads.");
                }
            } else
            {
                theBoard.addEventText("Invalid click! " + sender.GetType().ToString());
            }

            //Move to the next player in the turn order only when the previous player has both the settlement and road built.
            if (firstPass && p.getSettlementCount() == 1 && p.getRoadCount() == 1 || !firstPass && p.getSettlementCount() == 2 && p.getRoadCount() == 2)
            {
                playerNum++;
                if (playerNum >= playerTurnOrder.Count)
                {
                    theBoard.addEventText("All players have placed their settlements and roads!");
                    endExecution();
                } else
                {
                    firstPass = !(playerNum + 1 > theBoard.playerPanels.Count());
                    theBoard.addEventText("Player " + theBoard.playerOrder[playerTurnOrder[playerNum]].getPlayerName() 
                        + " please place your " + (firstPass? "first" : "second" ) + " settlement and road.");
                }
            }

        }
        public void endExecution()
        {

            disableEventObjects();
            owner.subeventEnded();

        }

        public void enableEventObjects()
        {
            foreach (Road rd in theBoard.roadLocations)
            {
                rd.Click += executeUpdate;
            }
            foreach (Settlement st in theBoard.settlementLocations)
            {
                st.Click += executeUpdate;
            }

        }

        public void disableEventObjects()
        {
            foreach (Road rd in theBoard.roadLocations)
            {
                rd.Click -= executeUpdate;
            }
            foreach (Settlement st in theBoard.settlementLocations)
            {
                st.Click -= executeUpdate;
            }
        }
    }
}
