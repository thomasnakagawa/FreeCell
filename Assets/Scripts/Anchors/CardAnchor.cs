using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// Base class for positions on screen where cards can be placed
/// </summary>
[RequireComponent(typeof(Image))]
public abstract class CardAnchor : MonoBehaviour
{
    protected Transform HeldCardsTransform;

    protected Color UnhoverColor;
    protected Image image;

    private void Start()
    {
        OnStart();
    }

    public virtual void OnStart()
    {
        image = GetComponent<Image>();
        UnhoverColor = image.color;

        HeldCardsTransform = transform.Find("HeldCards");
        Assert.IsNotNull(HeldCardsTransform);
    }

    public void OnCardDragHover(PlayingCard card)
    {
        Color hoverColor;
        if (CanAttachCard(card))
        {
            hoverColor = GameConfiguration.Instance.HoverEnabledColor;
        }
        else
        {
            hoverColor = GameConfiguration.Instance.HoverDisabledColor;
        }

        Image imageToColor = image;
        if (NumberOfHeldCards > 0)
        {
            imageToColor = TopCard.GetComponent<Image>();
        }
        imageToColor.color = hoverColor;
    }

    public void OnCardDragUnhover()
    {
        Image imageToColor = image;
        if (NumberOfHeldCards > 0)
        {
            imageToColor = TopCard.GetComponent<Image>();
        }
        imageToColor.color = Color.white;
    }

    public abstract bool CanAttachCard(PlayingCard card);

    public virtual void OnAttachCard(PlayingCard card)
    {
        card.transform.SetParent(HeldCardsTransform);
        card.transform.SetAsLastSibling();
    }

    protected int NumberOfHeldCards => HeldCardsTransform.childCount;
    protected PlayingCard TopCard
    {
        get
        {
            if (NumberOfHeldCards < 1)
            {
                return null;
            }
            return HeldCardsTransform.GetChild(HeldCardsTransform.childCount - 1).GetComponent<PlayingCard>();
        }
    }

    public virtual Vector3 GetAttachmentPosition(PlayingCard card)
    {
        return transform.position;
    }
}
