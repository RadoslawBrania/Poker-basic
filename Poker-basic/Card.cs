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
        J = 11, Q= 12, K = 13, A=14
    }
    enum Colour
    {
        Diamonds,
        Hearts,
        Spades,
        Clubs
    }
    public class Card
    {
        public int cardID;
        public int colour;
        public Card(int cardID, int colour)
        {
            this.cardID = cardID;
            this.colour = colour;
        }
    }


}
