using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class Judger
    {
        public enum Result
        {
            win,
            lose,
            draw,
        }

        public static Result Judge(HandCardResult a, HandCardResult b)
        {
            if(a.Level >= CardLevel.threeCardWithOneJoker)
            {
                if(a.OnesDigit == b.OnesDigit)
                {
                    return Result.draw;
                }
            }
            else if(a.Level == b.Level)
            {
                return Result.draw;
            }

            //个位数为零 胜 双王
            if((a.Level == CardLevel.twoJoker) && (b.Level == CardLevel.onesDigitIsZero))
            {
                return Result.lose;
            }

            if(a.Level < b.Level)
            {
                if(b.Level >= CardLevel.threeCardWithOneJoker)
                {
                    return Result.win;
                }
            }
            else if(a.Level == b.Level)
            {
                if(a.Level >= CardLevel.threeCardWithOneJoker)
                {
                    if(a.OnesDigit > b.OnesDigit)
                    {
                        return Result.win;
                    }
                }
            }

            return Result.lose;
        }
    }
} 