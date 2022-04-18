using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public bool hasBeenPlayed;

    private GameManager gm;

    public int attackPoints;
    public int maxAtkPoints;
    public int defensePoints;
    public int maxDefPoints;
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
        maxDefPoints = defensePoints;
        maxAtkPoints = attackPoints;
        gm = FindObjectOfType<GameManager>();

        attackPointsText.text = attackPoints.ToString();
        defensePointsText.text = defensePoints.ToString();
    }

    public void TakeDamage(int atkPoints)
    {
        defensePoints -= atkPoints;
        defensePointsText.text = defensePoints.ToString();
        defensePointsText.color = Color.red;
        hasBeenPlayed = true;

        if (defensePoints <= 0)
            Invoke("MoveToDiscardPile", 2f);
        else
            Invoke("RestoreToHand", 2f);
    }

    public void UseCard()
    {
        hasBeenPlayed = true;
        attackPointsText.color = Color.yellow;
        Invoke("RestoreToHand", 2f);

    }

    public void RestoreHealth()
    {
        defensePoints = maxDefPoints;
        defensePointsText.text = defensePoints.ToString();
        defensePointsText.color = Color.white;
    }
    void RestoreToHand()
    {

        RemoveCardFromPlay();
        if (cardOwner != gm.CurrentPlayerTurn())
            hasBeenPlayed = false;
    }
    public void RestorePlayability()
    {
        hasBeenPlayed = false;
    }
    void MoveToDiscardPile()
    {
        RestoreHealth();
        gm.RemoveFromBoard(this);
        if (cardOwner == 0)
            gm.availableCardSlotsP1[handIndex] = true;
        else
            gm.availableCardSlotsP2[handIndex] = true;
        if (cardOwner == 0)
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
        gm.PlaySelectSound();

        if (cardOwner == 0)
            transform.position += Vector3.up * 1.2f;
        else
            transform.position += Vector3.down * 1.2f;
    }
    public void RemoveCardFromPlay()
    {
        gm.PlayBackSound();
        attackPointsText.color = Color.white;
        if (cardOwner == 1)
            transform.position += Vector3.up * 1.2f;
        else
            transform.position += Vector3.down * 1.2f;
    }
}
