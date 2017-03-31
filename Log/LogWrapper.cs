using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musai
{
    public class LogWrapper
    {
        public string TwoCardHash;
        public HandCardResult ResultOfTwoCard;
        public HandCardResult ResultOfThreeCard;

        public LogWrapper(string twoCardHash, HandCardResult resultOfTwoCard, HandCardResult resultOfThreeCard)
        {
            this.TwoCardHash = twoCardHash;
            this.ResultOfTwoCard = resultOfTwoCard;
            this.ResultOfThreeCard = resultOfThreeCard;
        }
    }
}
