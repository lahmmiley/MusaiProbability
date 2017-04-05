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
            TestUnit testUnit = TestUnit.Singleton;
            if(!testUnit.TestRandom())
            {
                Console.Read();
                return;
            }


            Stopwatch sp = new Stopwatch();
            sp.Start();
            Poker poker = Poker.Singleton;
            Stopwatch spForShuffle = new Stopwatch();
            Stopwatch spForLog = new Stopwatch();
            //TODO 目标一千万
            for (int i = 0; i < 10000; i++)
            {
                spForShuffle.Start();
                poker.Shuffle();
                spForShuffle.Stop();
                for (int j = 0; j < 4; j++)
                {
                    spForLog.Start();
                    Logger.Log(GetLogWrapper(poker), GetLogWrapper(poker));
                    spForLog.Stop();
                }
            }

            Logger.Save();
            sp.Stop();
            //Console.WriteLine("洗牌耗时:" + spForShuffle.ElapsedMilliseconds);
            Console.WriteLine("获取牌形结果耗时:" + CardLevelJudgement.sp.ElapsedMilliseconds);
            //Console.WriteLine("判断耗时:" + Judger.sp.ElapsedMilliseconds);
            Console.WriteLine("顺子判断耗时:" + CardLevelJudgement.spForStraight.ElapsedMilliseconds);
            Console.WriteLine("比较记录耗时:" + spForLog.ElapsedMilliseconds);
            Console.WriteLine("游戏总耗时:" + sp.ElapsedMilliseconds);
            Console.Read();
        }

        private static LogWrapper GetLogWrapper(Poker poker)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(poker.GetCard());
            cardList.Add(poker.GetCard());
            string hash = GetHash(cardList);
            HandCardResult resultOfTwoCard = CardLevelJudgement.GetHandCardResult(cardList);
            cardList.Add(poker.GetCard());
            HandCardResult resultOfThreeCard = CardLevelJudgement.GetHandCardResult(cardList);
            return new LogWrapper(hash, resultOfTwoCard, resultOfThreeCard);
        }

        private static string GetHash(List<Card> list)
        {
            string hash = string.Empty;
            list.Sort(Card.Sort);
            LogKind logKind = LogKindJudgement.GetLogKind(list);
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
