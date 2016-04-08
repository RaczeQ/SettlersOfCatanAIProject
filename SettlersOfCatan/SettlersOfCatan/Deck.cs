using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan
{
    /*
        Deck represents a stack of cards.
     */
    [Serializable]
    public class Deck
    {
        Random cardRandom;
        List<Card> cardsInDeck;

        private int maxCards = 0;

        public Deck(int maxCardsInDeck)
        {
            cardRandom = new Random();
            maxCards = maxCardsInDeck;
            cardsInDeck = new List<Card>();
        }

        public void putCard(Card c)
        {
            cardsInDeck.Add(c);
        }

        public void putCardBottom(Card c)
        {
            cardsInDeck.Insert(0, c);
        }

        public int cardCount()
        {
            return cardsInDeck.Count();
        }

        /*

            Draw a card from the top of the deck. (last index)

         */
        public Card drawTopCard()
        {
            if (cardsInDeck.Count < 1)
            {
                throw new Exception("No cards in deck.");
            }
            Card drawnCard = cardsInDeck[cardsInDeck.Count - 1];
            cardsInDeck.Remove(drawnCard);
            return drawnCard;
        }

        /*

            Draw a card at random from the deck.

         */
        public Card drawRandomCard()
        {
            if (cardsInDeck.Count < 1)
            {
                throw new Exception("No cards in deck.");
            }
            int index = cardRandom.Next(0, cardsInDeck.Count);
            Card drawnCard = cardsInDeck[index];
            cardsInDeck.RemoveAt(index);
            return drawnCard;
        }

        /*

            Shuffles all cards in the current deck.

         */
        public void shuffleDeck()
        {
            if (cardsInDeck.Count < 1)
            {
                throw new Exception("No cards in deck.");
            }
            int deckCount = cardsInDeck.Count;

            List<Card> newList = new List<Card>();
            for (int i = 0; i < deckCount; i++)
            {
                newList.Add(drawRandomCard());
            }
            cardsInDeck = newList;
        }
    }
}
