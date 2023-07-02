using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace Poker_basic
{
    enum FaceCards
    {
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen= 12, King = 13, Ace=14
    }
    enum Colour
    {
        Diamonds=1,
        Hearts,
        Spades,
        Clubs
    }
    public class Card
    {
        public int cardID;
        public int colour;
        public Card(int cardID, int colour=0)
        {
            this.cardID = cardID;
            this.colour = colour;
        }
        
    }
   


}
