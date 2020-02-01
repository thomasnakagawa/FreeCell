using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnAnchor : CardAnchor
{
    [Tooltip("Vertical distance between two stacked cards. Measured as percent of screen height")]
    [SerializeField] private float CardStackingOffset = 0.05f;

    private float UnitOffset => (CardStackingOffset * Screen.height);

    public override bool CanAttachCard(PlayingCard card)
    {
        if (NumberOfHeldCards < 1)
        {
            return true;
        }

        PlayingCard topCard = TopCard;

        bool cardsAreDifferentColors = !topCard.Suit.IsSameColor(card.Suit);
        bool topCardIsOneRankHigher = (int)card.Rank == (int)topCard.Rank - 1;

        return cardsAreDifferentColors && topCardIsOneRankHigher;
    }

    public override void OnAttachCard(PlayingCard card)
    {
        base.OnAttachCard(card);
        foreach (Transform childCard in HeldCardsTransform)
        {
            childCard.GetComponent<PlayingCard>().MoveToAnchor();
        }
    }

    public override void OnCardDragHover()
    {
        Image imageToColor = GetComponent<Image>();
        if (NumberOfHeldCards > 0)
        {
            imageToColor = TopCard.GetComponent<Image>();
        }
        imageToColor.color = HoverColor;
    }

    public override void OnCardDragUnhover()
    {
        Image imageToColor = GetComponent<Image>();
        if (NumberOfHeldCards > 0)
        {
            imageToColor = TopCard.GetComponent<Image>();
        }
        imageToColor.color = Color.white;
    }
    
    public override Vector3 GetAttachmentPosition(PlayingCard card)
    {
        int cardNumber = card.transform.GetSiblingIndex();
        return transform.position + (Vector3.down * UnitOffset * cardNumber);
    }
}
