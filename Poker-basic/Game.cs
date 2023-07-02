using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    internal class Game
    {
        public readonly Table table = new();
        public void GameStart()
        {

            table.AddPlayer(2000);
            table.players[0].isDealer = true;
            table.AddPlayer(2000);
            table.AddPlayer(2000);
        }
        public bool BetIsCorrect(Player currentplayer, int number)
        {
            if (table.currentbet * 2 - currentplayer.playerBet <= number)
            {
                return true;
            }
            return false;
        }
        public void GetBet(Player currentplayer) {
            Console.WriteLine("Tura gracza " + currentplayer.id);
            Console.WriteLine("Karty na stole to: " );
            
            Console.WriteLine("Podaj o ile chcesz podbić (minimum: " + (table.currentbet * 2 - currentplayer.playerBet) + ")");
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine(number);
                if (BetIsCorrect(currentplayer, number))
                {
                    table.currentbet = number + currentplayer.playerBet
                        ;
                    table.pot += table.Bet(currentplayer, number);
                }
                else
                {
                    Console.WriteLine("Błędny bet, spróbuj ponownie");
                    GetBet(currentplayer);
                }
            }
            else
            {
                Console.WriteLine("Błędny bet, spróbuj ponownie");
                GetBet(currentplayer);
            }

        }
        public void GetDecision(Player currentplayer) {
            Console.WriteLine("Tura gracza: " + currentplayer.id + "     Żetony gracza: " + currentplayer.chips + "\nDałeś na tej karcie do pota: " + currentplayer.playerBet + "    Call kosztuje cię: " + (table.currentbet - currentplayer.playerBet));
            Console.WriteLine( "Masz na ręce:" );
            foreach(var card in currentplayer.hand) {
                Console.Write((FaceCards)card.cardID + " of " + (Colour)card.colour + "   |   "); 
            }
            Console.WriteLine("\nObecny pot " + table.pot);
            Console.WriteLine("Podaj decyzje CallOrWait/Raise/Fold (1/2/3)");
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                switch (number)
                {
                    case 1:
                        table.pot += currentplayer.Bet(table.currentbet - currentplayer.playerBet);
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
                        break;
                }
            }
            else {

                Console.WriteLine("Błędny input, spróbuj ponownie");
                GetDecision(currentplayer);
                return;
            }
        }
        #region cardevaluationlogic
        public static bool CheckIfTwoPair(Dictionary<Card, int> handholder)
        {
            int istwo = 0;
            foreach (var holder in handholder)
            {
                if (holder.Value == 2)
                {
                    istwo++;
                }
            }
            if (istwo >= 2)
            {
                return true;
            }
            return false;
        }
        public static Dictionary<int, int> GetTwoPair(Dictionary<int, int> handholder) // good
        {
            Dictionary<int, int> returnhand = new();
            Dictionary<int, int> cpy = new(handholder);
            int movedcards = 0;
            int totalvalue = 0;
            foreach (var holder in handholder)
            {
                if (holder.Value == 2 && movedcards < 2)
                {
                    returnhand.Add(holder.Key, 2);
                    cpy.Remove(holder.Key);
                    movedcards++;
                    totalvalue += 2;
                }

            }
            while (totalvalue < 5)
            {
                KeyValuePair<int, int> firstElement = cpy.First();
                returnhand.Add(firstElement.Key, 1);
                cpy.Remove(firstElement.Key);
                totalvalue++;
            }
            return returnhand;
        }
        public static Dictionary<int, int> GetOnePair(Dictionary<int, int> handholder) // good 
        {
            Dictionary<int, int> returnhand = new();
            Dictionary<int, int> cpy = new(handholder);
            foreach (var holder in cpy)
            {
                if (holder.Value == 2)
                {
                    returnhand.TryAdd(holder.Key, holder.Value);
                    cpy.Remove(holder.Key);
                }
            }
            while (returnhand.Count < 4) {
                returnhand.TryAdd(cpy.First().Key, cpy.First().Value);
                cpy.Remove(cpy.First().Key);
            }
            return returnhand;
        }
        public static Dictionary<int, int> GetTrips(Dictionary<int, int> handholder) // good
        {


            Dictionary<int, int> returnhand = new();
            Dictionary<int, int> cpy = new(handholder);
            foreach (var holder in cpy)
            {
                if (holder.Value == 3)
                {
                    returnhand.Add(holder.Key, holder.Value);
                    if (holder.Key == cpy.First().Key)
                    {
                        cpy.Remove(cpy.First().Key);
                        break;
                    }
                    else
                    {
                        cpy.Remove(cpy[holder.Key]);
                        break;
                    }
                }
            }
            returnhand.TryAdd(cpy.First().Key, cpy.First().Value);
            cpy.Remove(cpy.First().Key);
            returnhand.TryAdd(cpy.First().Key, cpy.First().Value);
            return returnhand;
        }
        /* public bool CheckIfStraight(List<Card> cards)
         {
             int loopcounter = 0;
             Dictionary<Card, int> ret = new();
             for (int i = 0; i < cards.Count; i++)
             {
                 if (cards[i].cardID-1 == (cards[(i + 1) % (cards.Count)].cardID)) 
                 {
                     loopcounter++;
                     Console.WriteLine(loopcounter +"here?");
                     if (loopcounter == 3 && cards[i+1].cardID == 2 && cards[0].cardID == 14) // Rozpatrzenie strita od 5 do asa
                     {
                         return true;
                     }
                     if (loopcounter == 4) // Reszta 
                     {
                         return true;
                     }
                 }
                 else { loopcounter = 0; }
             }
             return false; 
         }*/
        public static Dictionary<int, int>? GetStraight(List<Card> cards)
        {

            int loopcounter = 0;
            Dictionary<int, int> ret = new();
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].cardID - 1 == cards[(i + 1)].cardID)
                {
                    ret.Add(cards[i].cardID, 4);
                    loopcounter++;
                    if (loopcounter == 3 && i == cards.Count - 2 && cards[i + 1].cardID == 2 && cards[0].cardID == 14) // Rozpatrzenie strita od 5 do asa
                    {
                        ret.Add(14, 4);

                        return ret;
                    }
                    if (loopcounter == 4) // Reszta 
                    {
                        return ret;
                    }
                }
                else if (cards[i].cardID == cards[(i + 1)].cardID) { }
                else { loopcounter = 0; ret.Remove(cards[i].cardID); }
            }
            return null;
        } // good
        public static Dictionary<int, int>? GetFlush(List<Card> cards) { //good
            Dictionary<int, int> ret = new();
            int counter = 0;
            for (int i = 1; i <= 4; i++)
            {

                foreach (var card in cards)
                {
                    if (card.colour == i) {
                        counter++;

                    }
                }
                if (counter > 4)
                {
                    foreach (var card in cards)
                    {
                        if (card.colour == i)
                        {

                            ret.TryAdd(card.cardID, 5);
                        }
                    }
                    return ret;
                }
                counter = 0;
            }
            return null;
        } //good
        public static Dictionary<int, int>? GetFullHouse(Dictionary<int, int> handholder)
        {
            Dictionary<int, int> ret = new();
            Dictionary<int, int> cpy = new(handholder);

            if (!cpy.ContainsValue(3))
            {
                return null;
            }
            foreach (var card in cpy)
            {
                if (card.Value == 3)
                {
                    ret.Add(card.Key, 6);
                    cpy.Remove(card.Key);
                    break;
                }
            }
            if ((cpy.ContainsValue(3) | cpy.ContainsValue(2))) {
                foreach (var card in cpy)
                {

                    if (card.Value == 2 | card.Value == 3)
                    {
                        ret.Add(card.Key, 6);
                        return ret;
                    }
                }
            }
            return null;
        } // good
        public static Dictionary<int, int>? GetQuads(Dictionary<int, int> handholder) //good
        {
            Dictionary<int, int> ret = new();
            Dictionary<int, int> cpy = new(handholder);
            if (cpy.ContainsValue(4))
            {
                foreach (var card in cpy)
                {
                    if (card.Value == 4)
                    {
                        ret.Add(card.Key, 7);
                        cpy.Remove(card.Key);
                    }
                }
                ret.Add(cpy.First().Key, 7);
                return ret;
            }
            return null;
        }
        public static Dictionary<int, int>? GetStraightFlush(List<Card> cards) //good
        {
            List<Card> cpy = new(cards);
            for (int i = 1; i <= 4; i++)
            {
                int counter = 0;
                foreach (var card in cpy)
                {
                    if (card.colour == i)
                    {
                        counter++;
                    }
                }
                if (counter > 4)
                {
                    foreach (var card in cards)
                    {
                        if (card.colour != i)
                        {
                            cpy.Remove(card);
                        }
                    }
                }
            }
            if (GetStraight(cpy) is not null)
            {
                Dictionary<int, int> ret = new(GetStraight(cpy));


                return ret;
            }
            return null;
        }


        #endregion
        public void Transfermoney(Player player1, Player player2)
        {
          
                int temp = Math.Min((player1.totalbet - player2.chipstaken),(player2.totalbet-player2.chipstaken));
                player1.chips += temp;
                player1.winnings += temp;
                player2.chipstaken += temp;
        
        }
        public void ResetPlayers()
        {
            foreach (var player in this.table.players)
            {
                player.isFolded = false;
                player.hasMadedecision = false;
                player.isDealer = false;
                player.playerBet = 0;
                player.isAllIn = false;
                player.totalbet = 0;
                player.handvalue = 0;
                player.Besthandformed = new();
                player.winnings = 0;
                player.chipstaken = 0;
              
            }
        }
        public void MoveDealer()
        {
            Player temp = table.FindDealer();
            temp.isDealer = false;
            temp.Next(table.players).isDealer = true;
        }
        public void ResetTable()
        {
            this.table.pot = 0;
            this.table.currentbet = 0;
            this.table.commoncards = new();
            this.table.deck= new();
    }
        public Player Showdown()
        {
            List <Player> list = new List<Player>(table.players);
            
            foreach (Player player in table.players)
            {
                player.handvalue = DetermineHand(player).First().Value; //ustaw ręce graczy
            }
            
            list.Sort(new PlayerComparer()); // sortuję listę i ustawiam ich w kolejnosc
            list.Reverse();
            
            for (int i=0; i < list.Count; i++)
            {
                if (!list[i].isFolded)
                {
                    for(int j=1; j<list.Count; j++)
                    {
                        if (i+j<list.Count)
                        {
                            Transfermoney(list[i], list[j + i]); // zabieram żetony od każdego gracza z którym wygrywa (czyli po swojej prawej)
                        }                                        // ale nie więcej niż sam wrzucił do puli!!(WAŻNE) 
                    }
                }
            }
            foreach (Player player in list)
            {
                if (player.totalbet - player.chipstaken > 0)
                {
                    player.chips += player.totalbet - player.chipstaken; // Zwracam niewykorzystane żetony
                }
            }
            foreach(Player player in list)
            {
                if(player.winnings!=0)
                {
                    Console.WriteLine("Gracz: " + player.id + " wygrywa pulę: " + (player.winnings+player.totalbet));
                }
            }
            
            ResetPlayers();
            ResetTable();
            MoveDealer();
            
            return list[^1];
               
        }
        public Dictionary<int,int>? DetermineHand(Player currentplayer)
        {
            List<Card> cards = currentplayer.hand.Concat(table.commoncards).ToList();
            cards.Sort((x, y) => y.cardID.CompareTo(x.cardID));
            Dictionary<Card, int> handholder = new();
            Dictionary<int, int> holderforpairs = new();
            foreach (Card card in cards)
            {
                if (!handholder.ContainsKey(card)){
                    handholder.Add(card, 0);
                }
                handholder[card]++;
            }
            foreach (Card card in cards)
            {
                if (!holderforpairs.ContainsKey(card.cardID))
                {
                    holderforpairs.Add(card.cardID,0);
                }
                holderforpairs[card.cardID]++;
            }
            if(GetStraightFlush(cards) is not null ){ //STRAIGHTFLUSH CHECK
                return GetStraightFlush(cards);
            }
            if(GetQuads(holderforpairs) is not null)
            {
                return GetQuads(holderforpairs);
            }
            if(GetFullHouse(holderforpairs) is not null) {
                return GetFullHouse(holderforpairs);
            }
            if(GetFlush(cards) is not null){
                return GetFlush(cards);
            }
            if (GetStraight(cards) is not null)
            {
                return GetStraight(cards);
            }
            if (holderforpairs.ContainsValue(3)) {
                return GetTrips(holderforpairs);
            }
            if (holderforpairs.ContainsValue(2)) { // pair handling
                return GetTwoPair(holderforpairs);
            }
            holderforpairs.Remove(holderforpairs.Last().Key);
            holderforpairs.Remove(holderforpairs.Last().Key);
            return holderforpairs;
        }
        public void PostBlinds()
        {
            Player currentplayer = table.FindDealer();
            table.pot+=currentplayer.Next(table.players).Bet(table.blind);
            table.pot+=currentplayer.Next(table.players).Next(table.players).Bet(table.blind * 2);
            table.currentbet=table.blind*2;
        }
        public void FirstBettingRound()
        {
            foreach(Player  player in table.players)
            {
                player.hand.Add(table.deck.Dealcard());
                player.hand.Add(table.deck.Dealcard());
            }
            PostBlinds();
            Player currentplayer = table.FindDealer().Next(table.players).Next(table.players).Next(table.players);

            while (table.AllPlayersMadeDecision())
            {
                while (currentplayer.isFolded)
                {
                    currentplayer = currentplayer.Next(table.players);

                }
                GetDecision(currentplayer);
                currentplayer.hasMadedecision = true;
                currentplayer = currentplayer.Next(table.players);
            }
            table.Addcommoncards();
            table.Addcommoncards();
            table.Resetdecisionflags();
        }
        public void BettingRound()
        {
           table.Addcommoncards();
            foreach (Card c in table.commoncards)
            {
                FaceCards card = ((FaceCards)c.cardID);
                Console.WriteLine((FaceCards)c.cardID + " of " + (Colour)c.colour);
            }
            Player currentplayer = table.FindDealer();
            while (table.AllPlayersMadeDecision())
            {
                while (currentplayer.isFolded)
                {
                    currentplayer = currentplayer.Next(table.players);

                }
                GetDecision(currentplayer);
                currentplayer.hasMadedecision = true;
                currentplayer = currentplayer.Next(table.players);
            }
            table.Resetdecisionflags();
        }
    }
    }
    


