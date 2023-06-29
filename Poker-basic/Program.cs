// See https://aka.ms/new-console-template for more information
using Poker_basic;
using System.Net.Sockets;
using System.Numerics;

Console.WriteLine("Hello, World!");
Deck deck = new();
deck.Shuffledeck();
for (int i = 0; i < 50; i++)
{
    deck.Dealcard();
}
deck.Printdeck();
