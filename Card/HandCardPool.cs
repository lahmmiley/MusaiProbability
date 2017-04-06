using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public static class HandCardPool
    {
        public static Dictionary<string, HandCard> _dict = new Dictionary<string, HandCard>();

        public static HandCard Get(Card a, Card b)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(a);
            cardList.Add(b);
            cardList.Sort(Card.Sort);

            string key = string.Empty;
            for(int i = 0; i < cardList.Count; i++)
            {
                key += cardList[i].ToString();
            }
            if(!_dict.ContainsKey(key))
            {
                _dict.Add(key, new HandCard(a, b));
            }
            return _dict[key];
        }

        public static HandCard Get(Card a, Card b, Card c)
        {
            List<Card> cardList = new List<Card>();
            cardList.Add(a);
            cardList.Add(b);
            cardList.Add(c);
            cardList.Sort(Card.Sort);

            string key = string.Empty;
            for(int i = 0; i < cardList.Count; i++)
            {
                key += cardList[i].ToString();
            }
            if(!_dict.ContainsKey(key))
            {
                _dict.Add(key, new HandCard(a, b, c));
            }
            return _dict[key];
        }
    }
}
