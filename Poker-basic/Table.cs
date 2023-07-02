using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    internal class Table
    {
        public readonly List<Player> players = new();
        public int pot=0;
        public int currentbet = 0;
        public List<Card> commoncards = new();
        public Deck deck = new();
        public int blind = 50;
        public void AddPlayer(int tokens)
        {
            players.Add(new Player(tokens,players.Count));
        }
        public void Dealhands()
        {
            foreach (var player in players) 
            {
                player.hand.Add(deck.Dealcard());
                player.hand.Add(deck.Dealcard());
            }
        }
        public void Addcommoncards()
        {
            commoncards.Add(deck.Dealcard());
        }
        public void Increasepot(int chipammount, int playerid)
        {
            pot+= chipammount;
            players[playerid].Bet(chipammount);
        }       
        public bool AllPlayersMadeDecision()
        {
            foreach(var player in players)
            {
                if (!player.isFolded)
                {
                    if (!player.hasMadedecision)
                    {

                        return true;
                    }
                }
            }
            Console.WriteLine("Next Round");
            Resetdecisionflags();
            foreach (Player player1 in players)
            {
                player1.playerBet = 0;
            }
            this.currentbet= 0;
            return false;
        }
        public void Resetdecisionflags()
        {
            foreach(var player in players)
            {
                player.hasMadedecision = false;
            }
        }
        public int Bet(Player currentplayer, int number)
        {
            int local = currentplayer.Bet(number);
            this.pot += local;
            return local;
        }
        public Player FindDealer()
        {
            foreach(var player in players)
            {
                if (player.isDealer)
                {
                    return player;
                }
            }
            players[0].isDealer = true;
            return players[0];
        }

    }
}
