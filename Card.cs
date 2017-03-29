using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public int Number;
        public int CounterNumber;//J、Q、K计算时算10点
        public Kind CardKind;

        public override string ToString()
        {
            return ((int)CardKind).ToString() + "_" + Number.ToString();
        }

        public static int Sort(Card x, Card y)
        {
            if((int)x.CardKind < (int)y.CardKind)
            {
                return -1;
            }
            else if((int)x.CardKind == (int)y.CardKind)
            {
                if(x.Number > y.Number)
                {
                    return -1;
                }
            }
            return 1;
        }

        public static string GetKey(string str)
        {
            string[] splits = str.Split(' ');
            return ToCard(splits[0]) + ToCard(splits[1]);
        }

        public static string ToCard(string str)
        {
            Kind kind = (Kind)int.Parse(str[0].ToString());
            string kindStr = string.Empty;
            switch(kind)
            {
                //TODO 列出中文好懂
                case Kind.hearts:
                    kindStr = "红桃";
                    break;
                case Kind.spades:
                    kindStr = "黑桃";
                    break;
                case Kind.diamonds:
                    kindStr = "方片";
                    break;
                case Kind.club:
                    kindStr = "梅花";
                    break;
                case Kind.redJoker:
                    kindStr = "大王";
                    break;
                case Kind.blackJoker:
                    kindStr = "小王";
                    break;
            }
            //TODO number转为JQK
            int number = int.Parse(str.Substring(2));
            string numberStr = number.ToString();
            if(number == 11)
            {
                numberStr = "J";
            }
            else if(number == 12)
            {
                numberStr = "Q";
            }
            else if(number == 13)
            {
                numberStr = "K";
            }
            return kindStr + numberStr;
        }
    }
}
