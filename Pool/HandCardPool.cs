using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public static class HandCardPool
    {
        public class Wrapper
        {
            public HandCardResult Result;
            public string LogType = string.Empty;
        }
        public static Dictionary<int, Wrapper> _dict = new Dictionary<int, Wrapper>();

        public static void Get(List<Card> cardList, out HandCardResult result, out string logType)
        {
            int hash = GetHash(cardList);
            if (!_dict.ContainsKey(hash))
            {
                Wrapper wrapper = new Wrapper();
                HandCard handCard = new HandCard(cardList);
                wrapper.LogType = handCard.LogType;
                HandCardResult handCardResult = CardLevelJudgement.GetHandCardResult(handCard);
                wrapper.Result = handCardResult;
                _dict.Add(hash, wrapper);
            }
            result = _dict[hash].Result;
            logType = _dict[hash].LogType;
        }

        public static void Get(List<Card> cardList, out HandCardResult result)
        {
            int hash = GetHash(cardList);
            if (!_dict.ContainsKey(hash))
            {
                Wrapper wrapper = new Wrapper();
                HandCard handCard = new HandCard(cardList);
                HandCardResult handCardResult = CardLevelJudgement.GetHandCardResult(handCard);
                wrapper.Result = handCardResult;
                _dict.Add(hash, wrapper);
            }
            result = _dict[hash].Result;
        }

        private static int GetHash(List<Card> cardList)
        {
            if(cardList.Count == 2)
            {
                return cardList[0].GetHashCode() * 100 + cardList[1].GetHashCode();
            }
            else
            {
                return cardList[0].GetHashCode() * 10000 + cardList[1].GetHashCode() * 100 + cardList[2].GetHashCode();
            }
        }
    }
}
