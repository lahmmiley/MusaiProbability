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

        //多出两位方便测试首尾相连
        private static int[] _straightTestArray = new int[15];
        private static void ArraySetZero()
        {
            for(int i = 0; i <_straightTestArray.Length; i++)
            {
                _straightTestArray[i] = 0;
            }
        }

        public static bool IsStraightWithoutJoker(List<int> list)
        {
            ArraySetZero();

            for(int i = 0; i < list.Count; i++)
            {
                int point = list[i];
                _straightTestArray[point - 1] = 1;
                if(point == 1 || point == 2)
                {
                    _straightTestArray[point - 1 + Poker.PER_KIND_NUM] = 1;
                }
            }

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

        public static bool IsStraightWithOneJoker(List<int> list)
        {
            ArraySetZero();

            for(int i = 0; i < list.Count; i++)
            {
                int point = list[i];
                _straightTestArray[point - 1] = 1;
                if(point == 1 || point == 2)
                {
                    _straightTestArray[point - 1 + Poker.PER_KIND_NUM] = 1;
                }
            }

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
