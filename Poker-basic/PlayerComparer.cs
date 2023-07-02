using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_basic
{
    internal class PlayerComparer : IComparer<Player>
    {
            public int Compare(Player x, Player y)
            {
            if (x.handvalue == y.handvalue)
            {
                for (int i = 0; i < x.Besthandformed.Count; i++)
                {
                    if (x.Besthandformed[i].cardID != y.Besthandformed[i].cardID)
                    {
                        return (x.Besthandformed[i].cardID.CompareTo(y.Besthandformed[i].cardID));
                    }
                }

            }
                return x.handvalue.CompareTo(y.handvalue);
            }
    }
}
