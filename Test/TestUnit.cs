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
                TestResult(new Card(Card.Kind.redJoker, -1), new Card(Card.Kind.blackJoker, -1), 
                    CardLevel.twoJoker, 10, 9) &&
                TestResult(new Card(Card.Kind.redJoker, -1), new Card(Card.Kind.blackJoker, -1), 
                    CardLevel.twoJoker, 10, 9) &&
                TestResult(new Card(Card.Kind.redJoker, -1), new Card(Card.Kind.blackJoker, -1), 
                    CardLevel.twoJoker, 10, 9)
                )
            {
                return true;
            }
            return false;
        }

        private bool TestResult(Card cardA, Card cardB, CardLevel level, int odds, int onesDigit)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(cardA);
            cardList.Add(cardB);
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == level) && 
                (result.Odds == odds) && 
                (result.OnesDigit == onesDigit))
            {
                return true;
            }
            return false;
        }

        private bool TestResult(Card cardA, Card cardB, Card cardC, CardLevel level, int odds, int onesDigit)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(cardA);
            cardList.Add(cardB);
            cardList.Add(cardC);
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == level) && 
                (result.Odds == odds) && 
                (result.OnesDigit == onesDigit))
            {
                return true;
            }
            return false;
        }

        private bool TestResultTwoJoker()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.redJoker, -1));
            cardList.Add(new Card(Card.Kind.blackJoker, -1));
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == CardLevel.twoJoker) && (result.Odds == 10))
            {
                return true;
            }
            return false;
        }

        private bool TestTianGongNine()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.diamonds, 10));
            cardList.Add(new Card(Card.Kind.hearts, 9));
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == CardLevel.tianGongNine) &&
                (result.Odds == 1) &&
                (result.OnesDigit == 9))
            {
                return false;
            }
            return true;
        }

        private bool TestTianGongEight()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.spades, 8));
            cardList.Add(new Card(Card.Kind.spades, 9));
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == CardLevel.tianGongEight) &&
                (result.Odds == 2) &&
                (result.OnesDigit == 8))
            {
                return false;
            }
            return true;
        }

        private bool TestResultThreeCardWithSamePoint1()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.diamonds, 1));
            cardList.Add(new Card(Card.Kind.redJoker, -1));
            cardList.Add(new Card(Card.Kind.blackJoker, -1));
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == CardLevel.threeCardWithTwoJoker) && (result.Odds == 8))
            {
                return true;
            }
            return false;
        }

        private bool TestResultThreeCardWithSamePoint2()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.redJoker, -1));
            cardList.Add(new Card(Card.Kind.hearts, 9));
            cardList.Add(new Card(Card.Kind.blackJoker, -1));
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == CardLevel.threeCardWithTwoJoker) && (result.Odds == 8))
            {
                return true;
            }
            return false;
        }
        
        private bool TestResultThreeCardWithSamePoint2()
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(new Card(Card.Kind.redJoker, -1));
            cardList.Add(new Card(Card.Kind.hearts, 9));
            cardList.Add(new Card(Card.Kind.blackJoker, -1));
            HandCardResult result = CardLevelJudgement.GetHandCardResult(cardList);
            if((result.Level == CardLevel.threeCardWithTwoJoker) && (result.Odds == 8))
            {
                return true;
            }
            return false;
        }
    }
}
