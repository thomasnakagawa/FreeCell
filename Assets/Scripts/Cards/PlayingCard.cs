using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayingCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardRank Rank { get; private set; }
    public CardSuit Suit { get; private set; }

    private CardAnchor currentAnchor;

    private Vector3 mouseDragOffset;
    private List<CardAnchor> hoveredAnchors;

    public void InitializeCardValue(CardRank Rank, CardSuit Suit)
    {
        this.Rank = Rank;
        this.Suit = Suit;

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

        string cardContent = Rank.ToString() + "\n" + Suit.ToString();

        foreach (Text textElement in transform.GetComponentsInChildren<Text>())
        {
            textElement.text = cardContent;
            textElement.color = cardColor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseDragOffset = transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        // attach to hovered anchor if it can accept this card
        CardAnchor anchorToDropOn = ClosestHoveredAnchor();
        if (anchorToDropOn != null && anchorToDropOn.CanAttachCard(this))
        {
            // detach from current anchor
            if (currentAnchor != null)
            {
                currentAnchor.OnDetachCard(this);
            }

            // attach to new achor
            anchorToDropOn.OnAttachCard(this);
            currentAnchor = anchorToDropOn;
        }

        // unhover all anchors
        foreach (CardAnchor anchor in hoveredAnchors)
        {
            anchor.OnCardDragUnhover();
        }
        hoveredAnchors.Clear();

        // move to the anchor point
        transform.position = currentAnchor.transform.position;
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
