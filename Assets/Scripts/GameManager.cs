using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    private FoundationAnchor[] FoundationAnchors;

    private void Start()
    {
        FoundationAnchors = FindObjectsOfType<FoundationAnchor>();
        Assert.AreEqual(FoundationAnchors.Length, 4, "GameManager requires 4 FoundationAnchors in the scene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
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
        foreach (var card in FindObjectsOfType<PlayingCard>())
        {
            var rb = card.GetComponent<Rigidbody2D>();
            card.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Random.onUnitSphere * 10f);
            rb.gravityScale = 10f;
        }
    }
}
