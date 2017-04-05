using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tool;

namespace Musai
{
    public class Statistics
    {
        public int TotalCount = 0;
        public int WinCount = 0;
        public int DrawCount = 0;
        public int LoseCount = 0;
        public int Money = 0;

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

        public float GetMoney()
        {
            if (TotalCount == 0) return 0;
            return ((float)Money / TotalCount);
        }
    }

    public class Result
    {
        public Statistics TwoCard = new Statistics();
        public Statistics ThreeCard = new Statistics();
        public int TotalCount = 0;
    }

    public class Logger
    {
        private static Dictionary<string, Result> _resultDict = new Dictionary<string, Result>();
        public static void Log(LogWrapper logWrapperA, LogWrapper logWrapperB)
        {
            Result resultA = GetResult(logWrapperA.TwoCardHash);
            Result resultB = GetResult(logWrapperB.TwoCardHash);
            resultA.TotalCount += 1;
            resultB.TotalCount += 1;
            Judger.Result result;
            if(logWrapperA.ResultOfTwoCard != null && logWrapperB.ResultOfTwoCard != null)
            {
                result = Judger.Judge(logWrapperA.ResultOfTwoCard, logWrapperB.ResultOfTwoCard);
                LogResult(result, GetOdds(result, logWrapperA.ResultOfTwoCard, logWrapperB.ResultOfTwoCard), resultA.TwoCard, resultB.TwoCard);
            }

            if(logWrapperB.ResultOfTwoCard != null)
            {
                result = Judger.Judge(logWrapperA.ResultOfThreeCard, logWrapperB.ResultOfTwoCard);
                LogResult(result, GetOdds(result, logWrapperA.ResultOfThreeCard, logWrapperB.ResultOfTwoCard), resultA.ThreeCard, resultB.TwoCard);
            }

            if(logWrapperA.ResultOfTwoCard != null)
            {
                result = Judger.Judge(logWrapperA.ResultOfTwoCard, logWrapperB.ResultOfThreeCard);
                LogResult(result, GetOdds(result, logWrapperA.ResultOfTwoCard, logWrapperB.ResultOfThreeCard), resultA.TwoCard, resultB.ThreeCard);
            }

            result = Judger.Judge(logWrapperA.ResultOfThreeCard, logWrapperB.ResultOfThreeCard);
            LogResult(result, GetOdds(result, logWrapperA.ResultOfThreeCard, logWrapperB.ResultOfThreeCard), resultA.ThreeCard, resultB.ThreeCard);
        }

        private static int GetOdds(Judger.Result result, HandCardResult a, HandCardResult b)
        {
            int odds = int.MinValue;
            if (result == Judger.Result.win)
            {
                odds = a.Odds;
            }
            else if(result == Judger.Result.lose)
            {
                odds = b.Odds;
            }
            return odds;
        }

        private static void LogResult(Judger.Result judgerResult, int odds, Statistics a, Statistics b)
        {
            a.TotalCount += 1;
            b.TotalCount += 1;
            switch(judgerResult)
            {
                case Judger.Result.win:
                    a.WinCount += 1;
                    a.Money += odds;
                    b.LoseCount += 1;
                    b.Money -= odds;
                    break;
                case Judger.Result.lose:
                    a.LoseCount += 1;
                    a.Money -= odds;
                    b.WinCount += 1;
                    b.Money += odds;
                    break;
                case Judger.Result.draw:
                    a.DrawCount += 1;
                    b.DrawCount += 1;
                    break;
            }
        }

        public static void Save()
        {
            string content = @"类型1:手牌两个王
类型2:手牌一个王
类型3:两张牌同花，并且相差一个点，比如梅花1和梅花2 方块1和方块K
类型4:两张牌同花，并且相差两个点，比如梅花1和梅花3 方块1和方块Q
类型5:两张牌相差一个点，不同花，比如方块1和梅花2
类型6:两张牌相差两个点，不同花，比如方块2和梅花K
类型7:两张牌同花，不存在顺子可能，比如方块2和方块5
类型8:两张牌同点，比如方块2和梅花2
类型9:除上面之外的所有情况

";

            List<string> keyList = _resultDict.Keys.ToList<string>();
            keyList.Sort(KeySort);
            for(int i = 0; i < keyList.Count; i++)
            {
                string key = keyList[i];
                Result result = _resultDict[key];
                content += string.Format("{0, -15}", key) + "\t统计次数:" + 
                    string.Format("{0, -6}", result.TotalCount) +
                    "\t两牌不败:" + FormatRate(result.TwoCard.GetUnloseRate()) + 
                    "\t两牌收益率:" + FormatRate(result.TwoCard.GetMoney()) + 
                    "\t补牌不败:" + FormatRate(result.ThreeCard.GetUnloseRate()) + 
                    "\t补牌收益率:" + FormatRate(result.ThreeCard.GetMoney()) + "\n";
            }
            FileTool.Write("Statistics.dat", content);
        }

        private static int KeySort(string x, string y)
        {
            if(x == "双王")
            {
                return -1;
            }
            if(x == "单王" && y != "双王")
            {
                return -1;
            }
            if(y == "双王")
            {
                return 1;
            }
            if(y == "单王" && x != "双王")
            {
                return 1;
            }
            return x.CompareTo(y);
        }

        private static string FormatRate(float rate)
        {
            return String.Format("{0, -8}", rate.ToString("f2"));
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