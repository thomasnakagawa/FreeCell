using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayingCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardRank Rank { get; private set; }
    public CardSuit Suit { get; private set; }

    private CardAnchor currentAnchor;

    private Transform DeckTransform;

    private Vector3 mouseDragOffset;
    private List<CardAnchor> hoveredAnchors;

    private bool CanBeDragged => transform.GetSiblingIndex() == transform.parent.childCount - 1;

    public void InitializeCardValue(CardRank Rank, CardSuit Suit, Transform DeckTransform)
    {
        this.Rank = Rank;
        this.Suit = Suit;
        this.DeckTransform = DeckTransform;

        name = "Card_" + Rank + "_" + Suit;

        SetCardUI();

        hoveredAnchors = new List<CardAnchor>();
    }

    private void SetCardUI()
    {
        Color cardColor = Color.black;
        if (Suit == CardSuit.DIAMOND || Suit == CardSuit.HEART)
        {
            cardColor = Color.red;
        }

        string cardContent = Rank.ToUIString() + "\n" + Suit.ToString();

        foreach (Text textElement in transform.GetComponentsInChildren<Text>())
        {
            textElement.text = cardContent;
            textElement.color = cardColor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanBeDragged)
        {
            return;
        }

        mouseDragOffset = transform.position - Input.mousePosition;

        // detach from current anchor
        transform.SetParent(DeckTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanBeDragged)
        {
            return;
        }

        transform.position = Input.mousePosition + mouseDragOffset;

        // highlight just the closest hovered card anchor
        CardAnchor closestHoveredAnchor = ClosestHoveredAnchor();
        foreach (CardAnchor anchor in hoveredAnchors)
        {
            if (anchor == closestHoveredAnchor)
            {
                anchor.OnCardDragHover();
            }
            else
            {
                anchor.OnCardDragUnhover();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanBeDragged)
        {
            return;
        }

        // unhover all anchors
        foreach (CardAnchor anchor in hoveredAnchors)
        {
            anchor.OnCardDragUnhover();
        }

        // attach to hovered anchor if it can accept this card
        CardAnchor anchorToDropOn = ClosestHoveredAnchor();
        if (anchorToDropOn != null && anchorToDropOn.CanAttachCard(this))
        {
            AttachToAnchor(anchorToDropOn);
        } else
        {
            AttachToAnchor(currentAnchor);
        }
        MoveToAnchor();

        hoveredAnchors.Clear();
    }

    public void AttachToAnchor(CardAnchor anchor)
    {
        currentAnchor = anchor;
        anchor.OnAttachCard(this);
    }

    public void MoveToAnchor()
    {
        transform.position = currentAnchor.GetAttachmentPosition(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CardAnchor anchor = collider.GetComponent<CardAnchor>();
        if (anchor != null)
        {
            hoveredAnchors.Add(anchor);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        CardAnchor anchor = collider.GetComponent<CardAnchor>();
        if (anchor != null)
        {
            anchor.OnCardDragUnhover();
            if (hoveredAnchors.Contains(anchor))
            {
                hoveredAnchors.Remove(anchor);
            }
        }
    }

    private CardAnchor ClosestHoveredAnchor()
    {
        CardAnchor closestHoveredAnchor = null;
        float closestDistance = float.MaxValue;
        foreach (CardAnchor anchor in hoveredAnchors)
        {
            float distanceToAnchor = Vector3.Distance(anchor.transform.position, transform.position);
            if (distanceToAnchor < closestDistance)
            {
                closestDistance = distanceToAnchor;
                closestHoveredAnchor = anchor;
            }
        }
        return closestHoveredAnchor;
    }
}
