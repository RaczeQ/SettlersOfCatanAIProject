using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SettlersOfCatan
{
    class Player
    {
        //Player colors are red blue white orange green and brown
        public static String[] playerColorNames = { "Red", "Blue", "White", "Orange", "Green", "Brown" };
        public static Color[] playerColors = { Color.Red, Color.Blue, Color.White, Color.Orange, Color.Green, Color.Brown };

        private int playerNumber = 0;
        private List<ResourceCard> resources;

        public Player(int playerNumber)
        {
            this.playerNumber = playerNumber;
            resources = new List<ResourceCard>();
        }

        /**
            Gets the number of the specified resource type.
         */
        public int getResourceCount(Board.ResourceType type)
        {
            int count = 0;
            foreach (ResourceCard res in resources)
            {
                if (res.getResourceType() == type)
                {
                    count++;
                }
            }
            return count;
        }

        /**
            Gets a resource card witht the matching type from the player's
            deck. If no available card exists null is returned.
         */
        public ResourceCard takeResource(Board.ResourceType type)
        {
            ResourceCard rCard = null;
            foreach(ResourceCard res in resources)
            {
                if (res.getResourceType() == type)
                {
                    rCard = res;
                }
            }
            if (rCard != null)
            {
                resources.Remove(rCard);
            }
            return rCard;
        }

        public void giveResource(ResourceCard resCard)
        {
            this.resources.Add(resCard);
        }
    }
}
