using System;
using System.Collections.Generic;
using System.Diagnostics;
using Test;

namespace Musai
{
    class Program
    {
        private static Stopwatch _sp = new Stopwatch();
        private static Stopwatch _sp1 = new Stopwatch();
        private static Stopwatch _sp2 = new Stopwatch();
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
            for (int i = 0; i < 10000000; i++)
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

            Console.WriteLine("洗牌耗时:" + spForShuffle.ElapsedMilliseconds);
            Console.WriteLine("获取Wrapper耗时:" + spForGetWrapper.ElapsedMilliseconds);
            Console.WriteLine("获取牌形结果耗时:" + CardLevelJudgement.sp.ElapsedMilliseconds);
            Console.WriteLine("获取LogKind结果耗时:" + _sp1.ElapsedMilliseconds);
            //Console.WriteLine("获取牌形结果耗时2:" + _sp.ElapsedMilliseconds);
            //Console.WriteLine("New List:" + _sp2.ElapsedMilliseconds);
            //Console.WriteLine("判断耗时:" + Judger.sp.ElapsedMilliseconds);
            Console.WriteLine("比较记录耗时:" + spForLog.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            Card a, b, c;
            a = poker.GetCard();
            b = poker.GetCard();
            c = poker.GetCard();
            HandCard twoCard = HandCardPool.Get(a, b);
            HandCard threeeCard = HandCardPool.Get(a, b, c);
            HandCardResult resultOfTwoCard = CardLevelJudgement.GetHandCardResult(twoCard);
            HandCardResult resultOfThreeCard = CardLevelJudgement.GetHandCardResult(threeeCard);
            return new LogWrapper(twoCard.Hash, resultOfTwoCard, resultOfThreeCard);
        }
    }
}
