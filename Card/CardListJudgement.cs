using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    /// <summary>
    /// 判断牌形的通用接口
    /// </summary>
    public class CardListJudgement
    {
        //性能优化，先计算常用的属性，避免多次遍历
        public static void CalculateBaseProperty(List<Card> list, ref bool isTwoJoker, ref int jokerCount, 
            ref int onesDigit, ref bool isSamePoint, ref bool isSameKind)
        {
            int sum = 0;
            isSamePoint = true;
            int point = int.MinValue;
            isSameKind = true;
            Card.Kind kind = Card.Kind.invalid;

            for(int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if (card.IsJoker())
                {
                    jokerCount += 1;
                }
                else
                {
                    sum += card.CounterPoint;
                    if(isSamePoint)
                    {
                        if(point == int.MinValue)
                        {
                            point = card.Point;
                        }
                        else if(point != card.Point)
                        {
                            isSamePoint = false;
                        }
                    }
                    if(isSameKind)
                    {
                        if (kind == Card.Kind.invalid)
                        {
                            kind = card.CardKind;
                        }
                        else if (kind != card.CardKind)
                        {
                            isSameKind = false;
                        }
                    }
                }
            }

            isTwoJoker = (jokerCount == 2) && (list.Count == 2);
            onesDigit = (jokerCount > 0) ? 9 : sum % 10;
            
        }

        public static bool IsTwoJoker(List<Card> list)
        {
            return  list.Count == 2 && GetJokerCount(list) == 2;
        }

        public static int GetJokerCount(List<Card> list)
        {
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if (card.IsJoker())
                {
                    count += 1;
                }
            }
            return count;
        }

        public static bool HaveJoker(List<Card> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if (card.IsJoker())
                {
                    return true;
                }
            }
            return false;
        }

        public static int CalculateOnesDigit(List<Card> list)
        {
            int onesDigit = 0;
            for (int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if (card.IsJoker())
                {
                    //有大小王的牌至少9点
                    return 9;
                }
                onesDigit += card.CounterPoint;
            }
            return onesDigit % 10;
        }

        public static bool IsSamePoint(List<Card> cardList)
        {
            int number = int.MinValue;
            for (int i = 0; i < cardList.Count; i++)
            {
                Card card = cardList[i];
                if (card.IsJoker())
                {
                    continue;
                }
                if (number == int.MinValue)
                {
                    number = card.Point;
                }
                else if (number != card.Point)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsSameKind(List<Card> cardList)
        {
            Card.Kind kind = Card.Kind.invalid;
            for (int i = 0; i < cardList.Count; i++)
            {
                Card card = cardList[i];
                if (card.IsJoker())
                {
                    continue;
                }
                if (kind == Card.Kind.invalid)
                {
                    kind = card.CardKind;
                }
                else if (kind != card.CardKind)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<int> _pointList = new List<int>();
        public static bool IsStraight(List<Card> list)
        {
            _pointList.Clear();
            for(int i = 0; i < list.Count; i++)
            {
                Card card = list[i];
                if(!card.IsJoker())
                {
                    _pointList.Add(list[i].Point);
                }
            }

            if(CardListJudgement.HaveJoker(list))
            {
                return CardListJudgement.IsStraightWithOneJoker(_pointList);
            }
            else
            {
                return CardListJudgement.IsStraightWithoutJoker(_pointList);
            }
        }

        //多出两位方便测试首尾相连
        private static int[] _straightTestArray = new int[15];
        private static void ArraySetZero()
        {
            for(int i = 0; i <_straightTestArray.Length; i++)
            {
                _straightTestArray[i] = 0;
            }
        }

        private static void IndexSetIntoArray(List<int> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                int point = list[i];
                _straightTestArray[point - 1] = 1;
                if(point == 1 || point == 2)
                {
                    _straightTestArray[point - 1 + Poker.PER_KIND_NUM] = 1;
                }
            }
        }

        private static bool IsStraightWithoutJoker(List<int> list)
        {
            ArraySetZero();
            IndexSetIntoArray(list);

            int continuout = 0;
            for(int i = 0; i < _straightTestArray.Length; i++)
            {
                if(_straightTestArray[i] == 1)
                {
                    continuout += 1;
                    if(continuout == 3)
                    {
                        return true;
                    }
                }
                else
                {
                    continuout = 0;
                }
            }
            return false;
        }

        
        private static bool IsStraightWithOneJoker(List<int> list)
        {
            ArraySetZero();
            IndexSetIntoArray(list);

            for(int i = 0; i < _straightTestArray.Length - 2; i++)
            {
                if (_straightTestArray[i] == 1)
                {
                    if((_straightTestArray[i + 1] == 1) ||
                            (_straightTestArray[i + 2] == 1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
