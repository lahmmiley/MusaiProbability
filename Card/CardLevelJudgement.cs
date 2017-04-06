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
            private bool _twoJokerJudged = false;
            private bool _isTwoJoker = false;
            private bool _onesDigitJudged = false;
            private int _onesDigit = 0;
            private bool _straightJudged = false;
            private bool _isStraight = false;
            private bool _sameKindJudged = false;
            private bool _isSameKind = false;
            private bool _samePointJudged = false;
            private bool _isSamePoint = false;
            private bool _jokerCountJudged = false;
            private int _jokerCount = 0;

            public JudgeParamWrapper(List<Card> cardList)
            {
                this.CardList = cardList;
                _twoJokerJudged = true;
                _jokerCountJudged = true;
                _onesDigitJudged = true;
                _samePointJudged = true;
                _sameKindJudged = true;
                spForCalOnesDigit.Start();
                CardListJudgement.CalculateBaseProperty(cardList, ref _isTwoJoker, ref _jokerCount, ref _onesDigit, ref _isSamePoint, ref _isSameKind);
                spForCalOnesDigit.Stop();
            }

            public bool IsTwoJoker
            {
                get
                {
                    if(_twoJokerJudged == false)
                    {
                        _twoJokerJudged = true;
                        //spForTwoJoker.Start();
                        _isTwoJoker = CardListJudgement.IsTwoJoker(CardList);
                        //spForTwoJoker.Stop();
                    }
                    return _isTwoJoker;
                }
            }

            public int OnesDigit
            {
                get
                {
                    if(_onesDigitJudged == false)
                    {
                        _onesDigitJudged = true;
                        //spForOnesDigit.Start();
                        _onesDigit = CardListJudgement.CalculateOnesDigit(CardList);
                        //spForOnesDigit.Stop();
                    }
                    return _onesDigit;
                }
            }

            public int JokerCount
            {
                get
                {
                    if(_jokerCountJudged == false)
                    {
                        _jokerCountJudged = true;
                        _jokerCount = CardListJudgement.GetJokerCount(CardList);
                    }
                    return _jokerCount;
                }
            }

            public bool IsStraight
            {
                get
                {
                    if(_straightJudged == false)
                    {
                        _straightJudged = true;
                        spForStraight.Start();
                        _isStraight = CardListJudgement.IsStraight(CardList);
                        spForStraight.Stop();
                    }
                    return _isStraight;
                }
            }

            public bool IsSamePoint
            {
                get
                {
                    if(_samePointJudged == false)
                    {
                        _samePointJudged = true;
                        //spForSamePoint.Start();
                        _isSamePoint = CardListJudgement.IsSamePoint(CardList);
                        //spForSamePoint.Stop();
                    }
                    return _isSamePoint;
                }
            }

            public bool IsSameKind
            {
                get
                {
                    if(_sameKindJudged == false)
                    {
                        _sameKindJudged = true;
                        //spForSameKind.Start();
                        _isSameKind = CardListJudgement.IsSameKind(CardList);
                        //spForSameKind.Stop();
                    }
                    return _isSameKind;
                }
            }
        }

        public static Stopwatch sp = new Stopwatch();
        public static Stopwatch spForStraight = new Stopwatch();
        public static Stopwatch spForOnesDigit = new Stopwatch();
        public static Stopwatch spForTwoJoker = new Stopwatch();
        public static Stopwatch spForSameKind = new Stopwatch();
        public static Stopwatch spForSamePoint = new Stopwatch();
        public static Stopwatch spForOdds = new Stopwatch();
        public static Stopwatch spForCalOnesDigit = new Stopwatch();
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
            sp.Start();
            if(IsTwoCard(list) && CardListJudgement.GetJokerCount(list) == 1)//两张牌如果有一张是大小王则无效
            {
                sp.Stop();
                return null;
            }
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
            if(result.Level >= CardLevel.onesDigitIsZero)
            {
                //spForCalOnesDigit.Start();
                result.OnesDigit = paramWrapper.OnesDigit;
                //spForCalOnesDigit.Stop();
            }
            sp.Stop();
            return result;
        }

        private static int CaculateOdds(CardLevel level, JudgeParamWrapper paramWrapper)
        {
            //spForOdds.Start();
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

            //spForOdds.Stop();
            return result;
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
            return wrapper.IsStraight && wrapper.IsSameKind;
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

        private static bool IsTwoCard(List<Card> list)
        {
            return list.Count == 2;
        }
    }
}
