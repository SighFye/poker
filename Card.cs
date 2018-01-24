using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
    /// <summary>
    /// A card in a standard deck of cards.
    /// Ace is taken as a high value card with the give value of 14. One higher than King.
    /// </summary>
    internal class Card
    {
        /// <summary>
        /// public constants shared through all cards
        /// </summary>
        public const int _ACE = 14;
        public const int _KING = 13;
        public const int _QUEEN = 12;
        public const int _JACK = 11;

        private const Dictionary<Suit, string> SUITTEXT = new Dictionary<Suit, string>()
        {
            {Suit.Clubs, "Clubs"},
            {Suit.Hearts, "Hearts"},
            {Suit.Diamonds, "Diamonds"},
            {Suit.Spades, "Spades"},
        };
        private const Dictionary<int, string> FACETEXT = new Dictionary<int, string>()
        {
            {_ACE, "Ace"},
            {_KING, "King"},
            {_QUEEN, "Queen"},
            {_JACK, "Jack"},
        };

        public Suit suit { get; private set; }
        public int cardValue { get; private set; }

        /// <summary>
        /// Creates a new card with the given suit and value
        /// </summary>
        /// <param name="newSuit">The suit of the card to be assigned to suit</param>
        /// <param name="newValue">The value of the card to be assigned to value</param>
        internal Card(Suit newSuit, int newValue)
        {
            suit = newSuit;
            cardValue = newValue;
        }

        /// <summary>
        /// Overrides the toString method to combined the value and suit as a string
        /// </summary>
        /// <returns>The combined value and suit</returns>
        public override string ToString()
        {
            string faceValue = FACETEXT.ContainsKey(cardValue) ? FACETEXT[cardValue] : cardValue.ToString();
            return String.Format("{0} of {1}", faceValue, SUITTEXT[suit]);
        }

        public static enum Suit : int
        {
            Hearts = 1,
            Diamonds = 2,
            Clubs = 3,
            Spades = 4
        }
    }
}
