using UnityEngine;
using UnityEngine.UI;

public class FoundationAnchor : CardAnchor
{
    [SerializeField] private CardSuit Suit = CardSuit.CLUB;

    public override void OnStart()
    {
        base.OnStart();
        GetComponentInChildren<Text>().text = Suit.ToString();
    }

    public override bool CanAttachCard(PlayingCard card)
    {
        bool matchesSuit = card.Suit == Suit;
        if (NumberOfHeldCards < 1)
        {
            return matchesSuit && card.Rank == CardRank.ACE;
        }
        else
        {
            PlayingCard topCard = TopCard;
            bool isOneRankHigherThanTopCard = (int)card.Rank == ((int)topCard.Rank + 1);
            return matchesSuit && isOneRankHigherThanTopCard;
        }
    }

    public override void OnAttachCard(PlayingCard card)
    {
        base.OnAttachCard(card);
        // TODO: call into game manager to check if game is complete
    }
}
