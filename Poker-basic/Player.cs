using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    public class Player
    {
        public int id;
        public int chips;
        public List<Card> hand = new(2);
        public bool isFolded = false;
        public bool hasMadedecision = false;
        public bool isDealer = false;
        public int playerBet = 0;
        public Player(int chips,int id)
        {
            this.chips= chips;
            this.id = id;
        }
        public int Bet (int chipammount)
        {
            if (this.chips > chipammount)
            {
                this.chips -= chipammount;
                return chipammount;
            }
            else
            {
                chipammount = this.chips;
                this.chips = 0;
                return chipammount;
            }
        }
        public Player Next(List <Player> players) {
            return players[(this.id + 1) % (players.Count - 1)];
        }
    }

}
