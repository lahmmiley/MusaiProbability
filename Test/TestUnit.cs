using Musai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace Test
{
    public class TestUnit
    {
        private static TestUnit _singleton;
        public static TestUnit Singleton
        {
            get
            {
                if(_singleton == null)
                {
                    _singleton = new TestUnit();
                }
                return _singleton;
            }
        }

        private TestUnit(){}

        public void Test()
        {
            do 
            {
                if(!TestRandom())
                {
                    break;
                }

                if(!TestHandCardResult())
                {
                    break;
                }

                if(!TestJudger())
                {
                    break;
                }
            } while (false);
            Console.WriteLine("数据测试正确");
        }

        public bool TestRandom()
        {
            Dictionary<string, int> _dict = new Dictionary<string, int>();
            Poker poker = Poker.Singleton;
            int randomNum = 0;
            for (int i = 0; i < 54000; i++)
            {
                poker.Shuffle();
                for (int j = 0; j < 6; j++)
                {
                    randomNum++;
                    Card card = poker.GetCard();
                    string hash = card.ToString();
                    if (!_dict.ContainsKey(hash))
                    {
                        _dict.Add(hash, 0);
                    }
                    _dict[hash] += 1;
                }
            }
            string content = string.Empty;
            int max = int.MinValue;
            int min = int.MaxValue;
            foreach (string key in _dict.Keys)
            {
                content += string.Format("key:{0} count:{1}\n", key.ToCard(), _dict[key]);
                if (_dict[key] > max)
                {
                    max = _dict[key];
                }
                if (_dict[key] < min)
                {
                    min = _dict[key];
                }
            }
            int range = randomNum / 54 / 10;
            if (_dict.Count != Poker.CARD_NUM)
            {
                Console.WriteLine("生成卡牌数量错误");
                return false;
            }
            if (max - min > range)
            {
                Console.WriteLine("生成随机数不够随机");
                return false;
            }
            return true;
        }

        public bool TestHandCardResult()
        {
            if(
                TestResult(new Card(Card.Kind.redJoker), new Card(Card.Kind.blackJoker), 
                    CardLevel.twoJoker, 10) &&
                //TODO
                //TestResult(new Card(Card.Kind.redJoker, -1), new Card(Card.Kind.diamonds, 1), 
                //    CardLevel.invalid, 10, 9) &&

                TestResult(new Card(Card.Kind.diamonds, 10), new Card(Card.Kind.hearts, 9), 
                      CardLevel.tianGongNine, 1) &&
                TestResult(new Card(Card.Kind.diamonds, 10), new Card(Card.Kind.diamonds, 9), 
                    CardLevel.tianGongNine, 2) &&
                TestResult(new Card(Card.Kind.hearts, 13), new Card(Card.Kind.diamonds, 9), 
                    CardLevel.tianGongNine, 1) &&
                TestResult(new Card(Card.Kind.diamonds, 4), new Card(Card.Kind.diamonds, 4), 
                    CardLevel.tianGongEight, 2) &&
                TestResult(new Card(Card.Kind.hearts, 8), new Card(Card.Kind.diamonds, 12), 
                    CardLevel.tianGongEight, 1) &&
                TestResult(new Card(Card.Kind.spades, 4), new Card(Card.Kind.diamonds, 4), 
                    CardLevel.tianGongEight, 2) &&
                TestResult(new Card(Card.Kind.hearts, 1), new Card(Card.Kind.redJoker), new Card(Card.Kind.blackJoker), 
                    CardLevel.threeCardWithTwoJoker, 8) &&
                TestResult(new Card(Card.Kind.redJoker), new Card(Card.Kind.club, 12), new Card(Card.Kind.blackJoker), 
                    CardLevel.threeCardWithTwoJoker, 8) &&
                TestResult(new Card(Card.Kind.hearts, 1), new Card(Card.Kind.hearts, 2), new Card(Card.Kind.blackJoker), 
                    CardLevel.straightFlush, 7) &&
                TestResult(new Card(Card.Kind.hearts, 3), new Card(Card.Kind.hearts, 2), new Card(Card.Kind.hearts, 1), 
                    CardLevel.straightFlush, 7) &&
                TestResult(new Card(Card.Kind.hearts, 1), new Card(Card.Kind.hearts, 2), new Card(Card.Kind.hearts, 13), 
                    CardLevel.straightFlush, 7) &&
                TestResult(new Card(Card.Kind.hearts, 12), new Card(Card.Kind.hearts, 1), new Card(Card.Kind.hearts, 13), 
                    CardLevel.straightFlush, 7) &&
                TestResult(new Card(Card.Kind.hearts, 12), new Card(Card.Kind.diamonds, 12), new Card(Card.Kind.club, 12), 
                    CardLevel.threeCardWithSamePoint, 5) &&
                TestResult(new Card(Card.Kind.blackJoker), new Card(Card.Kind.hearts, 11), new Card(Card.Kind.club, 11), 
                    CardLevel.threeCardWithSamePoint, 5) &&
                TestResult(new Card(Card.Kind.hearts, 1), new Card(Card.Kind.hearts, 1), new Card(Card.Kind.hearts, 1), 
                    CardLevel.threeCardWithSamePoint, 5) &&
                TestResult(new Card(Card.Kind.hearts, 1), new Card(Card.Kind.hearts, 2), new Card(Card.Kind.club, 3), 
                    CardLevel.straight, 4) &&
                TestResult(new Card(Card.Kind.hearts, 11), new Card(Card.Kind.redJoker), new Card(Card.Kind.club, 13), 
                    CardLevel.straight, 4) &&
                TestResult(new Card(Card.Kind.club, 1), new Card(Card.Kind.diamonds, 2), new Card(Card.Kind.club, 3), 
                    CardLevel.straight, 4) &&
                TestResult(new Card(Card.Kind.club, 1), new Card(Card.Kind.diamonds, 2), new Card(Card.Kind.club, 7), 
                    CardLevel.onesDigitIsZero, 10, 0) &&
                TestResult(new Card(Card.Kind.diamonds, 2), new Card(Card.Kind.club, 8), 
                    CardLevel.onesDigitIsZero, 10, 0) &&
                TestResult(new Card(Card.Kind.club, 10), new Card(Card.Kind.diamonds, 12), new Card(Card.Kind.club, 13), 
                    CardLevel.onesDigitIsZero, 10, 0) &&
                TestResult(new Card(Card.Kind.club, 10), new Card(Card.Kind.club, 13), 
                    CardLevel.onesDigitIsZero, 20, 0) &&
                TestResult(new Card(Card.Kind.diamonds, 10), new Card(Card.Kind.club, 10), 
                    CardLevel.onesDigitIsZero, 20, 0) &&
                TestResult(new Card(Card.Kind.club, 11), new Card(Card.Kind.club, 10), new Card(Card.Kind.club, 13), 
                    CardLevel.onesDigitIsZero, 30, 0) &&
                TestResult(new Card(Card.Kind.club, 11), new Card(Card.Kind.club, 7),
                    CardLevel.other, 2, 7) &&
                TestResult(new Card(Card.Kind.club, 11), new Card(Card.Kind.club, 6),
                    CardLevel.other, 2, 6) &&
                TestResult(new Card(Card.Kind.diamonds, 11), new Card(Card.Kind.club, 1),
                    CardLevel.other, 1, 1) &&
                TestResult(new Card(Card.Kind.club, 11), new Card(Card.Kind.club, 2),
                    CardLevel.other, 2, 2) &&
                TestResult(new Card(Card.Kind.hearts, 2), new Card(Card.Kind.club, 4),
                    CardLevel.other, 1, 6) &&
                TestResult(new Card(Card.Kind.club, 1), new Card(Card.Kind.club, 3), new Card(Card.Kind.club, 5), 
                    CardLevel.other, 3, 9) &&
                TestResult(new Card(Card.Kind.club, 5), new Card(Card.Kind.club, 4), new Card(Card.Kind.club, 5), 
                    CardLevel.other, 3, 4) &&
                TestResult(new Card(Card.Kind.club, 2), new Card(Card.Kind.club, 3), new Card(Card.Kind.hearts, 6), 
                    CardLevel.other, 1, 1) &&
                TestResult(new Card(Card.Kind.spades, 11), new Card(Card.Kind.club, 10), new Card(Card.Kind.club, 2), 
                    CardLevel.other, 1, 2)
                )
            {
                return true;
            }
            Console.WriteLine("获取HandCardResult错误");
            return false;
        }

        private bool TestResult(List<Card> cardList, CardLevel level, int odds, int onesDigit = HandCardResult.INVALID_VALUE)
        {
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == level) && 
                (result.Odds == odds) && 
                (result.OnesDigit == onesDigit))
            {
                return true;
            }
            return false;
        }

        private bool TestResult(Card cardA, Card cardB, CardLevel level, int odds, int onesDigit = HandCardResult.INVALID_VALUE)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(cardA);
            cardList.Add(cardB);
            return TestResult(cardList, level, odds, onesDigit);
        }

        private bool TestResult(Card cardA, Card cardB, Card cardC, CardLevel level, int odds, int onesDigit = HandCardResult.INVALID_VALUE)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(cardA);
            cardList.Add(cardB);
            cardList.Add(cardC);
            return TestResult(cardList, level, odds, onesDigit);
        }

        public bool TestJudger()
        {
            if(
                TestJudge(new HandCardResult(CardLevel.twoJoker), new HandCardResult(CardLevel.straightFlush), Judger.Result.win) &&
                TestJudge(new HandCardResult(CardLevel.straightFlush), new HandCardResult(CardLevel.straightFlush), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.straightFlush), new HandCardResult(CardLevel.twoJoker), Judger.Result.lose) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.twoJoker), Judger.Result.win) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.straightFlush), Judger.Result.lose) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.onesDigitIsZero, 0), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.other, 1), new HandCardResult(CardLevel.onesDigitIsZero, 0), Judger.Result.win) &&
                TestJudge(new HandCardResult(CardLevel.other, 3), new HandCardResult(CardLevel.other, 3), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.other, 5), new HandCardResult(CardLevel.other, 3), Judger.Result.win) &&
                TestJudge(new HandCardResult(CardLevel.straight), new HandCardResult(CardLevel.straight), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.other, 3), Judger.Result.lose) &&
                TestJudge(new HandCardResult(CardLevel.threeCardWithTwoJoker), new HandCardResult(CardLevel.twoJoker), Judger.Result.lose)
            )
            {
                return true;
            }
            Console.WriteLine("HandCardResult判断错误");
            return false;
        }

        private bool TestJudge(HandCardResult a, HandCardResult b, Judger.Result result)
        {
            if(result == Judger.Judge(a, b))
            {
                return true;
            }
            return false;
        }
        
    }
}
