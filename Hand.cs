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
                Console.WriteLine("Hand Says: "+newCard+" Added");
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
            //Check for a flush
            if (hasFlush(cards))
            {
                //Check for straight flush
                if (hasStraight(bestHand)) //Check for a straight using the flush cards.
                {
                    bestHand.Sort((a, b) => { return a.getValue().CompareTo(b.getValue()); }); //Sort the besthand based on card value
                    bestHand = bestHand.GetRange(bestHand.Count - 5, 5); //We can do a simple getRange because only cards of the same suit are in the bestHand.
                    //Check for Royal flush
                    if (bestHand[bestHand.Count - 1].getValue() == Card._ACE && bestHand[bestHand.Count - 2].getValue() == Card._KING) //Are the top two cards the Ace and the King
                    {
                        Console.WriteLine("Player has royal flush!");
                    }
                    else
                    {
                        Console.WriteLine("Player has straight flush!");
                    }

                }
                else
                {
                    Console.WriteLine("Player has a flush");
                    bestHand.Sort((a, b) => { return a.getValue().CompareTo(b.getValue()); }); //Sort the besthand based on card value
                    //Get the highest value cards from the flush cards.
                    //The reason I use bestHand.Count - 5 is this will give me the index 5 cards from the end of the list getting me the top 5 valued cards.
                    bestHand = bestHand.GetRange(bestHand.Count - 5, 5);
                }
            }
            else
            {
                //Check For Straight
                if (hasStraight(cards)) //Check for a straight using all the cards
                {
                    Console.WriteLine("Has Straight");
                }
                else
                {
                    //Now we group the cards to find 4 of a kind, full house, 3 of a kind and pairs.
                    List<List<List<Card>>> groupedCards = groupCards(); //<-- My first legitimate 3 diamention array :D
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
        /// <summary>
        /// This method loop through all the cards and places references to each card is arrays is the match a pair, 3 of kind or 4 of kind
        /// </summary>
        /// <returns></returns>
        private List<List<List<Card>>> groupCards()
        {
            if (cards.Count > 1)
            {
                //Create the multidiamention list to hold the grouped cards
                List<List<List<Card>>> groupedCards = new List<List<List<Card>>>(3);
                Card lastCard = null;
                //How many pairs are possible
                int temp = (int)Math.Floor((float)(cards.Count / 2));
                groupedCards.Add(new List<List<Card>>(temp));
                //How many three of a kind are possible
                temp = (int)Math.Floor((float)(cards.Count / 3));
                groupedCards.Add(new List<List<Card>>(temp));
                //How many four of a kind are possible
                temp = (int)Math.Floor((float)(cards.Count / 4));
                groupedCards.Add(new List<List<Card>>(temp));
                
                List<Card> tempPair = new List<Card>(2);
                List<Card> temp3Kind = new List<Card>(3);
                List<Card> temp4Kind = new List<Card>(4);
                //Now loop through the cards and start sorting
                foreach (Card aCard in cards)
                {
                    //Temp Group Holders
                    
                    if (lastCard == null)
                    {
                        lastCard = aCard;
                    }
                    else
                    {
                        //If the last card and this card are of the same value then add it to the groups
                        if (lastCard.getValue() == aCard.getValue())
                        {
                            tempPair.Add(aCard);
                            tempPair.Add(lastCard);

                            if (temp3Kind.Count == 0)
                            {
                                temp3Kind.Add(aCard);
                                temp3Kind.Add(lastCard);
                            }
                            else
                                temp3Kind.Add(aCard);

                            if (temp4Kind.Count == 0)
                            {
                                temp4Kind.Add(aCard);
                                temp4Kind.Add(lastCard);
                            }
                            else
                                temp4Kind.Add(aCard);
                            
                            //If any of the groups are full add it to the appropriate List
                            if (tempPair.Count == 2) //Do we already have a pair?
                            {
                                int pairsFound = groupedCards[0].Count;
                                groupedCards[0].Add(tempPair.ToList()); //Make sure to create a copy of the temp list so the data is not lost
                                tempPair.Clear();
                            }
                            if (temp3Kind.Count == 3) //Do we already have 3 of a kind
                            {
                                int threesFound = groupedCards[1].Count;
                                groupedCards[1].Add(temp3Kind.ToList());
                                temp3Kind.Clear();
                            }
                            if (temp4Kind.Count == 4) //Do we already have 4 of a kind
                            {
                                int foursFound = groupedCards[2].Count;
                                groupedCards[2].Add(temp4Kind.ToList());
                                temp4Kind.Clear();
                            }
                        }
                        else
                        {
                            //The cards do not match so wipe the lists.
                            tempPair.Clear();
                            temp3Kind.Clear();
                            temp4Kind.Clear();
                            lastCard = aCard;
                        }
                    }
                }

                return groupedCards;
            }
            return null;
        }
    }
}
