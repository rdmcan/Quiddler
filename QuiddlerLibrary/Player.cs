/*
 * Library:         QuiddlerLibrary.dll
 * Module:          Player.cs
 * Author:          Ruben Dario  Mejia Cardona
 * Date:            February 11, 2022
 * Description:     Hidden class that implements IPlayer Interface
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuiddlerLibrary
{
    internal enum CPoints
    {
        a = 2, e = 2, i = 2, o = 2,
        l = 3, s = 3, t = 3,        
        u = 4, y = 4,
        d = 5, m = 5, n = 5, r = 5,
        f = 6, g = 6, p = 6,
        h = 7, er = 7, IN = 7,
        b = 8, c = 8, k = 8,
        qu = 9, th =9,
        w = 10, cl = 10,
        v = 11,
        x = 12,
        j = 13,
        z = 14,
        q = 15       
    }

    internal class Player : IPlayer
    {
        /*------------------------ Member Variables ------------------------*/
        public Deck newDeck;
        private List<string> cardsPLayer = null;

        // Current player content
        private string playerString;
        private int playerPoints;

        /*-------------------------- Constructor ---------------------------*/
        public Player(Deck d)
        {
            newDeck = d;

            if (newDeck.CardCount > 0)
            {
                // Initialize dictionary and TopUp the player's hand
                cardsPLayer = new List<string>();
                TopUp();
            }
        }

        /*------------------ Properties and Methods -----------------*/

        /*
        * Method:      CardCount
        * Description: Indicates how many cards are in the player’s hand.
        */
        public int CardCount
        {        
            get
            {
                int sum = 0;
                foreach (var card in cardsPLayer)
                {
                    ++sum;
                }
                return sum;
            }
        }

        /*
         * Method:      TotalPoints
         * Description: Indicats how many points in total have been 
         *              scored by the player from playing words.
         */
        public int TotalPoints
        {
            get
            {
                int points = playerPoints;                
                return points;
            }
        }

        /*
         * Method:      DrawCard
         * Description: Takes the top card from the deck, adds it to the player’s 
         *              hand and returns the card’s letter(s) as a string.
         */
        public string DrawCard()
        {
            if (newDeck.CardCount == 0)
            {
                throw new InvalidOperationException("The Deck is empty");
            }
            else
            {
                string drawCard = newDeck.GetCard();
                cardsPLayer.Add(drawCard);
                return drawCard;
            }
        }

        /*
         * Method:      Discard
         * Description: Removes a card from the player’s hand making that 
         *              card the top card on the deck’s discard pile.
         */
        public bool Discard(string discardCard)
        {
            if (cardsPLayer.Contains(discardCard))
            {
                cardsPLayer.Remove(discardCard);
                newDeck.discardPile.Push(discardCard);
                return true;
            }
            else
                return false;
        }

        /*
         * Method:      PickupTopDiscard
         * Description: Takes the top card in the discard pile and 
         *              adds it to the player’s hand.
         */
        public string PickupTopDiscard()
        {
            string pickupDiscard = newDeck.TopDiscard;
            cardsPLayer.Add(pickupDiscard);

            return pickupDiscard;
        }

        /*
         * Method:      PlayWord
         * Description: If a candidate word is worth more than 0 points 
         *              removed the cards from the player’s hand and 
         *              added to the player’s TotalPoints property.
         */
        public int PlayWord(string candidate)
        {
            string[] textSplit = candidate.Split(" ");
            int candidatePts = TestWord(candidate);
            
            if (candidatePts > 0)
            {
                // all the cards in the word will be remove from the hand 
                foreach (string c in textSplit)
                {
                    if (cardsPLayer.Contains(c))
                        cardsPLayer.Remove(c);
                }

                // add points to playerPoints
                playerPoints += candidatePts;
            }
            
            return candidatePts;
        }

        /*
         * Method:      TestWord
         * Description: Returns the point value based on:  
         *              the player has not used all their cards (need to discard)
         *              the letters of the candidate are a subset of the current rack 
         *              the candidate provided is a valid word as tested using the 
         *              Application object’s CheckSpelling() method.
         */
        public int TestWord(string candidate)
        {
            List<string> candidateList = candidate.Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            candidateList = candidateList.ConvertAll(c => c.ToLower());
            List<string> wordsPlayerTemp = new List<string>();

            // Letters of the candidate string are a subset of the letters in the rack
            int cardsCount = cardsPLayer.Count;

            foreach (var item in candidateList)
            {
                if (candidateList.Count < cardsCount && cardsPLayer.Contains(item))
                    wordsPlayerTemp.Add(item);
                else
                    return 0;
            }

            // A valid word is tested using the Application class’s CheckSpelling() method
            string trimCandidate = string.Join("", candidate.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            if (newDeck.checker.CheckSpelling(trimCandidate.ToLower()))
            {
                int total = 0;
                foreach (string item in wordsPlayerTemp)
                {
                    if (item == "in")
                        total += (int)Enum.Parse(typeof(CPoints), "IN");
                    else
                        total += (int)Enum.Parse(typeof(CPoints), item);
                }
                return total;
            }
            else
                return 0;
        }

        /*
         * Method:      ToString
         * Description: Returns a string containing all the hand’s card values (letters) 
         *              separated by spaces.
         */
        public override string ToString()
        {
            string playerHand = "";

            for (int i = 0; i < cardsPLayer.Count; i++)
            {
                if (i != cardsPLayer.Count - 1)
                    playerHand += cardsPLayer.ElementAt(i) + " ";
                else
                    playerHand += cardsPLayer.ElementAt(i);
            }

            return playerHand;
        }

       /*------------------------- Helper Methods -------------------------*/

       /*
        * Method:      TopUp
        * Description: Fill in the player hand with new cards from the Deck.
        */
        public string TopUp()
        {
            int numCards = newDeck.CardsPerPlayer;
            string value;

            if (cardsPLayer.Count != numCards)
            {
                // total letters 
                int cnt = cardsPLayer.Sum(c => c.Count());

                while (cnt != numCards)
                {
                    value = newDeck.GetCard();
                    cardsPLayer.Add(value);
                    cnt++;
                    playerString = value;
                }
            }
            else
                return playerString;

            return ToString();
        }
    }
}
