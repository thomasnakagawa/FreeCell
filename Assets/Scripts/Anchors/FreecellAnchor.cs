public class FreecellAnchor : CardAnchor
{
    public override bool CanAttachCard(PlayingCard card)
    {
        return NumberOfHeldCards < 1;
    }
}
