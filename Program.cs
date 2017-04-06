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
            //TODO 目标一千万
            for (int i = 0; i < 10000; i++)
            {
                //spForShuffle.Start();
                poker.Shuffle();
                //spForShuffle.Stop();
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

            //foreach(var key in CardLevelJudgement.countDict.Keys)
            //{
            //    Console.WriteLine(key.ToString() + " " + CardLevelJudgement.countDict[key].ToString());
            //}

            //Console.WriteLine("洗牌耗时:" + spForShuffle.ElapsedMilliseconds);
            //Console.WriteLine("获取Wrapper耗时:" + spForGetWrapper.ElapsedMilliseconds);
            Console.WriteLine("获取牌形结果耗时1:" + CardLevelJudgement.sp.ElapsedMilliseconds);
            //Console.WriteLine("获取牌形结果耗时2:" + _sp.ElapsedMilliseconds);
            //Console.WriteLine("获取LogKind结果耗时2:" + _sp1.ElapsedMilliseconds);
            //Console.WriteLine("New List:" + _sp2.ElapsedMilliseconds);
            //Console.WriteLine("判断耗时:" + Judger.sp.ElapsedMilliseconds);
            //Console.WriteLine("顺子判断耗时:" + CardLevelJudgement.spForStraight.ElapsedMilliseconds);
            //Console.WriteLine("个位判断耗时:" + CardLevelJudgement.spForOnesDigit.ElapsedMilliseconds);
            //Console.WriteLine("双王判断耗时:" + CardLevelJudgement.spForTwoJoker.ElapsedMilliseconds);
            //Console.WriteLine("同花色判断耗时:" + CardLevelJudgement.spForSameKind.ElapsedMilliseconds);
            //Console.WriteLine("同点数判断耗时:" + CardLevelJudgement.spForSamePoint.ElapsedMilliseconds);
            //Console.WriteLine("计算赔率耗时:" + CardLevelJudgement.spForOdds.ElapsedMilliseconds);
            //Console.WriteLine("比较记录耗时:" + spForLog.ElapsedMilliseconds);
            Console.WriteLine("耗时:" + CardLevelJudgement.spForCalOnesDigit.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            _sp2.Start();
            List<Card> cardList = new List<Card>(3);
            cardList.Add(poker.GetCard());
            cardList.Add(poker.GetCard());
            _sp2.Stop();
            string hash = GetHash(cardList);
            _sp.Start();
            HandCardResult resultOfTwoCard = CardLevelJudgement.GetHandCardResult(cardList);
            _sp.Stop();
            _sp2.Start();
            cardList.Add(poker.GetCard());
            _sp2.Stop();
            _sp.Start();
            HandCardResult resultOfThreeCard = CardLevelJudgement.GetHandCardResult(cardList);
            _sp.Stop();
            return new LogWrapper(hash, resultOfTwoCard, resultOfThreeCard);
        }

        private static string GetHash(List<Card> list)
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
            int onesDigit = CardListJudgement.CalculateOnesDigit(list);
            return string.Format("个数:{0} 牌型:{1}", onesDigit.ToString(), ((int)logKind).ToString());
        }

        //可以修改添加的卡牌，测试结果
        private static void TestHadnCardResult()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.diamonds, 12));
            cardList.Add(new Card(Card.Kind.club, 1));
            string hash = GetHash(cardList);
            HandCardResult resultOfTwoCard = CardLevelJudgement.GetHandCardResult(cardList);
            cardList.Add(new Card(Card.Kind.redJoker, 13));
            HandCardResult resultOfThreeCard = CardLevelJudgement.GetHandCardResult(cardList);
            Console.WriteLine(string.Format("{0} {1} {2}", cardList[0].ToCard(), cardList[1].ToCard(), cardList[2].ToCard() ));
            if(resultOfTwoCard == null)
            {
                Console.WriteLine(string.Format("twoCardResult null"));
            }
            else
            {
                Console.WriteLine(string.Format("twoCardResult level:{0} onesDigit:{1}", resultOfTwoCard.Level, resultOfTwoCard.OnesDigit));
            }
            Console.WriteLine(string.Format("threeCardResult  level:{0} onesDigit:{1}", resultOfThreeCard.Level, resultOfThreeCard.OnesDigit));
            Console.Read();
        }
    }
}
