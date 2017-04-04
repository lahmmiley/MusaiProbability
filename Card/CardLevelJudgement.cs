using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class HandCardResult
    {
        public CardLevel Level;
        public int OnesDigit;
        public int Odds;

        public override string ToString()
        {
            return string.Format("level:" + Level.ToString() + "    onesDigit:" + OnesDigit + " odds:" + Odds);
        }
    }

    /// <summary>
    /// 用于判断最终的牌形和个位大小
    /// </summary>
    public class CardLevelJudgement
    {
        public class JudgeFunction
        {
            public CardLevel Level;
            public Func<List<Card>, bool> Fun;
            public int CardNumLimit;
            public int Odds;//赔率

            public JudgeFunction(CardLevel level, Func<List<Card>, bool> fun, int cardNumLimit, int odds)
            {
                this.Level = level;
                this.Fun = fun;
                this.CardNumLimit = cardNumLimit;
                this.Odds = odds;//初始赔率为0代表 需要重新计算
            }
        }

        private static List<JudgeFunction> _judgeList = new List<JudgeFunction>();

        static CardLevelJudgement()
        {
            _judgeList.Add(new JudgeFunction(CardLevel.twoJoker, IsTwoJoker, 2, 10));
            _judgeList.Add(new JudgeFunction(CardLevel.tianGongNine, IsTianGongNine, 2, 0));
            _judgeList.Add(new JudgeFunction(CardLevel.tianGongEight, IsTianGongEight, 2, 0));
            _judgeList.Add(new JudgeFunction(CardLevel.threeCardWithTwoJoker, IsThreeCardWithTwoJoker, 3, 8));
            _judgeList.Add(new JudgeFunction(CardLevel.straightFlush, IsStraightFlush, 3, 7));
            _judgeList.Add(new JudgeFunction(CardLevel.threeCardWithSamePoint, IsThreeCardWithSamePoint, 3, 5));
            _judgeList.Add(new JudgeFunction(CardLevel.straight, IsStraight, 3, 4));
            _judgeList.Add(new JudgeFunction(CardLevel.onesDigitIsZero, IsOnesDigitIsZero, 0, 0));
            _judgeList.Add(new JudgeFunction(CardLevel.other, IsNormal, 0, 0));
        }

        public static HandCardResult GetHandCardResult(List<Card> list)
        {
            if(IsTwoCard(list) && CardListJudgement.HaveJoker(list))//两张牌如果有一张是大小王则无效
            {
                return null;
            }
            HandCardResult result = new HandCardResult();
            for(int i = 0; i < _judgeList.Count; i++)
            {
                JudgeFunction judgeFunction = _judgeList[i];
                int cardNumLimit = judgeFunction.CardNumLimit;
                if((cardNumLimit != 0) && (cardNumLimit != list.Count))
                {
                    continue;
                }
                if(judgeFunction.Fun.Invoke(list))
                {
                    result.Level = judgeFunction.Level;
                    result.Odds = judgeFunction.Odds;
                    if(judgeFunction.Odds == 0)
                    {
                        result.Odds = CaculateOdds(judgeFunction.Level, list);
                    }
                    break;
                }
            }
            if(result.Level >= CardLevel.onesDigitIsZero)
            {
                result.OnesDigit = CardListJudgement.CalculateOnesDigit(list);
            }
            return result;
        }

        private static int CaculateOdds(CardLevel level, List<Card> list)
        {
            int result = 1;
            if(level == CardLevel.onesDigitIsZero)
            {
                result = 10;
            }
            if(CardListJudgement.IsSamePoint(list))
            {
                result = result * 2;
            }
            else if(CardListJudgement.IsSameKind(list))
            {
                result = result * list.Count;
            }

            return result;
        }

        private static bool IsTwoJoker(List<Card> list)
        {
            return CardListJudgement.IsTwoJoker(list);
        }

        private static bool IsTianGongNine(List<Card> list)
        {
            if(CardListJudgement.HaveJoker(list))
            {
                return false;
            }
            int onesDigit = CardListJudgement.CalculateOnesDigit(list);
            return onesDigit == 9;
        }

        private static bool IsTianGongEight(List<Card> list)
        {
            if(CardListJudgement.HaveJoker(list))
            {
                return false;
            }
            int onesDigit = CardListJudgement.CalculateOnesDigit(list);
            return onesDigit == 8;
        }

        private static bool IsThreeCardWithTwoJoker(List<Card> list)
        {
            int jokerCount = CardListJudgement.GetJokerCount(list);
            return jokerCount == 2;
        }

        private static bool IsStraightFlush(List<Card> list)
        {
            return IsStraight(list) && CardListJudgement.IsSameKind(list);
        }

        private static bool IsThreeCardWithSamePoint(List<Card> list)
        {
            return CardListJudgement.IsSamePoint(list);
        }

        private static List<int> _pointList = new List<int>();
        private static bool IsStraight(List<Card> cardList)
        {
            _pointList.Clear();
            for(int i = 0; i < cardList.Count; i++)
            {
                Card card = cardList[i];
                if(!card.IsJoker())
                {
                    _pointList.Add(cardList[i].Point);
                }
            }

            if(CardListJudgement.HaveJoker(cardList))
            {
                return CardListJudgement.IsStraightWithOneJoker(_pointList);
            }
            else
            {
                return CardListJudgement.IsStraightWithoutJoker(_pointList);
            }
        }

        private static bool IsOnesDigitIsZero(List<Card> list)
        {
            if(CardListJudgement.HaveJoker(list))
            {
                return false;
            }
            int onesDigit = CardListJudgement.CalculateOnesDigit(list);
            return onesDigit == 0;
        }

        private static bool IsNormal(List<Card> list)
        {
            return true;
        }

        private static bool IsTwoCard(List<Card> list)
        {
            return list.Count == 2;
        }
    }
}
