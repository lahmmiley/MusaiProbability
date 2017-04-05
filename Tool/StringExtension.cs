using System;
using Kind = Musai.Card.Kind;

namespace Tool
{
    public static class StringExtension
    {
        public static string ToCard(this string str)
        {
            Kind kind = (Kind)int.Parse(str[0].ToString());
            string kindStr = string.Empty;
            switch(kind)
            {
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
                    return "大王";
                case Kind.blackJoker:
                    return "小王";
                case Kind.invalid:
                    throw new Exception("不可能无效");
            }
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
