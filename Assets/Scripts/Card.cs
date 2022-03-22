using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public bool hasBeenPlayed;

    private GameManager gm;

    public int attackPoints;
    public int defensePoints;
    public Text attackPointsText;
    public Text defensePointsText;

    int cardOwner;
    // Start is called before the first frame update

    public int handIndex;
    private void OnMouseDown()
    {
        if (hasBeenPlayed == false)
        {
            gm.SetCardInPlay(this);
            // if (gm.CurrentPlayerTurn() == 0)
            //     transform.position += Vector3.up * 2;
            // else
            //     transform.position += Vector3.down * 2;
            // hasBeenPlayed = true;
            // if (gm.CurrentPlayerTurn() == 0)
            //     gm.availableCardSlotsP1[handIndex] = true;
            // else
            //     gm.availableCardSlotsP2[handIndex] = true;
            // Invoke("MoveToDiscardPile", 2f);
        }
    }


    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

        attackPointsText.text = attackPoints.ToString();
        defensePointsText.text = defensePoints.ToString();
    }

    void MoveToDiscardPile()
    {
        if (gm.CurrentPlayerTurn() == 0)
            gm.discardPileP1.Add(this);
        else
            gm.discardPileP2.Add(this);
        gameObject.SetActive(false);
    }

    public int GetCardOwner()
    {
        return cardOwner;
    }
    public void SetCardOwner(int owner)
    {
        cardOwner = owner;
    }

    public void SetCardToPlay()
    {
        if (cardOwner == 0)
            transform.position += Vector3.up * 2;
        else
            transform.position += Vector3.down * 2;
    }
    public void RemoveCardFromPlay()
    {
        if (cardOwner == 1)
            transform.position += Vector3.up * 2;
        else
            transform.position += Vector3.down * 2;
    }
}
