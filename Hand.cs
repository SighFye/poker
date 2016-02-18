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
        //private int handValue = 0;
        //private string bestHandName = "";
        private List<Card> bestHand = new List<Card>(5);
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
        /// Returns the number of cards in the hand
        /// </summary>
        /// <returns>Number of cards</returns>
        public int countCards()
        {
            return cards.Count();
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
                Console.WriteLine("Hand Says: Card Added");
                cards.Add(newCard); //Add the new card to the cards array using the length as an index.
                result = true; //The result turns to true as the add card worked.
            }
            else
            {
                Console.WriteLine("Hand Says: Hand is full");
            }
            if (result) analyseHand(); //Only analyse the hand if a card was added
            return result;
        }
        /// <summary>
        /// Analyse the hand and set the bestHand bestHandName and handValue variables
        /// </summary>
        private void analyseHand()
        {
            
            if (cards.Count > 4)
            {
                //Check for a flush
                if (hasFlush(cards))
                {
                    Console.WriteLine("Has Flush Checking For Straight");
                    if (hasStraight(bestHand)) //Check for a straight using the flush cards.
                    {

                    }
                    else
                    {
                        Console.WriteLine("Player has a flush");
                        bestHand.Sort((a, b) => { return a.getValue().CompareTo(b.getValue()); }); //Sort the besthand based on card value
                        //Get the highest value cards from the flush cards.
                        //The reason I use bestHand.Count - 5 is this will give me the index 5 cards from the end of the list getting me the top 5 valued cards.
                        bestHand = bestHand.GetRange(bestHand.Count - 5, 5);
                        Console.WriteLine("Done");
                    }
                }
                else
                {
                    Console.WriteLine("No Flush Checking For Straight");
                    if (hasStraight(cards)) //Check for a straight using all the cards
                    {
                        Console.WriteLine("Has Straight");
                    }
                        
                }
            }
        }
        /// <summary>
        /// Check to see if there is a flush in the hand.
        /// </summary>
        /// <returns>True or false depending on the result.</returns>
        private bool hasFlush(List<Card> theCards)
        {
            //Sort all the cards based on suit
            theCards.Sort((a, b) => { return a.getSuit().CompareTo(b.getSuit()); });
            //Loop through the cards and check their suit
            int lastSuit = 0;
            List<Card> potentialBestHand = new List<Card>(maxCards);
            foreach (Card theCard in theCards)
            {
                if (lastSuit == theCard.getSuit()) //Does the current cards suit match the last cards suit.
                {
                    potentialBestHand.Add(theCard); //Add the card to the potential besthand cards
                    // We do not need to change lastSuit as this card matches the last
                }
                else
                {
                    if (potentialBestHand.Count < 5)
                    {
                        potentialBestHand.Clear(); //Get rid of all the cards in the best hand
                        potentialBestHand.Add(theCard); //Add in the current card to be a part of a new best hand.
                        lastSuit = theCard.getSuit(); //And change the suit to the current cards suit.
                    }
                }
            }
            if (potentialBestHand.Count > 4)
            {
                bestHand = potentialBestHand; //Since we have found a flush, this becomes our best hand.
                //=========================================
                //handValue = 0; //We'll add this in later
                //=========================================
                return true; //We have found a flush and have gathered all the cards matching the suit.
            }
            else
                return false;
        }
        /// <summary>
        /// Check to see if there is a straight in the hand.
        /// </summary>
        /// <returns>True or false depending on the result.</returns>
        private bool hasStraight(List<Card> theCards)
        {
            //Sort all the cards based on card value
            theCards.Sort((a, b) => { return a.getValue().CompareTo(b.getValue()); });
            //Loop through the cards and compare their value to the next.
            int lastValue = 0;
            int totalinStraight = 0; //Count how many cards are in the straight
            List<Card> potentialBestHand = new List<Card>(maxCards);
            foreach (Card theCard in theCards)
            {
                if (lastValue+1 == theCard.getValue() || lastValue == theCard.getValue()) //Does the current cards suit match the last cards suit.
                {
                    potentialBestHand.Add(theCard); //Add the card to the potential besthand cards
                    if (lastValue + 1 == theCard.getValue()) //Increase the amount only if the card is greater than the last card.
                    {
                        totalinStraight += 1;
                    }
                    lastValue = theCard.getValue();
                    // We do not need to change lastSuit as this card matches the last
                }
                else
                {
                    if (totalinStraight < 5)
                    {
                        potentialBestHand.Clear(); //Get rid of all the cards in the best hand
                        potentialBestHand.Add(theCard); //Add in the current card to be a part of a new best hand.
                        lastValue = theCard.getValue(); //And change the suit to the current cards suit.
                        totalinStraight = 1;
                    }
                }
            }
            if (totalinStraight > 4)
            {
                bestHand = potentialBestHand; //Since we have found a flush, this becomes our best hand.
                //=========================================
                //handValue = 0; //We'll add this in later
                //=========================================
                return true; //We have found a flush and have gathered all the cards matching the suit.
            }
            else
                return false;
        }
    }
}
