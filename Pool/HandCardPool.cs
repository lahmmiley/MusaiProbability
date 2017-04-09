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


        public static Stopwatch sw = new Stopwatch();
        public static Stopwatch sw1 = new Stopwatch();

        public static void Get(List<Card> cardList, out HandCardResult result, out string logType)
        {
            int hash = GetHash(cardList);
            if (!_dict.ContainsKey(hash))
            {
                sw1.Start();
                Wrapper wrapper = new Wrapper();
                HandCard handCard = new HandCard(cardList);
                wrapper.LogType = handCard.LogType;
                //logType = handCard.LogType;
                HandCardResult handCardResult = CardLevelJudgement.GetHandCardResult(handCard);
                wrapper.Result = handCardResult;
                //result = handCardResult;
                _dict.Add(hash, wrapper);
                sw1.Stop();
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
                //result = handCardResult;
                wrapper.Result = handCardResult;
                _dict.Add(hash, wrapper);
            }
            result = _dict[hash].Result;
        }

        private static int GetHash(List<Card> cardList)
        {
            sw1.Start();
            //cardList.Sort(Card.Sort);
            Sort(cardList);
            sw1.Stop();
            sw.Start();
            int hash = 0;
            if(cardList.Count == 2)
            {
                hash = cardList[0].GetHash() + 100 * cardList[1].GetHash();
            }
            else
            {
                hash = cardList[0].GetHash() + 100 * cardList[1].GetHash() + 10000 * cardList[2].GetHash();
            }
            sw.Stop();
            return hash;
        }

        public static bool Larger(Card x, Card y)
        {
            if((int)x.CardKind < (int)y.CardKind)
            {
                return true;
            }
            else if((int)x.CardKind == (int)y.CardKind)
            {
                if(x.Point > y.Point)
                {
                    return true;
                }
            }
            return false;
        }

        private static void Sort(List<Card> list)
        {
            if(list.Count == 2)
            {
                if(Larger(list[0], list[1]))
                {
                    Swap(list, 0, 1);
                }
            }
            else
            {
                //TODO
                if(Larger(list[0], list[1]))
                {
                    Swap(list, 0, 1);
                    if(Larger(list[1], list[2]))
                    {
                        Swap(list, 1, 2);
                    }
                }
                else
                {
                    if(Larger(list[1], list[2]))
                    {
                        Swap(list, 1, 2);
                    }
                }
            }
        }
        
        private static void Swap(List<Card> list, int x, int y)
        {
            Card temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }
    }
}
