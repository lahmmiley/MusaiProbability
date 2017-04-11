namespace Musai
{
    public enum CardLevel
    {
        invalid,//两张牌中带有一个王为无效牌
        threeJoker,//三王
        twoJoker,//双王
        tianGongNine,//天公九;
        tianGongEight,//天公八;
        threeCardWithTwoJoker,//双王带一张
        straightFlush,//同花顺
        threeCardWithSamePoint,//三张同点数
        straight,//顺子
        onesDigitIsZero,//个位数为零
        other,//普通牌
    }
}
