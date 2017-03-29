using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public enum CardLevel
    {
        tianGongNine,//天公9;
        tianGongEight,//天公8;
        straightFlush,//同花顺
        sameNumOfThreeCard,//3张同点数
        straight,//顺子
        other,//其他
    }

    public class JudgeInfo
    {
        public CardLevel Level;
        public int OnesDigit;
    }

    public class HandCard
    {
        public string Key = string.Empty;
        public JudgeInfo twoCardJudgeInfo = new JudgeInfo();
        public JudgeInfo threeCardJudgeInfo = new JudgeInfo();

        private List<Card> _list = new List<Card>();

        public HandCard(Card first, Card second)
        {
            _list.Add(first);
            _list.Add(second);
            _list.Sort(Card.Sort);

            SetKey(_list);
            twoCardJudgeInfo = GenerateJudgeInfo();
        }

        private void SetKey(List<Card> list)
        {
            string key = string.Empty;
            for(int i = 0; i < list.Count; i++)
            {
                key += list[i].ToString() + " ";
            }
            this.Key = key;
        }

        public void AddCard(Card card)
        {
            _list.Add(card);
            threeCardJudgeInfo = GenerateJudgeInfo();
        }

        public JudgeInfo GenerateJudgeInfo()
        {
            JudgeInfo judgeInfo = new JudgeInfo();
            int totalNum = 0;
            for(int i = 0; i < _list.Count; i++)
            {
                Card card = _list[i];
                totalNum += card.CounterNumber;
            }
            judgeInfo.OnesDigit = totalNum % 10;

            judgeInfo.Level = CardLevel.other;
            if(_list.Count == 2)
            {
                if(judgeInfo.OnesDigit == 9)
                {
                    judgeInfo.Level = CardLevel.tianGongNine;
                }
                else if(judgeInfo.OnesDigit == 8)
                {
                    judgeInfo.Level = CardLevel.tianGongEight;
                }
            }
            else
            {
                if(IsStraight(_list))
                {
                    if(IsSameKind(_list))
                    {
                        judgeInfo.Level = CardLevel.straightFlush;
                    }
                    else
                    {
                        judgeInfo.Level = CardLevel.straight;
                    }
                }
                else if(IsSameNumber(_list))
                {
                    judgeInfo.Level = CardLevel.sameNumOfThreeCard;
                }
            }
            return judgeInfo;
        }

        private bool IsStraight(List<Card> cardList)
        {
            if(cardList.Count != 3)
            {
                return false;
            }
            List<int> list = new List<int>();
            for(int i = 0; i < cardList.Count; i++)
            {
                list.Add(cardList[i].Number);
            }
            list.Sort();

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

        private bool IsSameNumber(List<Card> cardList)
        {
            bool isSame = true;
            int number = cardList[0].Number;
            for(int i = 1; i < cardList.Count; i++)
            {
                if(cardList[i].Number != number)
                {
                    isSame = false;
                    break;
                }
            }
            return isSame;
        }

        private bool IsSameKind(List<Card> cardList)
        {
            bool isSame = true;
            Card.Kind kind = cardList[0].CardKind;
            for(int i = 1; i < cardList.Count; i++)
            {
                if(cardList[i].CardKind != kind)
                {
                    isSame = false;
                    break;
                }
            }
            return isSame;
        }
    }
}
