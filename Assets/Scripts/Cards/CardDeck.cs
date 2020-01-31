using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private GameObject CardPrefab = default;

    List<PlayingCard> Cards;

    private void Start()
    {
        GenerateDeck();
    }

    public void GenerateDeck()
    {
        Cards = new List<PlayingCard>();
        foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
        {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                GameObject cardGO = Instantiate(CardPrefab, transform);
                PlayingCard card = cardGO.GetComponent<PlayingCard>();
                card.InitializeCardValue(rank, suit);
                Cards.Add(card);
            }
        }
        Assert.AreEqual(Cards.Count, 52);
    }

    public void ShuffleDeck()
    {

    }
}
