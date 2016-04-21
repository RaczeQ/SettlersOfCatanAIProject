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
        EvtOwnr owner;
        //int[] playerRolls = new int[4];

        public List<Player> playersToRoll;
        public List<int> playerRolls;
        int rollPosition = 0;
        public int state = 0;

        public void beginExecution(Board b, EvtOwnr evt)
        {
            theBoard = b;
            owner = evt;
            //Locks down all but the needed controls.
            //Adds the onExecuteUpdate to the controls that need it.
            enableEventObjects();
            playersToRoll = new List<Player>();
            playerRolls = new List<int>();
            foreach (Player p in b.playerPanels)
            {
                playersToRoll.Add(p);
            }
            theBoard.addEventText(UserMessages.PlayerDiceRollPrompt(playersToRoll[0]));
        }

        public void executeUpdate(Object sender, EventArgs e)
        {

            switch (state)
            {
                case 0:
                    Player p = playersToRoll[rollPosition];
                    int roll = theBoard.dice.getRollValue();
                    theBoard.addEventText(UserMessages.PlayerRolledANumber(p, roll));
                    playerRolls.Add(roll);
                    rollPosition++;
                    if (rollPosition >= playersToRoll.Count)
                    {
                        state++;
                        executeUpdate(sender, e);
                    } else
                    {
                        p = playersToRoll[rollPosition];
                        theBoard.addEventText(UserMessages.PlayerDiceRollPrompt(p));
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
                        theBoard.addEventText(UserMessages.THERE_WAS_A_TIE);
                        //There was a tie. Reset all things for another roll...
                        List<Player> newPlayers = new List<Player>();
                        for (int ind = 0; ind < playersToRoll.Count; ind++)
                        {
                            if (playerRolls[ind] == max)
                            {
                                newPlayers.Add(playersToRoll[ind]);
                                theBoard.addEventText(playersToRoll[ind].getName());
                            }
                        }
                        rollPosition = 0;
                        playerRolls.Clear();
                        playersToRoll.Clear();
                        playersToRoll = newPlayers;
                        state = 0;
                        theBoard.addEventText(UserMessages.PlayerDiceRollPrompt(playersToRoll[0]));
                    } else
                    {
                        //All was good the first player is *drumroll*
                        theBoard.firstPlayer = playersToRoll[playerInd];
                        theBoard.addEventText(UserMessages.PlayerWinsDiceRoll(theBoard.firstPlayer));
                        endExecution();
                    }
                    break;
            }

        }


        public void endExecution()
        {
            disableEventObjects();
            owner.subeventEnded();
        }

        public void disableEventObjects()
        {
            theBoard.dice.Click -= executeUpdate;
            theBoard.dice.Enabled = false;
        }

        public void enableEventObjects()
        {
            theBoard.dice.Click += executeUpdate;
            theBoard.btnPlayerTrade.Enabled = false;
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.pbBuildDevelopmentCard.Enabled = false;
        }

    }
}
