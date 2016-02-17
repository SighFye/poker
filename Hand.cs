using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
    /// <summary>
    /// A collection of standard playing cards used as a poker hand.
    /// This could either be 5 hand poker or texas holdem.
    /// </summary>
    internal class Hand
    {
        private List<Card> cards;
        private int maxCards;
        private int handValue = 0;
        /// <summary>
        /// Creates a hand of cards with a max number of cards it can hold
        /// </summary>
        /// <param name="newMaxCards">The max number of cards able to hold in this hand.</param>
        internal Hand(int newMaxCards)
        {
            maxCards = newMaxCards;
            cards = new List<Card>(maxCards);
        }
        /// <summary>
        /// Returns the card at the given index
        /// </summary>
        /// <param name="index">The index of the card requested</param>
        /// <returns>The cards asked for</returns>
        public Card getCard(int index)
        {
            return cards[index];
        }
        /// <summary>
        /// Adds a card to the cards array if the cards array's length is not more than maxCards.
        /// </summary>
        /// <param name="newCard">The Card to be added to the cards array</param>
        /// <returns>Returns true is card is added or false if its not.</returns>
        public bool addCard(Card newCard)
        {
            bool result = false; //Assume the add a card failed.
            //First Check the length of the hand.
            if (cards.Count < maxCards)
            {
                cards.Add(newCard); //Add the new card to the cards array using the length as an index.
                result = true; //The result turns to true as the add card worked.
            }
            return result;
        }
    }
}
