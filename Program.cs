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
            TestUnit.Singleton.Test();
            Stopwatch sp = new Stopwatch();
            Stopwatch spForShuffle = new Stopwatch();
            Stopwatch spForGetWrapper = new Stopwatch();
            Stopwatch spForLog = new Stopwatch();
            sp.Start();
            Poker poker = Poker.Singleton;
            //一千万耗时5分钟以上
            for (int i = 0; i < 1; i++)
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
            Console.WriteLine("排序耗时:" + sw.ElapsedMilliseconds);
            Console.WriteLine("hash耗时:" + HandCardPool.sw.ElapsedMilliseconds);
            Console.WriteLine("创建结果耗时:" + HandCardPool.sw1.ElapsedMilliseconds);
            //Console.WriteLine("比较记录耗时:" + spForLog.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            sw.Start();
            var a = poker.GetCard();
            var b = poker.GetCard();
            var c = poker.GetCard();
            //List<Card> twoCardList = new List<Card>() { poker.GetCard(), poker.GetCard() };
            List<Card> twoCardList = new List<Card>() { a, b, c };
            twoCardList.Sort(Card.Sort1);

            List<Card> twoCardList1 = new List<Card>() { a, b };
            Card.SortTwoCardList(twoCardList1);
            Card.InsertTwoCardList(twoCardList, poker.GetCard());
            if((twoCardList[0] == twoCardList1[0]) &&
                (twoCardList[1] == twoCardList1[1]) &&
                (twoCardList[2] == twoCardList1[2]))
            {

            }
            else
            {
                Console.WriteLine("sort error");
                Console.WriteLine(twoCardList[0]);
                Console.WriteLine(twoCardList1[0]);
                Console.WriteLine(twoCardList[1]);
                Console.WriteLine(twoCardList1[1]);
            }
            sw.Stop();
            HandCardResult resultOfTwoCard;
            string logType;
            HandCardPool.Get(twoCardList, out resultOfTwoCard, out logType);

            sw.Start();
            List<Card> threeCardList = new List<Card>() { twoCardList[0], twoCardList[1], poker.GetCard()};
            threeCardList.Sort(Card.Sort1);
            //Card.InsertTwoCardList(threeCardList, poker.GetCard());
            sw.Stop();
            HandCardResult resultOfThreeCard;
            HandCardPool.Get(threeCardList, out resultOfThreeCard);

            return new LogWrapper(logType, resultOfTwoCard, resultOfThreeCard);
        }
    }
}
