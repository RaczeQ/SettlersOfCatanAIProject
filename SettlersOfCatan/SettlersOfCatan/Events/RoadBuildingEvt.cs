using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Events
{
    /*
        When the player uses a development card.
    */
    class RoadBuildingEvt : Event
    {

        public Board theBoard;
        int builtRoads = 0;
        EvtOwnr owner;
        public override void beginExecution(Board board, EvtOwnr evt)
        {
            theBoard = board;
            owner = evt;
            theBoard.addEventText(UserMessages.PlayerUsedRoadBuilding(board.currentPlayer));
            theBoard.addEventText(UserMessages.ROAD_BUILDING_INSTRUCTIONS);
            enableEventObjects();
        }

        public override void executeUpdate(Object sender, EventArgs e)
        {
            //Runs when the player clicks on a road.
            if (sender is Road)
            {
                //We know this is a road object
                Road rd = (Road)sender;
                    //Try to build a road.
                try
                {
                    rd.buildRoad(theBoard.currentPlayer, false);
                    builtRoads++;
                    theBoard.addEventText(UserMessages.PlayerPlacedARoad(theBoard.currentPlayer));
                    theBoard.checkForWinner();
                } catch (BuildError be)
                {
                    theBoard.addEventText(be.Message);
                }

                if (builtRoads == 2)
                {
                    endExecution();
                }
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

        public override void enableEventObjects()
        {
            foreach (Road rd in theBoard.roadLocations)
            {
                rd.Click += executeUpdate;
            }
        }

        public override void disableEventObjects()
        {
            foreach (Road rd in theBoard.roadLocations)
            {
                rd.Click -= executeUpdate;
            }
        }
    }
}
