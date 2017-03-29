using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    class Program
    {
        static void Main(string[] args)
        {
            Poker poker = Poker.Singleton;
            for(int i = 0; i < 10000; i++)
            {
                poker.SwapCard();
                //避免频繁洗牌
                for(int j = 0; j < 8; j++)
                {
                    HandCard handCardA = new HandCard(poker.GetCard(), poker.GetCard());
                    handCardA.AddCard(poker.GetCard());

                    HandCard handCardB = new HandCard(poker.GetCard(), poker.GetCard());
                    handCardB.AddCard(poker.GetCard());

                    Logger.Log(handCardA, handCardB);
                }
            }

            Logger.Print();
        }
    }
}
