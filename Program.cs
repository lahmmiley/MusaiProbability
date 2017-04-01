using System;
using System.Collections.Generic;

namespace Musai
{
    class Program
    {
        static void Main(string[] args)
        {
            Poker poker = Poker.Singleton;
            for(int i = 0; i < 10000; i++)
            {
                poker.Shuffle();
                //避免频繁洗牌
                for(int j = 0; j < 5; j++)
                {
                    Logger.Log(GetLogWrapper(poker), GetLogWrapper(poker));
                }
            }

            Logger.Save();
        }
        
        private static LogWrapper GetLogWrapper(Poker poker)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(poker.GetCard());
            cardList.Add(poker.GetCard());
            string hash = GetHash(cardList);
            HandCardResult resultOfTwoCard = HandCardResultJudgement.GetHandCardResult(cardList);
            cardList.Add(poker.GetCard());
            HandCardResult resultOfThreeCard = HandCardResultJudgement.GetHandCardResult(cardList);
            return new LogWrapper(hash, resultOfTwoCard, resultOfThreeCard);
        }

        private static string GetHash(List<Card> list)
        {
            string hash = string.Empty;
            for(int i = 0; i < list.Count; i++)
            {
                hash += list[i].ToString() + " ";
            }
            return hash;
        }

        //可以修改添加的卡牌，测试结果
        private static void TestHadnCardResult()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.diamonds, 12));
            cardList.Add(new Card(Card.Kind.club, 1));
            string hash = GetHash(cardList);
            HandCardResult resultOfTwoCard = HandCardResultJudgement.GetHandCardResult(cardList);
            cardList.Add(new Card(Card.Kind.redJoker, 13));
            HandCardResult resultOfThreeCard = HandCardResultJudgement.GetHandCardResult(cardList);
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
