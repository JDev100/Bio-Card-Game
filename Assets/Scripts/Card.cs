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
            if (gm.CurrentPlayerTurn() == 0)
                transform.position += Vector3.up * 5;
            else
                transform.position += Vector3.down * 5;
            hasBeenPlayed = true;
            if (gm.CurrentPlayerTurn() == 0)
                gm.availableCardSlotsP1[handIndex] = true;
            else
                gm.availableCardSlotsP2[handIndex] = true;
            Invoke("MoveToDiscardPile", 2f);
        }
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void MoveToDiscardPile()
    {
        if (gm.CurrentPlayerTurn() == 0)
            gm.discardPileP1.Add(this);
        else
            gm.discardPileP2.Add(this);
        gameObject.SetActive(false);
    }
}
