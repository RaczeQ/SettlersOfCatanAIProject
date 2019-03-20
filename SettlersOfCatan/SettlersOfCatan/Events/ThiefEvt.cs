using SettlersOfCatan.GameObjects;
using SettlersOfCatan.Utils;
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
        public override void beginExecution(Board b, EvtOwnr evt)
        {
            theBoard = b;
            owner = evt;
            enableEventObjects();
            theBoard.addEventText(UserMessages.PlayerMoveThief(theBoard.currentPlayer));
        }

        /*
         * The actual event code is located in exec because executeUpdate is only called by buttons.
         * I have no way of handling button exceptions directly, so I use a secondary function to
         * run the main code then use the main function to handle the exceptions.
         */
        public override void executeUpdate(object sender, EventArgs e)
        {
            try
            {
                exec(sender, e);
            }
            catch (ThiefException ex)
            {
                theBoard.addEventText(ex.Message);
            }

        }

        /*
            A bit of an odd workaround for the issue of having no way to handle exceptions from buttons firing these 
                methods.
         */
        public void exec(object sender, EventArgs e)
        { 
            //Check
            switch (state)
            {
                case 0:
                    if (sender is NumberChip)
                    {
                        NumberChip nc = (NumberChip)sender;

                        //Check if the player has selected the desert
                        if (Board.THIEF_MUST_MOVE && nc.isBlocked())
                        {
                            throw new ThiefException(ThiefException.THIEF_CANNOT_STAY);
                        }

                        if (Board.THIEF_CANNOT_GO_HOME && nc.getNumber() == 0)
                        {
                            throw new ThiefException(ThiefException.THIEF_CANNOT_GO_DESERT);
                        }

                        theBoard.addEventText(UserMessages.RobberHasMoved(theBoard.currentPlayer));
                        oldThiefLocation.removeThief();
                        nc.placeThief();
                        oldThiefLocation = nc;
                        if (chitHasPlayers())
                        {
                            state++;
                            theBoard.addEventText(UserMessages.PLAYER_STEAL_RESOURCES);
                        } else
                        {
                            endExecution();
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
                        throw new ThiefException(ThiefException.CANT_STEAL_FROM_PLAYER);
                    }

                    //Get the player at the chosen location (sender is that location)
                    Player playerToStealFrom = ((Settlement)sender).getOwningPlayer();
                    
                    //Check if there is a player at the location.
                    if (playerToStealFrom== null)
                    {
                        throw new ThiefException(ThiefException.NO_PLAYER);
                    }

                    //Check if the player is not the current player.
                    if (playerToStealFrom == theBoard.currentPlayer)
                    {
                        throw new ThiefException(ThiefException.YOU_OWN_THIS_SETTLEMENT);
                    }

                    //At this point we can safely assume a card can be taken.
                    ResourceCard rCard = playerToStealFrom.takeRandomResource();
                    //We check if the card is null because if the player has no cards to give takeRandomResource() returns null.
                    if (rCard != null)
                    {
                        //What happens if there were cards to steal
                        theBoard.currentPlayer.giveResource(playerToStealFrom.takeRandomResource());
                        theBoard.addEventText(UserMessages.PlayerGotResourceFromPlayer(theBoard.currentPlayer, playerToStealFrom, rCard.getResourceType()));
                        state++;
                        endExecution();
                    }
                    else
                    {
                        //What happens if there were no cards to steal.
                        theBoard.addEventText(UserMessages.PlayerGoNoResourceFromPlayer(theBoard.currentPlayer, playerToStealFrom));
                        state++;
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
                if (set.getOwningPlayer() != null && set.getOwningPlayer() != theBoard.currentPlayer)
                {
                    return true;
                }
            }
            return false;
        }

        public override void enableEventObjects()
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
                }
            }

            foreach (Settlement set in theBoard.settlementLocations)
            {
                set.Click += executeUpdate;
            }
        }

        public override void disableEventObjects()
        {
            foreach (Tile t in theBoard.boardTiles)
            {
                if (t is TerrainTile)
                {
                    TerrainTile tt = (TerrainTile)t;
                    tt.getNumberChip().Click -= executeUpdate;
                }
            }
            foreach (Settlement set in theBoard.settlementLocations)
            {
                set.Click -= executeUpdate;
            }
        }
    }
}