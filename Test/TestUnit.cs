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

                if(!TestLogKind())
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
                TestResult(new Card(Card.Kind.redJoker), new Card(Card.Kind.blackJoker), new Card(Card.Kind.wildCard), 
                    CardLevel.threeJoker, 15) &&
                TestResult(new Card(Card.Kind.redJoker), new Card(Card.Kind.blackJoker), 
                    CardLevel.twoJoker, 10) &&
                TestResult(new Card(Card.Kind.redJoker), new Card(Card.Kind.diamonds, 1), 
                    CardLevel.invalid, -1) &&
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
        private bool TestResult(HandCard handCard, CardLevel level, int odds, int onesDigit = HandCardResult.INVALID_VALUE)
        {
            HandCardResult result = CardLevelJudgement.GetHandCardResult(handCard);
            if((result.Level == level) && 
                (result.Odds == odds))
            {
                if((onesDigit == HandCardResult.INVALID_VALUE) || (result.OnesDigit == onesDigit))
                {
                    return true;
                }
            }
            Console.WriteLine("计算结果 level:{0} odd:{1} onesDigit:{2}  正确结果 level:{3} odd:{4} onesDigit:{5}",
                result.Level, result.Odds, result.OnesDigit, level, odds, onesDigit);
            return false;
        }

        private bool TestResult(Card cardA, Card cardB, CardLevel level, int odds, int onesDigit = HandCardResult.INVALID_VALUE)
        {
            return TestResult(new HandCard(cardA, cardB), level, odds, onesDigit);
        }

        private bool TestResult(Card cardA, Card cardB, Card cardC, CardLevel level, int odds, int onesDigit = HandCardResult.INVALID_VALUE)
        {
            return TestResult(new HandCard(cardA, cardB, cardC), level, odds, onesDigit);
        }

        public bool TestJudger()
        {
            if(
                TestJudge(new HandCardResult(CardLevel.threeJoker, -1, 15), new HandCardResult(CardLevel.straightFlush), Judger.Result.win, 15) &&
                TestJudge(new HandCardResult(CardLevel.twoJoker, -1, 10), new HandCardResult(CardLevel.straightFlush), Judger.Result.win, 10) &&
                TestJudge(new HandCardResult(CardLevel.straightFlush), new HandCardResult(CardLevel.straightFlush), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.straightFlush), new HandCardResult(CardLevel.twoJoker, -1, 10), Judger.Result.lose, 10) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0, 10), new HandCardResult(CardLevel.twoJoker), Judger.Result.win, 10) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.straightFlush, 0, 7), Judger.Result.lose, 7) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.onesDigitIsZero, 0), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.other, 1, 2), new HandCardResult(CardLevel.onesDigitIsZero, 0, 1), Judger.Result.win, 2) &&
                TestJudge(new HandCardResult(CardLevel.other, 3, 3), new HandCardResult(CardLevel.other, 3, 2), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.other, 5, 1), new HandCardResult(CardLevel.other, 3, 3), Judger.Result.win, 1) &&
                TestJudge(new HandCardResult(CardLevel.straight), new HandCardResult(CardLevel.straight), Judger.Result.draw) &&
                TestJudge(new HandCardResult(CardLevel.onesDigitIsZero, 0), new HandCardResult(CardLevel.other, 3, 3), Judger.Result.lose, 3) &&
                TestJudge(new HandCardResult(CardLevel.threeCardWithTwoJoker), new HandCardResult(CardLevel.twoJoker, -1, 10), Judger.Result.lose, 10)
            )
            {
                return true;
            }
            Console.WriteLine("HandCardResult判断错误");
            return false;
        }

        private bool TestJudge(HandCardResult a, HandCardResult b, Judger.Result result, int odds = 0)
        {
            if(result == Judger.Judge(a, b))
            {
                if(result == Judger.Result.draw)
                {
                    return true;
                }
                else
                {
                    if(odds == Logger.GetOdds(result, a, b))
                    {
                        return true;
                    }
                }
            }
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(result);
            Console.WriteLine(odds);
            return false;
        }

        private bool TestLogKind()
        {
            if(
                TestLogKind(new Card(Card.Kind.blackJoker), new Card(Card.Kind.redJoker), LogKind.twoJoker) &&
                TestLogKind(new Card(Card.Kind.redJoker), new Card(Card.Kind.wildCard), LogKind.twoJoker) &&
                TestLogKind(new Card(Card.Kind.blackJoker), new Card(Card.Kind.club, 4), LogKind.oneJoker) &&
                TestLogKind(new Card(Card.Kind.club, 2), new Card(Card.Kind.blackJoker), LogKind.oneJoker) &&
                TestLogKind(new Card(Card.Kind.club, 3), new Card(Card.Kind.club, 4), LogKind.straightPossible1AndSameKind) &&
                TestLogKind(new Card(Card.Kind.club, 1), new Card(Card.Kind.club, 13), LogKind.straightPossible1AndSameKind) &&
                TestLogKind(new Card(Card.Kind.club, 13), new Card(Card.Kind.club, 1), LogKind.straightPossible1AndSameKind) &&
                TestLogKind(new Card(Card.Kind.club, 3), new Card(Card.Kind.club, 2), LogKind.straightPossible1AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 4), new Card(Card.Kind.diamonds, 2), LogKind.straightPossible2AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 4), new Card(Card.Kind.diamonds, 6), LogKind.straightPossible2AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 1), new Card(Card.Kind.diamonds, 12), LogKind.straightPossible2AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 12), new Card(Card.Kind.diamonds, 1), LogKind.straightPossible2AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 2), new Card(Card.Kind.diamonds, 13), LogKind.straightPossible2AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 13), new Card(Card.Kind.diamonds, 2), LogKind.straightPossible2AndSameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 1), new Card(Card.Kind.club, 13), LogKind.straightPossible1) &&
                TestLogKind(new Card(Card.Kind.diamonds, 13), new Card(Card.Kind.club, 1), LogKind.straightPossible1) &&
                TestLogKind(new Card(Card.Kind.diamonds, 11), new Card(Card.Kind.club, 13), LogKind.straightPossible2) &&
                TestLogKind(new Card(Card.Kind.diamonds, 1), new Card(Card.Kind.club, 12), LogKind.straightPossible2) &&
                TestLogKind(new Card(Card.Kind.diamonds, 1), new Card(Card.Kind.diamonds, 10), LogKind.sameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 2), new Card(Card.Kind.diamonds, 12), LogKind.sameKind) &&
                TestLogKind(new Card(Card.Kind.diamonds, 12), new Card(Card.Kind.club, 12), LogKind.samePoint) &&
                TestLogKind(new Card(Card.Kind.diamonds, 1), new Card(Card.Kind.club, 1), LogKind.samePoint) &&
                TestLogKind(new Card(Card.Kind.diamonds, 3), new Card(Card.Kind.club, 6), LogKind.other) &&
                TestLogKind(new Card(Card.Kind.diamonds, 9), new Card(Card.Kind.club, 6), LogKind.other) &&
                TestLogKind(new Card(Card.Kind.diamonds, 1), new Card(Card.Kind.club, 6), LogKind.other)
            )
            {
                return true;
            }
            Console.WriteLine("获取LogKind错误");
            return false;
        }

        private bool TestLogKind(Card card1, Card card2, LogKind logKind)
        {
            List<Card> twoCardList = new List<Card>() { card1, card2 };
            Card.SortTwoCardList(twoCardList);
            HandCard handCard = new HandCard(twoCardList);
            if( LogKindJudgement.GetLogKind(handCard) == logKind)
            {
                return true;
            }
            Console.Write(twoCardList[0]);
            Console.Write(twoCardList[1]);
            Console.Write(LogKindJudgement.GetLogKind(handCard));
            return false;
        }

    }
}
