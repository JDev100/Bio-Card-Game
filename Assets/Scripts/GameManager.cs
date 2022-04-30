using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    //Turn System
    int currentPlayerTurn = 0;

    public List<Card> deckP1 = new List<Card>();
    public List<Card> deckP2 = new List<Card>();
    List<Card> cardsInBoardP1 = new List<Card>();
    List<Card> cardsInBoardP2 = new List<Card>();
    public List<Card> discardPileP1 = new List<Card>();
    public List<Card> discardPileP2 = new List<Card>();
    public Transform[] cardSlotsP1;
    public Transform[] cardSlotsP2;
    public bool[] availableCardSlotsP1;
    public bool[] availableCardSlotsP2;

    Card cardInPlayP1 = null;
    Card cardInPlayP2 = null;

    public Animator anim;
    public Text indicatorText;
    public AudioSource indicatorSound;
    public AudioSource hurtSound;
    public AudioSource dealSound;
    public AudioSource selectSound;
    public AudioSource backSound;

    public Text deckSizeTextP1;
    public Text deckSizeTextP2;
    public Text discardPileTextP1;
    public Text discardPileTextP2;
    public Text cardLimitTextP1;
    public Text cardLimitTextP2;
    int cardLimitGlobal = 1;
    int cardLimitP1;
    int cardLimitP2;

    public GameObject hurtEffect;


    bool hasDrawn = false;

    void Start()
    {
        ResetCardLimits();
    }

    void ResetCardLimits()
    {
        cardLimitP1 = cardLimitP2 = cardLimitGlobal;
        cardLimitTextP1.text = cardLimitTextP2.text = "Limit: " + cardLimitGlobal.ToString();
    }

    bool CanDraw(int turn)
    {
        if (turn == 0)
        {
            return cardLimitP1 > 0;
        }
        else
            return cardLimitP2 > 0;
    }
    void MinusLimit(int turn)
    {
        if (turn == 0)
        {
            cardLimitP1 -= 1;
            if (cardLimitP1 < 0)
                cardLimitP1 = 0;

            cardLimitTextP1.text = "Limit: " + cardLimitP1.ToString();
        }
        else if (turn == 1)
        {
            cardLimitP2 -= 1;
            if (cardLimitP2 < 0)
                cardLimitP2 = 0;

            cardLimitTextP2.text = "Limit: " + cardLimitP2.ToString();
        }
    }
    public void DrawCard(int player)
    {
        if (CanDraw(currentPlayerTurn))
        {
            hasDrawn = true;
            dealSound.Play();
            if (player == 0 && currentPlayerTurn == 0)
            {
                if (deckP1.Count >= 1)
                {
                    Card randCard = deckP1[Random.Range(0, deckP1.Count)];

                    for (int i = 0; i < availableCardSlotsP1.Length; i++)
                    {
                        if (availableCardSlotsP1[i] == true)
                        {
                            MinusLimit(0);
                            randCard.gameObject.SetActive(true);
                            randCard.transform.position = cardSlotsP1[i].position;
                            randCard.hasBeenPlayed = false;
                            randCard.handIndex = i;
                            availableCardSlotsP1[i] = false;
                            randCard.SetCardOwner(0);
                            deckP1.Remove(randCard);
                            cardsInBoardP1.Add(randCard);
                            return;
                        }
                    }
                }
            }
            if (player == 1 && currentPlayerTurn == 1)
            {
                if (deckP2.Count >= 1)
                {
                    Card randCard = deckP2[Random.Range(0, deckP2.Count)];

                    for (int i = 0; i < availableCardSlotsP2.Length; i++)
                    {
                        if (availableCardSlotsP2[i] == true)
                        {
                            MinusLimit(1);
                            randCard.SetCardOwner(1);
                            randCard.gameObject.SetActive(true);
                            randCard.transform.position = cardSlotsP2[i].position;
                            randCard.hasBeenPlayed = false;
                            randCard.handIndex = i;
                            availableCardSlotsP2[i] = false;
                            deckP2.Remove(randCard);
                            cardsInBoardP2.Add(randCard);
                            return;
                        }
                    }
                }
            }
        }
    }

    public void RemoveFromBoard(Card card)
    {
        if (card.GetCardOwner() == 0)
        {
            cardsInBoardP1.Remove(card);
        }
        else
            cardsInBoardP2.Remove(card);
    }

    public void SetCardInPlay(Card card)
    {
        bool correctPlayer = (card.GetCardOwner() == currentPlayerTurn);

        if (correctPlayer || (!correctPlayer && cardInPlayP2 != null))
        {
            if (cardInPlayP1 == null)
            {
                cardInPlayP1 = card;
                card.SetCardToPlay();
            }
            else
            {
                //PlayBackSound();
                cardInPlayP1.RemoveCardFromPlay();
                if (cardInPlayP1 != card)
                {
                    cardInPlayP1 = card;
                    card.SetCardToPlay();
                }
                else
                    cardInPlayP1 = null;

            }
            if (cardInPlayP1 && cardInPlayP2)
            {
                Invoke("Fight", .35f);

            }
            return;
        }
        if (correctPlayer || (!correctPlayer && cardInPlayP1 != null))
        {
            if (cardInPlayP2 == null)
            {
                cardInPlayP2 = card;
                card.SetCardToPlay();
                PlaySelectSound();
            }
            else
            {
                PlayBackSound();
                cardInPlayP2.RemoveCardFromPlay();
                if (cardInPlayP2 != card)
                {
                    cardInPlayP2 = card;
                    card.SetCardToPlay();
                }
                else
                    cardInPlayP2 = null;

            }
            if (cardInPlayP1 && cardInPlayP2)
            {
                Invoke("Fight", .35f);

            }
            return;
        }
    }

    public bool IsOpponentInPlay(int index)
    {
        if (index == 0 && currentPlayerTurn == 1)
        {
            if (cardsInBoardP1.Count == 0)
            {
                Debug.Log("Checkin plyaer 1");
                if (cardInPlayP1)
                    return (cardInPlayP1.GetCardOwner() == 1);
                else return false;
            }
            else
                return false;
        }
        else if (index == 1 && currentPlayerTurn == 0)
        {
            if (cardsInBoardP2.Count == 0)
            {
                if (cardInPlayP1)
                    return (cardInPlayP1.GetCardOwner() == 0);
                else return false;
            }
            else return false;
        }
        else
            return false;
    }

    public void UseCardInPlay()
    {
        hurtSound.Play();
        cardInPlayP1.UseCard();
        cardInPlayP1 = null;

    }

    public void Fight()
    {
        hurtSound.Play();
        cardInPlayP1.TakeDamage(cardInPlayP2.attackPoints);
        cardInPlayP2.TakeDamage(cardInPlayP1.attackPoints);
        cardInPlayP1 = null;
        cardInPlayP2 = null;
    }

    public void Shuffle(int player)
    {
        if (player == 0)
        {
            foreach (Card card in discardPileP1)
            {
                deckP1.Add(card);
            }
            discardPileP1.Clear();
        }
        if (player == 1)
        {
            foreach (Card card in discardPileP2)
            {
                deckP2.Add(card);
            }
            discardPileP2.Clear();
        }
        if (discardPileP1.Count >= 1)
        {
            foreach (Card card in discardPileP1)
            {
                deckP1.Add(card);
            }
            discardPileP1.Clear();
        }
    }

    public void EndTurn()
    {
        hasDrawn = false;
        anim.SetTrigger("Show Indicator");
        indicatorSound.Play();





        if (!cardInPlayP1 && !cardInPlayP2)
        {
            if (currentPlayerTurn == 0)
            {
                currentPlayerTurn = 1;
                foreach (Card card1 in cardsInBoardP1)
                    card1.RestorePlayability();
                foreach (Card card in cardsInBoardP2)
                    card.RestoreHealth();
            }
            else
            {
                currentPlayerTurn = 0;
                foreach (Card card1 in cardsInBoardP2)
                    card1.RestorePlayability();
                foreach (Card card in cardsInBoardP1)
                    card.RestoreHealth();
            }
            Shuffle(currentPlayerTurn);
        }
        if (currentPlayerTurn == 0)
            cardLimitGlobal += 1;
        ResetCardLimits();
        indicatorText.text = "Player " + (currentPlayerTurn + 1).ToString() + "'s Turn";
    }

    private void Update()
    {
        deckSizeTextP1.text = deckP1.Count.ToString();
        discardPileTextP1.text = discardPileP1.Count.ToString();
        deckSizeTextP2.text = deckP2.Count.ToString();
        discardPileTextP2.text = discardPileP2.Count.ToString();
    }

    public int CurrentPlayerTurn()
    {
        return currentPlayerTurn;
    }

    public void PlaySelectSound()
    {
        selectSound.Play();
    }
    public void PlayBackSound()
    {
        backSound.Play();
    }

    public GameObject GetHurtEffect() {
        return hurtEffect;
    }
}
