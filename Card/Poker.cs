using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Tool;

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

        public const int CARD_NUM = 55;
        public static int PER_KIND_NUM = 13;
        private Card[] _cardArray = new Card[CARD_NUM];
        private int _cardIndex = 0;

        private Poker()
        {
            int index;
            int normalCardNum = PER_KIND_NUM * 4;
            for(index = 0; index < normalCardNum ; index++)
            {
                Card card = new Card((Card.Kind)(index / PER_KIND_NUM), index % PER_KIND_NUM + 1);
                _cardArray[index] = card;
            }
            AddJoker(Card.Kind.redJoker, ref index);
            AddJoker(Card.Kind.blackJoker, ref index);
            AddJoker(Card.Kind.wildCard, ref index);
        }

        private void AddJoker(Card.Kind kind, ref int index)
        {
            Card joker = new Card(kind);
            _cardArray[index] = joker;
            index++;
        }

        public void Shuffle()
        {
            _cardIndex = 0;
            int i = CARD_NUM;
            int j;
            Random random = new Random(GetRandomSeed());
            while(i > 0)
            {
                i--;
                j = random.Next() % CARD_NUM;
                Card temp = _cardArray[i];
                _cardArray[i] = _cardArray[j];
                _cardArray[j] = temp;
            }
        }
        
        public Card GetCard()
        {
            return _cardArray[_cardIndex++];
        }

        private int GetRandomSeed()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] seedBytes = new byte[4];
            rng.GetBytes(seedBytes);
            return BitConverter.ToInt32(seedBytes, 0);
        }
    }
}
