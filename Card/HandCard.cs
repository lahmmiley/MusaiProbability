using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class HandCard
    {
        public bool IsTwoJoker = false;
        public int JokerCount = 0;
        public int OnesDigit = 0;
        public bool IsStraight = false;
        public bool IsSameKind = true;
        public bool IsSamePoint = true;
        public bool IsInvalid = false;

        private bool _straightPossible1Judged = false;
        private bool _isStraightPossible1 = false;
        private bool _straightPossible2Judged = false;
        private bool _isStraightPossible2 = false;
        private string _logType = string.Empty;
        private List<Card> _cardList;

        //测试用
        public HandCard(Card a, Card b)
        {
            _cardList = new List<Card>();
            _cardList.Add(a);
            _cardList.Add(b);
            _cardList.Sort(Card.Sort);

            InitJudgeCondition();
        }

        public HandCard(Card a, Card b, Card c)
        {
            _cardList = new List<Card>();
            _cardList.Add(a);
            _cardList.Add(b);
            _cardList.Add(c);
            _cardList.Sort(Card.Sort);

            InitJudgeCondition();
        }

        public HandCard(List<Card> list)
        {
            _cardList = list;
            //传入的List有序
            InitJudgeCondition();
        }

        public string LogType
        {
            get
            {
                if(_logType == string.Empty)
                {
                    _logType = GetLogType();
                }
                return _logType;
            }
        }

        public bool IsStraightPossible1
        {
            get
            {
                if(_straightPossible1Judged == false)
                {
                    _straightPossible1Judged = true;
                    _isStraightPossible1 = JudgeStraightPossible1();
                }
                return _isStraightPossible1;
            }
        }

        public bool IsStraightPossible2
        {
            get
            {
                if(_straightPossible2Judged == false)
                {
                    _straightPossible2Judged = true;
                    _isStraightPossible2 = JudgeStraightPossible2();
                }
                return _isStraightPossible2;
            }
        }

        public int Count
        {
            get
            {
                return _cardList.Count;
            }
        }

        public void InitJudgeCondition()
        {
            int sum = 0;
            int point = int.MinValue;
            Card.Kind kind = Card.Kind.invalid;
            List<int> pointList = new List<int>();

            for (int i = 0; i < _cardList.Count; i++)
            {
                Card card = _cardList[i];
                if (card.IsJoker())
                {
                    JokerCount += 1;
                }
                else
                {
                    sum += card.CounterPoint;
                    pointList.Add(card.Point);
                    JudgeSamePoint(card, ref point, ref IsSamePoint);
                    JudgeSameKind(card, ref kind, ref IsSameKind);
                }
            }

            IsInvalid = (JokerCount == 1) && (_cardList.Count == 2);
            IsTwoJoker = (JokerCount == 2) && (_cardList.Count == 2);
            OnesDigit = (JokerCount > 0) ? 9 : sum % 10;
            JudgeStraight(JokerCount, pointList, ref IsStraight);
        }

        private string GetLogType()
        {
            LogKind logKind = LogKindJudgement.GetLogKind(this);
            if(logKind == LogKind.twoJoker)
            {
                return "双王";
            }
            else if(logKind ==LogKind.oneJoker)
            {
                return "单王";
            }
            else
            {
                return string.Format("个位:{0} 牌型:{1}", OnesDigit.ToString(), ((int)logKind).ToString());
            }
        }

        private static void JudgeSamePoint(Card card, ref int point, ref bool isSamePoint)
        {
            if (isSamePoint)
            {
                if (point == int.MinValue)
                {
                    point = card.Point;
                }
                else if (point != card.Point)
                {
                    isSamePoint = false;
                }
            }
        }

        private void JudgeSameKind(Card card, ref Card.Kind kind, ref bool isSameKind)
        {
            if (isSameKind)
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

        private void JudgeStraight(int jokerCount, List<int> pointList, ref bool isStraight)
        {
            if (jokerCount > 0)
            {
                isStraight = IsStraightWithOneJoker(pointList);
            }
            else
            {
                isStraight = IsStraightWithoutJoker(pointList);
            }
        }

        //多出两位方便测试首尾相连
        private static int[] _straightTestArray = new int[15];
        private void ArraySetZero()
        {
            for (int i = 0; i < _straightTestArray.Length; i++)
            {
                _straightTestArray[i] = 0;
            }
        }

        private void IndexSetIntoArray(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int point = list[i];
                _straightTestArray[point - 1] = 1;
                if (point == 1 || point == 2)
                {
                    _straightTestArray[point - 1 + Poker.PER_KIND_NUM] = 1;
                }
            }
        }

        private bool IsStraightWithoutJoker(List<int> list)
        {
            ArraySetZero();
            IndexSetIntoArray(list);

            int continuout = 0;
            for (int i = 0; i < _straightTestArray.Length; i++)
            {
                if (_straightTestArray[i] == 1)
                {
                    continuout += 1;
                    if (continuout == 3)
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


        private bool IsStraightWithOneJoker(List<int> list)
        {
            ArraySetZero();
            IndexSetIntoArray(list);

            for (int i = 0; i < _straightTestArray.Length - 2; i++)
            {
                if (_straightTestArray[i] == 1)
                {
                    if ((_straightTestArray[i + 1] == 1) ||
                            (_straightTestArray[i + 2] == 1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //两张牌相差一点
        private bool JudgeStraightPossible1()
        {
            Card a = _cardList[0];
            Card b = _cardList[1];
            if(Math.Abs(a.Point - b.Point) == 1)
            {
                return true;
            }
            if(a.Point == 1 && b.Point == 13)
            {
                return true;
            }
            return false;
        }

        //两张牌相差两点
        private bool JudgeStraightPossible2()
        {
            Card a = _cardList[0];
            Card b = _cardList[1];
            if(Math.Abs(a.Point - b.Point) == 2)
            {
                return true;
            }
            if((a.Point == 1 && b.Point == 12)
                || (a.Point == 2 && b.Point == 13))
            {
                return true;
            }
            return false;
        }
    }
}
