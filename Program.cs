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
            TestUnit.Singleton.Test();
            Stopwatch sp = new Stopwatch();
            Stopwatch spForShuffle = new Stopwatch();
            Stopwatch spForGetWrapper = new Stopwatch();
            Stopwatch spForLog = new Stopwatch();
            sp.Start();
            Poker poker = Poker.Singleton;
            //一千万耗时5分钟以上
            for (int i = 0; i < 100000; i++)
            {
                spForShuffle.Start();
                poker.Shuffle();
                spForShuffle.Stop();
                for (int j = 0; j < 4; j++)
                {
                    spForGetWrapper.Start();
                    LogWrapper a = GetLogWrapper(poker);
                    LogWrapper b = GetLogWrapper(poker);
                    spForGetWrapper.Stop();
                    spForLog.Start();
                    Logger.Log(a, b);
                    spForLog.Stop();
                }
            }

            Logger.Save();

            //Console.WriteLine("洗牌耗时:" + spForShuffle.ElapsedMilliseconds);
            Console.WriteLine("获取Wrapper耗时:" + spForGetWrapper.ElapsedMilliseconds);
            Console.WriteLine("sw:" + HandCardPool.sw.ElapsedMilliseconds);
            Console.WriteLine("sw1:" + HandCardPool.sw1.ElapsedMilliseconds);
            //Console.WriteLine("比较记录耗时:" + spForLog.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            Card a, b, c;
            a = poker.GetCard();
            b = poker.GetCard();
            c = poker.GetCard();

            List<Card> twoCardList = new List<Card>() { a, b };
            HandCardResult resultOfTwoCard;
            string logType;
            HandCardPool.Get(twoCardList, out resultOfTwoCard, out logType);

            List<Card> threeCardList = new List<Card>() { a, b, c };
            HandCardResult resultOfThreeCard;
            HandCardPool.Get(threeCardList, out resultOfThreeCard, out logType);

            return new LogWrapper(logType, resultOfTwoCard, resultOfThreeCard);
        }
    }
}
