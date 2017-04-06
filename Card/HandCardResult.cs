using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class HandCardResult
    {
        public const int INVALID_VALUE = -1;

        public CardLevel Level;
        public int OnesDigit = INVALID_VALUE;
        public int Odds;

        public HandCardResult() { }

        public HandCardResult(CardLevel level, int onesDigit = INVALID_VALUE)
        {
            this.Level = level;
            this.OnesDigit = onesDigit;
        }


        public override string ToString()
        {
            return string.Format("level:" + Level.ToString() + "    onesDigit:" + OnesDigit + " odds:" + Odds);
        }
    }
}
