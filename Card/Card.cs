using System;
using System.Collections.Generic;
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

        public int GetHash()
        {
            //从10开始，保证一定是两位数 最大为Kind.invalid 为99
            //Point从1开始，乘15比较安全
            return 10 + ((int)CardKind) * 15 + Point;
        }

        public override string ToString()
        {
            return ((int)CardKind).ToString() + "_" + Point.ToString();
        }

        public string ToCard()
        {
            return ToString().ToCard();
        }

        public bool IsJoker()
        {
            return CardKind == Kind.redJoker || CardKind == Kind.blackJoker;
        }

        public static int Sort(Card x, Card y)
        {
            if((int)x.CardKind < (int)y.CardKind)
            {
                return -1;
            }
            else if((int)x.CardKind == (int)y.CardKind)
            {
                if(x.Point > y.Point)
                {
                    return -1;
                }
            }
            return 1;
        }
    }
}
