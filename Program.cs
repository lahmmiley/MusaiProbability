using System;
using System.Collections.Generic;
using System.Diagnostics;
using Test;

namespace Musai
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            Stopwatch spForTest = new Stopwatch();
            spForTest.Start();
            TestUnit.Singleton.Test();
            spForTest.Stop();
            Poker poker = Poker.Singleton;
            int totalCount = 100000000;
            int staticCount = totalCount / 1000;
            for (int i = 0; i < totalCount; i++)
            {
                poker.Shuffle();
                for (int j = 0; j < 4; j++)
                {
                    LogWrapper a = GetLogWrapper(poker);
                    LogWrapper b = GetLogWrapper(poker);
                    if(i < staticCount)
                    {
                        Logger.StaticLog(a, b);
                    }
                    else
                    {
                        if(i == staticCount)
                        {
                            Logger.Save("静态统计数据.dat");
                        }
                        Logger.DynamicLog(a, b);
                    }
                }
            }

            Logger.Save("动态统计数据.dat");

            Console.WriteLine("测试耗时:" + spForTest.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            List<Card> twoCardList = new List<Card>() { poker.GetCard(), poker.GetCard() };
            Card.SortTwoCardList(twoCardList);
            HandCardResult resultOfTwoCard;
            string logType;
            HandCardPool.Get(twoCardList, out resultOfTwoCard, out logType);

            List<Card> threeCardList = new List<Card>() { twoCardList[0], twoCardList[1] };
            Card.InsertTwoCardList(threeCardList, poker.GetCard());
            HandCardResult resultOfThreeCard;
            HandCardPool.Get(threeCardList, out resultOfThreeCard);

            return new LogWrapper(logType, resultOfTwoCard, resultOfThreeCard);
        }
    }
}
