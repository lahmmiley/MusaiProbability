using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class HandCardResult
    {
        public CardLevel Level;
        public int OnesDigit;
        public int Odds;

        public override string ToString()
        {
            return string.Format("level:" + Level.ToString() + "    onesDigit:" + OnesDigit + " odds:" + Odds);
        }
    }
}
