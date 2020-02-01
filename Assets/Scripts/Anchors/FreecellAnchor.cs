/// <summary>
/// Cells where any card can be placed
/// </summary>
public class FreecellAnchor : CardAnchor
{
    public override bool CanAttachCard(PlayingCard card)
    {
        return NumberOfHeldCards < 1;
    }
}
