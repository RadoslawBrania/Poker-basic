using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    internal class Game
    {
        private Table table = new();
        public void GameStart()
        {
            
            while (true){
                //Add players?
                // Start game 
                table.AddPlayer(2000);
                table.players[0].isDealer = true;
                table.AddPlayer(2000);
                table.AddPlayer(2000);
                
            }
        }
        public bool BetIsCorrect(Player currentplayer, int number)
        {
            if(table.currentbet * 2 - currentplayer.playerBet>=number)
            {
                return true;
            }
            return false;
        }
        public void GetBet(Player currentplayer) {
            Console.WriteLine("Tura gracza " + currentplayer.id);
            Console.WriteLine("Podaj o ile chcesz podbić (minimum: " + (table.currentbet * 2 - currentplayer.playerBet) + ")") ;
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                if (BetIsCorrect(currentplayer, number))
                {
                  currentplayer.Bet(number);   
                }
            }
            Console.WriteLine("Błędny bet, spróbuj ponownie");
            GetBet(currentplayer);
        }
        public void GetDecision(Player currentplayer) {
            Console.WriteLine("Tura gracza " + currentplayer.id);
            Console.WriteLine("Podaj decyzje Call/Raise/Fold (1/2/3)");
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                switch (number)
                {
                    case 1:
                        currentplayer.Bet(table.currentbet - currentplayer.playerBet);
                        return;
                    case 2:
                        GetBet(currentplayer);
                        table.Resetdecisionflags();
                        return;
                    case 3:
                        currentplayer.isFolded = true;
                        return; 
                    default:
                        Console.WriteLine("Błędny input, spróbuj ponownie");
                        GetDecision(currentplayer);
                        return;
                }
            }
        }
        public void BettingRound()
        {
            Player currentplayer = table.FindDealer();
            while (!table.AllPlayersMadeDecision())
            {
                while (currentplayer.isFolded)
                {
                    currentplayer=currentplayer.Next(table.players);
                    
                }
                GetDecision(currentplayer);
                currentplayer = currentplayer.Next(table.players);
            }

        }

        
    }
}
