using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SettlersOfCatan
{
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

        public Bank()
        {
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

        }


    }
}
