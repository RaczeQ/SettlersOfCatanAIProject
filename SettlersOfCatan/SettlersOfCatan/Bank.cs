using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SettlersOfCatan
{
    [Serializable]
    public class Bank
    {

        //The correct count of development cards for a four player game.
        public static DevelopmentCard.DevCardType[] fourPlayerDevCards =
            {
                DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight,
                DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight,
                DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight,
                DevelopmentCard.DevCardType.Knight, DevelopmentCard.DevCardType.Knight,
                DevelopmentCard.DevCardType.Monopoly, DevelopmentCard.DevCardType.Monopoly,
                DevelopmentCard.DevCardType.Road, DevelopmentCard.DevCardType.Road,
                DevelopmentCard.DevCardType.Plenty, DevelopmentCard.DevCardType.Plenty,
                DevelopmentCard.DevCardType.Victory, DevelopmentCard.DevCardType.Victory, DevelopmentCard.DevCardType.Victory,
                DevelopmentCard.DevCardType.Victory, DevelopmentCard.DevCardType.Victory
            };
        public static String[] devCardImageNames = {
            "Resources/Development_Card_Back.png",
            "Resources/knight_card.png",
            "Resources/victory_point_card.png",
            "Resources/road_building_card.png",
            "Resources/year_of_plenty_card.png",
            "Resources/monopoly_card.png" };
        public static Bitmap[] devCardImages = new Bitmap[6];

        public Deck developmentCards;
        private List<ResourceCard> resources;

        public Bank()
        {
            resources = new List<ResourceCard>();
            //Load the development card images.
            for (int i = 0; i < 6; i++)
            {
                devCardImages[i] = new Bitmap(devCardImageNames[i]);
            }

            developmentCards = new Deck(25);
            //Create the development cards
            foreach (DevelopmentCard.DevCardType dct in fourPlayerDevCards)
            {
                DevelopmentCard dc = new DevelopmentCard();
                dc.setType(dct);
                developmentCards.putCard(dc);
            }

            for (int i = 0; i < 19; i ++)
            {
                resources.Add(new ResourceCard(Board.ResourceType.Brick));
                resources.Add(new ResourceCard(Board.ResourceType.Ore));
                resources.Add(new ResourceCard(Board.ResourceType.Sheep));
                resources.Add(new ResourceCard(Board.ResourceType.Wheat));
                resources.Add(new ResourceCard(Board.ResourceType.Wood));
            }
        }

        /*
            Players can get this type of resource. If no resource is available, none is given.
         */
        public ResourceCard giveResource(Board.ResourceType type)
        {
            ResourceCard c = null;
            for (int i = 0; i < resources.Count; i ++)
            {
                if (resources[i].getResourceType() == type)
                {
                    c = resources[i];
                    i = resources.Count;
                }
            }

            if (c != null)
            {
                resources.Remove(c);
            } else
            {
                MessageBox.Show("There are no more resource cards in the deck.");
            }
            return c;
        }

        public void putResourceCard(ResourceCard rc)
        {
            this.resources.Add(rc);
        }


    }
}
