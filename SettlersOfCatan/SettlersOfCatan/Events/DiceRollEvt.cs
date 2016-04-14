using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            enableEventObjects();
        }

        public void executeUpdate(Object sender, EventArgs e)
        {
            if (sender is Dice)
            {
                //
                Dice dice = (Dice)sender;
                theBoard.addEventText(theBoard.currentPlayer.getPlayerName() + " rolled a " + dice.getRollValue());
                if (dice.getRollValue()==7)
                {
                    //Thief event!!
                    theBoard.addEventText("Robber event! Players with more than 7 resource lose half of them.");
                    //Once the thief event has successfully run, we can terminate this event.
                    ThiefEvt evt = new ThiefEvt();
                    evt.beginExecution(theBoard, this);
                    disableEventObjects();
                } else
                {
                    //Players get resources
                    theBoard.addEventText("Get resources.");
                    foreach (Tile tile in theBoard.boardTiles)
                    {
                        if (tile is TerrainTile)
                        {
                            TerrainTile tt = (TerrainTile)tile;
                            if (tt.getGatherChance() == dice.getRollValue())
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
                                        } else
                                        {
                                            theBoard.addEventText("Not enough " + Board.RESOURCE_NAMES[(int)tt.getResourceType()] + " to give  to " + set.getOwningPlayer().getPlayerName());
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
            theBoard.btnPlayerTrade.Enabled = false;
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.pbBuildDevelopmentCard.Enabled = false;
        }

        public void disableEventObjects()
        {
            theBoard.dice.Click -= executeUpdate;
            theBoard.btnPlayerTrade.Enabled = true;
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
