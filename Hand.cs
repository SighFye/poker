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
        private string bestHandName = "";
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

        public void clear()
        {
            cards.Clear();
            bestHand.Clear();
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
        /// Returns the best possible 5 card hand from the current cards
        /// </summary>
        /// <returns>List containing the best possible hand</returns>
        public List<Card> getBestHand()
        {
            return bestHand;
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
                    bestHand.Sort((card1, card2) => { return card1.cardValue.CompareTo(card2.cardValue); }); //Sort the besthand based on card value
                    bestHand = bestHand.GetRange(bestHand.Count - 5, 5); //We can do a simple getRange because only cards of the same suit are in the bestHand.
                    //Check for Royal flush
                    if (bestHand[bestHand.Count - 1].cardValue == Card._ACE && bestHand[bestHand.Count - 2].cardValue == Card._KING) //Are the top two cards the Ace and the King
                    {
                        Console.WriteLine("Player has royal flush!");
                        bestHandName = "Royal Flush";
                    }
                    else
                    {
                        Console.WriteLine("Player has straight flush!");
                        bestHandName = "Straight Flush";
                    }

                }
                else
                {
                    Console.WriteLine("Player has a flush");
                    bestHandName = "Flush";
                    bestHand.Sort((card1, card2) => { return card1.cardValue.CompareTo(card2.cardValue); }); //Sort the besthand based on card value
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
                    bestHandName = "Straight";
                }
                else
                {
                    //Now we group the cards to find 4 of a kind, full house, 3 of a kind and pairs.
                    List<List<List<Card>>> groupedCards = groupCards(); //<-- My first legitimate 3 diamention array :D
                    if(groupedCards != null)
                    {
                        if (groupedCards[2].Count > 0) //if the four of a kind list has anything in it, then we have a four of a kind.
                        {
                            Console.WriteLine("Player has 4 of a kind.");
                            bestHandName = "Four of a Kind";
                        }
                        else
                        {
                            if (groupedCards[1].Count > 0)//if the three of a kind list has anything in it, then we have a three of a kind.
                            {
                                if (groupedCards[0].Count > 1)//Check if the player also has at least 2 pairs as one of the pairs WILL be part of the three of a kind
                                {
                                    Console.WriteLine("Player has Full House.");
                                    bestHandName = "Full House";
                                }
                                else
                                {
                                    Console.WriteLine("Player has Three of a Kind.");
                                    bestHandName = "Three of a Kind";
                                }
                            }
                            else
                            {
                                switch (groupedCards[0].Count)
                                {
                                    case 2:
                                        Console.WriteLine("Player has 2 pair.");
                                        bestHandName = "Two Pair";
                                        break;
                                    case 1:
                                        Console.WriteLine("Player has 1 pair.");
                                        bestHandName = "One Pair";
                                        break;
                                    case 0:
                                        Console.WriteLine("Player has high card.");
                                        bestHandName = "High Card";
                                        break;
                                }
                            }
                        }
                    }
                    Console.WriteLine("Done");
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
            theCards.Sort((card1, card2) => { return card1.suit.CompareTo(card2.suit); });
            //Loop through the cards and check their suit
            Card.Suit lastSuit = 0;
            List<Card> potentialBestHand = new List<Card>(maxCards);
            foreach (Card theCard in theCards)
            {
                if (lastSuit == theCard.suit) //Does the current cards suit match the last cards suit.
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
                        lastSuit = theCard.suit; //And change the suit to the current cards suit.
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
            theCards.Sort((card1, card2) => { return card1.cardValue.CompareTo(card2.cardValue); });
            //Loop through the cards and compare their value to the next.
            int lastValue = 0;
            int totalinStraight = 0; //Count how many cards are in the straight
            List<Card> potentialBestHand = new List<Card>(maxCards);
            foreach (Card theCard in theCards)
            {
                if (lastValue + 1 == theCard.cardValue || lastValue == theCard.cardValue) //Does the current cards suit match the last cards suit.
                {
                    potentialBestHand.Add(theCard); //Add the card to the potential besthand cards
                    if (lastValue + 1 == theCard.cardValue) //Increase the amount only if the card is greater than the last card.
                    {
                        totalinStraight += 1;
                    }
                    lastValue = theCard.cardValue;
                    // We do not need to change lastSuit as this card matches the last
                }
                else
                {
                    if (totalinStraight < 5)
                    {
                        potentialBestHand.Clear(); //Get rid of all the cards in the best hand
                        potentialBestHand.Add(theCard); //Add in the current card to be a part of a new best hand.
                        lastValue = theCard.cardValue; //And change the suit to the current cards suit.
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
        /// This method loops through all the cards and places references to each card is arrays is the match a pair, 3 of kind or 4 of kind
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
                bool lastCardUsedInPair = false;

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
                        if (lastCard.cardValue == aCard.cardValue)
                        {
                            if (!lastCardUsedInPair) //Was the card used in the last pair. We may get a four of a kind. This ensures the two pairs are seperated instead of getting 3 pairs out of 4 cards.
                            {
                                lastCardUsedInPair = true;
                                tempPair.Add(lastCard);
                                tempPair.Add(aCard);
                            }
                            else
                            {
                                lastCardUsedInPair = false;
                            }

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
                            lastCardUsedInPair = false;
                        }
                    }
                    lastCard = aCard;
                }

                return groupedCards;
            }
            return null;
        }
    }
}
