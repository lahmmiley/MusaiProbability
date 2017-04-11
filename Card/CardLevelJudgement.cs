using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    /// <summary>
    /// 用于判断最终的牌形和个位大小
    /// </summary>
    public class CardLevelJudgement
    {
        public class JudgeFunction
        {
            public CardLevel Level;
            public Func<HandCard, bool> Fun;
            public int CardNumLimit;
            public int Odds;//赔率

            public JudgeFunction(CardLevel level, Func<HandCard, bool> fun, int cardNumLimit, int odds)
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
            _judgeList.Add(new JudgeFunction(CardLevel.invalid, IsInvalid, 2, -1));
            _judgeList.Add(new JudgeFunction(CardLevel.threeJoker, IsThreeJoker, 3, 15));
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

        public static HandCardResult GetHandCardResult(HandCard handCard)
        {
            HandCardResult result = new HandCardResult();
            for(int i = 0; i < _judgeList.Count; i++)
            {
                JudgeFunction judgeFunction = _judgeList[i];
                int cardNumLimit = judgeFunction.CardNumLimit;
                if((cardNumLimit != 0) && (cardNumLimit != handCard.Count))
                {
                    continue;
                }
                if(judgeFunction.Fun.Invoke(handCard))
                {
                    result.Level = judgeFunction.Level;
                    result.Odds = judgeFunction.Odds;
                    if(judgeFunction.Odds == 0)
                    {
                        result.Odds = CaculateOdds(judgeFunction.Level, handCard);
                    }
                    break;
                }
            }

            // list.Count == 2是因为获取hash时需要OnesDigit
            if((handCard.Count == 2) || (result.Level >= CardLevel.onesDigitIsZero))
            {
                result.OnesDigit = handCard.OnesDigit;
            }
            return result;
        }

        private static int CaculateOdds(CardLevel level, HandCard handCard)
        {
            int result = 1;
            if(level == CardLevel.onesDigitIsZero)
            {
                result = 10;
            }
            if(handCard.IsSamePoint)
            {
                result = result * 2;
            }
            else if(handCard.IsSameKind)
            {
                result = result * handCard.Count;
            }

            return result;
        }

        private static bool IsInvalid(HandCard handCard)
        {
            return handCard.IsInvalid;
        }

        private static bool IsThreeJoker(HandCard handCard)
        {
            return handCard.IsThreeJoker;
        }

        private static bool IsTwoJoker(HandCard handCard)
        {
            return handCard.IsTwoJoker;
        }

        private static bool IsTianGongNine(HandCard handCard)
        {
            if(handCard.JokerCount != 0)
            {
                return false;
            }
            return handCard.OnesDigit == 9;
        }

        private static bool IsTianGongEight(HandCard handCard)
        {
            if(handCard.JokerCount != 0)
            {
                return false;
            }
            return handCard.OnesDigit == 8;
        }

        private static bool IsThreeCardWithTwoJoker(HandCard handCard)
        {
            return handCard.JokerCount == 2;
        }

        private static bool IsStraightFlush(HandCard handCard)
        {
            return handCard.IsSameKind && handCard.IsStraight;
        }

        private static bool IsThreeCardWithSamePoint(HandCard handCard)
        {
            return handCard.IsSamePoint;
        }

        private static bool IsStraight(HandCard handCard)
        {
            return handCard.IsStraight;
        }

        private static bool IsOnesDigitIsZero(HandCard handCard)
        {
            if(handCard.JokerCount != 0)
            {
                return false;
            }
            return handCard.OnesDigit == 0;
        }

        private static bool IsNormal(HandCard handCardt)
        {
            return true;
        }
    }
}
