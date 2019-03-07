using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SettlersOfCatan.Events
{
    class DiceRollEvt : Event, EvtOwnr
    {

        Board theBoard;
        EvtOwnr owner;
        public void beginExecution(Board b, EvtOwnr evt)
        {
            theBoard = b;
            owner = evt;
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

        public void executeUpdate(Object sender, EventArgs e)
        {
            if (sender is Dice)
            {
                //
                Dice dice = (Dice)sender;
                theBoard.addEventText(theBoard.currentPlayer.getName() + " rolled a " + dice.getRollValue());
                if (dice.getRollValue()==7)
                {
                    ////Thief event!!
                    ////Go through each player to see if they loose thier resources
                    ////Once the thief event has successfully run, we can terminate this event.
                    //MessageBox.Show(theBoard.currentPlayer.getName() + " rolled a 7. The thief has been activated!");
                    //RobberStealEvt evt = new RobberStealEvt();
                    //disableEventObjects();
                    //evt.beginExecution(theBoard, this);
                }
                else
                {
                    //Players get resources
                    foreach (Tile tile in theBoard.boardTiles)
                    {
                        if (tile is TerrainTile)
                        {
                            TerrainTile tt = (TerrainTile)tile;
                            if (tt.getGatherChance() == dice.getRollValue())
                            {
                                if (!tt.isGatherBlocked())
                                {
                                    //All players here get the resource.
                                    foreach (Settlement set in tt.adjascentSettlements)
                                    {
                                        if (set.getOwningPlayer() != null)
                                        {
                                            ResourceCard rc = Board.TheBank.giveOutResource(tt.getResourceType());
                                            if (rc != null)
                                            {
                                                set.getOwningPlayer().giveResource(rc);
                                                if (set.city())
                                                {
                                                    //Give an extra for cities
                                                    set.getOwningPlayer().giveResource(rc);
                                                }
                                            }
                                            else
                                            {
                                                theBoard.addEventText("Not enough " + Board.RESOURCE_NAMES[(int)tt.getResourceType()] + " to give  to " + set.getOwningPlayer().getName());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Event resolved
                    endExecution();
                }
            }
        }

        public void endExecution()
        {

            disableEventObjects();
            theBoard.subeventEnded();
        }

        public void enableEventObjects()
        {
            theBoard.dice.Click += executeUpdate;
            theBoard.dice.Enabled = true;
            
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.pbBuildDevelopmentCard.Enabled = false;
        }

        public void disableEventObjects()
        {
            theBoard.dice.Click -= executeUpdate;
            
            theBoard.btnBankTrade.Enabled = true;
            theBoard.btnEndTurn.Enabled = true;
            theBoard.pbBuildDevelopmentCard.Enabled = true;
            theBoard.dice.Enabled = false;
        }


        //Runs when the theif event has finished
        public void subeventEnded()
        {
            endExecution();
        }
    }
}
