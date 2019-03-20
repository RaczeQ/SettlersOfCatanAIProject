using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan.GameObjects
{
    public class Harbor : PictureBox
    {

        private int tradeRequiredResourceCount = 0;
        //If a harbor has resource type of desert, the player can choose their desired resource to trade for.
        private Board.ResourceType tradeOutputResource = Board.ResourceType.Desert;

        public List<Settlement> validTradeLocations;

        public Harbor(int requiredCount, Board.ResourceType resource) {
            tradeRequiredResourceCount = requiredCount;
            tradeOutputResource = resource;
            validTradeLocations = new List<Settlement>();
        }

        public void setRequiredResourceCount(int count)
        {
            tradeRequiredResourceCount = count;
        }

        public int getRequiredResourceCount()
        {
            return tradeRequiredResourceCount;
        }

        public void setTradeOutputResource (Board.ResourceType res)
        {
            tradeOutputResource = res;
        }

        public Board.ResourceType getTradeOutputResource()
        {
            return tradeOutputResource;
        }

        public void addTradeLocation(Settlement s)
        {
            this.validTradeLocations.Add(s);
        }

        /*
            Checks if the specified player has a settlement adjascent to this harbor.

         */
        public bool playerHasValidSettlement(Player pl)
        {
            foreach (Settlement settlement in validTradeLocations)
            {
                if (settlement.getOwningPlayer() == pl)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
