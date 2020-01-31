public enum CardSuit
{
    DIAMOND, CLUB, HEART, SPADE
}

public static class CardSuitExtensions
{
    public static bool IsSameColor(this CardSuit thisSuit, CardSuit otherSuit)
    {
        // because suit colors are alternating, the sum of two suits being even means they're the same color
        return ((int)thisSuit + (int)otherSuit) % 2 == 0;
    }
}