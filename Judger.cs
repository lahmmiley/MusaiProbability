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

        public static Result Judge(JudgeInfo a, JudgeInfo b)
        {
            //TODO 优化判断
            if(a.Level >= CardLevel.other)
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

            if(a.Level < b.Level)
            {
                if(b.Level >= CardLevel.other)
                {
                    return Result.win;
                }
            }
            else if(a.Level == b.Level)
            {
                if(a.Level >= CardLevel.other)
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