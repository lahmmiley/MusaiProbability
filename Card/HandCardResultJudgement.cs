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
    }

    public class HandCardResultJudgement
    {
        public class JudgeFunction
        {
            public CardLevel Level;
            public Func<List<Card>, bool> Fun;

            public JudgeFunction(CardLevel level, Func<List<Card>, bool> fun)
            {
                this.Level = level;
                this.Fun = fun;
            }
        }

        public static List<JudgeFunction> _judgeList = new List<JudgeFunction>();

        static HandCardResultJudgement()
        {
            _judgeList.Add(new JudgeFunction(CardLevel.twoJoker, IsTwoJoker));
            _judgeList.Add(new JudgeFunction(CardLevel.tianGongNine, IsTianGongNine));
            _judgeList.Add(new JudgeFunction(CardLevel.tianGongEight, IsTianGongEight));
            _judgeList.Add(new JudgeFunction(CardLevel.threeCardWithTwoJoker, IsThreeCardWithTwoJoker));
            _judgeList.Add(new JudgeFunction(CardLevel.straightFlush, IsStraightFlush));
            _judgeList.Add(new JudgeFunction(CardLevel.threeCardWithSamePoint, IsThreeCardWithSamePoint));
            _judgeList.Add(new JudgeFunction(CardLevel.straight, IsStraight));
            _judgeList.Add(new JudgeFunction(CardLevel.threeCardWithOneJoker, IsThreeCardWithOneJoker));
            _judgeList.Add(new JudgeFunction(CardLevel.threeCardWithSameKind, IsThreeCardWithSameKind));
            _judgeList.Add(new JudgeFunction(CardLevel.twoCardWithSameKind, IsTwoCardWithSameKind));
            _judgeList.Add(new JudgeFunction(CardLevel.twoCardWithSamePoint, IsTwoCardWithSamePoint));
            _judgeList.Add(new JudgeFunction(CardLevel.onesDigitIsZero, IsOnesDigitIsZero));
            _judgeList.Add(new JudgeFunction(CardLevel.other, IsNormal));
        }

        public static HandCardResult GetHandCardResult(List<Card> list)
        {
            if(IsTwoCard(list) && GetJokerCount(list) == 1)
            {
                //两张牌如果有一张是大小王则无效
                return null;
            }
            HandCardResult result = new HandCardResult();
            for(int i = 0; i < _judgeList.Count; i++)
            {
                JudgeFunction judgeFunction = _judgeList[i];
                if(judgeFunction.Fun.Invoke(list))
                {
                    result.Level = judgeFunction.Level;
                    break;
                }
            }
            if((int)result.Level >= (int)CardLevel.threeCardWithOneJoker)
            {
                result.OnesDigit = CalculateOnesDigit(list);
            }
            return result;
        }

        private static bool IsTwoJoker(List<Card> list)
        {
            if(!IsTwoCard(list))
            {
                return false;
            }

            for(int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if(!card.IsJoker())
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsTianGongNine(List<Card> list)
        {
            if(!IsTwoCard(list))
            {
                return false;
            }
            if(HaveJoker(list))
            {
                return false;
            }
            int onesDigit = CalculateOnesDigit(list);
            return onesDigit == 9;
        }

        private static bool IsTianGongEight(List<Card> list)
        {
            if(!IsTwoCard(list))
            {
                return false;
            }
            if(HaveJoker(list))
            {
                return false;
            }
            int onesDigit = CalculateOnesDigit(list);
            return onesDigit == 8;
        }

        private static bool IsThreeCardWithTwoJoker(List<Card> list)
        {
            if(!IsThreeCard(list))
            {
                return false;
            }
            int jokerCount = GetJokerCount(list);
            return jokerCount == 2;
        }

        private static bool IsStraightFlush(List<Card> list)
        {
            if(!IsThreeCard(list))
            {
                return false;
            }

            return IsStraight(list) && IsSameKind(list);
        }

        private static bool IsThreeCardWithSamePoint(List<Card> list)
        {
            if(!IsThreeCard(list))
            {
                return false;
            }
            return IsSamePoint(list);
        }

        //TODO 判断优化
        private static bool IsStraight(List<Card> cardList)
        {
            if(!IsThreeCard(cardList))
            {
                return false;
            }

            List<int> pointList = new List<int>();
            for(int i = 0; i < cardList.Count; i++)
            {
                Card card = cardList[i];
                if(!card.IsJoker())
                {
                    pointList.Add(cardList[i].Point);
                }
            }
            pointList.Sort(IncreaseSort);

            if(HaveJoker(cardList))
            {
                return IsStraightWithOneJoker(pointList);
            }
            else
            {
                return IsStraightWithoutJoker(pointList);
            }
        }

        private static bool IsThreeCardWithOneJoker(List<Card> list)
        {
            if(!IsThreeCard(list))
            {
                return false;
            }

            return GetJokerCount(list) == 1;
        }

        private static bool IsThreeCardWithSameKind(List<Card> list)
        {
            if(!IsThreeCard(list))
            {
                return false;
            }
            return IsSameKind(list);
        }

        private static bool IsTwoCardWithSameKind(List<Card> list)
        {
            if(!IsTwoCard(list))
            {
                return false;
            }
            return IsSameKind(list);
        }

        private static bool IsTwoCardWithSamePoint(List<Card> list)
        {
            if(!IsTwoCard(list))
            {
                return false;
            }
            return IsSamePoint(list);
        }

        private static bool IsOnesDigitIsZero(List<Card> list)
        {
            int onesDigit = CalculateOnesDigit(list);
            return onesDigit == 0;
        }

        private static bool IsNormal(List<Card> list)
        {
            return true;
        }

        private static bool IsStraightWithoutJoker(List<int> list)
        {
            if( ((list[1] - list[0]) == 1) && 
                ((list[2] - list[1]) == 1) )
            {
                return true;
            }
            if((list[0] == 1) &&
                (list[1] == 12) &&
                (list[2] == 13))
            {
                return true;
            }

            if((list[0] == 1) &&
                (list[1] == 2) &&
                (list[2] == 13))
            {
                return true;
            }

            return false;
        }

        private static bool IsStraightWithOneJoker(List<int> list)
        {
            if( ((list[1] - list[0]) == 2)
                || ((list[1] - list[0]) == 1))
            {
                return true;
            }

            if((list[0] == 1) && (list[1] == 13))
            {
                return true;
            }

            if((list[0] == 1) && (list[1] == 12))
            {
                return true;
            }

            if((list[0] == 2) && (list[1] == 13))
            {
                return true;
            }

            return false;
        }
        
        private static bool IsSamePoint(List<Card> cardList)
        {
            int number = -1;
            for(int i = 0; i < cardList.Count; i++)
            {
                Card card = cardList[i];
                if(card.IsJoker())
                {
                    continue;
                }
                if(number == -1)
                {
                    number = card.Point;
                }
                else if(number != card.Point)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsSameKind(List<Card> cardList)
        {
            Card.Kind kind = Card.Kind.invalid;
            for(int i = 0; i < cardList.Count; i++)
            {
                Card card = cardList[i];
                if(card.IsJoker())
                {
                    continue;
                }
                if(kind == Card.Kind.invalid)
                {
                    kind = card.CardKind;
                }
                else if(kind != card.CardKind)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsTwoCard(List<Card> list)
        {
            return list.Count == 2;
        }

        private static bool IsThreeCard(List<Card> list)
        {
            return list.Count == 3;
        }

        private static bool HaveJoker(List<Card> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if(card.IsJoker())
                {
                    return true;
                }
            }
            return false;
        }

        private static int GetJokerCount(List<Card> list)
        {
            int count = 0;
            for(int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if(card.IsJoker())
                {
                    count += 1;
                }
            }
            return count;
        }

        private static int CalculateOnesDigit(List<Card> list)
        {
            int onesDigit = 0;
            for(int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if(card.IsJoker())
                {
                    //有大小王的牌至少9点
                    return 9;
                }
                onesDigit += card.CounterPoint;
            }
            return onesDigit % 10;
        }

        private static int IncreaseSort(int x, int y)
        {
            if(x < y)
            {
                return -1;
            }
            return 1;
        }
    }
}
