// See https://aka.ms/new-console-template for more information
using Poker_basic;
using System.Net.Sockets;
using System.Numerics;
using System.Xml.Serialization;

Console.WriteLine("Hello, World!");

Game game = new();
game.GameStart();
while (true)
{
    
    game.FirstBettingRound();
    //game.BettingRound();
    //game.BettingRound();
    game.BettingRound();
    game.Showdown();
}


