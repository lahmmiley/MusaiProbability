using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

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
            if (TotalCount == 0) return 0;
            return ((float)WinCount / TotalCount) * 100;
        }

        public float GetDrawRate()
        {
            if (TotalCount == 0) return 0;
            return ((float)DrawCount / TotalCount) * 100;
        }

        public float GetUnloseRate()
        {
            if (TotalCount == 0) return 0;
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
        public static void Log(LogWrapper logWrapperA, LogWrapper logWrapperB)
        {
            Result resultA = GetResult(logWrapperA.TwoCardHash);
            Result resultB = GetResult(logWrapperB.TwoCardHash);
            Judger.Result result;
            if(logWrapperA.ResultOfTwoCard != null && logWrapperB.ResultOfTwoCard != null)
            {
                result = Judger.Judge(logWrapperA.ResultOfTwoCard, logWrapperB.ResultOfTwoCard);
                LogResult(result, resultA.TwoCard, resultB.TwoCard);
            }

            if(logWrapperB.ResultOfTwoCard != null)
            {
                result = Judger.Judge(logWrapperA.ResultOfThreeCard, logWrapperB.ResultOfTwoCard);
                LogResult(result, resultA.ThreeCard, resultB.TwoCard);
            }

            if(logWrapperA.ResultOfTwoCard != null)
            {
                result = Judger.Judge(logWrapperA.ResultOfTwoCard, logWrapperB.ResultOfThreeCard);
                LogResult(result, resultA.TwoCard, resultB.ThreeCard);

            }

            result = Judger.Judge(logWrapperA.ResultOfThreeCard, logWrapperB.ResultOfThreeCard);
            LogResult(result, resultA.ThreeCard, resultB.ThreeCard);
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

        public static void Save()
        {
            string content = string.Empty;
            foreach(string key in _resultDict.Keys)
            {
                Result result = _resultDict[key];
                content += GetKey(key) + "\t统计次数:" + 
                    string.Format("{0, 6}", result.TwoCard.TotalCount) +
                    "\t两张牌不败概率:" + FormatRate(result.TwoCard.GetUnloseRate()) + 
                    "\t补牌不败概率:" + FormatRate(result.ThreeCard.GetUnloseRate()) + "\n";
            }
            FileTool.Write("Statistics.dat", content);
        }

        private static string GetKey(string str)
        {
            string[] splits = str.Split(' ');
            return splits[0].ToCard() + splits[1].ToCard();
        }

        private static string FormatRate(float rate)
        {
            return String.Format("{0, 8}", rate.ToString("f2"));
        }

        private static Result GetResult(string hash)
        {
            if(!_resultDict.ContainsKey(hash))
            {
                _resultDict.Add(hash, new Result());
            }
            return _resultDict[hash];
        }
    }
}