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
        public bool isAllIn = false;
        public int totalbet = 0;
        public int chipstaken = 0;
        public int handvalue = 0;
        public int winnings = 0;
        public List<Card> Besthandformed = new();
        
        public Player(int chips,int id)
        {
            this.chips= chips;
            this.id = id;
        }

        public int Bet(int chipammount)
        {
            if (this.chips > chipammount)
            {
                this.chips -= chipammount;
                playerBet = chipammount;
                totalbet += chipammount;
                return chipammount;
            }
            else
            {
                chipammount = this.chips;
                playerBet = chipammount;
                isAllIn = true;
                totalbet += chipammount;
                this.chips = 0;
                return chipammount;
            }
        }
            public Player Next(List <Player> players) {
            return players[(this.id + 1) % (players.Count)];
        }
        public Player Prev(List <Player> players) { return players[(this.id - 1) % (players.Count)]; }
        public bool PlayersAreEqual(Player player)
        {
            if (this.handvalue == player.handvalue)
            {
                for (int i = 0; i < this.Besthandformed.Count; i++)
                {
                    if (this.Besthandformed[i].cardID != player.Besthandformed[i].cardID)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }

}
