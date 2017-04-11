using System;
using System.Collections.Generic;
using System.Diagnostics;
using Test;

namespace Musai
{
    class Program
    {
        public static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            Stopwatch spForShuffle = new Stopwatch();
            Stopwatch spForGetWrapper = new Stopwatch();
            Stopwatch spForLog = new Stopwatch();
            Stopwatch spForTest = new Stopwatch();
            spForTest.Start();
            TestUnit.Singleton.Test();
            spForTest.Stop();
            Poker poker = Poker.Singleton;
            //一千万耗时3分钟
            for (int i = 0; i < 500000; i++)
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

            Console.WriteLine("测试耗时:" + spForTest.ElapsedMilliseconds);
            Console.WriteLine("洗牌耗时:" + spForShuffle.ElapsedMilliseconds);
            Console.WriteLine("获取Wrapper耗时:" + spForGetWrapper.ElapsedMilliseconds);
            Console.WriteLine("排序耗时:" + sw.ElapsedMilliseconds);
            Console.WriteLine("比较记录耗时:" + spForLog.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            List<Card> twoCardList = new List<Card>() { poker.GetCard(), poker.GetCard() };
            Card.SortTwoCardList(twoCardList);
            sw.Start();
            HandCardResult resultOfTwoCard;
            string logType;
            HandCardPool.Get(twoCardList, out resultOfTwoCard, out logType);
            sw.Stop();

            List<Card> threeCardList = new List<Card>() { twoCardList[0], twoCardList[1], poker.GetCard()};
            Card.InsertTwoCardList(threeCardList, poker.GetCard());
            HandCardResult resultOfThreeCard;
            HandCardPool.Get(threeCardList, out resultOfThreeCard);

            return new LogWrapper(logType, resultOfTwoCard, resultOfThreeCard);
        }
    }
}
