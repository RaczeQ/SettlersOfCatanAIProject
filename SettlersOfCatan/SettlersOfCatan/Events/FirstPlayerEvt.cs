using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Events
{
    /*

        This event determines what player gets to go first.
        The process is "simple".

     */
    public class FirstPlayerEvt : Event
    {

        public Board theBoard;
        //int[] playerRolls = new int[4];

        public List<Player> playersToRoll;
        public List<int> playerRolls;
        int rollPosition = 0;
        public int state = 0;

        public FirstPlayerEvt()
        {
            
        }

        public void beginExecution(Board b)
        {
            theBoard = b;
            //Locks down all but the needed controls.
            //Adds the onExecuteUpdate to the controls that need it.
            theBoard.dice.Click += executeUpdate;
            theBoard.btnPlayerTrade.Enabled = false;
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.pbBuildDevelopmentCard.Enabled = false;

            playersToRoll = new List<Player>();
            playerRolls = new List<int>();
            foreach (Player p in b.playerPanels)
            {
                playersToRoll.Add(p);
            }
            theBoard.addEventText("Player " + playersToRoll[0].getPlayerName() + " please roll the dice.");
        }

        public void executeUpdate(Object sender, EventArgs e)
        {

            switch (state)
            {
                case 0:
                    Player p = playersToRoll[rollPosition];
                    int roll = theBoard.dice.getRollValue();
                    theBoard.addEventText("Player " + p.getPlayerName() + " rolled " + (roll==8||roll==11? "an " : "a ") + roll + ".");
                    playerRolls.Add(roll);
                    rollPosition++;
                    if (rollPosition >= playersToRoll.Count)
                    {
                        state++;
                        executeUpdate(sender, e);
                    } else
                    {
                        p = playersToRoll[rollPosition];
                        theBoard.addEventText("Player " + p.getPlayerName() + " please roll the dice.");
                    }
                    break;
                case 1:
                    //Tie prevention
                    int max = 0;
                    int playerInd = 0;
                    bool tieDetected = false;
                    for (int ind = 0; ind < playersToRoll.Count; ind ++)
                    {
                        if (playerRolls[ind] > max)
                        {
                            tieDetected = false;
                            playerInd = ind;
                            max = playerRolls[ind];

                        } else if (playerRolls[ind] == max)
                        {
                            tieDetected = true;
                        }
                    }

                    if (tieDetected)
                    {
                        theBoard.addEventText("There was a tie between:");
                        //There was a tie. Reset all things for another roll...
                        List<Player> newPlayers = new List<Player>();
                        for (int ind = 0; ind < playersToRoll.Count; ind++)
                        {
                            if (playerRolls[ind] == max)
                            {
                                newPlayers.Add(playersToRoll[ind]);
                                theBoard.addEventText(playersToRoll[ind].getPlayerName());
                            }
                        }
                        rollPosition = 0;
                        playerRolls.Clear();
                        playersToRoll.Clear();
                        playersToRoll = newPlayers;
                        state = 0;
                        theBoard.addEventText("");
                        theBoard.addEventText("Player " + playersToRoll[0].getPlayerName() + " please roll the dice.");
                    } else
                    {
                        //All was good the first player is *drumroll*
                        theBoard.firstPlayer = playersToRoll[playerInd];
                        theBoard.addEventText("Congrats player " + theBoard.firstPlayer.getPlayerName() + " you won the roll and are the first player!");
                        endExecution();
                    }
                    break;
            }

        }


        public void endExecution()
        {
            //Completes the cleanup and removes the event from the controls it used.
            theBoard.dice.Click -= executeUpdate;
            theBoard.dice.Enabled = false;
            theBoard.eventEnded();
        }

    }
}
