using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class Poker
    {
        private static Poker _singleton;
        public static Poker Singleton
        {
            get
            {
                if(_singleton == null)
                {
                    _singleton = new Poker();
                }
                return _singleton;
            }
        }

        public const int CARD_NUM = 52;
        public static int PER_KIND_NUM = 13;
        private Card[] _cardArray = new Card[CARD_NUM];
        private int _cardIndex = 0;

        private Poker()
        {
            for(int i = 0; i < CARD_NUM; i++)
            {
                Card card = new Card();
                card.Number = i % PER_KIND_NUM + 1;
                card.CounterNumber = card.Number;
                if(card.Number > 10)
                {
                    card.CounterNumber = 10;
                }
                card.CardKind = (Card.Kind)(i / PER_KIND_NUM);
                _cardArray[i] = card;
            }
        }

        private byte[] _seedBytes = new byte[4];
        private int GetRandomSeed()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_seedBytes);
            return BitConverter.ToInt32(_seedBytes, 0);
        }

        public void SwapCard()
        {
            _cardIndex = 0;
            int i = CARD_NUM;
            int j;
            while(i > 0)
            {
                i--;
                j = new Random(GetRandomSeed()).Next() % CARD_NUM;
                Card temp = _cardArray[i];
                _cardArray[i] = _cardArray[j];
                _cardArray[j] = temp;
            }
        }

        //随机性测试
        public void TestRandom()
        {
            Dictionary<string, int> _dict = new Dictionary<string, int>();
            for(int i = 0; i < 10000; i++)
            {
                SwapCard();
                for(int j = 0; j < 6; j++)
                {
                    Card card = GetCard();
                    string hash = card.ToString();
                    if(!_dict.ContainsKey(hash))
                    {
                        _dict.Add(hash, 0);
                    }
                    _dict[hash] += 1;
                }
            }
            string content = string.Empty;
            foreach(var key in _dict.Keys)
            {
                content += string.Format("key:{0} count:{1}\n", key, _dict[key]);
            }
            FileTool.Write("随机性测试数据.txt", content);
        }

        public Card GetCard()
        {
            _cardIndex++;
            return _cardArray[_cardIndex];
        }
    }
}
