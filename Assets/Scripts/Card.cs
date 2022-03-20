using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;

    private GameManager gm;
    // Start is called before the first frame update

    public int handIndex;
    private void OnMouseDown()
    {
        if (hasBeenPlayed == false)
        {
            transform.position += Vector3.up * 5;
            hasBeenPlayed = true;
            gm.availableCardSlots[handIndex] = true;
            Invoke("MoveToDiscardPile", 2f);
        }
    }

    private void Start() {
        gm = FindObjectOfType<GameManager>();
    }

    void MoveToDiscardPile() {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
