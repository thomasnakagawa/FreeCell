using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Handles starting the game, detecting the game being won, and ending the game
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject EndingScreen = default;

    private FoundationAnchor[] FoundationAnchors;
    private CardDeck Deck;

    private void Start()
    {
        FoundationAnchors = FindObjectsOfType<FoundationAnchor>();
        Assert.AreEqual(FoundationAnchors.Length, 4, "GameManager requires 4 FoundationAnchors in the scene");

        Deck = FindObjectOfType<CardDeck>();
        Assert.IsNotNull(Deck, "GameManager needs CardDeck to be in the scene");

        Assert.IsNotNull(EndingScreen, "GameManager needs reference to EndingScreen");
    }

    public void StartGame()
    {
        Deck.StartGame();
    }

    private void Update()
    {
        if (GameConfiguration.Instance.CheatsEnabled && Input.GetKeyDown(KeyCode.A))
        {
            OnGameCompleted();
        }
    }

    public void OnFoundationChanged()
    {
        if (IsGameCompleted())
        {
            OnGameCompleted();
        }
    }

    private bool IsGameCompleted()
    {
        foreach (FoundationAnchor foundation in FoundationAnchors)
        {
            if (foundation.IsComplete == false)
            {
                return false;
            }
        }
        return true;
    }

    private void OnGameCompleted()
    {
        StartCoroutine(EndOfGameSequence());
    }

    private IEnumerator EndOfGameSequence()
    {
        FindObjectOfType<Timer>().StopTimer();

        foreach (var card in FindObjectsOfType<PlayingCard>())
        {
            var rb = card.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 1f;
            rb.AddForce(Random.onUnitSphere * 10000f);
            rb.AddTorque(Random.Range(-5000f, 5000f));
            rb.gravityScale = 50f;
        }
        yield return new WaitForSeconds(4f);
        EndingScreen.SetActive(true);
    }
}
