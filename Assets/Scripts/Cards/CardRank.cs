public enum CardRank
{
    ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING
}

public static class CardRankExtension
{
    public static string ToUIString(this CardRank cardRank)
    {
        switch (cardRank)
        {
            case CardRank.ACE:
                return "A";
            case CardRank.TWO:
                return "2";
            case CardRank.THREE:
                return "3";
            case CardRank.FOUR:
                return "4";
            case CardRank.FIVE:
                return "5";
            case CardRank.SIX:
                return "6";
            case CardRank.SEVEN:
                return "7";
            case CardRank.EIGHT:
                return "8";
            case CardRank.NINE:
                return "9";
            case CardRank.TEN:
                return "10";
            case CardRank.JACK:
                return "J";
            case CardRank.QUEEN:
                return "Q";
            case CardRank.KING:
                return "K";
            default:
                throw new System.NotImplementedException("No UI string for rank " + cardRank);
        }
    }
}
