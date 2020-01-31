using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FreecellAnchor : CardAnchor
{
    [SerializeField] private Color HoverColor = Color.blue;
    private Color UnhoverColor;

    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        UnhoverColor = image.color;
    }
    public override bool CanAttachCard(PlayingCard card)
    {
        return HeldCards.Count < 1;
    }

    public override void OnCardDragHover()
    {
        image.color = HoverColor;
    }

    public override void OnCardDragUnhover()
    {
        image.color = UnhoverColor;
    }
}
