using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    internal class Game
    {
        private readonly Table table = new();
        public void GameStart()
        {
            while (true) {
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
            if (table.currentbet * 2 - currentplayer.playerBet >= number)
            {
                return true;
            }
            return false;
        }
        public void GetBet(Player currentplayer) {
            Console.WriteLine("Tura gracza " + currentplayer.id);
            Console.WriteLine("Podaj o ile chcesz podbić (minimum: " + (table.currentbet * 2 - currentplayer.playerBet) + ")");
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

        public bool CheckIfTwoPair(Dictionary<Card, int> handholder)
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
        public  static Dictionary<int, int> GetTwoPair(Dictionary<int, int> handholder) // good
        {
            Dictionary<int, int> returnhand = new();
            int movedcards = 0;
            foreach (var holder in handholder)
            {
                if (holder.Value == 2 && movedcards<2)
                {
                    Console.WriteLine("Removed");
                    returnhand.Add(holder.Key, 2);
                    handholder.Remove(holder.Key);
                    movedcards++;
                }
               
            }
            KeyValuePair<int, int> firstElement = handholder.First();
            returnhand.Add(firstElement.Key, firstElement.Value);
            return returnhand;
        }
        public static Dictionary<int, int> GetOnePair(Dictionary<int, int> handholder) // good 
        {
            Dictionary<int, int> returnhand = new();
            foreach (var holder in handholder)
            {
                if (holder.Value == 2)
                {
                    returnhand.TryAdd(holder.Key, holder.Value);
                    handholder.Remove(holder.Key);
                }
            }
            while (returnhand.Count < 4) {
                returnhand.TryAdd(handholder.First().Key, handholder.First().Value);
                handholder.Remove(handholder.First().Key);
            }
            return returnhand;
        }
        public static Dictionary<int, int> GetTrips(Dictionary<int, int> handholder) // good
        {
            Dictionary<int, int> returnhand = new();
            foreach (var holder in handholder)
            {
                if (holder.Value == 3)
                {
                    returnhand.Add(holder.Key, holder.Value);
                    if (holder.Key == handholder.First().Key)
                    {
                        handholder.Remove(handholder.First().Key);
                    }
                    else
                    {
                        handholder.Remove(handholder[holder.Key]);
                    }
                }
            }
            returnhand.TryAdd(handholder.First().Key, handholder.First().Value);
            handholder.Remove(handholder.First().Key);
            returnhand.TryAdd(handholder.First().Key, handholder.First().Value);
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
        public static Dictionary<int, int>? GetStraight(List<Card>cards) 
        {
            int loopcounter = 0;
            Dictionary<int, int> ret = new();
            for (int i = 0;i < cards.Count-1; i++)
            {
                if (cards[i].cardID - 1 == cards[(i + 1) % cards.Count].cardID)
                {
                    loopcounter++;
                    if (loopcounter == 3 && i>3 && cards[^1].cardID == 2 && cards[0].cardID == 14) // Rozpatrzenie strita od 5 do asa
                    {
                        ret.Add(5, 4);
                        Console.WriteLine("this1");
                        return ret;
                    }
                    if (loopcounter == 4) // Reszta 
                    {
                        ret.Add(cards[i - 3].cardID, 4);
                        return ret;
                    }
                }
                else if (cards[i].cardID == cards[(i + 1) % cards.Count].cardID) { } 
                else { loopcounter = 0; }
            }
            return null; 
        } // good
        public static Dictionary<int, int>? GetFlush(List<Card>cards) { //good
            Dictionary<int, int> ret = new();
            int counter = 0;
            for (int i=1;i <= 4; i++)
            {
                
                foreach (var card in cards)
                {
                    if(card.colour==i) {
                        counter++;
                        
                    }
                }
                if (counter > 4)
                {
                    foreach(var card in cards)
                    {
                        if(card.colour==i)
                        {
                            
                            ret.Add(card.cardID, 5);
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
            if((cpy.ContainsValue(3) | cpy.ContainsValue(2)) ) {
                foreach (var card in cpy)
                {
           
                    if(card.Value == 2 | card.Value==3)
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
            Dictionary <int, int> ret = new(); 
            Dictionary<int, int> cpy = new( handholder);
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
        public static Dictionary<int, int>? GetStraightFlush(List<Card>cards) //good
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
                Dictionary<int, int> ret = new();
                ret.Add(GetStraight(cpy).ElementAt(0).Key, 10);
                return ret;
            }
            return null;
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
            if (holderforpairs.Max().Value == 3) {
                return GetTrips(holderforpairs);
            }
            if (holderforpairs.Max().Value == 2) { // pair handling
                if (CheckIfTwoPair(handholder))
                {
                    return GetTwoPair(holderforpairs);
                }
                else
                {
                    GetOnePair(holderforpairs);
                }
            }
            return null;
        }

        
 

        public void BettingRound()
        {
            Player currentplayer = table.FindDealer();
            while (!table.AllPlayersMadeDecision())
            {
                while (currentplayer.isFolded)
                {
                    currentplayer = currentplayer.Next(table.players);

                }
                GetDecision(currentplayer);
                currentplayer = currentplayer.Next(table.players);
            }
        }
    }
    }
    


