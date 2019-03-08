﻿using System;
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

        public override void beginExecution(Board b, EvtOwnr evt)
        {
            theBoard = b;
            owner = evt;
            //Locks down all but the needed controls.
            //Adds the onExecuteUpdate to the controls that need it.
            playersToRoll = new List<Player>();
            playerRolls = new List<int>();
            foreach (Player p in b.playerPanels)
            {
                playersToRoll.Add(p);
            }
            theBoard.addEventText(UserMessages.PlayerDiceRollPrompt(playersToRoll[0]));
            theBoard.currentPlayer = playersToRoll[0];
            //enableEventObjects();
            if (!theBoard.currentPlayer.isAI)
            {
                enableEventObjects();
            }
            else
            {
                theBoard.dice.roll();
                executeUpdate(theBoard.dice, null);
            }
        }

        public override void executeUpdate(Object sender, EventArgs e)
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
                        theBoard.currentPlayer = p;
                        if (p.isAI)
                        {
                            theBoard.dice.roll();
                            executeUpdate(sender, e);
                        }
                        else
                        {
                            enableEventObjects();
                        }
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
                        if (playersToRoll[0].isAI)
                        {
                            theBoard.dice.roll();
                            executeUpdate(sender, e);
                        }
                        else
                        {
                            enableEventObjects();
                        }
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


        public override void endExecution()
        {
            disableEventObjects();
            if (!(owner is Board))
                owner.subeventEnded();
            else
                PlayerSemaphore.unlockGame();
        }

        public override void disableEventObjects()
        {
            theBoard.dice.Click -= executeUpdate;
            theBoard.dice.Enabled = false;
        }

        public override void enableEventObjects()
        {
            theBoard.dice.Click -= executeUpdate;
            theBoard.dice.Click += executeUpdate;
            
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.pbBuildDevelopmentCard.Enabled = false;
        }

    }
}
