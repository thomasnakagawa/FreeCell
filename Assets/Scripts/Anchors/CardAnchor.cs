
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAnchor : MonoBehaviour
{
    protected List<PlayingCard> HeldCards = new List<PlayingCard>();

    public abstract void OnCardDragHover();

    public abstract void OnCardDragUnhover();

    public abstract bool CanAttachCard(PlayingCard card);

    public virtual void OnAttachCard(PlayingCard card)
    {
        HeldCards.Add(card);
    }

    public virtual void OnDetachCard(PlayingCard card)
    {
        if (HeldCards.Contains(card) == false)
        {
            throw new System.InvalidOperationException("Cannot detach card " + card.name + " because it is not attached");
        }
        HeldCards.Remove(card);
    }
}
