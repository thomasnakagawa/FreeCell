using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Assertions;

public class PlayingCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Animation")]
    [SerializeField] private float MoveAnimationTime = 0.2f;
    [SerializeField] private AnimationCurve MoveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Sound")]
    [SerializeField] private AudioClip PickupClip = default;
    [SerializeField] private AudioClip DropClip = default;
    [SerializeField] private AudioClip PlaceClip = default;
    [SerializeField] private AudioClip SlideClip = default;

    private AudioSource audioSource;

    public CardRank Rank { get; private set; }
    public CardSuit Suit { get; private set; }

    private CardAnchor currentAnchor;

    private Transform DeckTransform;

    private Vector3 mouseDragOffset;
    private bool DidDrag = false;
    private List<CardAnchor> hoveredAnchors;

    private bool CanBeDragged {
        get
        {
            if (GameConfiguration.Instance.CheatsEnabled)
            {
                return true;
            }
            else
            {
                return transform.GetSiblingIndex() == transform.parent.childCount - 1;
            }
        }
    }
    private bool CanBeClicked => transform.GetSiblingIndex() == transform.parent.childCount - 1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    public void InitializeCard(CardRank Rank, CardSuit Suit, Transform DeckTransform)
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
        DidDrag = true;

        // detach from current anchor
        transform.SetParent(DeckTransform);

        audioSource.PlayOneShot(PickupClip);
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
                anchor.OnCardDragHover(this);
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
            audioSource.PlayOneShot(PlaceClip);
        }
        else
        {
            AttachToAnchor(currentAnchor);
            audioSource.PlayOneShot(DropClip);
        }
        MoveToAnchor();

        hoveredAnchors.Clear();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanBeClicked)
        {
            return;
        }

        // don't perform this click event if the card was just dragged
        if (DidDrag)
        {
            DidDrag = false;
            return;
        }

        // don't go to freecell if already at a freecell
        if (currentAnchor is FreecellAnchor)
        {
            return;
        }

        // attach to a freecell if one is open
        FreecellAnchor[] freecells = FindObjectsOfType<FreecellAnchor>();
        foreach (FreecellAnchor freecell in freecells)
        {
            if (freecell.CanAttachCard(this))
            {
                AttachToAnchor(freecell);
                MoveToAnchor();
                audioSource.PlayOneShot(SlideClip);
                return;
            }
        }
    }

    public void AttachToAnchor(CardAnchor anchor)
    {
        currentAnchor = anchor;
        anchor.OnAttachCard(this);
    }

    public void MoveToAnchor()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToLocation(currentAnchor.GetAttachmentPosition(this)));
    }

    private IEnumerator MoveToLocation(Vector3 target)
    {
        Vector3 startingPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < MoveAnimationTime)
        {
            float curvedNormalizedT = MoveCurve.Evaluate(elapsedTime / MoveAnimationTime);
            transform.position = Vector3.Lerp(startingPosition, target, curvedNormalizedT);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        transform.position = target;
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
