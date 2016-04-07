using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Events
{
    class FirstResourcesEvt : Event
    {

        Board theBoard;
        EvtOwnr owner;

        public void beginExecution(Board b, EvtOwnr evt)
        {
            //Since this event does not depend on user input we can safely turn
            // off all controls.
            theBoard = b;
            owner = evt;
            executeUpdate(this, new EventArgs());
        }

        public void executeUpdate(Object sender, EventArgs e)
        {
            foreach (Tile t in theBoard.boardTiles)
            {
                if (t is TerrainTile)
                {
                    TerrainTile tt = (TerrainTile)t;
                    tt.distributeResource();
                }
            }
            endExecution();
        }

        public void endExecution()
        {
            theBoard.subeventEnded();
        }

        public void enableEventObjects()
        {

        }

        public void disableEventObjects()
        {

        }

    }
}
