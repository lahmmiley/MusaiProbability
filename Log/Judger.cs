using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            invalid,
        }

        public static Result Judge(HandCardResult a, HandCardResult b)
        {
            if((a.Level == CardLevel.invalid) || (b.Level == CardLevel.invalid))
            {
                return Result.invalid;
            }
            //个位数为零 胜 双王
            if((a.Level == CardLevel.onesDigitIsZero) && (b.Level == CardLevel.twoJoker))
            {
                return Result.win;
            }
            if(a.Level < b.Level) 
            {
                if (a.Level <= CardLevel.straight)
                {
                    return Result.win;
                }
                else
                {
                    return JudgeByOnesDigit(a, b);
                }
            }
            else if(a.Level == b.Level)
            {
                if(a.Level <= CardLevel.straight)
                {
                    return Result.draw;
                }
                else
                {
                    return JudgeByOnesDigit(a, b);
                }
            }
            else
            //a.Level > b.level
            {
                if(b.Level > CardLevel.straight)
                {
                    return JudgeByOnesDigit(a, b);
                }
            }

            return Result.lose;
        }

        private static Result JudgeByOnesDigit(HandCardResult a, HandCardResult b)
        {
            if (a.OnesDigit > b.OnesDigit)
            {
                return Result.win;
            }
            else if(a.OnesDigit == b.OnesDigit)
            {
                return Result.draw;
            }
            return Result.lose;
        }
    }
} 