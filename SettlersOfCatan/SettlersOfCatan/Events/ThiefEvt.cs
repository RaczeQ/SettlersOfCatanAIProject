using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Events
{
    
    class ThiefEvt : Event
    {

        Board theBoard;
        EvtOwnr owner;

        NumberChip oldThiefLocation;
        TerrainTile terrainTileWithThief;
        private int state = 0;
        public void beginExecution(Board b, EvtOwnr evt)
        {
            theBoard = b;
            owner = evt;
            enableEventObjects();
            theBoard.addEventText(theBoard.currentPlayer.getPlayerName() + " please select a location to move the thief.");
        }

        /*

         */
        public void executeUpdate(Object sender, EventArgs e)
        {
            //Check
            switch (state)
            {
                case 0:
                    if (sender is NumberChip)
                    {
                        NumberChip nc = (NumberChip)sender;
                        if (nc.isBlocked() && nc.getNumber() != 0)
                        {
                            if (Board.THIEF_MUST_MOVE)
                            {
                                theBoard.addEventText("The robber cannot stay in the same location.");
                            }
                            else
                            {
                                theBoard.addEventText(theBoard.currentPlayer.getPlayerName() + " has chosen to keep the robber where it is.");
                                oldThiefLocation.removeThief();
                                nc.placeThief();
                                oldThiefLocation = nc;
                                if (chitHasPlayers())
                                {
                                    state++;
                                    theBoard.addEventText("Please select a player to steal 1 resource card from. The card is chosen at random.");
                                }
                                else
                                {
                                    endExecution();
                                }
                            }
                        }
                        else if (!nc.isBlocked() && nc.getNumber() != 0)
                        {
                            theBoard.addEventText(theBoard.currentPlayer.getPlayerName() + " has moved the robber.");
                            oldThiefLocation.removeThief();
                            nc.placeThief();
                            oldThiefLocation = nc;
                            if (chitHasPlayers())
                            {
                                state++;
                                theBoard.addEventText("Please select a player to steal 1 resource card from. The card is chosen at random.");
                            } else
                            {
                                endExecution();
                            }
                        }
                        else
                        {
                            if (Board.THIEF_CANNOT_GO_HOME)
                            {
                                theBoard.addEventText("You cannot move the robber back to the desert.");
                            }
                            else
                            {
                                theBoard.addEventText(theBoard.currentPlayer.getPlayerName() + " has moved the robber back to the desert.");
                                oldThiefLocation.removeThief();
                                nc.placeThief();
                                oldThiefLocation = nc;
                                if (chitHasPlayers())
                                {
                                    state++;
                                    theBoard.addEventText("Please select a player to steal 1 resource card from. The card is chosen at random.");
                                } else
                                {
                                    endExecution();
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    if (!(sender is Settlement))
                    {
                        //We only run this check in case something did not clean up properly from a previous event.
                        return;
                    }

                    //We check if the settlement is actually adjascent to the thief.
                    if (!terrainTileWithThief.hasSettlement((Settlement)sender))
                    {
                        throw new SettlementNotNearThiefException();
                    }

                    //Get the player at the chosen location (sender is that location)
                    Player playerToStealFrom = ((Settlement)sender).getOwningPlayer();
                    
                    //Check if there is a player at the location.
                    if (playerToStealFrom== null)
                    {
                        throw new NoPlayerAtSettlementException();
                    }

                    //Check if the player is not the current player.
                    if (playerToStealFrom == theBoard.currentPlayer)
                    {
                        throw new SamePlayerException();
                    }

                    //At this point we can safely assume a card can be taken.
                    ResourceCard rCard = playerToStealFrom.takeRandomResource();
                    //We check if the card is null because if the player has no cards to give takeRandomResource() returns null.
                    if (rCard != null)
                    {
                        //What happens if there were cards to steal
                        theBoard.currentPlayer.giveResource(playerToStealFrom.takeRandomResource());
                        theBoard.addEventText(UserMessages.PlayerGotResourceFromPlayer(theBoard.currentPlayer, playerToStealFrom, rCard.getResourceType()));
                    }
                    else
                    {
                        //What happens if there were no cards to steal.
                        theBoard.addEventText(UserMessages.PlayerGoNoResourceFromPlayer(theBoard.currentPlayer, playerToStealFrom));
                    }
                    break;
            }

        }

        public void endExecution()
        {
            disableEventObjects();
            owner.subeventEnded();
        }

        public bool chitHasPlayers()
        {
            //get the tiles that has the number chit
            foreach (Tile t in theBoard.boardTiles)
            {
                if (t is TerrainTile)
                {
                    if (((TerrainTile)t).isGatherBlocked())
                    {
                        //See if there are any players here
                        terrainTileWithThief = (TerrainTile)t;
                    }
                }
            }

            foreach (Settlement set in terrainTileWithThief.adjascentSettlements)
            {
                if (set.getOwningPlayer() != null)
                {
                    return true;
                }
            }
            return false;
        }

        public void enableEventObjects()
        {
            foreach (Tile t in theBoard.boardTiles)
            {
                if (t is TerrainTile)
                {
                    TerrainTile tt = (TerrainTile)t;
                    tt.getNumberChip().Click += executeUpdate;
                    if (tt.getNumberChip().isBlocked())
                    {
                        oldThiefLocation = tt.getNumberChip();
                    }
                    foreach (Settlement set in tt.adjascentSettlements)
                    {
                        set.Click += executeUpdate;
                    }
                }
            }
        }

        public void disableEventObjects()
        {
            foreach (Tile t in theBoard.boardTiles)
            {
                if (t is TerrainTile)
                {
                    TerrainTile tt = (TerrainTile)t;
                    tt.getNumberChip().Click -= executeUpdate;
                    foreach (Settlement set in tt.adjascentSettlements)
                    {
                        set.Click -= executeUpdate;
                    }
                }
            }
        }
    }
}