﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnAnchor : CardAnchor
{
    [SerializeField] private float CardStackingOffset = 30f;

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
    
    public override Vector3 GetAttachmentPosition()
    {
        return transform.position + (Vector3.down * CardStackingOffset * (NumberOfHeldCards - 1));
    }
}
