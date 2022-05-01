using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public float y_offset;
    Vector3 orginal_pos;
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

    
    Animator anim;
    
    // private void OnMouseDown()
    // {
    //     if (hasBeenPlayed == false)
    //     {
    //         gm.SetCardInPlay(this);

    //         // if (gm.CurrentPlayerTurn() == 0)
    //         //     transform.position += Vector3.up * 2;
    //         // else
    //         //     transform.position += Vector3.down * 2;
    //         // hasBeenPlayed = true;
    //         // if (gm.CurrentPlayerTurn() == 0)
    //         //     gm.availableCardSlotsP1[handIndex] = true;
    //         // else
    //         //     gm.availableCardSlotsP2[handIndex] = true;
    //         // Invoke("MoveToDiscardPile", 2f);
    //     }
    // }

    public void Click() {
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
        orginal_pos = transform.position;

        maxDefPoints = defensePoints;
        maxAtkPoints = attackPoints;
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();

        attackPointsText.text = attackPoints.ToString();
        defensePointsText.text = defensePoints.ToString();
    }

    public void TakeDamage(int atkPoints)
    {
        defensePoints -= atkPoints;
        defensePointsText.text = defensePoints.ToString();
        defensePointsText.color = Color.red;
        hasBeenPlayed = true;
        anim.SetTrigger("Damage");

        if (defensePoints <= 0)
            Invoke("MoveToDiscardPile", 1f);
        else
            Invoke("RestoreToHand", 1.5f);
    }

    public void UseCard()
    {
        anim.SetTrigger("Attack");
        hasBeenPlayed = true;
        attackPointsText.color = Color.yellow;
        Invoke("RestoreToHand", 1f);

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

        gm.PlayExplodeSound();
        GameObject effect = Instantiate(gm.GetHurtEffect(cardOwner), transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Destroy(effect, 5f);
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
        if (anim != null)
        anim.SetInteger("Player", owner);
    }

    public void SetCardToPlay()
    {
        gm.PlaySelectSound();

        GameObject effect = Instantiate(gm.drawEffect, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        Destroy(effect, 1f);

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
