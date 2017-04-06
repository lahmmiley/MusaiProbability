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
            for (int i = 0; i < 10000; i++)
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
            List<Card> cardList = new List<Card>(3);
            cardList.Add(poker.GetCard());
            cardList.Add(poker.GetCard());
            HandCardResult resultOfTwoCard = CardLevelJudgement.GetHandCardResult(cardList);
            string hash = GetHash(cardList, resultOfTwoCard.OnesDigit);
            cardList.Add(poker.GetCard());
            HandCardResult resultOfThreeCard = CardLevelJudgement.GetHandCardResult(cardList);
            return new LogWrapper(hash, resultOfTwoCard, resultOfThreeCard);
        }

        private static string GetHash(List<Card> list, int onesDigit)
        {
            string hash = string.Empty;
            list.Sort(Card.Sort);
            _sp1.Start();
            LogKind logKind = LogKindJudgement.GetLogKind(list);
            _sp1.Stop();
            if(logKind == LogKind.twoJoker)
            {
                return "双王";
            }
            if(logKind ==LogKind.oneJoker)
            {
                return "单王";
            }
            return string.Format("个位:{0} 牌型:{1}", onesDigit.ToString(), ((int)logKind).ToString());
        }
    }
}
