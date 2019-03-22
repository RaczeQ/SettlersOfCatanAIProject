﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using AutoMapper;
using SettlersOfCatan.AI;
using SettlersOfCatan.Utils;

namespace SettlersOfCatan.GameObjects
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

        public static Board.ResourceType[] CITY_COST = 
            {
                Board.ResourceType.Wheat, Board.ResourceType.Wheat, Board.ResourceType.Ore,
                Board.ResourceType.Ore, Board.ResourceType.Ore
            };
        public static Board.ResourceType[] SETTLEMENT_COST = 
            {
                Board.ResourceType.Brick, Board.ResourceType.Wood, Board.ResourceType.Wheat, Board.ResourceType.Sheep
            };
        public static Board.ResourceType[] ROAD_COST =
            {
                Board.ResourceType.Brick, Board.ResourceType.Wood
            };
        public static Board.ResourceType[] DEV_CARD_COST =
        {
            Board.ResourceType.Sheep, Board.ResourceType.Wheat, Board.ResourceType.Ore
        };

        public Deck developmentCards;
        public List<ResourceCard> resources;

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
            developmentCards.shuffleDeck();

            for (int i = 0; i < 19; i ++)
            {
                resources.Add(new ResourceCard(Board.ResourceType.Brick));
                resources.Add(new ResourceCard(Board.ResourceType.Ore));
                resources.Add(new ResourceCard(Board.ResourceType.Sheep));
                resources.Add(new ResourceCard(Board.ResourceType.Wheat));
                resources.Add(new ResourceCard(Board.ResourceType.Wood));
            }
        }

        public string ResourcesList()
        {
            var resDict = resources.GroupBy(x => x.getResourceType()).ToDictionary(k => k.Key, v => v.Count());
            var result = new List<string>();
            foreach (Board.ResourceType r in Enum.GetValues(typeof(Board.ResourceType)))
            {
                result.Add($"{r}: {resources.Where(rc => rc.getResourceType() == r).Count()}");
            }
            return string.Join(", ", result);
        }

        /*
            Players can get this type of resource. If no resource is available, none is given.
         */
        public ResourceCard giveOutResource(Board.ResourceType type)
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
                //MessageBox.Show("There are no more resource cards in the deck.");
            }
            return c;
        }

        public bool canGiveOutResource(Board.ResourceType type, int count)
        {
            bool can = false;
            int cardCount = 0;
            foreach (ResourceCard rc in resources)
            {
                if (rc.getResourceType()==type)
                {
                    cardCount++;
                }
            }
            if (cardCount>= count)
            {
                can = true;
            }
            return can;
        }

        public void putResourceCard(ResourceCard rc)
        {
            this.resources.Add(rc);
        }


        public void takePayment(Player p, Board.ResourceType[] paymentList)
        {
            foreach (Board.ResourceType resType in paymentList)
            {
                var resource = p.takeResource(resType);
                this.resources.Add(resource);
            }
        }

        public static bool hasPayment(Player p, Board.ResourceType[] paymentList)
        {
            int[] resourceList = new int[6];
            bool satisfied = true;
            foreach (Board.ResourceType res in paymentList)
            {
                resourceList[(int)res]++;
            }
            for (int i = 0; i < 6; i++)
            {
                if (!(p.getResourceCount((Board.ResourceType)i) >= resourceList[i]))
                {
                    satisfied = false;
                }
            }
            return satisfied;
        }

        public DevelopmentCard purchaseDevelopmentCard(Player p)
        {
            //Check for payment
            //Check for available cards
            if (!hasPayment(p, Bank.DEV_CARD_COST))
            {
                throw new BuildError(BuildError.NOT_ENOUGH_RESOURCES);
            }

            if (developmentCards.cardCount() < 1)
            {
                throw new BuildError(BuildError.NOT_ENOUGH_DEV_CARDS);
            }

            //We passed the previous checks, give the player a card
            return (DevelopmentCard)developmentCards.drawTopCard();
        }

        public bool tradeWithBank(Player player, TradeProposition proposition, IDictionary<Board.ResourceType, int> paymentCosts)
        {
            var selledResourceAmount = proposition.boughtResourceAmount * paymentCosts[proposition.selledResource];
            if (!canGiveOutResource(proposition.boughtResource, proposition.boughtResourceAmount)
                || player.getResourceCount(proposition.selledResource) < selledResourceAmount) {
                return false;
            }

            //Give bank this resource
            for (int i = 0; i < selledResourceAmount; i++)
            {
                ResourceCard rc = player.takeResource(proposition.selledResource);
                putResourceCard(rc);
            }

            //Get cards
            for (int i = 0; i < proposition.boughtResourceAmount; i++)
            {
                player.giveResource(giveOutResource(proposition.boughtResource));
            }
            return true;
        }
    }
}
