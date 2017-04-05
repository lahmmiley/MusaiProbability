﻿using System;
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

        public const int CARD_NUM = 54;
        public static int PER_KIND_NUM = 13;
        private Card[] _cardArray = new Card[CARD_NUM];
        private int _cardIndex = 0;

        private Poker()
        {
            int index;
            for(index = 0; index < CARD_NUM - 2; index++)
            {
                Card card = new Card((Card.Kind)(index / PER_KIND_NUM), index % PER_KIND_NUM + 1);
                _cardArray[index] = card;
            }
            AddJoker(Card.Kind.redJoker, ref index);
            AddJoker(Card.Kind.blackJoker, ref index);
        }

        private void AddJoker(Card.Kind kind, ref int index)
        {
            Card joker = new Card(kind, -1);
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

        //private byte[] _seedBytes = new byte[4];
        private int GetRandomSeed()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] seedBytes = new byte[4];
            rng.GetBytes(seedBytes);
            return BitConverter.ToInt32(seedBytes, 0);
        }

        //随机性测试
        public void TestRandom(Stopwatch spForSeed)
        {
            Dictionary<string, int> _dict = new Dictionary<string, int>();
            for(int i = 0; i < 1; i++)
            {
                Shuffle();
                for(int j = 0; j < 1; j++)
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
            int max = int.MinValue;
            int min = int.MaxValue;
            foreach(var key in _dict.Keys)
            {
                content += string.Format("key:{0} count:{1}\n", key.ToCard(), _dict[key]);
                if(_dict[key] > max)
                {
                    max = _dict[key];
                }
                if(_dict[key] < min)
                {
                    min = _dict[key];
                }
            }
            Console.WriteLine("max:" + max + "min:" + min + "\n");
            FileTool.Write("随机性测试数据.dat", content);
        }

    }
}
