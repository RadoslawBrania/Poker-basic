using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    internal class Deck
    {
        public readonly List<Card> cards; 
        public Deck() { 
        cards= new List<Card>(52);
            Initdeck();
            Shuffledeck();
        }
        public void Initdeck()
        {
            for (int i=2; i<=14; i++)
            {
                for(int j=1; j <= 4; j++)
                {
                    cards.Add(new Card(i, j));
                }
            }
        }
        public void Printdeck()
        {
            foreach (Card card in cards)
            {
                Console.WriteLine(card.cardID);
            }
        }
        public void Shuffledeck()
        {
            Random rdm= new();
            int n = cards.Count;
            while (n > 0) { 
            n--;
            int k = rdm.Next(n);
                (cards[n], cards[k]) = (cards[k], cards[n]);
            }
        }
        public Card Dealcard()
        {
            Card ret = cards[0];
            cards.RemoveAt(0);
            return ret;
        }
    }
}
