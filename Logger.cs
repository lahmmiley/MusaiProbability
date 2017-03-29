using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class Statistics
    {
        public int TotalCount = 0;
        public int WinCount = 0;
        public int DrawCount = 0;
        public int LoseCount = 0;

        public float GetWinRate()
        {
            return ((float)WinCount / TotalCount) * 100;
        }

        public float GetDrawRate()
        {
            return ((float)DrawCount / TotalCount) * 100;
        }

        public float GetUnloseRate()
        {
            return ((float)(WinCount + DrawCount) / TotalCount) * 100;
        }
    }

    public class Result
    {
        public Statistics TwoCard = new Statistics();
        public Statistics ThreeCard = new Statistics();
    }

    public class Logger
    {
        private static Dictionary<string, Result> _resultDict = new Dictionary<string, Result>();
        public static void Log(HandCard handCardA, HandCard handCardB)
        {
            Result resultA = GetResult(handCardA);
            Result resultB = GetResult(handCardB);
            Judger.Result result = Judger.Judge(handCardA.twoCardJudgeInfo, handCardB.twoCardJudgeInfo);
            LogResult(result, resultA.TwoCard, resultB.TwoCard);

            result = Judger.Judge(handCardA.threeCardJudgeInfo, handCardB.twoCardJudgeInfo);
            LogResult(result, resultA.ThreeCard, resultB.TwoCard);

            result = Judger.Judge(handCardA.twoCardJudgeInfo, handCardB.threeCardJudgeInfo);
            LogResult(result, resultA.TwoCard, resultB.ThreeCard);

            result = Judger.Judge(handCardA.threeCardJudgeInfo, handCardB.threeCardJudgeInfo);
            LogResult(result, resultA.ThreeCard, resultB.ThreeCard);
        }

        public static void Print()
        {
            string content = string.Empty;
            foreach(string key in _resultDict.Keys)
            {
                Result result = _resultDict[key];
                content += Card.GetKey(key) + "count:" + result.TwoCard.TotalCount +
                    "  two card unlose:" + result.TwoCard.GetUnloseRate() + 
                    "  three unlose:" + result.ThreeCard.GetUnloseRate() + "\n";
            }
            FileTool.Write("Statistics.dat", content);
        }

        private static Result GetResult(HandCard handCardA)
        {
            if(!_resultDict.ContainsKey(handCardA.Key))
            {
                _resultDict.Add(handCardA.Key, new Result());
            }
            return _resultDict[handCardA.Key];
        }

        public static void LogResult(Judger.Result judgerResult, Statistics first, Statistics second)
        {
            first.TotalCount += 1;
            second.TotalCount += 1;
            switch(judgerResult)
            {
                case Judger.Result.win:
                    first.WinCount += 1;
                    second.LoseCount += 1;
                    break;
                case Judger.Result.lose:
                    first.LoseCount += 1;
                    second.WinCount += 1;
                    break;
                case Judger.Result.draw:
                    first.DrawCount += 1;
                    second.DrawCount += 1;
                    break;
            }
        }
    }
}