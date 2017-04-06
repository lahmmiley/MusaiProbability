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
            public Func<JudgeParamWrapper, bool> Fun;
            public int CardNumLimit;
            public int Odds;//赔率

            public JudgeFunction(CardLevel level, Func<JudgeParamWrapper, bool> fun, int cardNumLimit, int odds)
            {
                this.Level = level;
                this.Fun = fun;
                this.CardNumLimit = cardNumLimit;
                this.Odds = odds;//初始赔率为0代表 需要重新计算
            }
        }

        public class JudgeParamWrapper
        {
            public List<Card> CardList = new List<Card>();
            public bool IsTwoJoker = false;
            public int JokerCount = 0;
            public int OnesDigit = 0;
            public bool IsStraight = false;
            public bool IsSameKind = false;
            public bool IsSamePoint = false;
            public bool IsInvalid = false;

            public JudgeParamWrapper(List<Card> cardList)
            {
                this.CardList = cardList;
                CardListJudgement.SetAllProperty(cardList, ref IsInvalid, ref IsTwoJoker, ref JokerCount, 
                    ref OnesDigit, ref IsSamePoint, ref IsSameKind, ref IsStraight);
            }
        }

        public static Stopwatch sp = new Stopwatch();
        public static Stopwatch spForOdds = new Stopwatch();
        private static List<JudgeFunction> _judgeList = new List<JudgeFunction>();

        static CardLevelJudgement()
        {
            _judgeList.Add(new JudgeFunction(CardLevel.invalid, IsInvalid, 2, -1));
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
            sp.Start();
            HandCardResult result = new HandCardResult();
            JudgeParamWrapper paramWrapper = new JudgeParamWrapper(list);
            for(int i = 0; i < _judgeList.Count; i++)
            {
                JudgeFunction judgeFunction = _judgeList[i];
                int cardNumLimit = judgeFunction.CardNumLimit;
                if((cardNumLimit != 0) && (cardNumLimit != list.Count))
                {
                    continue;
                }
                if(judgeFunction.Fun.Invoke(paramWrapper))
                {
                    result.Level = judgeFunction.Level;
                    result.Odds = judgeFunction.Odds;
                    if(judgeFunction.Odds == 0)
                    {
                        result.Odds = CaculateOdds(judgeFunction.Level, paramWrapper);
                    }
                    break;
                }
            }

            // list.Count == 2是因为获取hash时需要OnesDigit
            if((list.Count == 2) || (result.Level >= CardLevel.onesDigitIsZero))
            {
                result.OnesDigit = paramWrapper.OnesDigit;
            }
            sp.Stop();
            return result;
        }

        private static int CaculateOdds(CardLevel level, JudgeParamWrapper paramWrapper)
        {
            int result = 1;
            if(level == CardLevel.onesDigitIsZero)
            {
                result = 10;
            }
            if(paramWrapper.IsSamePoint)
            {
                result = result * 2;
            }
            else if(paramWrapper.IsSameKind)
            {
                result = result * paramWrapper.CardList.Count;
            }

            return result;
        }

        private static bool IsInvalid(JudgeParamWrapper wrapper)
        {
            return wrapper.IsInvalid;
        }

        private static bool IsTwoJoker(JudgeParamWrapper wrapper)
        {
            return wrapper.IsTwoJoker;
        }

        private static bool IsTianGongNine(JudgeParamWrapper wrapper)
        {
            if(wrapper.JokerCount != 0)
            {
                return false;
            }
            return wrapper.OnesDigit == 9;
        }

        private static bool IsTianGongEight(JudgeParamWrapper wrapper)
        {
            if(wrapper.JokerCount != 0)
            {
                return false;
            }
            return wrapper.OnesDigit == 8;
        }

        private static bool IsThreeCardWithTwoJoker(JudgeParamWrapper wrapper)
        {
            return wrapper.JokerCount == 2;
        }

        private static bool IsStraightFlush(JudgeParamWrapper wrapper)
        {
            return wrapper.IsSameKind && wrapper.IsStraight;
        }

        private static bool IsThreeCardWithSamePoint(JudgeParamWrapper wrapper)
        {
            return wrapper.IsSamePoint;
        }

        private static bool IsStraight(JudgeParamWrapper wrapper)
        {
            return wrapper.IsStraight;
        }

        private static bool IsOnesDigitIsZero(JudgeParamWrapper wrapper)
        {
            if(wrapper.JokerCount != 0)
            {
                return false;
            }
            return wrapper.OnesDigit == 0;
        }

        private static bool IsNormal(JudgeParamWrapper wrappert)
        {
            return true;
        }
    }
}
