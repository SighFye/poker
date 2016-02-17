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
        public const int _HEARTS = 1;
        public const int _DIAMONDS = 2;
        public const int _CLUBS = 3;
        public const int _SPADES = 4;
        public const int _ACE = 14;
        public const int _KING = 13;
        public const int _QUEEN = 12;
        public const int _JACK = 11;

        /// <summary>
        /// Private variables for this instance only
        /// </summary>
        private int suit;
        private int cardValue;

        /// <summary>
        /// Creates a new card with the given suit and value
        /// </summary>
        /// <param name="newSuit">The suit of the card to be assigned to suit</param>
        /// <param name="newValue">The value of the card to be assigned to value</param>
        internal Card(int newSuit, int newValue)
        {
            suit = newSuit;
            cardValue = newValue;
        }
        //======================================================================================================
        //==NO SETTER FOR SUIT OR VALUE AS ONCE THEY ARE SET ON CREATING THE CARD THEY SHOULD NOT BE CHANGED. ==
        //======================================================================================================
        /// <summary>
        /// Returns the suit of the card
        /// </summary>
        /// <returns>Returns the suit of the card as and integer value</returns>
        public int getSuit()
        {
            return suit;
        }
        /// <summary>
        /// Returns the card value
        /// </summary>
        /// <returns>Returns the card value</returns>
        public int getValue()
        {
            return cardValue;
        }
        /// <summary>
        /// Overrides the toString method to combined the value and suit as a string
        /// </summary>
        /// <returns>The combined value and suit</returns>
        public override string ToString()
        {
            string result = "";
            //Translate the value
            switch (cardValue)
            {
                case _ACE:
                    result += "Ace";
                    break;
                case _KING:
                    result += "King";
                    break;
                case _QUEEN:
                    result += "Queen";
                    break;
                case _JACK:
                    result += "Jack";
                    break;
                default:
                    result += cardValue;
                    break;
            }
            //Translate the suit
            switch (suit)
            {
                case _HEARTS:
                    result += " of Hearts";
                    break;
                case _DIAMONDS:
                    result += " of Diamonds";
                    break;
                case _CLUBS:
                    result += " of Clubs";
                    break;
                case _SPADES:
                    result += " of Spades";
                    break;
            }
            return result;
        }
    }
}
