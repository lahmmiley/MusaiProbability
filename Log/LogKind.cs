using System;
using System.Collections.Generic;

namespace Musai 
{
    public enum LogKind
    {
        twoJoker = 1, //双王
        oneJoker, //单王
        straightPossible1AndSameKind,//两张牌相差一点 和 同花
        straightPossible2AndSameKind,//两张牌相差两点 和 同花
        straightPossible1,//两张牌相差一点
        straightPossible2,//两张牌相差两点
        sameKind,//同花
        samePoint,//同点
        other,//其他类型
    }

    public class LogKindJudgement
    {
        private class JudgeFunction
        {
            public LogKind Kind;
            public Func<HandCard, bool> Fun;

            public JudgeFunction(LogKind kind, Func<HandCard, bool> fun)
            {
                this.Kind = kind;
                this.Fun = fun;
            }
        }

        private static List<JudgeFunction> _judgeFunciontList = new List<JudgeFunction>();

        static LogKindJudgement()
        {
            _judgeFunciontList.Add(new JudgeFunction(LogKind.twoJoker, IsTwoJoker));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.oneJoker, IsOneJoker));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.straightPossible1AndSameKind, IsStraightPossible1AndSameKind));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.straightPossible2AndSameKind, IsStraightPossible2AndSameKind));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.straightPossible1, IsStraightPossible1));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.straightPossible2, IsStraightPossible2));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.sameKind, IsSameKind));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.samePoint,IsSamePoint));
            _judgeFunciontList.Add(new JudgeFunction(LogKind.other, IsOther));
        }

        public static LogKind GetLogKind(HandCard handCard)
        {
            LogKind result = LogKind.other;
            for(int i = 0; i < _judgeFunciontList.Count; i++)
            {
                JudgeFunction judgeFunction = _judgeFunciontList[i];
                if(judgeFunction.Fun.Invoke(handCard))
                {
                    result = judgeFunction.Kind;
                    break;
                }
            }
            return result;
        }

        private static bool IsTwoJoker(HandCard handCard)
        {
            return handCard.IsTwoJoker;
        }

        private static bool IsOneJoker(HandCard handCard)
        {
            return handCard.JokerCount == 1;
        }

        private static bool IsStraightPossible1AndSameKind(HandCard handCard)
        {
            return handCard.IsSameKind && handCard.IsStraightPossible1;
        }

        private static bool IsStraightPossible2AndSameKind(HandCard handCard)
        {
            return handCard.IsSameKind && handCard.IsStraightPossible2;
        }

        private static bool IsStraightPossible1(HandCard handCard)
        {
            return handCard.IsStraightPossible1;
        }

        private static bool IsStraightPossible2(HandCard handCard)
        {
            return handCard.IsStraightPossible2;
        }

        private static bool IsSamePoint(HandCard handCard)
        {
            return handCard.IsSamePoint;
        }

        private static bool IsSameKind(HandCard handCard)
        {
            return handCard.IsSameKind;
        }

        private static bool IsOther(HandCard handCard)
        {
            return true;
        }
    }
}