using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private GameObject CardPrefab = default;

    private CardAnchor[] ColumnAnchors;

    List<PlayingCard> Cards;

    private void Start()
    {
        Assert.IsNotNull(CardPrefab);

        ColumnAnchors = FindObjectsOfType<ColumnAnchor>()
            .OrderBy(col => col.transform.GetSiblingIndex())
            .ToArray();
        Assert.IsTrue(ColumnAnchors.Length > 0);

        GenerateDeck();
        ShuffleDeck();
        StartCoroutine(Deal());
    }

    private void GenerateDeck()
    {
        Cards = new List<PlayingCard>();
        foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
        {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                GameObject cardGO = Instantiate(CardPrefab, transform);
                PlayingCard card = cardGO.GetComponent<PlayingCard>();
                card.InitializeCard(rank, suit, transform);
                Cards.Add(card);
            }
        }
        Assert.AreEqual(Cards.Count, 52);
    }

    private void ShuffleDeck()
    {
        System.Random random = new System.Random();
        int n = Cards.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            PlayingCard value = Cards[k];
            Cards[k] = Cards[n];
            Cards[n] = value;
        }
    }

    private IEnumerator Deal()
    {
        yield return new WaitForSeconds(0.5f);
        int columnIndex = 0;
        foreach (PlayingCard card in Cards)
        {
            card.AttachToAnchor(ColumnAnchors[columnIndex]);
            card.MoveToAnchor();
            columnIndex = (columnIndex + 1) % ColumnAnchors.Length;
            yield return new WaitForSeconds(0.02f);
        }
    }

}
