using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace Musai
{
    public class Card
    {
        public enum Kind
        {
            hearts,     //红桃
            spades,     //黑桃
            diamonds,   //方片
            club,       //梅花
            redJoker,   //大王
            blackJoker, //小王
            invalid,    //无效
        }

        public int Point;
        public int CounterPoint;//J、Q、K计算时算10点
        public Kind CardKind;

        public Card(Kind kind, int point = -1)
        {
            this.CardKind = kind;
            this.Point = point;
            this.CounterPoint = point;
            if(point > 10)
            {
                this.CounterPoint = 10;
            }
        }

        public override int GetHashCode()
        {
            //从10开始，保证一定是两位数
            if(CardKind <= Kind.club)
            {
                return 10 + (int)CardKind + Point * 4;
            }
            else
            {
                return 70 + (int)CardKind;
            }
        }

        public override string ToString()
        {
            return ((int)CardKind).ToString() + "_" + Point.ToString();
        }

        public bool IsJoker()
        {
            return CardKind == Kind.redJoker || CardKind == Kind.blackJoker;
        }

        //从小到大
        public static void SortTwoCardList(List<Card> list)
        {
            int a = list[0].GetHashCode();
            int b = list[1].GetHashCode();
            if(a > b)
            {
                Card temp = list[0];
                list[0] = list[1];
                list[1] = temp;
            }
        }

        //list已经有序
        public static void InsertTwoCardList(List<Card> twoCardList, Card card)
        {
            int hashCode = card.GetHashCode();
            if(hashCode < twoCardList[0].GetHashCode())
            {
                twoCardList.Insert(0, card);
            }
            else
            {
                if(hashCode < twoCardList[1].GetHashCode())
                {
                    twoCardList.Insert(1, card);
                }
                else
                {
                    twoCardList.Add(card);
                }
            }
        }

        public static int Sort(Card x, Card y)
        {
            if(x.GetHashCode() < y.GetHashCode())
            {
                return -1;
            }
            return 1;
        }
    }
}
