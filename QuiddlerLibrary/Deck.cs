/*
 * Library:         QuiddlerLibrary.dll
 * Module:          Deck.cs
 * Author:          Ruben Dario  Mejia Cardona
 * Date:            February 11, 2022
 * Description:     Exposed class that implements IDeck Interface
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;


namespace QuiddlerLibrary
{
    public class Deck : IDeck
    {
        /*------------------------ Member Variables ------------------------*/
        private Dictionary<string, int> cards;
        private int cardsPerPlayer = 0;        
        internal Stack<string> discardPile;
        private bool disposedValue;
        public Application checker = new Application();

        /*-------------------------- Constructor ---------------------------*/
        public Deck()
        {
            cards = new Dictionary<string, int>();
            PopulateDeck();
        }

        /*------------------ Public Properties and Methods -----------------*/

       /*
        * Method:      PopulateDeck
        * Description: Populate the Deck with cards.
        */
        private void PopulateDeck()
        {
            Dictionary<string, int> cardsDic = new Dictionary<string, int>()
            {
                { "b", 2 },{ "c", 2 },{ "f", 2 },{ "h", 2 },{ "j", 2 },{ "k", 2 },{ "m", 2 },{ "p", 2 },{ "q", 2 },{"v", 2 },{ "w", 2 },{ "x", 2 },{ "z", 2 },{ "cl", 2 },{ "er", 2 },{ "in", 2 },{ "qu", 2 },{ "th", 2 },
                { "d", 4 },{ "g", 4 },{ "l", 4 },{ "s", 4 },{ "y", 4 },
                { "n", 6 },{ "r", 6 },{ "t", 6 },{ "u", 6 },
                { "i", 8 },{ "o", 8 },
                { "a", 10 },
                { "e", 12 }
            };

            foreach (var item in cardsDic.OrderBy(i => i.Key).OrderBy(i => i.Key.Length))
            {
                cards.Add(item.Key, item.Value);
            }
        }

        /*
         * Method:      About
         * Description: Identifies the library and its developer.
         */
        public string About
        {
            get { return "R. Mejia"; }
        }

        /*
         * Method:      CardCount
         * Description: Indicating how many undealt cards remain.
         */
        public int CardCount
        {
            get
            {
                int sum = 0;
                foreach (KeyValuePair<String, int> card in cards)
                {
                    sum += card.Value;
                }
                return sum;
            }
        }

        /*
         * Method:      CardsPerPlayer
         * Description: Represents the number of cards initially dealt to each player.
         */
        public int CardsPerPlayer
        {
            get
            {
                return cardsPerPlayer;
            }

            set
            {
                if(value >= 3 && value <= 10)
                    cardsPerPlayer = value;
                else
                    throw new ArgumentOutOfRangeException("\nThe cards per player must be a number between 3 to 10 (inclusive).");
            }
        }

        /*
         * Method:      TopDiscard
         * Description: Indicates the top card on the discard pile.
         */
        public string TopDiscard
        {
            get
            {
                if (discardPile == null)
                {
                    discardPile = new Stack<string>();
                    discardPile.Push(GetCard());
                }
                return discardPile.Peek();
            }            
        }

        /*
         * Method:      NewPlayer
         * Description: Creates a new player object implicit and populates 
         *              it with CardsPerPlayer cards.
         */
        public IPlayer NewPlayer()
        {
            return new Player(this);
        }

        /*
         * Method:      ToString
         * Description: Returns a formatted string describing the inventory 
         *              of cards available in the deck in alphabetical order.
         */
        public override string ToString()
        {
            int cnt = 0;
            string cardSet = "";

            Dictionary<string, int> RemainedCards = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> items in cards)
            {
                if (items.Value != 0)
                {
                    RemainedCards.Add(items.Key, items.Value);
                }
            }

            foreach (KeyValuePair<string, int> kvp in RemainedCards)
            {
                cnt++;
                cardSet += kvp.Key + "(" + kvp.Value + ")\t";
                if (cnt == 12 | cnt == 24)
                    cardSet += "\n";
            }
            return cardSet;
        }

        /*------------------------- Helper Methods -------------------------*/

        /*
         * Method:      GetCard
         * Description: Returns a random card from the Deck.
         */
        internal string GetCard()
        {
            string newCard = "";
            bool flag; 

            do
            {
                Random rng = new Random();
                int index = rng.Next(cards.Count);
                newCard = cards.Keys.ElementAt(index);
                
                if (cards.Values.ElementAt(index) == 0)
                {
                    cards.Remove(newCard);
                    flag = true;
                }
                else
                {
                    cards[newCard] -= 1;
                    flag = false;
                }
            } while (flag);

            return newCard;
        }

        /*
         * Method:      Dispose
         * Description: Cleaning up unmanaged resources.
         */
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) 
                checker.Quit();
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Deck()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
