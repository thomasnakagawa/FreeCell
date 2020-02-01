using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/// <summary>
/// Handles spawning, shuffling and dealing cards
/// </summary>
public class CardDeck : MonoBehaviour
{
    [SerializeField] private GameObject CardPrefab = default;
    [SerializeField] private AudioClip DealSound = default;

    private AudioSource audioSource;

    private CardAnchor[] ColumnAnchors;
    private System.Random Random;

    List<PlayingCard> Cards;

    private void Start()
    {
        Random = new System.Random(GameConfiguration.Instance.RNGSeed);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        Assert.IsNotNull(CardPrefab);

        ColumnAnchors = FindObjectsOfType<ColumnAnchor>()
            .OrderBy(col => col.transform.GetSiblingIndex())
            .ToArray();
        Assert.IsTrue(ColumnAnchors.Length > 0);
    }

    public void StartGame()
    {
        // stop previous deal if it's still happening
        audioSource.Stop();
        StopAllCoroutines();

        // destroy cards from previous game, if there are any
        var oldCards = FindObjectsOfType<PlayingCard>();
        foreach (PlayingCard card in oldCards)
        {
            Destroy(card.gameObject);
        }

        // create a new deck and deal
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

    // https://stackoverflow.com/questions/273313/randomize-a-listt
    private void ShuffleDeck()
    {
        int n = Cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Next(n + 1);
            PlayingCard value = Cards[k];
            Cards[k] = Cards[n];
            Cards[n] = value;
        }
    }

    private IEnumerator Deal()
    {
        yield return new WaitForSeconds(0.5f);
        int columnIndex = 0;
        audioSource.loop = true;
        audioSource.clip = DealSound;
        audioSource.Play();
        foreach (PlayingCard card in Cards)
        {
            card.AttachToAnchor(ColumnAnchors[columnIndex]);
            card.MoveToAnchor();
            columnIndex = (columnIndex + 1) % ColumnAnchors.Length;
            yield return new WaitForSeconds(0.02f);
        }
        audioSource.Stop();

        FindObjectOfType<Timer>().RestartTimer();
    }

}
