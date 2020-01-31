using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class CardAnchor : MonoBehaviour
{
    [SerializeField] protected Color HoverColor = Color.blue;

    //protected List<PlayingCard> HeldCards = new List<PlayingCard>();
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

    public virtual void OnCardDragHover()
    {
        image.color = HoverColor;
    }

    public virtual void OnCardDragUnhover()
    {
        image.color = UnhoverColor;
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

    public virtual Vector3 GetAttachmentPosition()
    {
        return transform.position;
    }
}
