// See https://aka.ms/new-console-template for more information
using Poker_basic;
using System.Net.Sockets;
using System.Numerics;
using System.Xml.Serialization;

Console.WriteLine("Hello, World!");
Deck deck = new();
deck.Shuffledeck();
deck.cards.Sort((x, y) => y.cardID.CompareTo(x.cardID));
deck.Printdeck();
List<Card> cards = new()
{
    new Card(14, 3),
    new Card(13, 3),
    new Card(12, 2),
    new Card(5, 3),
    new Card(4, 3),
    new Card(3, 3),
    new Card(2, 3)
};
Dictionary<int, int> handholder = new();

foreach(var print in handholder)
{
    Console.WriteLine(print.Value+"---" + print.Key);
}
Game gra = new();
if (true) {
    Dictionary<int,int> hold = new();
    if (Game.GetStraightFlush(cards) is not null)
    {
        hold = Game.GetStraightFlush(cards);
        foreach (var card in hold)
        {
            Console.WriteLine(card.Key + "---" + card.Value);
        }
    }
    else
    {
        Console.WriteLine("WHYYY");
    }
        };