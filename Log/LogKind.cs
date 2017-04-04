using System;
using System.Collections.Generic;

namespace Musai 
{
    public enum LogKind
    {
        twoJoker, //双王
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
            public Func<List<Card>, bool> Fun;

            public JudgeFunction(LogKind kind, Func<List<Card>, bool> fun)
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

        public static LogKind GetLogKind(List<Card> list)
        {
            LogKind result = LogKind.other;
            for(int i = 0; i < _judgeFunciontList.Count; i++)
            {
                JudgeFunction judgeFunction = _judgeFunciontList[i];
                if(judgeFunction.Fun.Invoke(list))
                {
                    result = judgeFunction.Kind;
                    break;
                }
            }
            return result;
        }

        private static bool IsTwoJoker(List<Card> list)
        {
            return CardListJudgement.IsTwoJoker(list);
        }

        private static bool IsOneJoker(List<Card> list)
        {
            return CardListJudgement.GetJokerCount(list) == 1;
        }

        private static bool IsStraightPossible1AndSameKind(List<Card> list)
        {
            return CardListJudgement.IsSameKind(list) && _IsStraightPossible1(list);
        }

        private static bool IsStraightPossible2AndSameKind(List<Card> list)
        {
            return CardListJudgement.IsSameKind(list) && _IsStraightPossible2(list);
        }

        private static bool IsStraightPossible1(List<Card> list)
        {
            return _IsStraightPossible1(list);
        }

        private static bool IsStraightPossible2(List<Card> list)
        {
            return _IsStraightPossible2(list);
        }

        private static bool IsSamePoint(List<Card> list)
        {
            return CardListJudgement.IsSamePoint(list);
        }

        private static bool IsSameKind(List<Card> list)
        {
            return CardListJudgement.IsSameKind(list);
        }

        private static bool IsOther(List<Card> list)
        {
            return true;
        }

        private static bool _IsStraightPossible1(List<Card> list)
        {
            Card a = list[0];
            Card b = list[1];
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

        private static bool _IsStraightPossible2(List<Card> list)
        {
            Card a = list[0];
            Card b = list[1];
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
